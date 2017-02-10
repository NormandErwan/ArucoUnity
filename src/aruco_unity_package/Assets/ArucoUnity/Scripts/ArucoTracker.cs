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
    [Tooltip("Display the detected markers in the CameraImageTexture")]
    private bool drawDetectedMarkers = true;

    [SerializeField]
    [Tooltip("Display the rejected markers candidates")]
    private bool drawRejectedCandidates = false;

    [SerializeField]
    [Tooltip("Estimate the detected markers pose (position, rotation)")]
    private bool estimateTransforms = true;

    // Properties

    /// <summary>
    /// Display the detected markers in the <see cref="ArucoObjectDetector.CameraImageTexture"/>.
    /// </summary>
    public bool DrawDetectedMarkers { get { return drawDetectedMarkers; } set { drawDetectedMarkers = value; } }

    /// <summary>
    /// Display the rejected markers candidates.
    /// </summary>
    public bool DrawRejectedCandidates { get { return drawRejectedCandidates; } set { drawRejectedCandidates = value; } }

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
      Draw();
      EstimateTranforms();
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

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        MarkerIds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>();
        RejectedCandidateCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        Rvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        Tvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          MarkerIds[cameraId].Add(dictionary, new VectorInt());
          RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          Rvecs[cameraId].Add(dictionary, new VectorVec3d());
          Tvecs[cameraId].Add(dictionary, new VectorVec3d());

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
    /// Hide all the aruco objects.
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

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;
          VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
          VectorInt markerIds;

          Functions.DetectMarkers(ArucoCamera.Images[cameraId], dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);
          MarkerCorners[cameraId][dictionary] = markerCorners;
          MarkerIds[cameraId][dictionary] = markerIds;
          RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
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
          if (DrawDetectedMarkers && MarkerIds[cameraId][dictionary].Size() > 0)
          {
            Functions.DrawDetectedMarkers(cameraImages[cameraId], MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary]);
            updatedCameraImage = true;
          }

          // Draw rejected marker candidates
          if (DrawRejectedCandidates && RejectedCandidateCorners[cameraId][dictionary].Size() > 0)
          {
            Functions.DrawDetectedMarkers(cameraImages[cameraId], RejectedCandidateCorners[cameraId][dictionary], new Color(100, 0, 255));
            updatedCameraImage = true;
          }

          // TODO: draw grid board, charuco board, diamonds
        }
      }

      if (updatedCameraImage)
      {
        ArucoCamera.Images = cameraImages;
      }
    }

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

          // Skip if don't estimate nor markers detected
          if (!EstimateTransforms || MarkerIds[cameraId][dictionary].Size() <= 0)
          {
            Rvecs[cameraId][dictionary] = null;
            Tvecs[cameraId][dictionary] = null;
            continue;
          }

          CameraParameters cameraParameters = ArucoCamera.CameraParameters[cameraId];

          // Estimate markers pose
          VectorVec3d rvecs, tvecs;
          Functions.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], ESTIMATE_POSE_MARKER_LENGTH, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);
          Rvecs[cameraId][dictionary] = rvecs;
          Tvecs[cameraId][dictionary] = tvecs;

          // TODO: estimate grid board, charuco board, diamond
        }
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
        for (uint i = 0; i < MarkerIds[cameraId][dictionary].Size(); i++)
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