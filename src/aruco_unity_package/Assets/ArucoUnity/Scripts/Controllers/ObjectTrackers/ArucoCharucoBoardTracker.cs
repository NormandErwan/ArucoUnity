using ArucoUnity.Objects;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.ObjectTrackers
  {
    public class ArucoCharucoBoardTracker : ArucoObjectTracker
    {
      // ArucoObjectTracker methods

      public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        ArucoMarkerTracker markerTracker = arucoTracker.MarkerTracker;

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoTracker.RefineDetectedMarkers)
          {
            Aruco.RefineDetectedMarkers(image, arucoCharucoBoard.Board, markerTracker.MarkerCorners[cameraId][dictionary],
              markerTracker.MarkerIds[cameraId][dictionary], markerTracker.RejectedCandidateCorners[cameraId][dictionary]);
            markerTracker.DetectedMarkers[cameraId][dictionary] = (int)markerTracker.MarkerIds[cameraId][dictionary].Size();
          }

          Std.VectorPoint2f charucoCorners = null;
          Std.VectorInt charucoIds = null;

          if (markerTracker.DetectedMarkers[cameraId][dictionary] > 0)
          {
            if (cameraParameters == null)
            {
               Aruco.InterpolateCornersCharuco(markerTracker.MarkerCorners[cameraId][dictionary],
                markerTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId],
                (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners, out charucoIds);
            }
            else
            {
              Aruco.InterpolateCornersCharuco(markerTracker.MarkerCorners[cameraId][dictionary],
                markerTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId],
                (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners, out charucoIds, cameraParameters.CameraMatrices[cameraId],
                cameraParameters.DistCoeffs[cameraId]);
            }
          }

          arucoCharucoBoard.DetectedCorners = charucoCorners;
          arucoCharucoBoard.DetectedIds = charucoIds;
        }
      }

      public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
      {
        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          Cv.Vec3d rvec = null, tvec = null;
          bool validTransform = false;

          if (arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] > 0 && cameraParameters != null)
          {
            validTransform = Aruco.EstimatePoseCharucoBoard(arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds,
            (Aruco.CharucoBoard)arucoCharucoBoard.Board, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId], out rvec,
            out tvec);
          }

          arucoCharucoBoard.Rvec = rvec;
          arucoCharucoBoard.Tvec = tvec;
          arucoCharucoBoard.ValidTransform = validTransform;
        }
      }

      public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoCharucoBoard.DetectedIds.Size() > 0)
          {
            if (arucoTracker.DrawDetectedCharucoMarkers)
            {
              Aruco.DrawDetectedCornersCharuco(image, arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds);
            }

            if (arucoTracker.DrawAxes && cameraParameters != null && arucoCharucoBoard.Rvec != null)
            {
              Aruco.DrawAxis(image, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
                arucoCharucoBoard.Rvec, arucoCharucoBoard.Tvec, arucoCharucoBoard.AxisLength);
            }
          }
        }
      }

      public override void Place(int cameraId, Aruco.Dictionary dictionary)
      {
        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoCharucoBoard.Rvec != null)
          {
            PlaceArucoObject(arucoCharucoBoard, arucoCharucoBoard.Rvec, arucoCharucoBoard.Tvec, cameraId);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}