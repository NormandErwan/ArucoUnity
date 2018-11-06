using ArucoUnity.Plugin;
using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity.Cameras
{
    /// <summary>
    /// Captures images of a camera.
    /// </summary>
    /// <remarks>
    /// To add a new camera, you need to derive this class. See <see cref="ArucoWebcam"/> as example. You need to
    /// implement <see cref="Controller.Configuring"/>, <see cref="Controller.Starting"/>,
    /// and <see cref="UpdatingImages"/>.
    /// </remarks>
    public abstract class ArucoCamera : Controller, IArucoCamera
    {
        // Constants

        protected readonly int? dontFlipCode = null;
        private const int buffersCount = 2;

        // IArucoCamera events

        public event Action ImagesUpdated = delegate { };
        public event Action<Cv.Mat[], byte[][]> UndistortRectifyImages = delegate { };

        // IArucoCamera properties

        public abstract int CameraNumber { get; }
        public abstract string Name { get; protected set; }

        public Texture2D[] Textures { get; private set; }
        public Cv.Mat[] Images { get { return imageBuffers[currentBuffer]; } }
        public byte[][] ImageDatas { get { return imageDataBuffers[currentBuffer]; } }
        public int[] ImageDataSizes { get; private set; }
        public float[] ImageRatios { get; private set; }

        protected Cv.Mat[] NextImages { get { return imageBuffers[NextBuffer()]; } }
        protected byte[][] NextImageDatas { get { return imageDataBuffers[NextBuffer()]; } }

        // Variables

        protected uint currentBuffer = 0;
        protected Cv.Mat[][] imageBuffers = new Cv.Mat[buffersCount][];
        protected byte[][][] imageDataBuffers = new byte[buffersCount][][];

        protected Cv.Mat[] imagesToTextures;
        protected byte[][] imagesToTextureDatas;

        protected bool imagesUpdatedThisFrame = false;
        protected bool flipHorizontallyImages = false,
                                     flipVerticallyImages = false;
        protected int? imagesFlipCode;

        // MonoBehaviour methods

        /// <summary>
        /// When configured and started, calls <see cref="UpdatingImages"/> then <see cref="OnImagesUpdated"/>.
        /// </summary>
        protected virtual void Update()
        {
            if (IsConfigured && IsStarted)
            {
                if (UpdatingImages())
                {
                    OnImagesUpdated();
                }
            }
        }

        // ConfigurableController methods

        /// <summary>
        /// Configures the properties.
        /// </summary>
        protected override void Configuring()
        {
            base.Configuring();

            if (CameraNumber <= 0)
            {
                throw new Exception("It must have at least one camera.");
            }

            Textures = new Texture2D[CameraNumber];
            ImageDataSizes = new int[CameraNumber];
            ImageRatios = new float[CameraNumber];

            imagesToTextures = new Cv.Mat[CameraNumber];
            imagesToTextureDatas = new byte[CameraNumber][];

            for (int bufferId = 0; bufferId < buffersCount; bufferId++)
            {
                imageBuffers[bufferId] = new Cv.Mat[CameraNumber];
                imageDataBuffers[bufferId] = new byte[CameraNumber][];
            }

            if (!flipHorizontallyImages && !flipVerticallyImages)
            {
                imagesFlipCode = Cv.verticalFlipCode;
            }
            else if (flipHorizontallyImages && !flipVerticallyImages)
            {
                imagesFlipCode = Cv.bothAxesFlipCode;
            }
            else if (!flipHorizontallyImages && flipVerticallyImages)
            {
                imagesFlipCode = dontFlipCode; // Don't flip because the image textures are already vertically flipped
            }
            else if (flipHorizontallyImages && flipVerticallyImages)
            {
                imagesFlipCode = Cv.horizontalFlipCode; // Image textures are already vertically flipped
            }
        }

        /// <summary>
        /// Initializes the <see cref="Images"/>, <see cref="ImageDataSizes"/>, <see cref="ImageDatas"/>,
        /// <see cref="NextImages"/>, <see cref="NextImageTextures"/> and <see cref="NextImageDatas"/> properties from the
        /// <see cref="Textures"/> property.
        /// </summary>
        protected override void OnStarted()
        {
            for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
            {
                for (int bufferId = 0; bufferId < buffersCount; bufferId++)
                {
                    imageBuffers[bufferId][cameraId] = new Cv.Mat(Textures[cameraId].height, Textures[cameraId].width,
                        CvMatExtensions.ImageType(Textures[cameraId].format));
                }

                ImageDataSizes[cameraId] = (int)(Images[cameraId].ElemSize() * Images[cameraId].Total());
                ImageRatios[cameraId] = Textures[cameraId].width / (float)Textures[cameraId].height;

                for (int bufferId = 0; bufferId < buffersCount; bufferId++)
                {
                    imageDataBuffers[bufferId][cameraId] = new byte[ImageDataSizes[cameraId]];
                    imageBuffers[bufferId][cameraId].DataByte = imageDataBuffers[bufferId][cameraId];
                }

                imagesToTextures[cameraId] = new Cv.Mat(Textures[cameraId].height, Textures[cameraId].width,
                        CvMatExtensions.ImageType(Textures[cameraId].format));
                imagesToTextureDatas[cameraId] = new byte[ImageDataSizes[cameraId]];
                imagesToTextures[cameraId].DataByte = imagesToTextureDatas[cameraId];
            }

            base.OnStarted();
        }

        // Methods

        /// <summary>
        /// Updates <see cref="NextImages"/> with the new camera images.
        /// </summary>
        /// <returns>If <see cref="NextImages"/> have been updated.</returns>
        protected abstract bool UpdatingImages();

        /// <summary>
        /// Calls <see cref="UndistortRectifyImages"/> with the <see cref="NextImages"/>, swaps <see cref="Images"/> and
        /// <see cref="ImageDatas"/>, the calls <see cref="ImagesUpdated"/> and applies the changes made on the
        /// <see cref="Images"/> to the <see cref="Textures"/>.
        /// </summary>
        protected virtual void OnImagesUpdated()
        {
            // Undistort next images
            if (imagesFlipCode != dontFlipCode)
            {
                for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
                {
                    Cv.Flip(NextImages[cameraId], NextImages[cameraId], (int)imagesFlipCode);
                }
            }
            UndistortRectifyImages(NextImages, NextImageDatas);

            // Swap images
            currentBuffer = NextBuffer();
            ImagesUpdated();

            // Update Textures with Images
            for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
            {
                Cv.Flip(Images[cameraId], imagesToTextures[cameraId], Cv.verticalFlipCode);
                Textures[cameraId].LoadRawTextureData(imagesToTextures[cameraId].DataIntPtr, ImageDataSizes[cameraId]);
                Textures[cameraId].Apply(false);
            }
        }

        /// <summary>
        /// Returns the index of the next buffer.
        /// </summary>
        private uint NextBuffer()
        {
            return (currentBuffer + 1) % buffersCount;
        }
    }
}