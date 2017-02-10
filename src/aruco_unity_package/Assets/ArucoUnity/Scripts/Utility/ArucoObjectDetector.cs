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
      private ArucoDetectorParametersController detectorParametersController;

      // Events

      public delegate void ArucoObjectDetectorEventHandler();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event ArucoObjectDetectorEventHandler Configured = delegate { };

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
      /// True when the detector is ready and configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties.
      /// </summary>
      protected virtual void Awake()
      {
        ArucoCamera = arucoCamera;
        DetectorParameters = detectorParametersController.DetectorParameters;
      }

      // Methods

      /// <summary>
      /// The configuration content of derived classes.
      /// </summary>
      protected abstract void PreConfigure();

      /// <summary>
      /// Update the camera images.
      /// </summary>
      protected abstract void ArucoCameraImageUpdated();

      /// <summary>
      /// Configure the detection.
      /// </summary>
      private void Configure()
      {
        IsConfigured = false;

        // Execute the configuration of derived classes
        PreConfigure();

        // Update the state and notify
        IsConfigured = true;
        Configured();
      }
    }
  }

  /// \} aruco_unity_package
}