using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Base for any traked objects detection class.
    /// </summary>
    public abstract class TrackedObjectsDetector : MonoBehaviour
    {
      // Events

      public delegate void CameraDeviceMakersDetectorAction();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event CameraDeviceMakersDetectorAction OnConfigured;

      // Properties

      /// <summary>
      /// True when the detector is ready and configured.
      /// </summary>
      public bool Configured { get; protected set; }

      /// <summary>
      /// The dictionary to use for the detection.
      /// </summary>
      // TODO: move to TrackedObject class
      public Dictionary Dictionary { get; set; }

      /// <summary>
      /// The parameters to use for the detection.
      /// </summary>
      public DetectorParameters DetectorParameters { get; set; }

      /// <summary>
      /// The side length of the markers that will be detected (in meters).
      /// </summary>
      // TODO: move to TrackedObject class
      public float MarkerSideLength { get; set; }

      // TODO: doc
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
      /// Estimate the detected markers pose (position, rotation).
      /// </summary>
      public bool EstimatePose { get; set; }

      // TODO: inverse the ref
      public TrackedObjectsController TrackedObjectsController { get; set; }

      // Variables

      private ArucoCamera arucoCameraValue;

      // MonoBehaviour methods

      /// <summary>
      /// Subscribe to <see cref="ArucoCamera"/> and execute the configuration if the camera is already started.
      /// </summary>
      protected virtual void OnEnable()
      {
        if (ArucoCamera != null)
        {
          ArucoCamera.OnStarted += Configure;

          if (ArucoCamera.Started)
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
      /// Configure the detection.
      /// </summary>
      private void Configure()
      {
        Configured = false;

        PreConfigure();

        if (ArucoCamera.CameraParameters != null)
        {
          TrackedObjectsController.SetCamera(ArucoCamera);
          TrackedObjectsController.MarkerSideLength = MarkerSideLength;
        }
        else
        {
          EstimatePose = false;
        }

        // Update the state and notify
        Configured = true;
        if (OnConfigured != null)
        {
          OnConfigured();
        }
      }
    }
  }

  /// \} aruco_unity_package
}