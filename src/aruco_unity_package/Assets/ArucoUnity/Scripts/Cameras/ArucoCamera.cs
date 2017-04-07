using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Manages to retrieve and display every frame the images of any system with a fixed number of cameras to use with ArucoUnity.
    /// </summary>
    public abstract class ArucoCamera : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Start the cameras automatically after configured it. Call StartCameras() alternatively.")]
      private bool autoStart = true;

      [SerializeField]
      [Tooltip("Display automatically or not the camera images on screen.")]
      private bool displayImages = true;

      [SerializeField]
      [Tooltip("Undistort automatically each image according to the camera parameters.")]
      private bool autoUndistortWithCameraParameters = true;

      [SerializeField]
      [Tooltip("Are the camera fisheye?")]
      private bool isFisheye = false;

      // Events

      public delegate void CameraEventHandler();

      /// <summary>
      /// Executed when the camera system is configured.
      /// </summary>
      public event CameraEventHandler Configured = delegate { };

      /// <summary>
      /// Executed when the camera system starts.
      /// </summary>
      public event CameraEventHandler Started = delegate { };

      /// <summary>
      /// Executed when the camera system stops.
      /// </summary>
      public event CameraEventHandler Stopped = delegate { };

      /// <summary>
      /// Executed when the images has been updated.
      /// </summary>
      public event CameraEventHandler ImagesUpdated = delegate { };

      // Properties

      /// <summary>
      /// Start the camera system automatically after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// Display automatically or not the camera images on screen.
      /// </summary>
      public bool DisplayImages { get { return displayImages; } set { displayImages = value; } }

      /// <summary>
      /// Undistort automatically each image according to the camera parameters.
      /// </summary>
      public bool AutoUndistortWithCameraParameters { get { return autoUndistortWithCameraParameters; } set { autoUndistortWithCameraParameters = value; } }

      /// <summary>
      /// The number of cameras in the system.
      /// </summary>
      public abstract int CameraNumber { get; protected set; }

      /// <summary>
      /// The name of the camera system used.
      /// </summary>
      public abstract string Name { get; protected set; }

      /// <summary>
      /// True when the camera system is configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      /// <summary>
      /// True when the camera system is started.
      /// </summary>
      public bool IsStarted { get; protected set; }

      /// <summary>
      /// Are the camera fisheye?
      /// </summary>
      public bool IsFisheye { get { return isFisheye; } set { isFisheye = value; } }

      /// <summary>
      /// The images in a OpenCV format. When getting the property, a new Cv.Core.Mat is created for each image from the corresponding 
      /// <see cref="ImageTextures"/> content. When setting, the <see cref="ImageTextures"/> content is updated for each image from the Cv.Core.Mat array.
      /// </summary>
      public virtual Cv.Core.Mat[] Images
      {
        get
        {
          return images;
        }
        set
        {
          if (images != null && value != null && value.Length == CameraNumber)
          {
            for (int i = 0; i < CameraNumber; i++)
            {
              images[i] = value[i];
            }
          }
        }
      }

      /// <summary>
      /// Image textures, updated each frame.
      /// </summary>
      public Texture2D[] ImageTextures { get; protected set; }

      /// <summary>
      /// The parameters of each camera.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      /// <summary>
      /// The Unity camera components that will capture the <see cref="ImageTextures"/>.
      /// </summary>
      public Camera[] ImageCameras { get; protected set; }

      /// <summary>
      /// The image ratios.
      /// </summary>
      public virtual float[] ImageRatios { get; protected set; }

      /// <summary>
      /// The recommended mesh plane to display the <see cref="ImageTextures"/>.
      /// </summary>
      public virtual Mesh[] ImageMeshes { get; protected set; }

      // Variables

      protected Cv.Core.Mat[] images;
      protected int[] imageDataSizes;
      protected Cv.Core.Mat[] undistordedImages;
      protected Cv.Core.Mat[][] undistordedImageMaps;
      protected bool flipHorizontallyImages = false, flipVerticallyImages = false;
      protected int? flipCode; // Convert the images from Unity's left-handed coordinate system to OpenCV's right-handed coordinate system

      // MonoBehaviour methods

      /// <summary>
      /// Initialize camera system state.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;
      }

      /// <summary>
      /// Configure the camera at start if <see cref="AutoStart"/> is true.
      /// </summary>
      protected virtual void Start()
      {
        if (AutoStart)
        {
          Configure();
        }
      }

      /// <summary>
      /// Reset <see cref="Images"/> and retrieve the new images for this frame.
      /// </summary>
      protected virtual void Update()
      {
        UpdateCameraImages();
      }

      /// <summary>
      /// Apply the changes made on the <see cref="Images"/> during the frame to the <see cref="ImageTextures"/>.
      /// </summary>
      protected virtual void LateUpdate()
      {
        for (int i = 0; i < CameraNumber; i++)
        {
          // Convert back to the images from OpenCV's right-handed coordinate system to Unity's left-handed coordinate system
          int verticalFlipCode = 0;
          Cv.Core.Flip(Images[i], Images[i], verticalFlipCode);

          // Load back the data from the Images to the ImageTextures
          ImageTextures[i].LoadRawTextureData(Images[i].dataIntPtr, imageDataSizes[i]);
          ImageTextures[i].Apply(false);
        }
      }

      /// <summary>
      /// Automatically stop the camera.
      /// </summary>
      protected virtual void OnDestroy()
      {
        StopCameras();
      }

      // Methods

      /// <summary>
      /// Configure the cameras and their properties.
      /// </summary>
      public virtual void Configure()
      {
        // Configure the flip code to load the ImageTextures to the Images
        if (flipHorizontallyImages && !flipVerticallyImages)
        {
          flipCode = -1;
        }
        else if (!flipHorizontallyImages && flipVerticallyImages)
        {
          flipCode = null;
        }
        else if (flipHorizontallyImages && flipVerticallyImages)
        {
          flipCode = 1;
        }
        else if (!flipHorizontallyImages && !flipVerticallyImages)
        {
          flipCode = 0;
        }

        // Update state
        IsConfigured = true;
        OnConfigured();

        // AutoStart
        if (AutoStart)
        {
          StartCameras();
        }
      }

      /// <summary>
      /// Start the camera system.
      /// </summary>
      public abstract void StartCameras();

      /// <summary>
      /// Stop the camera system.
      /// </summary>
      public abstract void StopCameras();

      /// <summary>
      /// Undistort the images according to the <see cref="Utility.CameraParameters"/>, if not null. <see cref="Images"/> is immediatly updated. 
      /// <see cref="ImageTextures"/> will be updated at LateUpdate().
      /// </summary>
      public virtual void Undistort()
      {
        if (CameraParameters == null)
        {
          return;
        }

        for (int i = 0; i < CameraNumber; i++)
        {
          Cv.Imgproc.Remap(Images[i], undistordedImages[i], undistordedImageMaps[i][0], undistordedImageMaps[i][1], Cv.Imgproc.InterpolationFlags.Linear);
        }
        Images = undistordedImages;
      }

      /// <summary>
      /// Update <see cref="ImageTextures"/> if the camera system has started.
      /// </summary>
      protected abstract void UpdateCameraImages();

      /// <summary>
      /// Execute the <see cref="Started"/> action.
      /// </summary>
      protected void OnStarted()
      {
        InitializeMatImages();
        Started();
      }

      /// <summary>
      /// Execute the <see cref="Started"/> action.
      /// </summary>
      protected void OnStopped()
      {
        Stopped();
      }

      /// <summary>
      /// Execute the <see cref="Configured"/> action.
      /// </summary>
      protected void OnConfigured()
      {
        Configured();
      }

      /// <summary>
      /// Update the <see cref="Images"/> property from the <see cref="ImageTextures"/> property.
      /// </summary>
      protected void OnImagesUpdated()
      {
        for (int i = 0; i < CameraNumber; i++)
        {
          images[i].dataByte = ImageTextures[i].GetRawTextureData();
          if (flipCode != null)
          {
            Cv.Core.Flip(Images[i], Images[i], (int)flipCode);
          }
        }

        if (AutoUndistortWithCameraParameters)
        {
          Undistort();
        }

        ImagesUpdated();
      }

      /// <summary>
      /// Returns the OpenCV type equivalent to the format of the texture.
      /// </summary>
      /// <param name="imageTexture">The texture to analyze.</param>
      /// <returns>The equivalent OpenCV type.</returns>
      protected Cv.Core.Type ImageType(Texture2D imageTexture)
      {
        Cv.Core.Type type;
        var format = imageTexture.format;
        switch (format)
        {
          case TextureFormat.RGB24:
            type = Cv.Core.Type.CV_8UC3;
            break;
          case TextureFormat.BGRA32:
          case TextureFormat.ARGB32:
          case TextureFormat.RGBA32:
            type = Cv.Core.Type.CV_8UC4;
            break;
          default:
            throw new ArgumentException("This type of texture is actually not supported: " + imageTexture.format + ".", "imageTexture");
        }
        return type;
      }

      /// <summary>
      /// Initialize the <see cref="Images"/> property, the undistortion maps and the undistorded images.
      /// </summary>
      protected void InitializeMatImages()
      {
        images = new Cv.Core.Mat[CameraNumber];
        imageDataSizes = new int[CameraNumber];

        Cv.Core.Mat undistordedImageRectifications = null;
        if (CameraParameters != null)
        {
          undistordedImages = new Cv.Core.Mat[CameraNumber];
          undistordedImageMaps = new Cv.Core.Mat[CameraNumber][];
          undistordedImageRectifications = new Cv.Core.Mat();
        }

        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          // Init the images
          byte[] imageData = ImageTextures[cameraId].GetRawTextureData();
          images[cameraId] = new Cv.Core.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, ImageType(ImageTextures[cameraId]),
            imageData);
          imageDataSizes[cameraId] = (int)(images[cameraId].ElemSize() * images[cameraId].Total());

          // Init the undistortion maps
          if (CameraParameters != null)
          {
            undistordedImageMaps[cameraId] = new Cv.Core.Mat[2]; // map1 and map2
            if (!IsFisheye)
            {
              Cv.Imgproc.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
                undistordedImageRectifications, CameraParameters.CameraMatrices[cameraId], Images[cameraId].size, Cv.Core.Type.CV_16SC2,
                out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1]);
            }
            else
            {
              Cv.Calib3d.Fisheye.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
                undistordedImageRectifications, CameraParameters.CameraMatrices[cameraId], Images[cameraId].size, Cv.Core.Type.CV_16SC2,
                out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1]);
            }
            undistordedImages[cameraId] = new Cv.Core.Mat(undistordedImageMaps[cameraId][0].size, ImageType(ImageTextures[cameraId]));
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}