using ArucoUnity.Plugin;
using ArucoUnity.Plugin.std;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public abstract class ArucoObjectDetector : ArucoObjectController
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera to use for the detection.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The parameters to use for the marker detection")]
      private ArucoDetectorParametersController detectorParametersController;

      // Events

      public delegate void CameraDeviceMakersDetectorAction();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event CameraDeviceMakersDetectorAction OnConfigured;

      // Properties

      public ArucoCamera ArucoCamera
      {
        get { return arucoCamera; }
        set
        {
          // Reset configuration
          Configured = false;

          // Unsubscribe from the previous ArucoCamera
          if (arucoCamera != null)
          {
            arucoCamera.OnStarted -= Configure;
          }

          // Subscribe to the new ArucoCamera
          arucoCamera = value;
          arucoCamera.OnStarted += Configure;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configure();
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
      public bool Configured { get; protected set; }

      /// <summary>
      /// Vector of the detected marker corners. Updated by <see cref="Detect"/>.
      /// </summary>
      public VectorVectorPoint2f MarkerCorners { get; protected set; }

      /// <summary>
      /// Vector of identifiers of the detected markers. Updated by <see cref="Detect"/>.
      /// </summary>
      public VectorInt MarkerIds { get; protected set; }

      /// <summary>
      /// Vector of the corners with not a correct identification. Updated by <see cref="Detect"/>.
      /// </summary>
      public VectorVectorPoint2f RejectedCandidateCorners { get; protected set; }

      // MonoBehaviour methods

      protected override void Start()
      {
        base.Start();

        ArucoCamera = arucoCamera;
        DetectorParameters = detectorParametersController.detectorParameters;
      }

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
          ArucoCamera.OnImageUpdated -= ArucoCameraImageUpdated;
        }
      }

      // Methods

      /// <summary>
      /// Detect the markers on the <see cref="ArucoObjectDetector.CameraImageTexture"/> and estimate their poses. Should be called during LateUpdate(),
      /// after the update of the CameraImageTexture.
      /// </summary>
      // TODO: detect in a separate thread for performances
      public void Detect()
      {
        if (!Configured)
        {
          return;
        }

        VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
        VectorInt markerIds;

        Functions.DetectMarkers(ArucoCamera.Image, Dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);
        MarkerCorners = markerCorners;
        RejectedCandidateCorners = rejectedCandidateCorners;
        MarkerIds = markerIds;
      }

      /// <summary>
      /// The configuration content of derived classes.
      /// </summary>
      protected abstract void PreConfigure();

      protected abstract void ArucoCameraImageUpdated();

      /// <summary>
      /// Configure the detection.
      /// </summary>
      private void Configure()
      {
        // Execute the configuration of derived classes
        Configured = false;
        PreConfigure();

        // Update the state and notify
        Configured = true;
        if (OnConfigured != null)
        {
          OnConfigured();
        }

        // Subscribe to ArucoCamera events
        ArucoCamera.OnImageUpdated += ArucoCameraImageUpdated;
      }
    }
  }

  /// \} aruco_unity_package
}