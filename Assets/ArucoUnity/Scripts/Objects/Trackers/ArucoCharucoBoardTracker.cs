using ArucoUnity.Plugin;

namespace ArucoUnity.Objects.Trackers
{
  public class ArucoCharucoBoardTracker : ArucoObjectTracker
  {
    // ArucoObjectTracker methods

    public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Detect(cameraId, dictionary, image);

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
          if (arucoCameraUndistortion == null)
          {
            Aruco.InterpolateCornersCharuco(markerTracker.MarkerCorners[cameraId][dictionary],
             markerTracker.MarkerIds[cameraId][dictionary], arucoCamera.Images[cameraId],
             (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners, out charucoIds);
          }
          else
          {
            Aruco.InterpolateCornersCharuco(markerTracker.MarkerCorners[cameraId][dictionary],
              markerTracker.MarkerIds[cameraId][dictionary], arucoCamera.Images[cameraId],
              (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners, out charucoIds, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId],
              arucoCameraUndistortion.UndistortedDistCoeffs[cameraId]);
          }
        }

        arucoCharucoBoard.DetectedCorners = charucoCorners;
        arucoCharucoBoard.DetectedIds = charucoIds;
      }
    }

    public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Draw(cameraId, dictionary, image);

      foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
      {
        if (arucoCharucoBoard.DetectedIds != null && arucoCharucoBoard.DetectedIds.Size() > 0)
        {
          if (arucoTracker.DrawDetectedCharucoMarkers)
          {
            Aruco.DrawDetectedCornersCharuco(image, arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds);
          }

          if (arucoTracker.DrawAxes && arucoCameraUndistortion != null && arucoCharucoBoard.Rvec != null)
          {
            Aruco.DrawAxis(image, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId], arucoCameraUndistortion.UndistortedDistCoeffs[cameraId],
              arucoCharucoBoard.Rvec, arucoCharucoBoard.Tvec, arucoCharucoBoard.AxisLength);
          }
        }
      }
    }

    public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.EstimateTransforms(cameraId, dictionary);

      foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
      {
        Cv.Vec3d rvec = null, tvec = null;
        bool validTransform = false;

        if (arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] > 0 && arucoCameraUndistortion != null)
        {
          validTransform = Aruco.EstimatePoseCharucoBoard(arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds,
          (Aruco.CharucoBoard)arucoCharucoBoard.Board, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId], arucoCameraUndistortion.UndistortedDistCoeffs[cameraId],
          out rvec, out tvec);
        }

        arucoCharucoBoard.Rvec = rvec;
        arucoCharucoBoard.Tvec = tvec;
        arucoCharucoBoard.ValidTransform = validTransform;
      }
    }

    public override void UpdateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.UpdateTransforms(cameraId, dictionary);

      foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
      {
        if (arucoCharucoBoard.Rvec != null)
        {
          arucoCameraDisplay.PlaceArucoObject(arucoCharucoBoard.transform, cameraId, arucoCharucoBoard.Tvec.ToPosition(),
            arucoCharucoBoard.Rvec.ToRotation());
        }
      }
    }
  }
}
