using ArucoUnity.Cameras;
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
      public override void Detect(int cameraId, Aruco.Dictionary dictionary)
      {
        if (!IsActivated)
        {
          return;
        }

        CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoTracker.RefineDetectedMarkers)
          {
            Aruco.RefineDetectedMarkers(arucoTracker.ArucoCamera.Images[cameraId], arucoCharucoBoard.Board, arucoTracker.MarkerTracker.MarkerCorners[cameraId][dictionary],
              arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary], arucoTracker.MarkerTracker.RejectedCandidateCorners[cameraId][dictionary]);
            arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] = (int)arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary].Size();
          }

          Std.VectorPoint2f charucoCorners = null;
          Std.VectorInt charucoIds = null;

          if (arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] > 0)
          {
            if (cameraParameters == null)
            {
              arucoCharucoBoard.InterpolatedCorners = Aruco.InterpolateCornersCharuco(arucoTracker.MarkerTracker.MarkerCorners[cameraId][dictionary],
                arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId], (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners,
                out charucoIds);
            }
            else
            {
              arucoCharucoBoard.InterpolatedCorners = Aruco.InterpolateCornersCharuco(arucoTracker.MarkerTracker.MarkerCorners[cameraId][dictionary],
                arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId], (Aruco.CharucoBoard)arucoCharucoBoard.Board, out charucoCorners,
                out charucoIds, cameraParameters.CamerasMatrix[cameraId], cameraParameters.DistCoeffs[cameraId]);
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
      public override void EstimateTranforms(int cameraId, Aruco.Dictionary dictionary)
      {
        if (!IsActivated || arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] <= 0)
        {
          return;
        }

        CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          Cv.Core.Vec3d rvec, tvec;
          arucoCharucoBoard.ValidTransform = Aruco.EstimatePoseCharucoBoard(arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds,
            (Aruco.CharucoBoard)arucoCharucoBoard.Board, cameraParameters.CamerasMatrix[cameraId], cameraParameters.DistCoeffs[cameraId], out rvec, out tvec);

          arucoCharucoBoard.Rvec = rvec;
          arucoCharucoBoard.Tvec = tvec;
        }
      }

      /// <summary>
      /// <see cref="ArucoObjectTracker.Draw(int, Dictionary, HashSet{ArucoObject})"/>
      /// </summary>
      public override void Draw(int cameraId, Aruco.Dictionary dictionary)
      {
        if (!IsActivated || arucoTracker.MarkerTracker.DetectedMarkers[cameraId][dictionary] <= 0)
        {
          return;
        }

        bool updatedCameraImage = false;
        Cv.Core.Mat[] cameraImages = arucoTracker.ArucoCamera.Images;
        CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

        foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
        {
          if (arucoCharucoBoard.InterpolatedCorners > 0 && arucoCharucoBoard.Rvec != null)
          {
            if (arucoTracker.DrawDetectedCharucoMarkers)
            {
              Aruco.DrawDetectedCornersCharuco(cameraImages[cameraId], arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds);
              updatedCameraImage = true;
            }

            if (arucoTracker.DrawAxes && cameraParameters != null && arucoCharucoBoard.ValidTransform)
            {
              Aruco.DrawAxis(cameraImages[cameraId], cameraParameters.CamerasMatrix[cameraId], cameraParameters.DistCoeffs[cameraId],
                arucoCharucoBoard.Rvec, arucoCharucoBoard.Tvec, arucoCharucoBoard.AxisLength);
              updatedCameraImage = true;
            }
          }
        }

        if (updatedCameraImage)
        {
          arucoTracker.ArucoCamera.Images = cameraImages;
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