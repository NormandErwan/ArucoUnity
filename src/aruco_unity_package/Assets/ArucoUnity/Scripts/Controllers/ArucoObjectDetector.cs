using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public abstract class ArucoObjectDetector : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use for the detection.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The parameters to use for the marker detection.")]
      private DetectorParametersController detectorParametersController;

      [SerializeField]
      [Tooltip("Start automatically when the configuration is done. Call alternatively StartDetector().")]
      private bool autoStart = true;

      // Events

      public delegate void ArucoObjectDetectorEventHandler();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event ArucoObjectDetectorEventHandler Configured = delegate { };

      /// <summary>
      /// Executed when the detector is started.
      /// </summary>
      public event ArucoObjectDetectorEventHandler Started = delegate { };

      /// <summary>
      /// Executed when the detector is stopped.
      /// </summary>
      public event ArucoObjectDetectorEventHandler Stopped = delegate { };

      // Properties

      /// <summary>
      /// The camera system to use for the detection.
      /// </summary>
      public ArucoCamera ArucoCamera
      {
        get { return arucoCamera; }
        set
        {
          // Reset configuration
          IsConfigured = false;

          // Unsubscribe from the previous ArucoCamera
          if (arucoCamera != null)
          {
            arucoCamera.ImagesUpdated -= ArucoCameraImageUpdated;
            arucoCamera.Started -= Configure;
          }

          // Subscribe to the new ArucoCamera
          arucoCamera = value;
          if (arucoCamera != null)
          {
            if (ArucoCamera.IsStarted)
            {
              Configure();
            }
            arucoCamera.Started += Configure;
            arucoCamera.ImagesUpdated += ArucoCameraImageUpdated;
          }
        }
      }

      /// <summary>
      /// The parameters to use for the detection.
      /// </summary>
      public DetectorParameters DetectorParameters { get; set; }

      /// <summary>
      /// Start automatically when the configuration is done. Call alternatively StartDetection().
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// True when the detector is ready and configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      /// <summary>
      /// True when the detector is started.
      /// </summary>
      public bool IsStarted { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;

        ArucoCamera = arucoCamera;
        DetectorParameters = detectorParametersController.DetectorParameters;
      }

      protected virtual void OnDestroy()
      {
        StopDetector();
      }

      // Methods

      /// <summary>
      /// The configuration content of derived classes.
      /// </summary>
      protected abstract void PreConfigure();

      /// <summary>
      /// Called when the camera images has been updated.
      /// </summary>
      protected abstract void ArucoCameraImageUpdated();

      /// <summary>
      /// Start the detector.
      /// </summary>
      protected virtual void StartDetector()
      {
        if (!IsConfigured || IsStarted)
        {
          return;
        }

        IsStarted = true;
        Started();
      }

      /// <summary>
      /// Stop the detector.
      /// </summary>
      protected virtual void StopDetector()
      {
        if (!IsConfigured || !IsStarted)
        {
          return;
        }

        IsStarted = false;
        Stopped();
      }

      /// <summary>
      /// Configure the detector. It needs to be stopped before.
      /// </summary>
      private void Configure()
      {
        if (IsStarted)
        {
          return;
        }

        IsConfigured = false;

        // Execute the configuration of derived classes
        PreConfigure();

        // Update the state and notify
        IsConfigured = true;
        Configured();

        // AutoStart
        if (AutoStart)
        {
          StartDetector();
        }
      }
    }
  }

  /// \} aruco_unity_package
}