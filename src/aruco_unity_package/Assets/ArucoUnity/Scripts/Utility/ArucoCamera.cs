using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Base for any camera sytem to use with ArucoUnity. Manages to retrieve and display the camera's image every frame.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public abstract class ArucoCamera : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Display automatically or not the camera's image on screen.")]
      private bool displayImage = true;

      [SerializeField]
      [Tooltip("Start the camera automatically after configured it.")]
      private bool autoStart = true;

      // Events

      public delegate void ArucoCameraAction();

      /// <summary>
      /// Executed when the camera is configured.
      /// </summary>
      public event ArucoCameraAction OnConfigured;

      /// <summary>
      /// Executed when the camera starts.
      /// </summary>
      public event ArucoCameraAction OnStarted;

      /// <summary>
      /// Executed when the camera stops.
      /// </summary>
      public event ArucoCameraAction OnStopped;

      /// <summary>
      /// Executed when the image has been updated.
      /// </summary>
      public event ArucoCameraAction OnImageUpdated;

      // Properties

      /// <summary>
      /// Display automatically or not the camera's image on screen.
      /// </summary>
      public bool DisplayImage { get { return displayImage; } set { displayImage = value; } }

      /// <summary>
      /// Start the camera automatically after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// True when the camera has started.
      /// </summary>
      public bool Started { get; protected set; }

      /// <summary>
      /// True when the camera is configured.
      /// </summary>
      public bool Configured { get; protected set; }

      /// <summary>
      /// True when the image has been updated this frame.
      /// </summary>
      public bool ImageUpdatedThisFrame { get; protected set; }

      /// <summary>
      /// The image in a OpenCV format. When getting, a new Mat is created from the <see cref="ImageTexture"/> content. When setting, the
      /// <see cref="ImageTexture"/> content is updated from the Mat.
      /// </summary>
      public Mat Image 
      {
        get
        {
          if (image == null)
          {
            byte[] imageData = ImageTexture.GetRawTextureData();
            image = new Mat(ImageTexture.height, ImageTexture.width, TYPE.CV_8UC3, imageData);
          }
          return image;
        }
        set
        {
          image = value;
          imageHasBeenSet = true;
        }
      }

      /// <summary>
      /// Image texture, updated each frame.
      /// </summary>
      public Texture2D ImageTexture { get; protected set; }

      /// <summary>
      /// The parameters of the camera.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      /// <summary>
      /// The Unity camera component that will capture the <see cref="ImageTexture"/>.
      /// </summary>
      public Camera ImageCamera { get; protected set; }

      /// <summary>
      /// The correct image orientation.
      /// </summary>
      public virtual Quaternion ImageRotation { get; protected set; }

      /// <summary>
      /// The image ratio.
      /// </summary>
      public virtual float ImageRatio { get; protected set; }

      /// <summary>
      /// Allow to unflip the image if vertically flipped (use for mesh plane).
      /// </summary>
      public virtual Mesh ImageMesh { get; protected set; }

      /// <summary>
      /// Allow to unflip the image if vertically flipped (use for canvas).
      /// </summary>
      public virtual Rect ImageUvRectFlip { get; protected set; }

      /// <summary>
      /// Mirror front-facing camera's image horizontally to look more natural.
      /// </summary>
      public virtual Vector3 ImageScaleFrontFacing { get; protected set; }

      // Variables

      private Mat image;
      private bool imageHasBeenSet;

      // MonoBehaviour methods

      /// <summary>
      /// Configure the camera if <see cref="AutoStart"/> is true.
      /// </summary>
      private void Start()
      {
        Configured = false;
        Started = false;
        ImageUpdatedThisFrame = false;

        if (AutoStart)
        {
          Configure();
        }
      }

      private void Update()
      {
        image = null;
        imageHasBeenSet = false;

        UpdateCameraImage();
      }

      private void LateUpdate()
      {
        Undistord();

        if (imageHasBeenSet)
        {
          int imageDataSize = (int)(image.ElemSize() * image.Total());
          ImageTexture.LoadRawTextureData(image.data, imageDataSize);
          ImageTexture.Apply(false);
        }
      }

      // Methods

      /// <summary>
      /// Configure the camera and its properties.
      /// </summary>
      public abstract void Configure();

      /// <summary>
      /// Start the camera.
      /// </summary>
      public abstract void StartCamera();

      /// <summary>
      /// Stop the camera.
      /// </summary>
      public abstract void StopCamera();

      public void Undistord()
      {
        Mat undistordedImage;
        Imgproc.Undistord(Image, out undistordedImage, CameraParameters.CameraMatrix, CameraParameters.DistCoeffs);
        Image = undistordedImage;
      }

      protected abstract void UpdateCameraImage();

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
      /// Execute the <see cref="OnImageUpdated"/> action.
      /// </summary>
      protected void RaiseOnImageUpdated()
      {
        if (OnImageUpdated != null)
        {
          OnImageUpdated();
        }
      }
    }
  }

  /// \} aruco_unity_package
}
