using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Base for any Aruco detection class.
    /// </summary>
    public abstract class ArucoDetector : MonoBehaviour
    {
      // Events

      public delegate void CameraDeviceMakersDetectorAction();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event CameraDeviceMakersDetectorAction OnConfigured;

      // Properties

      // State properties
      /// <summary>
      /// True when the detector is ready and configured.
      /// </summary>
      public bool Configured { get; protected set; }

      // Detection configuration properties
      /// <summary>
      /// The dictionary to use for the detection.
      /// </summary>
      public Dictionary Dictionary { get; set; }

      /// <summary>
      /// The parameters to use for the detection.
      /// </summary>
      public DetectorParameters DetectorParameters { get; set; }

      /// <summary>
      /// The side length of the markers that will be detected (in meters).
      /// </summary>
      public float MarkerSideLength { get; set; }

      public ArucoCamera ArucoCamera
      {
        get { return arucoCameraValue; }
        set
        {
          // Reset configuration
          Configured = false;

          // Unsubscribe from the previous ArucoCamera
          if (arucoCameraValue != null)
          {
            arucoCameraValue.OnStarted -= Configure;
          }

          // Subscribe to the new ArucoCamera
          arucoCameraValue = value;
          arucoCameraValue.OnStarted += Configure;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configure();
          }
        }
      }

      /// <summary>
      /// If <see cref="EstimatePose "/> or <see cref="CameraPlaneConfigured"/> is false, the CameraImageTexture will be displayed on this canvas.
      /// </summary>
      public ArucoCameraCanvasDisplay ArucoCameraCanvasDisplay { get; set; }

      // Pose estimation properties
      /// <summary>
      /// Estimate the detected markers pose (position, rotation).
      /// </summary>
      public bool EstimatePose { get; set; }

      public MarkerObjectsController MarkerObjectsController { get; set; }

      // Variables

      private ArucoCamera arucoCameraValue = null;

      // MonoBehaviour methods

      /// <summary>
      /// Subscribe to <see cref="ArucoCamera"/> and execute the configuration if the active camera device has already started.
      /// </summary>
      protected virtual void OnEnable()
      {
        if (ArucoCamera != null)
        {
          ArucoCamera.OnStarted += Configure;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configure();
          }
        }
      }

      /// <summary>
      /// Unsubscribe from <see cref="ArucoCamera"/>.
      /// </summary>
      protected virtual void OnDisable()
      {
        Configured = false;

        if (ArucoCamera != null)
        {
          ArucoCamera.OnStarted -= Configure;
        }
      }

      // Methods

      /// <summary>
      /// The configuration content of derived classes.
      /// </summary>
      protected abstract void PreConfigure();

      /// <summary>
      /// Configure the detection and the results display.
      /// </summary>
      private void Configure()
      {
        Configured = false;

        PreConfigure();

        // Configure the camera-plane group or configure the canvas
        if (ArucoCamera.CameraParameters != null)
        {
          MarkerObjectsController.SetCamera(ArucoCamera);
          MarkerObjectsController.MarkerSideLength = MarkerSideLength;
        }
        else
        {
          EstimatePose = false;
        }
        ArucoCameraCanvasDisplay.gameObject.SetActive(!EstimatePose);

        // Update the state and notify
        if (OnConfigured != null)
        {
          OnConfigured();
        }
        Configured = true;
      }
    }
  }

  /// \} aruco_unity_package
}