using ArucoUnity.Controllers.CameraDisplays;
using ArucoUnity.Controllers.CameraUndistortions;
using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using ArucoUnity.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using ArucoUnity.Cameras;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.ObjectTrackers
  {
    /// <summary>
    /// Detects <see cref="ArucoObject"/>, displays detections and applies the estimated transforms to gameObjects associated to the ArUco objects.
    /// 
    /// See the OpenCV documentation for more information about the marker detection: http://docs.opencv.org/3.2.0/d5/dae/tutorial_aruco_detection.html
    /// </summary>
    public abstract class ArucoObjectsGenericTracker<T> : ArucoObjectsController<T>, IArucoObjectsTracker where T : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      [SerializeField]
      [Tooltip("Apply refine strategy to detect more markers using the boards in the Aruco Object list.")]
      private bool refineDetectedMarkers = true;

      [SerializeField]
      [Tooltip("Display the detected markers in the CameraImageTexture.")]
      private bool drawDetectedMarkers = true;

      [SerializeField]
      [Tooltip("Display the rejected markers candidates.")]
      private bool drawRejectedCandidates = false;

      [SerializeField]
      [Tooltip("Display the axis of the detected boards and diamonds.")]
      private bool drawAxes = true;

      [SerializeField]
      [Tooltip("Display the markers of the detected ChArUco boards.")]
      private bool drawDetectedCharucoMarkers = true;

      [SerializeField]
      [Tooltip("Display the detected diamonds.")]
      private bool drawDetectedDiamonds = true;

      [SerializeField]
      [Tooltip("Update automatically the transforms of the detected ArUco objects.")]
      private bool autoUpdateTransforms = true;

      [SerializeField]
      [Tooltip("Update the transforms of the detected ArUco objects relative to this camera.")]
      private int transformsRelativeCameraId;

      // IArucoObjectsTracker properties

      IArucoCameraDisplay IArucoObjectsTracker.ArucoCameraDisplay { get { return ArucoCameraDisplay; } }
      IArucoCameraUndistortion IArucoObjectsTracker.ArucoCameraUndistortion { get { return arucoCameraUndistortion; } }
      public bool RefineDetectedMarkers { get { return refineDetectedMarkers; } set { refineDetectedMarkers = value; } }
      public bool DrawDetectedMarkers { get { return drawDetectedMarkers; } set { drawDetectedMarkers = value; } }
      public bool DrawRejectedCandidates { get { return drawRejectedCandidates; } set { drawRejectedCandidates = value; } }
      public bool DrawAxes { get { return drawAxes; } set { drawAxes = value; } }
      public bool DrawDetectedCharucoMarkers { get { return drawDetectedCharucoMarkers; } set { drawDetectedCharucoMarkers = value; } }
      public bool DrawDetectedDiamonds { get { return drawDetectedDiamonds; } set { drawDetectedDiamonds = value; } }
      public bool AutoUpdateTransforms { get { return autoUpdateTransforms; } set { autoUpdateTransforms = value; } }
      public ArucoMarkerTracker MarkerTracker { get; protected set; }

      // Properties

      /// <summary>
      /// Gets or sets the camera display associated with the ArucoCamera.
      /// </summary>
      public abstract ArucoCameraGenericDisplay<T> ArucoCameraDisplay { get; }

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      public int TransformsRelativeCameraId { get { return transformsRelativeCameraId; } set { transformsRelativeCameraId = value; } }

      // Variables

      protected Dictionary<Type, ArucoObjectTracker> additionalTrackers;

      protected bool arucoCameraImagesUpdated;
      protected Cv.Mat[] trackingImages;
      protected byte[][] trackingImagesData;
      protected byte[][] arucoCameraImageCopyData;
      protected Thread trackingThread;
      protected Mutex trackingMutex;
      protected Exception trackingException;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the trackers list and the tracking thread and susbcribe to events from ArucoObjectController for every ArUco object added or removed.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();

        // Initialize the trackers
        MarkerTracker = new ArucoMarkerTracker();
        additionalTrackers = new Dictionary<Type, ArucoObjectTracker>()
        {
          { typeof(ArucoGridBoard), new ArucoGridBoardTracker() },
          { typeof(ArucoCharucoBoard), new ArucoCharucoBoardTracker() },
          { typeof(ArucoDiamond), new ArucoDiamondTracker() }
        };

        // Initialize the tracking thread
        trackingMutex = new Mutex();
        trackingThread = new Thread(() =>
        {
          try
          {
            while (IsConfigured && IsStarted)
            {
              trackingMutex.WaitOne();
              Track();
              trackingMutex.ReleaseMutex();
            }
          }
          catch (Exception e)
          {
            trackingException = e;
            trackingMutex.ReleaseMutex();
          }
        });

        // Susbcribe to events from ArucoObjectController
        ArucoObjectAdded += ArucoObjectsController_ArucoObjectAdded;
        ArucoObjectRemoved += ArucoObjectsController_ArucoObjectRemoved;
      }

      /// <summary>
      /// Unsuscribes from ArucoObjectController events, and abort the tracking thread.
      /// </summary>
      protected override void OnDestroy()
      {
        base.OnDestroy();
        ArucoObjectAdded -= ArucoObjectsController_ArucoObjectAdded;
        ArucoObjectRemoved -= ArucoObjectsController_ArucoObjectRemoved;
      }

      // ArucoCameraController methods

      /// <summary>
      /// Initializes the properties, the ArUco object list, and the tracking images.
      /// </summary>
      public override void Configure()
      {
        base.Configure();

        trackingImages = new Cv.Mat[ArucoCamera.CameraNumber];
        trackingImagesData = new byte[ArucoCamera.CameraNumber][];
        arucoCameraImageCopyData = new byte[ArucoCamera.CameraNumber][];
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          arucoCameraImageCopyData[cameraId] = new byte[ArucoCamera.ImageDataSizes[cameraId]];
          trackingImagesData[cameraId] = new byte[ArucoCamera.ImageDataSizes[cameraId]];

          Texture2D imageTexture = ArucoCamera.ImageTextures[cameraId];
          trackingImages[cameraId] = new Cv.Mat(imageTexture.height, imageTexture.width, CvMatExtensions.ImageType(imageTexture.format));
          trackingImages[cameraId].DataByte = trackingImagesData[cameraId];
        }

        OnConfigured();
      }

      /// <summary>
      /// Starts the tracking.
      /// </summary>
      public override void StartController()
      {
        base.StartController();

        MarkerTracker.Activate(this, ArucoCamera);

        arucoCameraImagesUpdated = false;
        ArucoCamera.ImagesUpdated += ArucoCamera_ImagesUpdated;
        trackingThread.Start();

        OnStarted();
      }

      /// <summary>
      /// Stops the tracking.
      /// </summary>
      public override void StopController()
      {
        base.StopController();

        MarkerTracker.Deactivate();

        OnStopped();
      }

      /// <summary>
      /// Draws the results of the detection and place each detected ArUco object on the <see cref="ArucoObjectsController.ArucoObjects"/> list, 
      /// according to the results of the tracking thread and re-throw the tracking thread exceptions.
      /// </summary>
      protected virtual void ArucoCamera_ImagesUpdated()
      {
        if (IsConfigured && IsStarted)
        {
          Exception e = null;

          // Check for exception in the tracking thread, or transfer images data with the tracking thread
          trackingMutex.WaitOne();
          {
            if (trackingException != null)
            {
              e = trackingException;
              trackingException = null;
            }
            else
            {
              if (!arucoCameraImagesUpdated)
              {
                arucoCameraImagesUpdated = true;

                // Copy the current images of the camera for the tracking thread
                for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
                {
                  Array.Copy(ArucoCamera.ImageDatas[cameraId], arucoCameraImageCopyData[cameraId], ArucoCamera.ImageDataSizes[cameraId]);
                }

                // Copy back the previous images use by the tracking thread (for synchronization of the images with the tranforms of the tracked objects
                for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
                {
                  Array.Copy(trackingImagesData[cameraId], ArucoCamera.ImageDatas[cameraId], ArucoCamera.ImageDataSizes[cameraId]);
                }

                // Update the transforms of the tracked objects
                if (AutoUpdateTransforms)
                {
                  DeactivateArucoObjects();
                  UpdateTransforms();
                }
              }
            }
          }
          trackingMutex.ReleaseMutex();

          // Stop if there was an exception in the tracking thread
          if (e != null)
          {
            StopController();
            throw e;
          }
        }
      }

      // ArucoObjectController methods

      /// <summary>
      /// Activates the tracker associated with the <paramref name="arucoObject"/> and configure its gameObject.
      /// </summary>
      /// <param name="arucoObject">The added ArUco object.</param>
      protected virtual void ArucoObjectsController_ArucoObjectAdded(ArucoObject arucoObject)
      {
        if (arucoObject.GetType() != typeof(ArucoMarker))
        {
          ArucoObjectTracker tracker = null;
          if (!additionalTrackers.TryGetValue(arucoObject.GetType(), out tracker))
          {
            throw new ArgumentException("No tracker found for the type '" + arucoObject.GetType() + "'.", "arucoObject");
          }
          else if (!tracker.IsActivated)
          {
            tracker.Activate(this, ArucoCamera);
          }
        }
      }

      /// <summary>
      /// Deactivates the tracker associated with the <paramref name="arucoObject"/> if it was the last one of this type.
      /// </summary>
      /// <param name="arucoObject">The removed</param>
      protected virtual void ArucoObjectsController_ArucoObjectRemoved(ArucoObject arucoObject)
      {
        ArucoObjectTracker tracker = null;
        if (arucoObject.GetType() == typeof(ArucoMarker) || !additionalTrackers.TryGetValue(arucoObject.GetType(), out tracker))
        {
          return;
        }

        if (tracker.IsActivated)
        {
          bool deactivateTracker = true;

          // Try to find at leat one object of the same type as arucoObject
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            foreach (var arucoObject2 in arucoObjectDictionary.Value)
            {
              if (arucoObject2.GetType() == arucoObject.GetType())
              {
                deactivateTracker = false;
                break;
              }
            }
            if (!deactivateTracker)
            {
              break;
            }
          }

          if (deactivateTracker)
          {
            tracker.Deactivate();
          }
        }
      }

      // IArucoObjectsTracker Methods

      public void DeactivateArucoObjects()
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            arucoObject.Value.gameObject.SetActive(false);
          }
        }
      }

      public void Detect(Cv.Mat[] images)
      {
        if (!IsConfigured)
        {
          throw new Exception("The tracker must be configured before tracking ArUco objects.");
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            MarkerTracker.Detect(cameraId, arucoObjectDictionary.Key, images[cameraId]);
            foreach (var tracker in additionalTrackers)
            {
              if (tracker.Value.IsActivated)
              {
                tracker.Value.Detect(cameraId, arucoObjectDictionary.Key, images[cameraId]);
              }
            }
          }
        }
      }

      public void Detect()
      {
        Detect(ArucoCamera.Images);
      }

      public void Draw(Cv.Mat[] images)
      {
        if (!IsConfigured)
        {
          throw new Exception("The tracker must be configured before tracking ArUco objects.");
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            MarkerTracker.Draw(cameraId, arucoObjectDictionary.Key, images[cameraId]);
            foreach (var tracker in additionalTrackers)
            {
              if (tracker.Value.IsActivated)
              {
                tracker.Value.Draw(cameraId, arucoObjectDictionary.Key, images[cameraId]);
              }
            }
          }
        }
      }

      public void Draw()
      {
        Draw(ArucoCamera.Images);
      }

      public void EstimateTransforms()
      {
        if (!IsConfigured)
        {
          throw new Exception("The tracker must be configured before tracking ArUco objects.");
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            MarkerTracker.EstimateTransforms(cameraId, arucoObjectDictionary.Key);
            foreach (var tracker in additionalTrackers)
            {
              if (tracker.Value.IsActivated)
              {
                tracker.Value.EstimateTransforms(cameraId, arucoObjectDictionary.Key);
              }
            }
          }
        }
      }

      public void UpdateTransforms()
      {
        if (!IsConfigured)
        {
          throw new Exception("The tracker must be configured before tracking ArUco objects.");
        }

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          MarkerTracker.UpdateTransforms(TransformsRelativeCameraId, arucoObjectDictionary.Key);
          foreach (var tracker in additionalTrackers)
          {
            if (tracker.Value.IsActivated)
            {
              tracker.Value.UpdateTransforms(TransformsRelativeCameraId, arucoObjectDictionary.Key);
            }
          }
        }
      }

      // Methods

      /// <summary>
      /// Detects and estimates the transforms of ArUco objects on the <see cref="ArucoObjectsController.ArucoObjects"/> list. Executed on a
      /// separated tracking thread.
      /// </summary>
      protected void Track()
      {
        if (arucoCameraImagesUpdated)
        {
          arucoCameraImagesUpdated = false;
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            Array.Copy(arucoCameraImageCopyData[cameraId], trackingImagesData[cameraId], ArucoCamera.ImageDataSizes[cameraId]);
          }

          Detect(trackingImages);
          EstimateTransforms();
          Draw(trackingImages);
        }
      }
    }
  }

  /// \} aruco_unity_package
}