using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  public class ArucoMarkerTracker : ArucoObjectTracker
  {
    // Constants

    protected readonly float ESTIMATE_POSE_MARKER_LENGTH = 1f;

    protected readonly Color REJECTED_MARKERS_CANDIDATES_COLOR = new Color(100, 0, 255);

    // Constructor

    public ArucoMarkerTracker(ArucoTracker arucoTracker) : base(arucoTracker)
    {
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Detect(int, Dictionary)"/>
    /// </summary>
    public override void Detect(int cameraId, Dictionary dictionary)
    {
      VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
      VectorInt markerIds;

      Functions.DetectMarkers(arucoTracker.ArucoCamera.Images[cameraId], dictionary, out markerCorners, out markerIds, 
        arucoTracker.DetectorParameters, out rejectedCandidateCorners);
      arucoTracker.DetectedMarkers[cameraId][dictionary] = (int)markerIds.Size();

      arucoTracker.MarkerCorners[cameraId][dictionary] = markerCorners;
      arucoTracker.MarkerIds[cameraId][dictionary] = markerIds;
      arucoTracker.RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.EstimateTranforms(int, Dictionary)"/>
    /// </summary>
    public override void EstimateTranforms(int cameraId, Dictionary dictionary)
    {
      CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters[cameraId];

      VectorVec3d rvecs, tvecs;
      Functions.EstimatePoseSingleMarkers(arucoTracker.MarkerCorners[cameraId][dictionary], ESTIMATE_POSE_MARKER_LENGTH,
        cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);

      arucoTracker.Rvecs[cameraId][dictionary] = rvecs;
      arucoTracker.Tvecs[cameraId][dictionary] = tvecs;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Draw(int, Dictionary)"/>
    /// </summary>
    public override void Draw(int cameraId, Dictionary dictionary)
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = arucoTracker.ArucoCamera.Images;

      // Draw the detected markers
      // TODO: draw only markers in ArucoObjects list + add option to draw all the detected markers
      if (arucoTracker.DrawDetectedMarkers && arucoTracker.DetectedMarkers[cameraId][dictionary] > 0)
      {
        Functions.DrawDetectedMarkers(cameraImages[cameraId], arucoTracker.MarkerCorners[cameraId][dictionary], arucoTracker.MarkerIds[cameraId][dictionary]);
        updatedCameraImage = true;
      }

      // Draw the rejected marker candidates
      if (arucoTracker.DrawRejectedCandidates && arucoTracker.RejectedCandidateCorners[cameraId][dictionary].Size() > 0)
      {
        Functions.DrawDetectedMarkers(cameraImages[cameraId], arucoTracker.RejectedCandidateCorners[cameraId][dictionary], REJECTED_MARKERS_CANDIDATES_COLOR);
        updatedCameraImage = true;
      }

      if (updatedCameraImage)
      {
        arucoTracker.ArucoCamera.Images = cameraImages;
      }
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Place(int, Dictionary)"/>
    /// </summary>
    public override void Place(int cameraId, Dictionary dictionary)
    {
      foreach (var arucoMarker in arucoTracker.GetArucoObjects<ArucoMarker>(dictionary))
      {
        for (uint i = 0; i < arucoTracker.DetectedMarkers[cameraId][dictionary]; i++)
        {
          if (arucoMarker.Id == arucoTracker.MarkerIds[cameraId][dictionary].At(i))
          {
            PlaceArucoObject(arucoMarker, arucoTracker.Rvecs[cameraId][dictionary].At(i), arucoTracker.Tvecs[cameraId][dictionary].At(i), cameraId);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}