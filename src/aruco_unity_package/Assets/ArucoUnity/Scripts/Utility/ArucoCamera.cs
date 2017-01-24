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
      public Camera CameraImage { get; protected set; }

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

      // MonoBehaviour methods

      /// <summary>
      /// Configure the camera if <see cref="AutoStart"/> is true.
      /// </summary>
      protected void Start()
      {
        Configured = false;
        Started = false;

        if (AutoStart)
        {
          Configure();
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
    }
  }

  /// \} aruco_unity_package
}
