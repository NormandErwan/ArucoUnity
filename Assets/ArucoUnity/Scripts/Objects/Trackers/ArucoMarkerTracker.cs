using ArucoUnity.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity.Objects.Trackers
{
  public class ArucoMarkerTracker : ArucoObjectTracker
  {
    // Constants

    protected const float estimatePoseMarkerLength = 1f;
    protected readonly Color rejectedMarkerCandidatesColor = new Color(100, 0, 255);

    // Properties

    public Dictionary<Aruco.Dictionary, int>[] DetectedMarkers { get; protected internal set; }

    /// <summary>
    /// Vector of the detected marker corners on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[] MarkerCorners { get; protected internal set; }

    /// <summary>
    /// Vector of identifiers of the detected markers on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<Aruco.Dictionary, Std.VectorInt>[] MarkerIds { get; protected internal set; }

    /// <summary>
    /// Vector of the corners with not a correct identification on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[] RejectedCandidateCorners { get; protected internal set; }

    /// <summary>
    /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<Aruco.Dictionary, Std.VectorVec3d>[] MarkerRvecs { get; protected internal set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<Aruco.Dictionary, Std.VectorVec3d>[] MarkerTvecs { get; protected internal set; }

    // ArucoObjectsController related methods

    protected override void ArucoObjectsController_DictionaryAdded(Aruco.Dictionary dictionary)
    {
      for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
      {
        MarkerCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
        MarkerIds[cameraId].Add(dictionary, new Std.VectorInt());
        RejectedCandidateCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
        MarkerRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
        MarkerTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
        DetectedMarkers[cameraId].Add(dictionary, 0);
      }
    }

    protected override void ArucoObjectsController_DictionaryRemoved(Aruco.Dictionary dictionary)
    {
      for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
      {
        MarkerCorners[cameraId].Remove(dictionary);
        MarkerIds[cameraId].Remove(dictionary);
        RejectedCandidateCorners[cameraId].Remove(dictionary);
        MarkerRvecs[cameraId].Remove(dictionary);
        MarkerTvecs[cameraId].Remove(dictionary);
        DetectedMarkers[cameraId].Remove(dictionary);
      }
    }

    // ArucoObjectTracker methods

    public override void Activate(IArucoObjectsTracker arucoTracker)
    {
      base.Activate(arucoTracker);

      // Initialize the properties and the ArUco objects
      MarkerCorners = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[arucoCamera.CameraNumber];
      MarkerIds = new Dictionary<Aruco.Dictionary, Std.VectorInt>[arucoCamera.CameraNumber];
      RejectedCandidateCorners = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[arucoCamera.CameraNumber];
      MarkerRvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoCamera.CameraNumber];
      MarkerTvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoCamera.CameraNumber];
      DetectedMarkers = new Dictionary<Aruco.Dictionary, int>[arucoCamera.CameraNumber];

      for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
      {
        MarkerCorners[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>();
        MarkerIds[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorInt>();
        RejectedCandidateCorners[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>();
        MarkerRvecs[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>();
        MarkerTvecs[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>();
        DetectedMarkers[cameraId] = new Dictionary<Aruco.Dictionary, int>();

        foreach (var arucoObjectDictionary in arucoTracker.ArucoObjects)
        {
          Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

          MarkerCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
          MarkerIds[cameraId].Add(dictionary, new Std.VectorInt());
          RejectedCandidateCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
          MarkerRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
          MarkerTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
          DetectedMarkers[cameraId].Add(dictionary, 0);
        }
      }
    }

    public override void Deactivate()
    {
      base.Deactivate();

      MarkerCorners = null;
      MarkerIds = null;
      RejectedCandidateCorners = null;
      MarkerRvecs = null;
      MarkerTvecs = null;
      DetectedMarkers = null;
    }

    public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Detect(cameraId, dictionary, image);

      Std.VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
      Std.VectorInt markerIds;

      Aruco.DetectMarkers(image, dictionary, out markerCorners, out markerIds, arucoTracker.DetectorParameters, out rejectedCandidateCorners);

      DetectedMarkers[cameraId][dictionary] = (int)markerIds.Size();
      MarkerCorners[cameraId][dictionary] = markerCorners;
      MarkerIds[cameraId][dictionary] = markerIds;
      RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
    }

    public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Draw(cameraId, dictionary, image);

      if (DetectedMarkers[cameraId][dictionary] > 0)
      {
        // Draw all the detected markers
        if (arucoTracker.DrawDetectedMarkers)
        {
          Aruco.DrawDetectedMarkers(image, MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary]);
        }

        // Draw axes of detected tracked markers
        if (arucoTracker.DrawAxes && arucoCameraUndistortion != null && MarkerRvecs[cameraId][dictionary] != null)
        {
          for (uint i = 0; i < DetectedMarkers[cameraId][dictionary]; i++)
          {
            ArucoObject foundArucoObject;
            int detectedMarkerHashCode = ArucoMarker.GetArucoHashCode(MarkerIds[cameraId][dictionary].At(i));
            if (arucoTracker.ArucoObjects[dictionary].TryGetValue(detectedMarkerHashCode, out foundArucoObject))
            {
              Aruco.DrawAxis(image, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId], arucoCameraUndistortion.UndistortedDistCoeffs[cameraId],
                MarkerRvecs[cameraId][dictionary].At(i), MarkerTvecs[cameraId][dictionary].At(i), estimatePoseMarkerLength);
            }
          }
        }
      }

      // Draw the rejected marker candidates
      if (arucoTracker.DrawRejectedCandidates && RejectedCandidateCorners[cameraId][dictionary].Size() > 0)
      {
        Aruco.DrawDetectedMarkers(image, RejectedCandidateCorners[cameraId][dictionary]);
      }
    }

    public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.EstimateTransforms(cameraId, dictionary);

      Std.VectorVec3d rvecs = null, tvecs = null;

      if (DetectedMarkers[cameraId][dictionary] > 0 && arucoCameraUndistortion != null)
      {
        Aruco.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], estimatePoseMarkerLength, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId],
          arucoCameraUndistortion.UndistortedDistCoeffs[cameraId], out rvecs, out tvecs);
      }

      MarkerRvecs[cameraId][dictionary] = rvecs;
      MarkerTvecs[cameraId][dictionary] = tvecs;
    }

    public override void UpdateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.UpdateTransforms(cameraId, dictionary);

      if (MarkerRvecs[cameraId][dictionary] != null)
      {
        for (uint i = 0; i < DetectedMarkers[cameraId][dictionary]; i++)
        {
          ArucoObject foundArucoObject;
          int detectedMarkerHashCode = ArucoMarker.GetArucoHashCode(MarkerIds[cameraId][dictionary].At(i));
          if (arucoTracker.ArucoObjects[dictionary].TryGetValue(detectedMarkerHashCode, out foundArucoObject))
          {
            var localPosition = MarkerTvecs[cameraId][dictionary].At(i).ToPosition() * foundArucoObject.MarkerSideLength / estimatePoseMarkerLength;
            arucoCameraDisplay.PlaceArucoObject(foundArucoObject.transform, cameraId, localPosition,
              MarkerRvecs[cameraId][dictionary].At(i).ToRotation());
          }
        }
      }
    }
  }
}