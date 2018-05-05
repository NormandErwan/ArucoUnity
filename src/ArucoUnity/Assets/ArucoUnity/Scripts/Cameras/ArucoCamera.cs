using ArucoUnity.Plugin;
using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Captures images of a camera.
    /// </summary>
    /// <remarks>
    /// If you want to use a custom physical camera not supported by Unity, you need to derive this class. See
    /// <see cref="WebcamArucoCamera"/> as example. You will need to implement <see cref="StartController"/>, <see cref="StopController"/>,
    /// <see cref="Configure"/> and to set <see cref="ImageDatas"/> when <see cref="UpdateCameraImages"/> is called.
    /// </remarks>
    public abstract class ArucoCamera : ConfigurableController, IArucoCamera
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

      /// <summary>
      /// Gets the the current camera images manipulated by Unity. They are updated at <see cref="LateUpdate"/> from the OpenCV <see cref="Images"/>.
      /// </summary>
      public Texture2D[] ImageTextures { get; private set; }

      /// <summary>
      /// Gets or sets the current camera images manipulated by OpenCV. They are updated at <see cref="UpdateCameraImages"/>.
      /// </summary>
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
      /// Calls <see cref="UpdateCameraImages"> if configured and started.
      /// </summary>
      protected virtual void Update()
      {
        if (IsConfigured && IsStarted)
        {
          imagesUpdatedThisFrame = false;
          UpdateCameraImages();
        }
      }

      /// <summary>
      /// Applies the changes made on the <see cref="Images"/> during the frame to the <see cref="ImageTextures"/>
      /// then swaps <see cref="Images"/> and <see cref="ImageDatas"/> with <see cref="NextImages"/> and <see cref="NextImageDatas"/>.
      /// </summary>
      protected virtual void LateUpdate()
      {
        if (IsConfigured && IsStarted && imagesUpdatedThisFrame)
        {
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            Cv.Flip(Images[cameraId], imagesToTextures[cameraId], Cv.verticalFlipCode);
            ImageTextures[cameraId].LoadRawTextureData(imagesToTextures[cameraId].DataIntPtr, ImageDataSizes[cameraId]);
            ImageTextures[cameraId].Apply(false);
          }
          currentBuffer = NextBuffer();
        }
      }

      // ConfigurableController methods

      /// <summary>
      /// Configures all the images related properties.
      /// </summary>
      protected override void Configuring()
      {
        base.Configuring();

        if (CameraNumber <= 0)
        {
          throw new Exception("It must have at least one camera.");
        }

        ImageTextures = new Texture2D[CameraNumber];
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
          imagesFlipCode = Cv.horizontalFlipCode; // Don't flip vertically because the image textures are already vertically flipped
        }
      }

      /// <summary>
      /// Calls <see cref="InitializeImages"/>.
      /// </summary>
      protected override void Starting()
      {
        base.Starting();
        InitializeImages();
      }

      // Methods

      /// <summary>
      /// Updates <see cref="NextImage"/> with the new camera images and calls <see cref="OnImagesUpdated"/>.
      /// </summary>
      protected abstract void UpdateCameraImages();

      /// <summary>
      /// Returns the index of the next buffer.
      /// </summary>
      protected uint NextBuffer()
      {
        return (currentBuffer + 1) % buffersCount;
      }

      /// <summary>
      /// Calls <see cref="UndistortRectifyImages"/> with the <see cref="NextImages"/> and <see cref="ImagesUpdated"/>.
      /// </summary>
      protected void OnImagesUpdated()
      {
        if (imagesFlipCode != null)
        {
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            Cv.Flip(NextImages[cameraId], NextImages[cameraId], (int)imagesFlipCode);
          }
        }
        UndistortRectifyImages(NextImages, NextImageDatas);

        imagesUpdatedThisFrame = true;
        ImagesUpdated();
      }

      /// <summary>
      /// Initializes the <see cref="Images"/>, <see cref="ImageDataSizes"/>, <see cref="ImageDatas"/>, <see cref="NextImages"/>,
      /// <see cref="NextImageTextures"/> and <see cref="NextImageDatas"/> properties from the <see cref="ImageTextures"/> property.
      /// </summary>
      protected virtual void InitializeImages()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          for (int bufferId = 0; bufferId < buffersCount; bufferId++)
          {
            imageBuffers[bufferId][cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width,
              CvMatExtensions.ImageType(ImageTextures[cameraId].format));
          }

          ImageDataSizes[cameraId] = (int)(Images[cameraId].ElemSize() * Images[cameraId].Total());
          ImageRatios[cameraId] = ImageTextures[cameraId].width / (float)ImageTextures[cameraId].height;

          for (int bufferId = 0; bufferId < buffersCount; bufferId++)
          {
            imageDataBuffers[bufferId][cameraId] = new byte[ImageDataSizes[cameraId]];
            imageBuffers[bufferId][cameraId].DataByte = imageDataBuffers[bufferId][cameraId];
          }

          imagesToTextures[cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width,
              CvMatExtensions.ImageType(ImageTextures[cameraId].format));
          imagesToTextureDatas[cameraId] = new byte[ImageDataSizes[cameraId]];
          imagesToTextures[cameraId].DataByte = imagesToTextureDatas[cameraId];
        }
      }
    }
  }

  /// \} aruco_unity_package
}