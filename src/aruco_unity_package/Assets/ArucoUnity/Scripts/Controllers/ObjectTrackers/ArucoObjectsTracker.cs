using ArucoUnity.Controllers.CameraDisplays;
using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using ArucoUnity.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraUndistortions;

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
    public class ArucoObjectsTracker : ArucoObjectsController, IArucoObjectsTracker
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      [SerializeField]
      [Tooltip("The optional camera display associated with the ArucoCamera.")]
      private ArucoCameraDisplay arucoCameraDisplay;

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

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // IArucoObjectsTracker properties

      public IArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } }
      public IArucoCameraDisplay ArucoCameraDisplay { get { return arucoCameraDisplay; } }
      public bool RefineDetectedMarkers { get { return refineDetectedMarkers; } set { refineDetectedMarkers = value; } }
      public bool DrawDetectedMarkers { get { return drawDetectedMarkers; } set { drawDetectedMarkers = value; } }
      public bool DrawRejectedCandidates { get { return drawRejectedCandidates; } set { drawRejectedCandidates = value; } }
      public bool DrawAxes { get { return drawAxes; } set { drawAxes = value; } }
      public bool DrawDetectedCharucoMarkers { get { return drawDetectedCharucoMarkers; } set { drawDetectedCharucoMarkers = value; } }
      public bool DrawDetectedDiamonds { get { return drawDetectedDiamonds; } set { drawDetectedDiamonds = value; } }
      public ArucoMarkerTracker MarkerTracker { get; protected set; }

      // Variables

      protected Dictionary<Type, ArucoObjectTracker> additionalTrackers;

      protected bool arucoCameraImagesUpdated;
      protected Cv.Mat[] trackingImages;
      protected byte[][] trackingImagesData;
      protected byte[][] arucoCameraImageCopyData;
      protected Thread trackingThread;
      protected Mutex trackingMutex = new Mutex();
      protected Exception trackingException;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the trackers list.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();
        
        MarkerTracker = new ArucoMarkerTracker();
        additionalTrackers = new Dictionary<Type, ArucoObjectTracker>()
        {
          { typeof(ArucoGridBoard), new ArucoGridBoardTracker() },
          { typeof(ArucoCharucoBoard), new ArucoCharucoBoardTracker() },
          { typeof(ArucoDiamond), new ArucoDiamondTracker() }
        };
      }

      // ArucoCameraController methods

      /// <summary>
      /// Setups controller dependencies and initializes the tracking.
      /// </summary>
      public override void Configure()
      {
        base.Configure();

        // Setup controller dependencies
        ControllerDependencies.Add(ArucoCameraUndistortion);
        if (ArucoCameraDisplay != null)
        {
          ControllerDependencies.Add(ArucoCameraDisplay);
        }

        // Initialize the tracking
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
      /// Activates the trackers, susbcribes to the <see cref="ArucoObjectsController{T}.ArucoObjectAdded"/> and
      /// <see cref="ArucoObjectsController{T}.ArucoObjectRemoved"/> events and starts the tracking thread.
      /// </summary>
      public override void StartController()
      {
        base.StartController();

        // Activate the trackers
        MarkerTracker.Activate(this);
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            ArucoObjectsController_ArucoObjectAdded(arucoObject.Value);
          }
        }

        // Subscribes to ArucoObjectsController and ArucoCamera events
        ArucoObjectAdded += ArucoObjectsController_ArucoObjectAdded;
        ArucoObjectRemoved += ArucoObjectsController_ArucoObjectRemoved;

        arucoCameraImagesUpdated = false;
        ArucoCamera.ImagesUpdated += ArucoCamera_ImagesUpdated;

        // Start the tracking thread
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
        trackingThread.Start();

        OnStarted();
      }

      /// <summary>
      /// Unsuscribes from ArucoObjectController events, deactivates the trackers and abort the tracking thread and stops the tracking thread.
      /// </summary>
      public override void StopController()
      {
        base.StopController();

        ArucoObjectAdded -= ArucoObjectsController_ArucoObjectAdded;
        ArucoObjectRemoved -= ArucoObjectsController_ArucoObjectRemoved;

        MarkerTracker.Deactivate();
        foreach (var tracker in additionalTrackers)
        {
          if (tracker.Value.IsActivated)
          {
            tracker.Value.Deactivate();
          }
        }

        OnStopped();
      }

      /// <summary>
      /// Draws the results of the detection and place each detected ArUco object according to the results of the tracking thread and re-throw the
      /// tracking thread exceptions.
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
                DeactivateArucoObjects();
                if (ArucoCameraDisplay != null)
                {
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
            tracker.Activate(this);
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

        ExecuteOnActivatedTrackers((tracker, cameraId, dictionary) =>
        {
          tracker.Detect(cameraId, dictionary, images[cameraId]);
        });
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

        ExecuteOnActivatedTrackers((tracker, cameraId, dictionary) =>
        {
          tracker.Draw(cameraId, dictionary, images[cameraId]);
        });
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

        ExecuteOnActivatedTrackers((tracker, cameraId, dictionary) =>
        {
          tracker.EstimateTransforms(cameraId, dictionary);
        });
      }

      public void UpdateTransforms()
      {
        if (!IsConfigured)
        {
          throw new Exception("The tracker must be configured before tracking ArUco objects.");
        }

        ExecuteOnActivatedTrackers((tracker, cameraId, dictionary) =>
        {
          tracker.UpdateTransforms(cameraId, dictionary);
        });
      }

      // Methods

      /// <summary>
      /// Detects and estimates the transforms of the detected ArUco objects. Executed on a separated tracking thread.
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

      /// <summary>
      /// Executes an <paramref name="actionOnTracker"/> on all the activated <see cref="ArucoObjectTracker"/>.
      /// </summary>
      protected void ExecuteOnActivatedTrackers(Action<ArucoObjectTracker, int, Aruco.Dictionary> actionOnTracker)
      {
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            actionOnTracker(MarkerTracker, cameraId, arucoObjectDictionary.Key);
            foreach (var tracker in additionalTrackers)
            {
              if (tracker.Value.IsActivated)
              {
                actionOnTracker(tracker.Value, cameraId, arucoObjectDictionary.Key);
              }
            }
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}