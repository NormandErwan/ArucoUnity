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
  public class Tracker : ArucoObjectDetector
  {
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

    [SerializeField]
    [Tooltip("The default game object to place above the detected markers")]
    private GameObject defaultTrackedGameObject;

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
    /// The default game object to place above the detected markers.
    /// </summary>
    public GameObject DefaultTrackedGameObject { get { return defaultTrackedGameObject; } set { defaultTrackedGameObject = value; } }

    /// <summary>
    /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] Rvecs { get; protected set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] Tvecs { get; protected set; }

    // Variables

    protected Dictionary<int, GameObject> defaultTrackedMarkerObjects;

    // ArucoObjectDetector methods

    /// <summary>
    /// When configured, detect markers and show results each frame.
    /// </summary>
    protected override void ArucoCameraImageUpdated()
    {
      DeactivateArucoObjects();

      if (Configured)
      {
        Detect();
        Draw();
        EstimateTranforms();
        Place();
      }
    }

    protected override void PreConfigure()
    {
      if (ArucoCamera.CameraParameters == null)
      {
        EstimateTransforms = false;
      }

      // Initialize the properties
      int camerasNumber = ArucoCamera.ImageTextures.Length;
      Rvecs = new Dictionary<Dictionary, VectorVec3d>[camerasNumber];
      Tvecs = new Dictionary<Dictionary, VectorVec3d>[camerasNumber];
      defaultTrackedMarkerObjects = new Dictionary<int, GameObject>();

      for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
      {
        Rvecs[cameraId] = new Dictionary<Dictionary, VectorVec3d>();
        Tvecs[cameraId] = new Dictionary<Dictionary, VectorVec3d>();

        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          Rvecs[cameraId].Add(dictionary, new VectorVec3d());
          Tvecs[cameraId].Add(dictionary, new VectorVec3d());
        }
      }
    }

    protected override void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
    {
      if (Configured)
      {
        base.ArucoObjectController_DictionaryAdded(dictionary);

        for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
        {
          Rvecs[cameraId].Add(dictionary, new VectorVec3d());
          Tvecs[cameraId].Add(dictionary, new VectorVec3d());
        }
      }
    }

    protected override void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
    {
      if (Configured)
      {
        base.ArucoObjectController_DictionaryRemoved(dictionary);

        for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
        {
          Rvecs[cameraId].Remove(dictionary);
          Tvecs[cameraId].Remove(dictionary);
        }
      }
    }

    // Methods

    public void Draw()
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = ArucoCamera.Images;

      for (int cameraId = 0; cameraId < cameraImages.Length; cameraId++)
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
        }
      }

      if (updatedCameraImage)
      {
        ArucoCamera.Images = cameraImages;
      }
    }

    public void EstimateTranforms()
    {
      for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
      {
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          if (!EstimateTransforms || MarkerIds[cameraId][dictionary].Size() <= 0)
          {
            Rvecs[cameraId][dictionary] = null;
            Tvecs[cameraId][dictionary] = null;
            continue;
          }

          CameraParameters cameraParameters = ArucoCamera.CameraParameters[cameraId];

          // Estimate markers pose
          if (MarkerIds[cameraId][dictionary].Size() > 0)
          {
            VectorVec3d rvecs, tvecs;
            Functions.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], MarkerSideLength, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);
            Rvecs[cameraId][dictionary] = rvecs;
            Tvecs[cameraId][dictionary] = tvecs;
          }
        }
      }
    }

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

      foreach (var arucoObject in defaultTrackedMarkerObjects)
      {
        arucoObject.Value.SetActive(false);
      }
    }

    /// <summary>
    /// Place and orient the object to match the marker on the first camera image.
    /// </summary>
    public void Place()
    {
      int cameraId = 0; // TODO: editor field parameter

      foreach (var arucoObjectDictionary in ArucoObjects)
      {
        Dictionary dictionary = arucoObjectDictionary.Key;

        for (uint i = 0; i < MarkerIds[cameraId][dictionary].Size(); i++)
        {
          int markerId = MarkerIds[cameraId][dictionary].At(i);

          bool foundArucoObject = false;
          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            Marker marker = arucoObject as Marker;
            if (marker != null && marker.Id == markerId)
            {
              foundArucoObject = true;
              PlaceGameObject(marker.gameObject, Rvecs[cameraId][dictionary].At(i), Tvecs[cameraId][dictionary].At(i), cameraId);
            }
          }

          if (!foundArucoObject)
          {
            // Found the default tracked game object for the current tracked marker
            GameObject arucoGameObject = null;
            if (!defaultTrackedMarkerObjects.TryGetValue(markerId, out arucoGameObject))
            {
              // If not found, instantiate it
              arucoGameObject = Instantiate(DefaultTrackedGameObject);
              arucoGameObject.name = markerId.ToString();
              arucoGameObject.transform.SetParent(this.transform);
              arucoGameObject.transform.localScale *= MarkerSideLength;

              defaultTrackedMarkerObjects.Add(markerId, arucoGameObject);
            }
            PlaceGameObject(arucoGameObject, Rvecs[cameraId][dictionary].At(i), Tvecs[cameraId][dictionary].At(i), cameraId);
          }
        }
      }
    }

    protected void PlaceGameObject(GameObject arucoGameObject, Vec3d rvec, Vec3d tvec, int cameraId)
    {
      // Place and orient the object to match the marker
      arucoGameObject.transform.rotation = rvec.ToRotation();
      arucoGameObject.transform.position = tvec.ToPosition();

      // Adjust the object position
      Camera camera = ArucoCamera.ImageCameras[cameraId];
      Vector3 cameraOpticalCenter = ArucoCamera.CameraParameters[cameraId].OpticalCenter;

      Vector3 imageCenter = new Vector3(0.5f, 0.5f, arucoGameObject.transform.position.z);
      Vector3 opticalCenter = new Vector3(cameraOpticalCenter.x, cameraOpticalCenter.y, arucoGameObject.transform.position.z);
      Vector3 opticalShift = camera.ViewportToWorldPoint(opticalCenter) - camera.ViewportToWorldPoint(imageCenter);

      Vector3 positionShift = opticalShift // Take account of the optical center not in the image center
        + arucoGameObject.transform.up * arucoGameObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
      arucoGameObject.transform.localPosition += positionShift;

      print(arucoGameObject.name + " - imageCenter: " + imageCenter.ToString("F3") + "; opticalCenter: " + opticalCenter.ToString("F3")
        + "; positionShift: " + (arucoGameObject.transform.rotation * opticalShift).ToString("F4"));

      arucoGameObject.SetActive(true);
    }
  }

  /// \} aruco_unity_package
}