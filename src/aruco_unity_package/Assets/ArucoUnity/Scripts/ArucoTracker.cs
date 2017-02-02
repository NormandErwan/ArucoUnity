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
  public class ArucoTracker : ArucoObjectDetector
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
    /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] Rvecs { get; protected set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] Tvecs { get; protected set; }

    // ArucoObjectController methods

    protected override void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
    {
      base.ArucoObjectController_DictionaryAdded(dictionary);
      if (IsConfigured)
      {
        for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
        {
          Rvecs[cameraId].Add(dictionary, new VectorVec3d());
          Tvecs[cameraId].Add(dictionary, new VectorVec3d());
        }
      }
    }

    protected override void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
    {
      base.ArucoObjectController_DictionaryRemoved(dictionary);
      if (IsConfigured)
      {
        for (int cameraId = 0; cameraId < ArucoCamera.ImageTextures.Length; cameraId++)
        {
          Rvecs[cameraId].Remove(dictionary);
          Tvecs[cameraId].Remove(dictionary);
        }
      }
    }

    // ArucoObjectDetector methods

    /// <summary>
    /// When configured, detect markers and show results each frame.
    /// </summary>
    protected override void ArucoCameraImageUpdated()
    {
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

      // Initialize the properties
      int camerasNumber = ArucoCamera.ImageTextures.Length;
      Rvecs = new Dictionary<Dictionary, VectorVec3d>[camerasNumber];
      Tvecs = new Dictionary<Dictionary, VectorVec3d>[camerasNumber];

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

    // Methods

    public void Draw()
    {
      if (!IsConfigured)
      {
        return;
      }

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
      if (!IsConfigured)
      {
        return;
      }

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
            Functions.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], ESTIMATE_POSE_MARKER_LENGTH, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);
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

        for (uint i = 0; i < MarkerIds[cameraId][dictionary].Size(); i++)
        {
          int markerId = MarkerIds[cameraId][dictionary].At(i);

          foreach (var arucoObject in arucoObjectDictionary.Value)
          {
            Marker marker = arucoObject as Marker;
            if (marker != null && marker.Id == markerId)
            {
              PlaceArucoObject(marker, Rvecs[cameraId][dictionary].At(i), Tvecs[cameraId][dictionary].At(i), cameraId);
            }
          }
        }
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