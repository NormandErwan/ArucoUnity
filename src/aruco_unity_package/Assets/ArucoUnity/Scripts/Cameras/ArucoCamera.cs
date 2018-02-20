using ArucoUnity.Controllers;
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
    /// Captures the image frames of a camera system.
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
      public event Action UndistortRectifyImages = delegate { };

      // IArucoCamera properties

      public abstract int CameraNumber { get; }
      public abstract string Name { get; protected set; }

      /// <summary>
      /// Gets the the current images frame manipulated by Unity. They are updated at <see cref="LateUpdate"/> from the OpenCV <see cref="Images"/>.
      /// </summary>
      public Texture2D[] ImageTextures { get { return imageTexturesBuffers[currentBuffer]; } }

      /// <summary>
      /// Gets or sets the current images frame manipulated by OpenCV. They are updated at <see cref="UpdateCameraImages"/>.
      /// </summary>
      public virtual Cv.Mat[] Images { get { return imageBuffers[currentBuffer]; } }

      public byte[][] ImageDatas { get { return imageDataBuffers[currentBuffer]; } }
      public int[] ImageDataSizes { get; private set; }
      public float[] ImageRatios { get; private set; }

      protected Texture2D[] NextImageTextures { get { return imageTexturesBuffers[NextBuffer()]; } }
      protected virtual Cv.Mat[] NextImages { get { return imageBuffers[NextBuffer()]; } }
      protected byte[][] NextImageDatas { get { return imageDataBuffers[NextBuffer()]; } }

      // Variables

      protected uint currentBuffer = 0;
      protected Texture2D[][] imageTexturesBuffers = new Texture2D[buffersCount][];
      protected Cv.Mat[][] imageBuffers = new Cv.Mat[buffersCount][];
      protected byte[][][] imageDataBuffers = new byte[buffersCount][][];

      protected bool imagesUpdatedThisFrame = false;
      protected bool flipHorizontallyImages = false,
                     flipVerticallyImages = false;
      protected int? preDetectflipCode, // Convert the images from Unity's left-handed coordinate system to OpenCV's right-handed coordinate system
                     postDetectflipCode; // Convert back the images

      // MonoBehaviour methods

      /// <summary>
      /// Updates <see cref="Images"/> with the new frame images.
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
      /// Applies the changes made on the <see cref="Images"/> during the frame to the <see cref="ImageTextures"/>.
      /// </summary>
      protected virtual void LateUpdate()
      {
        if (!IsConfigured || !IsStarted || !imagesUpdatedThisFrame)
        {
          return;
        }

        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          // Flip the Images if needed and load them to the textures
          if (postDetectflipCode != null)
          {
            Cv.Flip(NextImages[cameraId], NextImages[cameraId], (int)postDetectflipCode);
          }

          // Load the Images to the ImageTextures
          NextImageTextures[cameraId].LoadRawTextureData(NextImages[cameraId].DataIntPtr, ImageDataSizes[cameraId]);
          NextImageTextures[cameraId].Apply(false);
        }

        currentBuffer = NextBuffer();
      }

      // Methods

      /// <summary>
      /// Configures the all the images related properties, calls the <see cref="Configured"/> event, and calls <see cref="StartController"/> if
      /// <see cref="AutoStart"/> is true.
      /// </summary>
      protected override void OnConfigured()
      {
        if (CameraNumber <= 0)
        {
          throw new Exception("It must have at least one camera.");
        }

        // Initialize the properties
        ImageDataSizes = new int[CameraNumber];
        ImageRatios = new float[CameraNumber];

        for (int bufferId = 0; bufferId < buffersCount; bufferId++)
        {
          imageBuffers[bufferId] = new Cv.Mat[CameraNumber];
          imageTexturesBuffers[bufferId] = new Texture2D[CameraNumber];
          imageDataBuffers[bufferId] = new byte[CameraNumber][];
        }

        // Configure the flip codes to transfer images from Unity to OpenCV and vice-versa
        // The raw bytes from a Texture to a Mat and from a Mat to a Texture needs to be vertically flipped to be in the correct orientation
        if (!flipHorizontallyImages && !flipVerticallyImages)
        {
          preDetectflipCode = postDetectflipCode = Cv.verticalFlipCode;
        }
        else if (flipHorizontallyImages && !flipVerticallyImages)
        {
          preDetectflipCode = Cv.verticalFlipCode;
          postDetectflipCode = Cv.bothAxesFlipCode;
        }
        else if (!flipHorizontallyImages && flipVerticallyImages)
        {
          preDetectflipCode = dontFlipCode; // Don't flip because texture image is already vertically flipped
          postDetectflipCode = Cv.verticalFlipCode;
        }
        else if (flipHorizontallyImages && flipVerticallyImages)
        {
          preDetectflipCode = dontFlipCode; // Don't flip because texture image is already vertically flipped
          postDetectflipCode = Cv.bothAxesFlipCode;
        }

        base.OnConfigured();
      }

      /// <summary>
      /// Calls <see cref="InitializeImages"/> and the <see cref="Started"/> event.
      /// </summary>
      protected override void OnStarted()
      {
        InitializeImages();
        base.OnStarted();
      }

      /// <summary>
      /// Updates <see cref="ImageDatas"/> with the current frame images and calls <see cref="OnImagesUpdated"/>.
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
      /// Calls the <see cref="UndistortRectifyImages"/> and <see cref="ImagesUpdated"/> events.
      /// </summary>
      protected void OnImagesUpdated()
      {
        // Flip the images if needed
        if (preDetectflipCode != null)
        {
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            Cv.Flip(NextImages[cameraId], NextImages[cameraId], (int)preDetectflipCode);
          }
        }

        // Undistort images
        UndistortRectifyImages();

        // Update state
        imagesUpdatedThisFrame = true;
        ImagesUpdated();
      }

      /// <summary>
      /// Initializes the <see cref="Images"/>, <see cref="ImageDataSizes"/> and <see cref="ImageDatas"/> properties from the
      /// <see cref="ImageTextures"/> property.
      /// </summary>
      protected virtual void InitializeImages()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          for (int bufferId = 0; bufferId < buffersCount; bufferId++)
          {
            if (bufferId != currentBuffer)
            {
              imageTexturesBuffers[bufferId][cameraId] = new Texture2D(ImageTextures[cameraId].width, ImageTextures[cameraId].height, ImageTextures[cameraId].format, false);
            }
            imageBuffers[bufferId][cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, CvMatExtensions.ImageType(ImageTextures[cameraId].format));
          }

          ImageDataSizes[cameraId] = (int)(Images[cameraId].ElemSize() * Images[cameraId].Total());
          ImageRatios[cameraId] = ImageTextures[cameraId].width / (float)ImageTextures[cameraId].height;

          for (int bufferId = 0; bufferId < buffersCount; bufferId++)
          {
            imageDataBuffers[bufferId][cameraId] = new byte[ImageDataSizes[cameraId]];
            imageBuffers[bufferId][cameraId].DataByte = imageDataBuffers[bufferId][cameraId];
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}