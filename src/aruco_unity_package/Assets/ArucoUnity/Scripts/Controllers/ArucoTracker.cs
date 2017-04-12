using UnityEngine;
using ArucoUnity.Plugin;
using System.Collections.Generic;
using System.Threading;
using System.Collections;
using ArucoUnity.Objects;
using ArucoUnity.Controllers.ObjectTrackers;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Detect markers, display results and place game objects on the detected markers transform.
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
      [Tooltip("Estimate the detected markers pose (position, rotation).")]
      private bool estimateTransforms = true;

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
      /// Estimate the detected markers transform.
      /// </summary>
      public bool EstimateTransforms { get { return estimateTransforms; } set { estimateTransforms = value; } }

      public ArucoMarkerTracker MarkerTracker { get; protected set; }

      // Variables

      protected Dictionary<System.Type, ArucoObjectTracker> additionalTrackers;
      private bool arucoCameraImageUpdated;
      private Thread trackingThread;
      private Mutex trackingMutex;
      private System.Exception trackingException;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the trackers list.
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
            while (IsStarted)
            {
              Track();
            }
          }
          catch (System.Exception e)
          {
            trackingMutex.WaitOne();
            trackingException = e;
            trackingMutex.ReleaseMutex();
          }
        });
      }

      /// <summary>
      /// Susbcribe to events from ArucoObjectController for every ArUco object added or removed.
      /// </summary>
      protected override void Start()
      {
        base.Start();
        base.ArucoObjectAdded += ArucoObjectController_ArucoObjectAdded;
        base.ArucoObjectRemoved += ArucoObjectController_ArucoObjectRemoved;
      }

      /// <summary>
      /// Unsuscribe from ArucoObjectController events, and abort the tracking thread.
      /// </summary>
      protected override void OnDestroy()
      {
        base.OnDestroy();
        base.ArucoObjectAdded -= ArucoObjectController_ArucoObjectAdded;
        base.ArucoObjectRemoved -= ArucoObjectController_ArucoObjectRemoved;
      }

      // ArucoObjectController methods

      /// <summary>
      /// Suscribe to the property events of an ArUco object, and hide its gameObject since it has not been detected yet.
      /// </summary>
      /// <param name="arucoObject">The new ArUco object to suscribe.</param>
      protected virtual void ArucoObjectController_ArucoObjectAdded(ArucoObject arucoObject)
      {
        ArucoObjectTracker tracker = null;
        if (arucoObject.GetType() != typeof(ArucoMarker) && !additionalTrackers.TryGetValue(arucoObject.GetType(), out tracker))
        {
          // TODO: exception
          Debug.LogError("No tracker found for the type '" + arucoObject.GetType() + "'. Removing the object '" + arucoObject.gameObject.name +
            "' from the tracking list.");
          Remove(arucoObject);
          return;
        }

        if (tracker != null && !tracker.IsActivated)
        {
          tracker.Activate(this);
        }

        arucoObject.gameObject.SetActive(false);
      }

      protected virtual void ArucoObjectController_ArucoObjectRemoved(ArucoObject arucoObject)
      {
        // TODO
      }

      // ArucoObject methods

      /// <summary>
      /// Before the ArUco object's properties will be updated, restore the game object's scale of this object.
      /// </summary>
      /// <param name="arucoObject"></param>
      protected override void ArucoObject_PropertyUpdating(ArucoObject arucoObject)
      {
        base.ArucoObject_PropertyUpdating(arucoObject);
        if (arucoObject.GetType() == typeof(ArucoMarker))
        {
          MarkerTracker.RestoreGameObjectScale(arucoObject);
        }
        else
        {
          additionalTrackers[arucoObject.GetType()].RestoreGameObjectScale(arucoObject);
        }
      }

      /// <summary>
      /// Adjust the game object's scale of the ArUco object according to its MarkerSideLength property.
      /// </summary>
      /// <param name="arucoObject"></param>
      protected override void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
      {
        base.ArucoObject_PropertyUpdated(arucoObject);
        if (arucoObject.GetType() == typeof(ArucoMarker))
        {
          MarkerTracker.AdjustGameObjectScale(arucoObject);
        }
        else
        {
          additionalTrackers[arucoObject.GetType()].AdjustGameObjectScale(arucoObject);
        }
      }

      // ArucoObjectDetector methods

      /// <summary>
      /// Initialize the properties, the ArUco object list, and the tracking.
      /// </summary>
      protected override void PreConfigure()
      {
        if (ArucoCamera.CameraParameters == null)
        {
          EstimateTransforms = false;
        }

        // Trackers configuration
        MarkerTracker.Activate(this);
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          // List the aruco objects to delete from the list
          List<ArucoObject> arucoObjectsToDelete = new List<ArucoObject>();
          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            if (arucoObject.Value.GetType() != typeof(ArucoMarker))
            {
              ArucoObjectTracker tracker;
              if (additionalTrackers.TryGetValue(arucoObject.Value.GetType(), out tracker))
              {
                // Activate tracker
                if (!tracker.IsActivated)
                {
                  tracker.Activate(this);
                }
              }
              else
              {
                arucoObjectsToDelete.Add(arucoObject.Value);
              }
            }
          }

          // Remove aruco objects with no associated trackers from the list
          foreach (var arucoObject in arucoObjectsToDelete)
          {
            Remove(arucoObject);
            Debug.LogError("No tracker found for the type '" + arucoObject.GetType() + "'. Removing the object '" + arucoObject.gameObject.name
              + "' from the tracking list.");
          }

          // Make adjustements on the tracked aruco objects
          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            ArucoObject_PropertyUpdated(arucoObject.Value);
          }
        }
      }

      /// <summary>
      /// Set the tracking thread to track the next it will be executed, and re-throw the tracking thread exceptions.
      /// </summary>
      protected override void ArucoCameraImageUpdated()
      {
        trackingMutex.WaitOne();

        arucoCameraImageUpdated = true;

        System.Exception e = null;
        if (trackingException != null)
        {
          e = trackingException;
        }

        trackingMutex.ReleaseMutex();

        if (e != null)
        {
          throw e;
        }
      }

      /// <summary>
      /// Start the tracking.
      /// </summary>
      protected override void StartDetector()
      {
        base.StartDetector();
        trackingThread.Start();
        StartCoroutine("ApplyTracking");
      }

      /// <summary>
      /// Stop the tracking.
      /// </summary>
      protected override void StopDetector()
      {
        base.StopDetector();
        StopCoroutine("ApplyTracking"); // The thread will stop automatically with the flag IsStarted false
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
      /// <see cref="ArucoObjectTracker.Detect(int, Dictionary)"/>
      /// </summary>
      public void Detect()
      {
        if (!IsConfigured)
        {
          return;
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

            MarkerTracker.Detect(cameraId, dictionary);
            foreach (var tracker in additionalTrackers)
            {
              tracker.Value.Detect(cameraId, dictionary);
            }
          }
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.EstimateTranforms(int, Dictionary)"/>
      /// </summary>
      public void EstimateTranforms()
      {
        // Skip if no configurate, or don't estimate
        if (!IsConfigured || !EstimateTransforms)
        {
          return;
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

            MarkerTracker.EstimateTranforms(cameraId, dictionary);
            foreach (var tracker in additionalTrackers)
            {
              tracker.Value.EstimateTranforms(cameraId, dictionary);
            }
          }
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.Draw(int, Dictionary)"/>
      /// </summary>
      public void Draw()
      {
        // Skip if no configurate
        if (!IsConfigured)
        {
          return;
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          foreach (var arucoObjectDictionary in ArucoObjects)
          {
            Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

            MarkerTracker.Draw(cameraId, dictionary);
            foreach (var tracker in additionalTrackers)
            {
              tracker.Value.Draw(cameraId, dictionary);
            }
          }
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.Place(int, Dictionary)"/>
      /// </summary>
      public void Place()
      {
        // Skip if no configurate, or don't estimate
        if (!IsConfigured || !EstimateTransforms)
        {
          return;
        }

        int cameraId = 0; // TODO: editor field parameter

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

          MarkerTracker.Place(cameraId, dictionary);
          foreach (var tracker in additionalTrackers)
          {
            tracker.Value.Place(cameraId, dictionary);
          }
        }
      }

      /// <summary>
      /// Executed on a separated tracking thread, detect and estimate the transforms of ArUco objects on the 
      /// <see cref="ArucoObjectsController.ArucoObjects"/> list.
      /// </summary>
      protected void Track()
      {
        trackingMutex.WaitOne();

        if (IsConfigured && IsStarted && arucoCameraImageUpdated)
        {
          Detect();
          EstimateTranforms();
          arucoCameraImageUpdated = false;
        }

        trackingMutex.ReleaseMutex();
      }

      /// <summary>
      /// Draw the results of the detection and place each detected ArUco object on the <see cref="ArucoObjectsController.ArucoObjects"/> list, 
      /// according to the results of the tracking thread.
      /// </summary>
      /// <remarks>
      /// In a coroutine to be executed after the <see cref="ArucoCamera.Images"/> update on the Update() function, but before the 
      /// <see cref="ArucoCamera.ImageTextures"/> update on the LateUpdate() function. See: https://docs.unity3d.com/Manual/ExecutionOrder.html.
      /// </remarks>
      protected IEnumerator ApplyTracking()
      {
        while (true)
        {
          yield return null;
          trackingMutex.WaitOne();

          DeactivateArucoObjects();
          Draw();
          Place();

          trackingMutex.ReleaseMutex();
        }
      }
    }
  }

  /// \} aruco_unity_package
}