using ArucoUnity.Plugin;

namespace ArucoUnity.Objects.Trackers
{
  public class ArucoGridBoardTracker : ArucoObjectTracker
  {
    // ArucoObjectTracker methods

    public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Detect(cameraId, dictionary, image);

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

    public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Draw(cameraId, dictionary, image);

      foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
      {
        if (arucoTracker.DrawAxes && arucoCameraUndistortion != null && arucoGridBoard.Rvec != null)
        {
          Aruco.DrawAxis(image, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId], arucoCameraUndistortion.UndistortedDistCoeffs[cameraId],
            arucoGridBoard.Rvec, arucoGridBoard.Tvec, arucoGridBoard.AxisLength);
        }
      }
    }

    public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.EstimateTransforms(cameraId, dictionary);

      foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
      {
        Cv.Vec3d rvec = null, tvec = null;
        int markersUsedForEstimation = 0;

        if (arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] > 0 && arucoCameraUndistortion != null)
        {
          markersUsedForEstimation = Aruco.EstimatePoseBoard(arucoTracker.MarkerTracker.MarkerCorners[cameraId][dictionary],
            arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary], arucoGridBoard.Board, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId],
            arucoCameraUndistortion.UndistortedDistCoeffs[cameraId], out rvec, out tvec);
        }

        arucoGridBoard.Rvec = rvec;
        arucoGridBoard.Tvec = tvec;
        arucoGridBoard.MarkersUsedForEstimation = markersUsedForEstimation;
      }
    }

    public override void UpdateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.UpdateTransforms(cameraId, dictionary);

      // Update transform of each tracked board
      foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
      {
        if (arucoGridBoard.Rvec != null)
        {
          // Adjust the estimated coordinates
          var position = arucoGridBoard.Tvec.ToPosition()
            + arucoGridBoard.transform.right * arucoGridBoard.GetGameObjectScale().x / 2
            + arucoGridBoard.transform.forward * arucoGridBoard.GetGameObjectScale().z / 2;

          arucoCameraDisplay.PlaceArucoObject(arucoGridBoard.transform, cameraId, position, arucoGridBoard.Rvec.ToRotation());
        }
      }
    }
  }
}