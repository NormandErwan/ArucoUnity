using ArucoUnity.Objects;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.ObjectTrackers
  {
    public class ArucoGridBoardTracker : ArucoObjectTracker
    {
      // ArucoObjectTracker methods

      public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        ArucoMarkerTracker markerTracker = arucoTracker.MarkerTracker;

        if (arucoTracker.RefineDetectedMarkers && arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] > 0)
        {
          foreach (var arucoBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
          {
            Aruco.RefineDetectedMarkers(image, arucoBoard.Board, markerTracker.MarkerCorners[cameraId][dictionary],
              markerTracker.MarkerIds[cameraId][dictionary], markerTracker.RejectedCandidateCorners[cameraId][dictionary]);
            markerTracker.DetectedMarkers[cameraId][dictionary] = (int)markerTracker.MarkerIds[cameraId][dictionary].Size();
          }
        }
      }

      public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
      {
        foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          Cv.Vec3d rvec = null, tvec = null;
          int markersUsedForEstimation = 0;

          if (arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] > 0 && cameraParameters != null)
          {
            markersUsedForEstimation = Aruco.EstimatePoseBoard(arucoTracker.MarkerTracker.MarkerCorners[cameraId][dictionary],
              arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary], arucoGridBoard.Board, cameraParameters.CameraMatrices[cameraId],
              cameraParameters.DistCoeffs[cameraId], out rvec, out tvec);
          }

          arucoGridBoard.Rvec = rvec;
          arucoGridBoard.Tvec = tvec;
          arucoGridBoard.MarkersUsedForEstimation = markersUsedForEstimation;
        }
      }

      public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          if (arucoTracker.DrawAxes && cameraParameters != null && arucoGridBoard.Rvec != null)
          {
            Aruco.DrawAxis(image, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
              arucoGridBoard.Rvec, arucoGridBoard.Tvec, arucoGridBoard.AxisLength);
          }
        }
      }

      public override void Place(int cameraId, Aruco.Dictionary dictionary)
      {
        foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          if (arucoGridBoard.Rvec != null)
          {
            PlaceArucoObject(arucoGridBoard, arucoGridBoard.Rvec, arucoGridBoard.Tvec, cameraId);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}