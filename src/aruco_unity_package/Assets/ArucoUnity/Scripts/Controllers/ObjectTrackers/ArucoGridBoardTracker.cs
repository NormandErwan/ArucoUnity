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
    public class ArucoGridBoardTracker : ArucoObjectTracker
    {
      // ArucoObjectTracker methods

      /// <summary>
      /// <see cref="ArucoObjectTracker.Detect(int, Dictionary, HashSet{ArucoObject})"/>
      /// </summary>
      public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        if (!IsActivated)
        {
          return;
        }

        ArucoMarkerTracker markerTracker = arucoTracker.MarkerTracker;

        if (arucoTracker.RefineDetectedMarkers)
        {
          foreach (var arucoBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
          {
            Aruco.RefineDetectedMarkers(image, arucoBoard.Board, markerTracker.MarkerCorners[cameraId][dictionary],
              markerTracker.MarkerIds[cameraId][dictionary], markerTracker.RejectedCandidateCorners[cameraId][dictionary]);
            markerTracker.DetectedMarkers[cameraId][dictionary] = (int)markerTracker.MarkerIds[cameraId][dictionary].Size();
          }
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

        foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          Cv.Vec3d rvec = null, tvec = null;
          arucoGridBoard.MarkersUsedForEstimation = Aruco.EstimatePoseBoard(arucoTracker.MarkerTracker.MarkerCorners[cameraId][dictionary],
            arucoTracker.MarkerTracker.MarkerIds[cameraId][dictionary], arucoGridBoard.Board, cameraParameters.CameraMatrices[cameraId],
            cameraParameters.DistCoeffs[cameraId], out rvec, out tvec);

          arucoGridBoard.Rvec = rvec;
          arucoGridBoard.Tvec = tvec;
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

        foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          if (arucoTracker.DrawAxes && cameraParameters != null && arucoGridBoard.MarkersUsedForEstimation > 0 && arucoGridBoard.Rvec != null)
          {
            Aruco.DrawAxis(image, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
              arucoGridBoard.Rvec, arucoGridBoard.Tvec, arucoGridBoard.AxisLength);
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

        foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          if (arucoGridBoard.MarkersUsedForEstimation > 0 && arucoGridBoard.Rvec != null)
          {
            PlaceArucoObject(arucoGridBoard, arucoGridBoard.Rvec, arucoGridBoard.Tvec, cameraId);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}