using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using System.Collections.Generic;
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

      public delegate void ArucoObjectDetectorEventHandler();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event ArucoObjectDetectorEventHandler Configured = delegate { };

      // Properties

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
            arucoCamera.Started -= Configure;
          }

          // Subscribe to the new ArucoCamera
          arucoCamera = value;
          if (arucoCamera != null)
          {
            arucoCamera.Started += Configure;
            if (ArucoCamera.IsStarted)
            {
              Configure();
            }
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

      /// <summary>
      /// Vector of the detected marker corners on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
      /// </summary>
      public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] MarkerCorners { get; protected set; }

      /// <summary>
      /// Vector of identifiers of the detected markers on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
      /// </summary>
      public Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>[] MarkerIds { get; protected set; }

      /// <summary>
      /// Vector of the corners with not a correct identification on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
      /// </summary>
      public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] RejectedCandidateCorners { get; protected set; }

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();

        ArucoCamera = arucoCamera;
        DetectorParameters = detectorParametersController.DetectorParameters;
      }

      protected virtual void Start()
      {
        base.DictionaryAdded += ArucoObjectController_DictionaryAdded;
        base.DictionaryRemoved += ArucoObjectController_DictionaryRemoved;
      }

      /// <summary>
      /// Subscribe to <see cref="ArucoCamera"/> and execute the configuration if the camera is already started.
      /// </summary>
      protected virtual void OnEnable()
      {
        if (ArucoCamera != null)
        {
          ArucoCamera.Started += Configure;

          if (ArucoCamera.IsStarted)
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
        IsConfigured = false;

        if (ArucoCamera != null)
        {
          ArucoCamera.Started -= Configure;
          ArucoCamera.ImagesUpdated -= ArucoCameraImageUpdated;
        }
      }

      // Methods

      /// <summary>
      /// Detect the markers on each <see cref="ArucoCamera.Images"/>. Should be called during the OnImagesUpdated() event,
      /// after the update of the CameraImageTexture.
      /// </summary>
      // TODO: detect in a separate thread for performances
      public void Detect()
      {
        if (!IsConfigured)
        {
          return;
        }

        Mat[] cameraImages = ArucoCamera.Images;
        for (int cameraId = 0; cameraId < cameraImages.Length; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            Dictionary dictionary = arucoObjectDictionary.Key;
            VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
            VectorInt markerIds;

            Functions.DetectMarkers(cameraImages[cameraId], dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);
            MarkerCorners[cameraId][dictionary] = markerCorners;
            MarkerIds[cameraId][dictionary] = markerIds;
            RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
          }
        }
      }

      /// <summary>
      /// The configuration content of derived classes.
      /// </summary>
      protected abstract void PreConfigure();

      /// <summary>
      /// Update the camera images.
      /// </summary>
      protected abstract void ArucoCameraImageUpdated();

      protected virtual void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
      {
        if (IsConfigured)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
          {
            MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
            MarkerIds[cameraId].Add(dictionary, new VectorInt());
            RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          }
        }
      }

      protected virtual void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
      {
        if (IsConfigured)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
          {
            MarkerCorners[cameraId].Remove(dictionary);
            MarkerIds[cameraId].Remove(dictionary);
            RejectedCandidateCorners[cameraId].Remove(dictionary);
          }
        }
      }

      /// <summary>
      /// Configure the detection.
      /// </summary>
      private void Configure()
      {
        IsConfigured = false;

        // Initialize the properties
        int camerasNumber = ArucoCamera.ImageTextures.Length;
        MarkerCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[camerasNumber];
        MarkerIds = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>[camerasNumber];
        RejectedCandidateCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[camerasNumber];

        for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
        {
          MarkerCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
          MarkerIds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>();
          RejectedCandidateCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();

          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            Dictionary dictionary = arucoObjectDictionary.Key;

            MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
            MarkerIds[cameraId].Add(dictionary, new VectorInt());
            RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          }
        }

        // Execute the configuration of derived classes
        PreConfigure();

        // Update the state and notify
        IsConfigured = true;
        Configured();

        // Subscribe to ArucoCamera events
        ArucoCamera.ImagesUpdated += ArucoCameraImageUpdated;
      }
    }
  }

  /// \} aruco_unity_package
}