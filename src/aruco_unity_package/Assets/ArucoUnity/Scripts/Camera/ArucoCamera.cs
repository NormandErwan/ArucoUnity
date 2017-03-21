using ArucoUnity.Plugin.cv;
using System;
using System.Collections;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
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
      public abstract int CamerasNumber { get; protected set; }

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
      /// True when the images has been updated this frame.
      /// </summary>
      public bool ImagesUpdatedThisFrame { get; protected set; }

      /// <summary>
      /// The images in a OpenCV format. When getting the property, a new Mat is created for each image from the corresponding 
      /// <see cref="ImageTextures"/> content. When setting, the <see cref="ImageTextures"/> content is updated for each image from the Mat array.
      /// </summary>
      public virtual Mat[] Images 
      {
        get
        {
          return images;
        }
        set
        {
          if (images != null && value != null && value.Length == CamerasNumber)
          {
            for (int i = 0; i < CamerasNumber; i++)
            {
              images[i] = value[i];
            }
            imagesHasBeenSetThisFrame = true; // The ImageTextures should be updated at the end of the frame
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

      protected bool imagesHasBeenSetThisFrame;
      protected Mat[] images;
      protected int[] imageDataSizes;
      protected Mat[] undistordedImages;
      protected Mat[][] undistordedImages_maps;

      // MonoBehaviour methods

      /// <summary>
      /// Initialize camera system state.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;
        ImagesUpdatedThisFrame = false;
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
        if (imagesHasBeenSetThisFrame)
        {
          for (int i = 0; i < CamerasNumber; i++)
          {
            ImageTextures[i].LoadRawTextureData(Images[i].dataIntPtr, imageDataSizes[i]);
            ImageTextures[i].Apply(false);
          }
          imagesHasBeenSetThisFrame = false;
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
      public abstract void Configure();

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

        for (int i = 0; i < CamerasNumber; i++)
        {
          Imgproc.Remap(Images[i], undistordedImages[i], undistordedImages_maps[i][0], undistordedImages_maps[i][1], InterpolationFlags.INTER_LINEAR);
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
        StartCoroutine("AutoUndistort");
        Started();
      }

      /// <summary>
      /// Execute the <see cref="Started"/> action.
      /// </summary>
      protected void OnStopped()
      {
        StopCoroutine("AutoUndistort");
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
      /// Execute the <see cref="ImagesUpdated"/> action.
      /// </summary>
      protected void OnImagesUpdated()
      {
        UpdateMatImages();
        ImagesUpdated();
      }

      /// <summary>
      /// Returns the OpenCV type equivalent to the format of the texture.
      /// </summary>
      /// <param name="imageTexture">The texture to analyze.</param>
      /// <returns>The equivalent OpenCV type.</returns>
      protected TYPE ImageType(Texture2D imageTexture)
      {
        TYPE type;
        var format = imageTexture.format;
        switch (format)
        {
          case TextureFormat.RGB24:
            type = TYPE.CV_8UC3;
            break;
          case TextureFormat.BGRA32:
          case TextureFormat.ARGB32:
          case TextureFormat.RGBA32:
            type = TYPE.CV_8UC4;
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
        images = new Mat[CamerasNumber];
        imageDataSizes = new int[CamerasNumber];

        Mat undistordedImages_R = null;
        if (CameraParameters != null)
        {
          undistordedImages = new Mat[CamerasNumber];
          undistordedImages_maps = new Mat[CamerasNumber][];
          undistordedImages_R = new Mat();
        }

        for (int cameraId = 0; cameraId < CamerasNumber; cameraId++)
        {
          // Image
          byte[] imageData = ImageTextures[cameraId].GetRawTextureData();
          images[cameraId] = new Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, ImageType(ImageTextures[cameraId]), imageData);
          imageDataSizes[cameraId] = (int)(images[cameraId].ElemSize() * images[cameraId].Total());

          // Undistortion maps
          if (CameraParameters != null)
          {
            Mat cameraMatrix = CameraParameters.CamerasMatrix[cameraId];
            undistordedImages_maps[cameraId] = new Mat[2]; // map1 and map2
            Imgproc.InitUndistortRectifyMap(cameraMatrix, CameraParameters.DistCoeffs[cameraId], undistordedImages_R,
              cameraMatrix, Images[cameraId].size, TYPE.CV_16SC2, out undistordedImages_maps[cameraId][0], out undistordedImages_maps[cameraId][1]);
            undistordedImages[cameraId] = new Mat(undistordedImages_maps[cameraId][0].size, ImageType(ImageTextures[cameraId]));
          }
        }
      }

      /// <summary>
      /// Update the <see cref="Images"/> property from the <see cref="ImageTextures"/> property.
      /// </summary>
      protected void UpdateMatImages()
      {
        for (int i = 0; i < CamerasNumber; i++)
        {
          images[i].dataByte = ImageTextures[i].GetRawTextureData();
        }
      }

      /// <summary>
      /// When the camera is started, undistort the images if <see cref="AutoUndistortWithCameraParameters"/> is true.
      /// </summary>
      protected IEnumerator AutoUndistort()
      {
        while (true)
        {
          yield return null;
          if (AutoUndistortWithCameraParameters)
          {
            Undistort();
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}