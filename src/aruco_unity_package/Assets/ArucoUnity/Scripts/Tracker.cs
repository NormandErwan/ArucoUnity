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
    public VectorVec3d[] Rvecs { get; protected set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public VectorVec3d[] Tvecs { get; protected set; }

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

      // Initialize the properties
      int camerasNumber = ArucoCamera.ImageTextures.Length;
      Rvecs = new VectorVec3d[camerasNumber];
      Tvecs = new VectorVec3d[camerasNumber];
    }

    // Methods

    public void Draw()
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = ArucoCamera.Images;

      for (int i = 0; i < cameraImages.Length; i++)
      {
        // Draw the detected markers
        if (DrawDetectedMarkers && MarkerIds[i].Size() > 0)
        {
          Functions.DrawDetectedMarkers(cameraImages[i], MarkerCorners[i], MarkerIds[i]);
          updatedCameraImage = true;
        }

        // Draw rejected marker candidates
        if (DrawRejectedCandidates && RejectedCandidateCorners[i].Size() > 0)
        {
          Functions.DrawDetectedMarkers(cameraImages[i], RejectedCandidateCorners[i], new Color(100, 0, 255));
          updatedCameraImage = true;
        }
      }

      if (updatedCameraImage)
      {
        ArucoCamera.Images = cameraImages;
      }
    }

    public void EstimateTranforms()
    {
      for (int i = 0; i < ArucoCamera.ImageTextures.Length; i++)
      {
        if (!EstimateTransforms || MarkerIds[i].Size() <= 0)
        {
          Rvecs[i] = null;
          Tvecs[i] = null;
          continue;
        }

        CameraParameters cameraParameters = ArucoCamera.CameraParameters[i];

        // Estimate markers pose
        if (MarkerIds[i].Size() > 0)
        {
          VectorVec3d rvecs, tvecs;
          Functions.EstimatePoseSingleMarkers(MarkerCorners[i], MarkerSideLength, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);
          Rvecs[i] = rvecs;
          Tvecs[i] = tvecs;
        }
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
      int cameraId = 0;

      for (uint i = 0; i < MarkerIds[cameraId].Size(); i++)
      {
        int markerId = MarkerIds[cameraId].At(i);

        bool foundArucoObject = false;
        foreach (ArucoObject arucoObject in ArucoObjects)
        {
          Marker marker = arucoObject as Marker;
          if (marker != null && marker.Id == markerId)
          {
            foundArucoObject = true;
            PlaceGameObject(marker.gameObject, Rvecs[cameraId].At(i), Tvecs[cameraId].At(i), cameraId);
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
          PlaceGameObject(arucoGameObject, Rvecs[cameraId].At(i), Tvecs[cameraId].At(i), cameraId);
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