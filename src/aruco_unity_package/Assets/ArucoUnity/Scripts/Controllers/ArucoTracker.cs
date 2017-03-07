using UnityEngine;
using ArucoUnity.Plugin;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

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

    public Dictionary<ArucoUnity.Plugin.Dictionary, int>[] DetectedMarkers { get; protected internal set; }

    /// <summary>
    /// Vector of the detected marker corners on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] MarkerCorners { get; protected internal set; }

    /// <summary>
    /// Vector of identifiers of the detected markers on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>[] MarkerIds { get; protected internal set; }

    /// <summary>
    /// Vector of the corners with not a correct identification on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] RejectedCandidateCorners { get; protected internal set; }

    /// <summary>
    /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] MarkerRvecs { get; protected internal set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] MarkerTvecs { get; protected internal set; }

    // Variables

    protected Dictionary<System.Type, ArucoObjectTracker> trackers;
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
      trackers = new Dictionary<System.Type, ArucoObjectTracker>()
      {
        { typeof(ArucoMarker), new ArucoMarkerTracker() },
        { typeof(ArucoGridBoard), new ArucoGridBoardTracker() },
        { typeof(ArucoCharucoBoard), new ArucoCharucoBoardTracker() },
        { typeof(ArucoDiamond), new ArucoDiamondTracker() }
      };

      // Initialize the tracking
      trackingMutex = new Mutex();
      trackingThread = new Thread(() =>
      {
        try
        {
          while (true)
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

      // Remove aruco objects with no associated trackers from the list
      foreach (var arucoObjectDictionary in ArucoObjects)
      {
        List<ArucoObject> arucoObjectsToDelete = new List<ArucoObject>();

        foreach (var arucoObject in arucoObjectDictionary.Value)
        {
          ArucoObjectTracker tracker;
          if (!trackers.TryGetValue(arucoObject.Value.GetType(), out tracker))
          {
            Debug.LogError("No tracker found for the type '" + arucoObject.Value.GetType() + "'. Removing the object '"
              + arucoObject.Value.gameObject.name + "' from the tracking list.");
            arucoObjectsToDelete.Add(arucoObject.Value);
          }
        }

        foreach (var arucoObject in arucoObjectsToDelete)
        {
          Remove(arucoObject);
        }
      }

      // Suscribe to ArucoObjectController events
      base.ArucoObjectAdded += ArucoObjectController_ArucoObjectAdded;
      base.DictionaryAdded += ArucoObjectController_DictionaryAdded;
      base.DictionaryRemoved += ArucoObjectController_DictionaryRemoved;
    }

    /// <summary>
    /// Unsuscribe from ArucoObjectController events, and abort the tracking thread.
    /// </summary>
    protected virtual void OnDestroy()
    {
      base.ArucoObjectAdded -= ArucoObjectController_ArucoObjectAdded;
      base.DictionaryAdded -= ArucoObjectController_DictionaryAdded;
      base.DictionaryRemoved -= ArucoObjectController_DictionaryRemoved;

      if (trackingThread != null)
      {
        trackingThread.Abort();
      }
    }

    // ArucoObjectController methods

    /// <summary>
    /// Suscribe to the property events of an ArUco object, and hide its gameObject since it has not been detected yet.
    /// </summary>
    /// <param name="arucoObject">The new ArUco object to suscribe.</param>
    protected virtual void ArucoObjectController_ArucoObjectAdded(ArucoObject arucoObject)
    {
      ArucoObjectTracker tracker;
      if (!trackers.TryGetValue(arucoObject.GetType(), out tracker))
      {
        Debug.LogError("No tracker found for the type '" + arucoObject.GetType() + "'. Removing the object '" + arucoObject.gameObject.name +
          "' from the tracking list.");
        Remove(arucoObject);
        return;
      }

      arucoObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update the properties when a new dictionary is added.
    /// </summary>
    /// <param name="dictionary">The new dictionary.</param>
    protected void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
    {
      if (!IsConfigured)
      {
        return;
      }

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
        MarkerIds[cameraId].Add(dictionary, new VectorInt());
        RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
        MarkerRvecs[cameraId].Add(dictionary, new VectorVec3d());
        MarkerTvecs[cameraId].Add(dictionary, new VectorVec3d());
        DetectedMarkers[cameraId].Add(dictionary, 0);
      }

      foreach (var tracker in trackers)
      {
        tracker.Value.ArucoObjectController_DictionaryAdded(dictionary);
      }
    }

    /// <summary>
    /// Update the properties when a dictionary is removed.
    /// </summary>
    /// <param name="dictionary">The removed dictionary.</param>
    protected void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
    {
      if (!IsConfigured)
      {
        return;
      }

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId].Remove(dictionary);
        MarkerIds[cameraId].Remove(dictionary);
        RejectedCandidateCorners[cameraId].Remove(dictionary);
        MarkerRvecs[cameraId].Remove(dictionary);
        MarkerTvecs[cameraId].Remove(dictionary);
        DetectedMarkers[cameraId].Remove(dictionary);
      }

      foreach (var tracker in trackers)
      {
        tracker.Value.ArucoObjectController_DictionaryRemoved(dictionary);
      }
    }

    // ArucoObject methods

    /// <summary>
    /// Before the ArUco object's properties will be updated, restore the game object's scale of this object.
    /// </summary>
    /// <param name="arucoObject"></param>
    protected override void ArucoObject_PropertyUpdating(ArucoObject arucoObject)
    {
      base.ArucoObject_PropertyUpdating(arucoObject);
      trackers[arucoObject.GetType()].ArucoObject_PropertyUpdating(arucoObject);
    }

    /// <summary>
    /// Adjust the game object's scale of the ArUco object according to its MarkerSideLength property.
    /// </summary>
    /// <param name="arucoObject"></param>
    protected override void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
    {
      base.ArucoObject_PropertyUpdated(arucoObject);
      trackers[arucoObject.GetType()].ArucoObject_PropertyUpdated(arucoObject);
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

      // Initialize the properties and the ArUco objects
      MarkerCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[ArucoCamera.CamerasNumber];
      MarkerIds = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>[ArucoCamera.CamerasNumber];
      RejectedCandidateCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[ArucoCamera.CamerasNumber];
      MarkerRvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[ArucoCamera.CamerasNumber];
      MarkerTvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[ArucoCamera.CamerasNumber];
      DetectedMarkers = new Dictionary<ArucoUnity.Plugin.Dictionary, int>[ArucoCamera.CamerasNumber];

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        MarkerIds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>();
        RejectedCandidateCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        MarkerRvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        MarkerTvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        DetectedMarkers[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, int>();

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          MarkerIds[cameraId].Add(dictionary, new VectorInt());
          RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          MarkerRvecs[cameraId].Add(dictionary, new VectorVec3d());
          MarkerTvecs[cameraId].Add(dictionary, new VectorVec3d());
          DetectedMarkers[cameraId].Add(dictionary, 0);
        }
      }

      // Configure the trackers
      foreach (var tracker in trackers)
      {
        tracker.Value.Configure(this);
      }

      // Trackers do adjustements on the aruco objects
      foreach (var arucoObjectDictionary in ArucoObjects)
      {
        foreach (var arucoObject in arucoObjectDictionary.Value)
        {
          trackers[arucoObject.Value.GetType()].ArucoObject_PropertyUpdated(arucoObject.Value);
        }
      }

      // Start the tracking
      trackingThread.Start();
      StartCoroutine("ApplyTracking");
    }

    /// <summary>
    /// Set the tracking thread to track the next it will be executed and re-throw the tracking thread exceptions.
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

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          foreach (var tracker in trackers)
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
      if (!IsConfigured)
      {
        return;
      }

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          // Skip if don't estimate, or no markers detected, or no camera parameters
          if (!EstimateTransforms || DetectedMarkers[cameraId][dictionary] <= 0 || ArucoCamera.CameraParameters == null)
          {
            MarkerRvecs[cameraId][dictionary] = null;
            MarkerTvecs[cameraId][dictionary] = null;
            continue;
          }

          foreach (var tracker in trackers)
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
      // Skip if no configurate, or don't estimate, or no camera parameters
      if (!IsConfigured || !EstimateTransforms || ArucoCamera.CameraParameters == null)
      {
        return;
      }

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          // Skip if  no markers detected
          if (DetectedMarkers[cameraId][dictionary] <= 0)
          {
            continue;
          }

          foreach (var tracker in trackers)
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
      // Skip if no configurate, or don't estimate, or no camera parameters
      if (!IsConfigured || !EstimateTransforms || ArucoCamera.CameraParameters == null)
      {
        return;
      }

      int cameraId = 0; // TODO: editor field parameter

      foreach (var arucoObjectDictionary in ArucoObjects)
      {
        Dictionary dictionary = arucoObjectDictionary.Key;

        // Skip if  no markers detected
        if (DetectedMarkers[cameraId][dictionary] <= 0)
        {
          continue;
        }

        foreach (var tracker in trackers)
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

  /// \} aruco_unity_package
}