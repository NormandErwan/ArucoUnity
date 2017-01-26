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
    /// Vector of rotation vectors of the detected markers.
    /// </summary>
    public VectorVec3d Rvecs { get; protected set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers.
    /// </summary>
    public VectorVec3d Tvecs { get; protected set; }

    // Variables

    protected Dictionary<int, GameObject> defaultTrackedMarkerObjects;

    // MonoBehaviour methods

    protected override void Start()
    {
      base.Start();

      defaultTrackedMarkerObjects = new Dictionary<int, GameObject>();
    }

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

    // ArucoDetector methods

    protected override void PreConfigure()
    {
      if (ArucoCamera.CameraParameters == null)
      {
        EstimateTransforms = false;
      }
    }

    // Methods

    public void Draw()
    {
      Mat cameraImage = ArucoCamera.Image;
      bool updatedCameraImage = false;

      // Draw the detected markers
      if (DrawDetectedMarkers && MarkerIds.Size() > 0)
      {
        Functions.DrawDetectedMarkers(cameraImage, MarkerCorners, MarkerIds);
        updatedCameraImage = true;
      }

      // Draw rejected marker candidates
      if (DrawRejectedCandidates && RejectedCandidateCorners.Size() > 0)
      {
        Functions.DrawDetectedMarkers(cameraImage, RejectedCandidateCorners, new Color(100, 0, 255));
        updatedCameraImage = true;
      }

      if (updatedCameraImage)
      {
        ArucoCamera.Image = cameraImage;
      }
    }

    public void EstimateTranforms()
    {
      if (!EstimateTransforms || MarkerIds.Size() < 0)
      {
        Rvecs = null;
        Tvecs = null;
      }

      // Estimate markers pose
      if (MarkerIds.Size() > 0)
      {
        VectorVec3d rvecs, tvecs;
        Functions.EstimatePoseSingleMarkers(MarkerCorners, MarkerSideLength, ArucoCamera.CameraParameters.CameraMatrix, ArucoCamera.CameraParameters.DistCoeffs, out rvecs, out tvecs);
        Rvecs = rvecs;
        Tvecs = tvecs;
      }
    }

    /// <summary>
    /// Hide all the aruco objects.
    /// </summary>
    public void DeactivateArucoObjects()
    {
      foreach (ArucoObject arucoObject in ArucoObjects)
      {
        arucoObject.gameObject.SetActive(false);
      }
    }

    /// <summary>
    /// Place and orient the object to match the marker.
    /// </summary>
    public void Place()
    {
      for (uint i = 0; i < MarkerIds.Size(); i++)
      {
        int markerId = MarkerIds.At(i);

        bool foundArucoObject = false;
        foreach (ArucoObject arucoObject in ArucoObjects)
        {
          Marker marker = arucoObject as Marker;
          if (marker != null && marker.Id == markerId)
          {
            foundArucoObject = true;
            PlaceGameObject(marker.gameObject, Rvecs.At(i), Tvecs.At(i));
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

            defaultTrackedMarkerObjects.Add(markerId, arucoGameObject);
          }
          PlaceGameObject(arucoGameObject, Rvecs.At(i), Tvecs.At(i));
        }
      }
    }

    protected void PlaceGameObject(GameObject arucoGameObject, Vec3d rvec, Vec3d tvec)
    {
      // Place and orient the object to match the marker
      arucoGameObject.transform.rotation = rvec.ToRotation();
      arucoGameObject.transform.position = tvec.ToPosition();

      // Adjust the object position
      Vector3 imageCenterMarkerObject = new Vector3(0.5f, 0.5f, arucoGameObject.transform.position.z);
      Vector3 opticalCenterMarkerObject = new Vector3(ArucoCamera.CameraParameters.OpticalCenter.x, ArucoCamera.CameraParameters.OpticalCenter.y, arucoGameObject.transform.position.z);
      Vector3 opticalShift = ArucoCamera.ImageCamera.ViewportToWorldPoint(opticalCenterMarkerObject) - ArucoCamera.ImageCamera.ViewportToWorldPoint(imageCenterMarkerObject);

      Vector3 positionShift = opticalShift // Take account of the optical center not in the image center
        + arucoGameObject.transform.up * arucoGameObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
      arucoGameObject.transform.localPosition += positionShift;

      print(arucoGameObject.name + " - imageCenter: " + imageCenterMarkerObject.ToString("F3") + "; opticalCenter: " + opticalCenterMarkerObject.ToString("F3")
        + "; positionShift: " + (arucoGameObject.transform.rotation * opticalShift).ToString("F4"));

      arucoGameObject.SetActive(true);
    }
  }

  /// \} aruco_unity_package
}