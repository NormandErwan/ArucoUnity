using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using UnityEngine;
using System;

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
      [Tooltip("Display automatically or not the camera images on screen.")]
      private bool displayImages = true;

      [SerializeField]
      [Tooltip("Start the cameras automatically after configured it.")]
      private bool autoStart = true;

      // Events

      public delegate void ArucoCameraAction();

      /// <summary>
      /// Executed when the camera system is configured.
      /// </summary>
      public event ArucoCameraAction OnConfigured;

      /// <summary>
      /// Executed when the camera system starts.
      /// </summary>
      public event ArucoCameraAction OnStarted;

      /// <summary>
      /// Executed when the camera system stops.
      /// </summary>
      public event ArucoCameraAction OnStopped;

      /// <summary>
      /// Executed when the images has been updated.
      /// </summary>
      public event ArucoCameraAction OnImagesUpdated;

      // Properties

      /// <summary>
      /// Display automatically or not the camera images on screen.
      /// </summary>
      public bool DisplayImages { get { return displayImages; } set { displayImages = value; } }

      /// <summary>
      /// Start the camera system automatically after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// True when the camera system has started.
      /// </summary>
      public bool Started { get; protected set; }

      /// <summary>
      /// True when the camera system is configured.
      /// </summary>
      public bool Configured { get; protected set; }

      /// <summary>
      /// True when the images has been updated this frame.
      /// </summary>
      public bool ImagesUpdatedThisFrame { get; protected set; }

      /// <summary>
      /// The images in a OpenCV format. When getting the property, a new Mat is created for each image from the corresponding 
      /// <see cref="ImageTextures"/> content. When setting, the <see cref="ImageTextures"/> content is updated for each image from the Mat array.
      /// </summary>
      public Mat[] Images 
      {
        get
        {
          // Initialize
          if (images == null)
          {
            images = new Mat[ImageTextures.Length];
            imageDataSizes = new int[ImageTextures.Length];

            for (int i = 0; i < ImageTextures.Length; i++)
            {
              byte[] imageData = ImageTextures[i].GetRawTextureData();
              images[i] = new Mat(ImageTextures[i].height, ImageTextures[i].width, TYPE.CV_8UC3, imageData);
              imageDataSizes[i] = (int)(images[i].ElemSize() * images[i].Total());
            }
            imagesGetThisFrame = true;
          }
          else if (!imagesGetThisFrame)
          {
            for (int i = 0; i < ImageTextures.Length; i++)
            {
              images[i].dataByte = ImageTextures[i].GetRawTextureData();
            }
            imagesGetThisFrame = true;
          }

          return images;
        }
        set
        {
          if (value.Length == images.Length)
          {
            Array.Clear(images, 0, images.Length);
            images = value;
            imagesHasBeenSetThisFrame = true;
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
      public CameraParameters[] CameraParameters { get; protected set; }

      /// <summary>
      /// The Unity camera components that will capture the <see cref="ImageTextures"/>.
      /// </summary>
      public Camera[] ImageCameras { get; protected set; }

      /// <summary>
      /// The correct image orientations.
      /// </summary>
      public virtual Quaternion[] ImageRotations { get; protected set; }

      /// <summary>
      /// The image ratios.
      /// </summary>
      public virtual float[] ImageRatios { get; protected set; }

      /// <summary>
      /// Allow to unflip an image if vertically flipped (use for mesh plane).
      /// </summary>
      public virtual Mesh[] ImageMeshes { get; protected set; }

      /// <summary>
      /// Allow to unflip an image if vertically flipped (use for canvas).
      /// </summary>
      public virtual Rect[] ImageUvRectFlips { get; protected set; }

      /// <summary>
      /// Mirror front-facing camera images horizontally to look more natural.
      /// </summary>
      public virtual Vector3[] ImageScalesFrontFacing { get; protected set; }

      // Variables

      protected bool imagesHasBeenSetThisFrame;
      protected Mat[] images;
      protected int[] imageDataSizes;
      protected bool imagesGetThisFrame;

      // MonoBehaviour methods

      /// <summary>
      /// Initialize camera system state.
      /// </summary>
      protected virtual void Awake()
      {
        Configured = false;
        Started = false;
        ImagesUpdatedThisFrame = false;
        imagesGetThisFrame = false;
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
        imagesGetThisFrame = false;
        imagesHasBeenSetThisFrame = false;

        UpdateCameraImages();
      }

      /// <summary>
      /// Apply on <see cref="ImageTextures"/> the changes made on <see cref="Images"/> during the frame.
      /// </summary>
      protected virtual void LateUpdate()
      {
        Undistord();

        if (imagesHasBeenSetThisFrame)
        {
          for (int i = 0; i < ImageTextures.Length; i++)
          {
            ImageTextures[i].LoadRawTextureData(images[i].dataIntPtr, imageDataSizes[i]);
            ImageTextures[i].Apply(false);
          }
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
      /// Undistord the images according to the <see cref="Utility.CameraParameters"/>, if not null. <see cref="Images"/> is immediatly updated. 
      /// <see cref="ImageTextures"/> will be updated at LateUpdate().
      /// </summary>
      public virtual void Undistord()
      {
        if (CameraParameters == null)
        {
          return;
        }

        Mat[] undistordedImages = new Mat[ImageTextures.Length];
        for (int i = 0; i < ImageTextures.Length; i++)
        {
          Imgproc.Undistord(Images[i], out undistordedImages[i], CameraParameters[i].CameraMatrix, CameraParameters[i].DistCoeffs);
        }
        Images = undistordedImages;
      }

      /// <summary>
      /// Update <see cref="ImageTextures"/> if the camera system has started.
      /// </summary>
      protected abstract void UpdateCameraImages();

      /// <summary>
      /// Execute the <see cref="OnStarted"/> action.
      /// </summary>
      protected void RaiseOnStarted()
      {
        if (OnStarted != null)
        {
          OnStarted();
        }
      }

      /// <summary>
      /// Execute the <see cref="OnStarted"/> action.
      /// </summary>
      protected void RaiseOnStopped()
      {
        if (OnStopped != null)
        {
          OnStopped();
        }
      }

      /// <summary>
      /// Execute the <see cref="OnConfigured"/> action.
      /// </summary>
      protected void RaiseOnConfigured()
      {
        if (OnConfigured != null)
        {
          OnConfigured();
        }
      }

      /// <summary>
      /// Execute the <see cref="OnImagesUpdated"/> action.
      /// </summary>
      protected void RaiseOnImageUpdated()
      {
        if (OnImagesUpdated != null)
        {
          OnImagesUpdated();
        }
      }
    }
  }

  /// \} aruco_unity_package
}
