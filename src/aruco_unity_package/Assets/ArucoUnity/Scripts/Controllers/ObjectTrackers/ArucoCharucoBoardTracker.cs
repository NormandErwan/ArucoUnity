using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
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

      /// <summary>
      /// <see cref="ArucoObjectTracker.Detect(int, Aruco.Dictionary, HashSet{ArucoObject})"/>
      /// </summary>
      public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        if (!IsActivated)
        {
          return;
        }

        CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;
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
              arucoCharucoBoard.InterpolatedCorners = Aruco.InterpolateCornersCharuco(markerTracker.MarkerCorners[cameraId][dictionary],
                markerTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId],
                (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners, out charucoIds);
            }
            else
            {
              arucoCharucoBoard.InterpolatedCorners = Aruco.InterpolateCornersCharuco(markerTracker.MarkerCorners[cameraId][dictionary],
                markerTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId],
                (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners, out charucoIds, cameraParameters.CameraMatrices[cameraId],
                cameraParameters.DistCoeffs[cameraId]);
            }
          }
          else
          {
            arucoCharucoBoard.InterpolatedCorners = 0;
          }

          arucoCharucoBoard.DetectedCorners = charucoCorners;
          arucoCharucoBoard.DetectedIds = charucoIds;
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.EstimateTranforms(int, Dictionary, HashSet{ArucoObject})"/>
      /// </summary>
      public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
      {
        if (!IsActivated || arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] <= 0)
        {
          return;
        }

        CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          Cv.Vec3d rvec, tvec;
          arucoCharucoBoard.ValidTransform = Aruco.EstimatePoseCharucoBoard(arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds,
            (Aruco.CharucoBoard)arucoCharucoBoard.Board, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId], out rvec,
            out tvec);

          arucoCharucoBoard.Rvec = rvec;
          arucoCharucoBoard.Tvec = tvec;
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.Draw(int, Dictionary, HashSet{ArucoObject})"/>
      /// </summary>
      public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        if (!IsActivated || arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] <= 0)
        {
          return;
        }

        CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoCharucoBoard.InterpolatedCorners > 0 && arucoCharucoBoard.Rvec != null)
          {
            if (arucoTracker.DrawDetectedCharucoMarkers)
            {
              Aruco.DrawDetectedCornersCharuco(image, arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds);
            }

            if (arucoTracker.DrawAxes && cameraParameters != null && arucoCharucoBoard.ValidTransform)
            {
              Aruco.DrawAxis(image, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
                arucoCharucoBoard.Rvec, arucoCharucoBoard.Tvec, arucoCharucoBoard.AxisLength);
            }
          }
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.Place(int, Dictionary, HashSet{ArucoObject})"/>
      /// </summary>
      public override void Place(int cameraId, Aruco.Dictionary dictionary)
      {
        if (!IsActivated || arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] <= 0)
        {
          return;
        }

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoCharucoBoard.ValidTransform)
          {
            PlaceArucoObject(arucoCharucoBoard, arucoCharucoBoard.Rvec, arucoCharucoBoard.Tvec, cameraId);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}