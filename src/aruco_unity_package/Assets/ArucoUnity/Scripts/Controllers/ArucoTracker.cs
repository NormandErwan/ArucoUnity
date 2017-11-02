using UnityEngine;
using ArucoUnity.Plugin;
using System.Collections.Generic;
using System.Threading;
using ArucoUnity.Objects;
using ArucoUnity.Controllers.ObjectTrackers;
using ArucoUnity.Controllers.Utility;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Detect ArUco objects, display detections and apply the estimated transform to associated gameObjects.
    /// 
    /// See the OpenCV documentation for more information about the marker detection: http://docs.opencv.org/3.2.0/d5/dae/tutorial_aruco_detection.html
    /// </summary>
    public class ArucoTracker : ArucoObjectsController
    {
      // Editor fields

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
      [Tooltip("Update the transforms of the detected ArUco objects.")]
      private bool updateTransforms = true;

      [SerializeField]
      [Tooltip("Update the transforms of the detected ArUco objects relative to this camera.")]
      private int transformsRelativeCameraId;

      // Properties

      /// <summary>
      /// Apply refine strategy to detect more markers using the <see cref="ArucoBoard"/> in the <see cref="ArucoObjectsController.ArucoObjects"/> list.
      /// </summary>
      public bool RefineDetectedMarkers { get { return refineDetectedMarkers; } set { refineDetectedMarkers = value; } }

      /// <summary>
      /// Display the detected markers in the <see cref="ArucoObjectDetector.CameraImageTexture"/>.
      /// </summary>
      public bool DrawDetectedMarkers { get { return drawDetectedMarkers; } set { drawDetectedMarkers = value; } }

      /// <summary>
      /// Display the rejected markers candidates.
      /// </summary>
      public bool DrawRejectedCandidates { get { return drawRejectedCandidates; } set { drawRejectedCandidates = value; } }

      /// <summary>
      /// Display the axes of the detected boards and diamonds.
      /// </summary>
      public bool DrawAxes { get { return drawAxes; } set { drawAxes = value; } }

      /// <summary>
      /// Display the markers of the detected ChArUco boards.
      /// </summary>
      public bool DrawDetectedCharucoMarkers { get { return drawDetectedCharucoMarkers; } set { drawDetectedCharucoMarkers = value; } }

      /// <summary>
      /// Display the detected diamonds.
      /// </summary>
      public bool DrawDetectedDiamonds { get { return drawDetectedDiamonds; } set { drawDetectedDiamonds = value; } }

      /// <summary>
      /// Update the transforms of the detected ArUco objects.
      /// </summary>
      public bool UpdateTransforms { get { return updateTransforms; } set { updateTransforms = value; } }

      /// <summary>
      /// Update the transforms of the detected ArUco objects relative to this camera.
      /// </summary>
      public int TransformsRelativeCameraId { get { return transformsRelativeCameraId; } set { transformsRelativeCameraId = value; } }

      /// <summary>
      /// The tracker of ArUco markers used.
      /// </summary>
      public ArucoMarkerTracker MarkerTracker { get; protected set; }

      // Variables

      protected Dictionary<System.Type, ArucoObjectTracker> additionalTrackers;

      protected bool arucoCameraImagesUpdated;
      protected Cv.Mat[] trackingImages;
      protected byte[][] trackingImagesData;
      protected byte[][] arucoCameraImageCopyData;
      protected Thread trackingThread;
      protected Mutex trackingMutex;
      protected System.Exception trackingException;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the trackers list and the tracking thread and susbcribe to events from ArucoObjectController for every ArUco object added or removed.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();

        // Initialize the trackers
        MarkerTracker = new ArucoMarkerTracker();
        additionalTrackers = new Dictionary<System.Type, ArucoObjectTracker>()
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
          catch (System.Exception e)
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
      /// Unsuscribe from ArucoObjectController events, and abort the tracking thread.
      /// </summary>
      protected override void OnDestroy()
      {
        base.OnDestroy();
        ArucoObjectAdded -= ArucoObjectsController_ArucoObjectAdded;
        ArucoObjectRemoved -= ArucoObjectsController_ArucoObjectRemoved;
      }

      // ArucoObjectController methods

      /// <summary>
      /// Activate the tracker associated with the <paramref name="arucoObject"/> and configure its gameObject.
      /// </summary>
      /// <param name="arucoObject">The added ArUco object.</param>
      protected virtual void ArucoObjectsController_ArucoObjectAdded(ArucoObject arucoObject)
      {
        if (arucoObject.GetType() != typeof(ArucoMarker))
        {
          ArucoObjectTracker tracker = null;
          if (!additionalTrackers.TryGetValue(arucoObject.GetType(), out tracker))
          {
            throw new System.ArgumentException("No tracker found for the type '" + arucoObject.GetType() + "'.", "arucoObject");
          }
          else if (!tracker.IsActivated)
          {
            tracker.Activate(this);
          }
        }
      }

      /// <summary>
      /// Deactivate the tracker associated with the <paramref name="arucoObject"/> if it was the last one of this type.
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

      // ArucoObjectDetector methods

      /// <summary>
      /// Initialize the properties, the ArUco object list, and the tracking images.
      /// </summary>
      protected override void PreConfigure()
      {
        trackingImages = new Cv.Mat[ArucoCamera.CameraNumber];
        trackingImagesData = new byte[ArucoCamera.CameraNumber][];
        arucoCameraImageCopyData = new byte[ArucoCamera.CameraNumber][];
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          arucoCameraImageCopyData[cameraId] = new byte[ArucoCamera.ImageDataSizes[cameraId]];
          trackingImagesData[cameraId] = new byte[ArucoCamera.ImageDataSizes[cameraId]];

          Texture2D imageTexture = ArucoCamera.ImageTextures[cameraId];
          trackingImages[cameraId] = new Cv.Mat(imageTexture.height, imageTexture.width, ArucoCamera.ImageType(imageTexture.format));
          trackingImages[cameraId].DataByte = trackingImagesData[cameraId];
        }

        MarkerTracker.Activate(this);
      }

      /// <summary>
      /// Start the tracking.
      /// </summary>
      protected override void StartDetector()
      {
        base.StartDetector();
        arucoCameraImagesUpdated = false;
        trackingThread.Start();
      }

      /// <summary>
      /// Draw the results of the detection and place each detected ArUco object on the <see cref="ArucoObjectsController.ArucoObjects"/> list, 
      /// according to the results of the tracking thread and re-throw the tracking thread exceptions.
      /// </summary>
      protected override void ArucoCamera_ImagesUpdated()
      {
        if (IsConfigured && IsStarted)
        {
          System.Exception e = null;

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
                  System.Array.Copy(ArucoCamera.ImageDatas[cameraId], arucoCameraImageCopyData[cameraId], ArucoCamera.ImageDataSizes[cameraId]);
                }

                // Copy back the previous images use by the tracking thread (for synchronization of the images with the tranforms of the tracked objects
                for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
                {
                  System.Array.Copy(trackingImagesData[cameraId], ArucoCamera.ImageDatas[cameraId], ArucoCamera.ImageDataSizes[cameraId]);
                }

                // Update the transforms of the tracked objects
                if (UpdateTransforms)
                {
                  DeactivateArucoObjects();
                  Place();
                }
              }
            }
          }
          trackingMutex.ReleaseMutex();

          // Stop if there was an exception in the tracking thread
          if (e != null)
          {
            StopDetector();
            throw e;
          }
        }
      }

      // Methods

      /// <summary>
      /// Hide all the ArUco objects.
      /// </summary>
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

      /// <summary>
      /// Detect the ArUco objects for the <see cref="ArucoCamera"/> camera system, on a set of custom images.
      /// </summary>
      /// <param name="images">The images set.</param>
      public void Detect(Cv.Mat[] images)
      {
        if (!IsConfigured)
        {
          return;
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

      /// <summary>
      /// Detect the ArUco objects on the current images of the <see cref="ArucoCamera"/> camera system.
      /// </summary>
      public void Detect()
      {
        Detect(ArucoCamera.Images);
      }

      /// <summary>
      /// Estimate the gameObject's transform of each detected ArUco object of the <see cref="ArucoCamera"/> camera system.
      /// </summary>
      public void EstimateTransforms()
      {
        if (!IsConfigured)
        {
          return;
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

      /// <summary>
      /// Draw the detected ArUco objects for the <see cref="ArucoCamera"/> camera system, on a set of custom images.
      /// </summary>
      /// <param name="images">The images set.</param>
      public void Draw(Cv.Mat[] images)
      {
        if (!IsConfigured)
        {
          return;
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

      /// <summary>
      /// Draw the detected ArUco objects of the <see cref="ArucoCamera"/> camera system on the current images of these cameras.
      /// </summary>
      public void Draw()
      {
        Draw(ArucoCamera.Images);
      }

      /// <summary>
      /// Place and orient the detected ArUco objects relative to the camera with the <see cref="TransformsRelativeCameraId"/> id.
      /// </summary>
      public void Place()
      {
        if (!IsConfigured)
        {
          return;
        }

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          MarkerTracker.Place(TransformsRelativeCameraId, arucoObjectDictionary.Key);
          foreach (var tracker in additionalTrackers)
          {
            if (tracker.Value.IsActivated)
            {
              tracker.Value.Place(TransformsRelativeCameraId, arucoObjectDictionary.Key);
            }
          }
        }
      }

      /// <summary>
      /// Detect and estimate the transforms of ArUco objects on the <see cref="ArucoObjectsController.ArucoObjects"/> list. Executed on a separated
      /// tracking thread.
      /// </summary>
      protected void Track()
      {
        if (arucoCameraImagesUpdated)
        {
          arucoCameraImagesUpdated = false;
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            System.Array.Copy(arucoCameraImageCopyData[cameraId], trackingImagesData[cameraId], ArucoCamera.ImageDataSizes[cameraId]);
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