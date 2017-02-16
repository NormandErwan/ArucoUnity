using UnityEngine;
using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  /// <summary>
  /// Detect markers, display results and place game objects on the detected markers transform.
  /// </summary>
  public class ArucoTracker : ArucoObjectsController
  {
    // Constants

    protected float ESTIMATE_POSE_MARKER_LENGTH = 1f;

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

    /// <summary>
    /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] Rvecs { get; protected set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] Tvecs { get; protected set; }

    // Variables

    protected Dictionary<ArucoUnity.Plugin.Dictionary, int>[] detectedMarkers;

    // MonoBehaviour methods

    /// <summary>
    /// Susbcribe to events from ArucoObjectController for every ArUco object added or removed.
    /// </summary>
    protected override void Start()
    {
      base.Start();

      base.ArucoObjectAdded += ArucoObjectController_ArucoObjectAdded;
      base.ArucoObjectRemoved += ArucoObjectController_ArucoObjectRemoved;

      base.DictionaryAdded += ArucoObjectController_DictionaryAdded;
      base.DictionaryRemoved += ArucoObjectController_DictionaryRemoved;
    }

    /// <summary>
    /// Unsuscribe from ArucoObjectController events.
    /// </summary>
    protected virtual void OnDestroy()
    {
      base.ArucoObjectAdded -= ArucoObjectController_ArucoObjectAdded;
      base.ArucoObjectRemoved -= ArucoObjectController_ArucoObjectRemoved;

      base.DictionaryAdded -= ArucoObjectController_DictionaryAdded;
      base.DictionaryRemoved -= ArucoObjectController_DictionaryRemoved;
    }

    // ArucoObjectController methods

    /// <summary>
    /// Suscribe to the property events of an ArUco object, and hide its gameObject since it has not been detected yet.
    /// </summary>
    /// <param name="arucoObject">The new ArUco object to suscribe.</param>
    protected virtual void ArucoObjectController_ArucoObjectAdded(ArucoObject arucoObject)
    {
      arucoObject.gameObject.SetActive(false);

      arucoObject.PropertyUpdating += ArucoObject_PropertyUpdating;
      arucoObject.PropertyUpdated += ArucoObject_PropertyUpdated;
    }

    /// <summary>
    /// Unsuscribe from the property events of an ArUco object.
    /// </summary>
    /// <param name="arucoObject">The ArUco object to unsuscribe.</param>
    protected virtual void ArucoObjectController_ArucoObjectRemoved(ArucoObject arucoObject)
    {
      arucoObject.PropertyUpdating -= ArucoObject_PropertyUpdating;
      arucoObject.PropertyUpdated -= ArucoObject_PropertyUpdated;
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
        Rvecs[cameraId].Add(dictionary, new VectorVec3d());
        Tvecs[cameraId].Add(dictionary, new VectorVec3d());

        detectedMarkers[cameraId].Add(dictionary, 0);
      }
    }

    /// <summary>
    /// Update the properties when a dictionary is removed.
    /// </summary>
    /// <param name="dictionary">The dictionary removed.</param>
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
        Rvecs[cameraId].Remove(dictionary);
        Tvecs[cameraId].Remove(dictionary);

        detectedMarkers[cameraId].Remove(dictionary);
      }
    }

    // ArucoObject methods

    /// <summary>
    /// Before the ArUco object's properties will be updated, restore the game object's scale of this object.
    /// </summary>
    /// <param name="arucoObject"></param>
    protected void ArucoObject_PropertyUpdating(ArucoObject arucoObject)
    {
      if (arucoObject.MarkerSideLength != 0)
      {
        arucoObject.gameObject.transform.localScale /= arucoObject.MarkerSideLength;
      }
    }

    /// <summary>
    /// Adjust the game object's scale of the ArUco object according to its MarkerSideLength property.
    /// </summary>
    /// <param name="arucoObject"></param>
    protected void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
    {
      if (arucoObject.MarkerSideLength != 0)
      {
        arucoObject.gameObject.transform.localScale *= arucoObject.MarkerSideLength;
      }
    }

    // ArucoObjectDetector methods

    /// <summary>
    /// When configured and started, detect markers and show results each frame.
    /// </summary>
    protected override void ArucoCameraImageUpdated()
    {
      if (!IsConfigured || !IsStarted)
      {
        return;
      }

      DeactivateArucoObjects();
      Detect();
      EstimateTranforms();
      Draw();
      Place();
    }

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
      Rvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[ArucoCamera.CamerasNumber];
      Tvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[ArucoCamera.CamerasNumber];

      detectedMarkers = new Dictionary<ArucoUnity.Plugin.Dictionary, int>[ArucoCamera.CamerasNumber];

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        MarkerIds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>();
        RejectedCandidateCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        Rvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        Tvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();

        detectedMarkers[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, int>();

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          MarkerIds[cameraId].Add(dictionary, new VectorInt());
          RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          Rvecs[cameraId].Add(dictionary, new VectorVec3d());
          Tvecs[cameraId].Add(dictionary, new VectorVec3d());

          detectedMarkers[cameraId].Add(dictionary, 0);

          // Adjust the scale of the game object of each ArUco object
          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            if (arucoObject.MarkerSideLength != 0)
            {
              arucoObject.gameObject.transform.localScale *= arucoObject.MarkerSideLength;
            }
          }
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
          arucoObject.gameObject.SetActive(false);
        }
      }
    }

    /// <summary>
    /// Detect the ArUco objects on each <see cref="ArucoCamera.Images"/>. Should be called during the OnImagesUpdated() event, after the update of 
    /// the CameraImageTexture.
    /// </summary>
    // TODO: detect in a separate thread for performances
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
          CameraParameters[] cameraParameters = ArucoCamera.CameraParameters;
          Mat cameraImage = ArucoCamera.Images[cameraId];

          // Detect marker of the dictionary
          VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
          VectorInt markerIds;

          Functions.DetectMarkers(cameraImage, dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);
          detectedMarkers[cameraId][dictionary] = (int)markerIds.Size();

          // Apply refine strategy with the boards of the dictionary's aruco objects collection
          if (RefineDetectedMarkers)
          {
            foreach (var arucoBoard in GetArucoObjects<ArucoBoard<Board>>(dictionary))
            {
              Functions.RefineDetectedMarkers(cameraImage, arucoBoard.Board, markerCorners, markerIds, rejectedCandidateCorners);
              detectedMarkers[cameraId][dictionary] = (int)markerIds.Size();
            }
          }

          // Detect charuco corners or set the detected properties to null
          foreach (var arucoCharucoBoard in GetArucoObjects<ArucoCharucoBoard>(dictionary))
          {
            VectorPoint2f charucoCorners = null;
            VectorInt charucoIds = null;

            if (detectedMarkers[cameraId][dictionary] > 0)
            {
              if (cameraParameters == null)
              {
                Functions.InterpolateCornersCharuco(markerCorners, markerIds, cameraImage, arucoCharucoBoard.Board, out charucoCorners,
                  out charucoIds);
              }
              else
              {
                Functions.InterpolateCornersCharuco(markerCorners, markerIds, cameraImage, arucoCharucoBoard.Board, out charucoCorners,
                  out charucoIds, cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs);
              }
            }

            arucoCharucoBoard.DetectedCorners = charucoCorners;
            arucoCharucoBoard.DetectedIds = charucoIds;
          }

          // Detect diamonds
          foreach (var arucoDiamond in GetArucoObjects<ArucoDiamond>(dictionary))
          {
            VectorVectorPoint2f diamondCorners = null;
            VectorVec4i diamondIds = null;

            if (detectedMarkers[cameraId][dictionary] > 0)
            {
              if (cameraParameters == null)
              {
                Functions.DetectCharucoDiamond(cameraImage, markerCorners, markerIds, arucoDiamond.SquareSideLength / arucoDiamond.MarkerSideLength,
                  out diamondCorners, out diamondIds);
              }
              else
              {
                Functions.DetectCharucoDiamond(cameraImage, markerCorners, markerIds, arucoDiamond.SquareSideLength / arucoDiamond.MarkerSideLength,
                  out diamondCorners, out diamondIds, cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs);
              }
            }

            arucoDiamond.DetectedCorners = diamondCorners;
            arucoDiamond.DetectedIds = diamondIds;
          }

          MarkerCorners[cameraId][dictionary] = markerCorners;
          MarkerIds[cameraId][dictionary] = markerIds;
          RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
        }
      }
    }

    /// <summary>
    /// Estimate the gameObject's transform of each detected ArUco object.
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
          if (!EstimateTransforms || detectedMarkers[cameraId][dictionary] <= 0 || ArucoCamera.CameraParameters == null)
          {
            Rvecs[cameraId][dictionary] = null;
            Tvecs[cameraId][dictionary] = null;
            continue;
          }

          CameraParameters cameraParameters = ArucoCamera.CameraParameters[cameraId];

          // Estimate marker transforms
          VectorVec3d rvecs, tvecs;
          Functions.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], ESTIMATE_POSE_MARKER_LENGTH,
            cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);

          Rvecs[cameraId][dictionary] = rvecs;
          Tvecs[cameraId][dictionary] = tvecs;

          // Estimate grid board transforms
          foreach (var arucoGridBoard in GetArucoObjects<ArucoGridBoard>(dictionary))
          {
            Vec3d rvec, tvec;
            arucoGridBoard.detectedMarkers = Functions.EstimatePoseBoard(MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary], 
              arucoGridBoard.Board, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvec, out tvec);

            arucoGridBoard.Rvec = rvec;
            arucoGridBoard.Tvec = tvec;
          }

          // Estimate charuco board transforms
          foreach (var arucoCharucoBoard in GetArucoObjects<ArucoCharucoBoard>(dictionary))
          {
            Vec3d rvec, tvec;
            arucoCharucoBoard.ValidTransform = Functions.EstimatePoseCharucoBoard(arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds, 
              arucoCharucoBoard.Board, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvec, out tvec);

            arucoCharucoBoard.Rvec = rvec;
            arucoCharucoBoard.Tvec = tvec;
          }

          // TODO: add autoscale feature (see: https://github.com/opencv/opencv_contrib/blob/master/modules/aruco/samples/detect_diamonds.cpp#L203)
          // Estimate diamond transforms
          foreach (var arucoDiamond in GetArucoObjects<ArucoDiamond>(dictionary))
          {
            VectorVec3d adRvecs, adTvecs;
            Functions.EstimatePoseSingleMarkers(arucoDiamond.DetectedCorners, arucoDiamond.SquareSideLength, cameraParameters.CameraMatrix, 
              cameraParameters.DistCoeffs, out adRvecs, out adTvecs);

            arucoDiamond.Rvecs = adRvecs;
            arucoDiamond.Tvecs = adTvecs;
          }
        }
      }
    }

    public void Draw()
    {
      if (!IsConfigured)
      {
        return;
      }

      bool updatedCameraImage = false;
      Mat[] cameraImages = ArucoCamera.Images;

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          // Draw the detected markers
          if (DrawDetectedMarkers && detectedMarkers[cameraId][dictionary] > 0)
          {
            Functions.DrawDetectedMarkers(cameraImages[cameraId], MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary]);
            updatedCameraImage = true;
          }

          // Draw the rejected marker candidates
          if (DrawRejectedCandidates && RejectedCandidateCorners[cameraId][dictionary].Size() > 0)
          {
            Functions.DrawDetectedMarkers(cameraImages[cameraId], RejectedCandidateCorners[cameraId][dictionary], new Color(100, 0, 255));
            updatedCameraImage = true;
          }

          // Draw the detected grid boards
          foreach (var arucoGridBoard in GetArucoObjects<ArucoGridBoard>(dictionary))
          {
            if (DrawAxes && arucoGridBoard.detectedMarkers > 0)
            {
              // TODO
            }
          }

          // Draw the detected charuco boards
          foreach (var arucoCharucoBoard in GetArucoObjects<ArucoCharucoBoard>(dictionary))
          {
            if (DrawDetectedCharucoMarkers)
            {
              Functions.DrawDetectedCornersCharuco(cameraImages[cameraId], arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds);
            }

            if (DrawAxes && arucoCharucoBoard.ValidTransform)
            {
              // TODO
            }
          }

          // Draw the detected diamonds
          foreach (var arucoDiamond in GetArucoObjects<ArucoDiamond>(dictionary))
          {
            if (DrawDetectedDiamonds)
            {
              Functions.DrawDetectedDiamonds(cameraImages[cameraId], arucoDiamond.DetectedCorners, arucoDiamond.DetectedIds);
            }

            if (DrawAxes && arucoDiamond.Rvecs != null)
            {
              // TODO
            }
          }
        }
      }

      if (updatedCameraImage)
      {
        ArucoCamera.Images = cameraImages;
      }
    }

    /// <summary>
    /// Place and orient the object to match the marker on the first camera image.
    /// </summary>
    public void Place()
    {
      if (!IsConfigured)
      {
        return;
      }

      int cameraId = 0; // TODO: editor field parameter

      foreach (var arucoObjectDictionary in ArucoObjects)
      {
        Dictionary dictionary = arucoObjectDictionary.Key;

        // Place ArUco markers
        for (uint i = 0; i < detectedMarkers[cameraId][dictionary]; i++)
        {
          int markerId = MarkerIds[cameraId][dictionary].At(i);

          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            ArucoMarker marker = arucoObject as ArucoMarker;
            if (marker != null && marker.Id == markerId)
            {
              PlaceArucoObject(marker, Rvecs[cameraId][dictionary].At(i), Tvecs[cameraId][dictionary].At(i), cameraId);
            }
          }
        }

        // TODO: place grid board, charuco board, diamond
      }
    }

    protected void PlaceArucoObject(ArucoObject arucoObject, Vec3d rvec, Vec3d tvec, int cameraId)
    {
      GameObject arucoGameObject = arucoObject.gameObject;

      // Place and orient the object to match the marker
      arucoGameObject.transform.position = tvec.ToPosition() * arucoObject.MarkerSideLength;
      arucoGameObject.transform.rotation = rvec.ToRotation();

      // Adjust the object position
      Camera camera = ArucoCamera.ImageCameras[cameraId];
      Vector3 cameraOpticalCenter = ArucoCamera.CameraParameters[cameraId].OpticalCenter;

      Vector3 imageCenter = new Vector3(0.5f, 0.5f, arucoGameObject.transform.position.z);
      Vector3 opticalCenter = new Vector3(cameraOpticalCenter.x, cameraOpticalCenter.y, arucoGameObject.transform.position.z);
      Vector3 opticalShift = camera.ViewportToWorldPoint(opticalCenter) - camera.ViewportToWorldPoint(imageCenter);

      Vector3 positionShift = opticalShift // Take account of the optical center not in the image center
        + arucoGameObject.transform.up * arucoGameObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
      arucoGameObject.transform.localPosition += positionShift;

      //print(arucoGameObject.name + " - imageCenter: " + imageCenter.ToString("F3") + "; opticalCenter: " + opticalCenter.ToString("F3")
      //  + "; positionShift: " + (arucoGameObject.transform.rotation * opticalShift).ToString("F4"));

      arucoGameObject.SetActive(true);
    }
  }

  /// \} aruco_unity_package
}