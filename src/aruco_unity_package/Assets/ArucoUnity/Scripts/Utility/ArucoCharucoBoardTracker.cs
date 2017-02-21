using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  public class ArucoCharucoBoardTracker : ArucoObjectTracker
  {
    public ArucoCharucoBoardTracker(ArucoTracker arucoTracker) : base(arucoTracker)
    {
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Detect(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Detect(int cameraId, Dictionary dictionary)
    {
      CameraParameters[] cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

      foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
      {
        if (arucoTracker.RefineDetectedMarkers)
        {
          Functions.RefineDetectedMarkers(arucoTracker.ArucoCamera.Images[cameraId], arucoCharucoBoard.Board, arucoTracker.MarkerCorners[cameraId][dictionary],
            arucoTracker.MarkerIds[cameraId][dictionary], arucoTracker.RejectedCandidateCorners[cameraId][dictionary]);
          arucoTracker.DetectedMarkers[cameraId][dictionary] = (int)arucoTracker.MarkerIds[cameraId][dictionary].Size();
        }

        VectorPoint2f charucoCorners = null;
        VectorInt charucoIds = null;

        if (arucoTracker.DetectedMarkers[cameraId][dictionary] > 0)
        {
          if (cameraParameters == null)
          {
            arucoCharucoBoard.InterpolatedCorners = Functions.InterpolateCornersCharuco(arucoTracker.MarkerCorners[cameraId][dictionary], 
              arucoTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId], arucoCharucoBoard.Board, out charucoCorners,
              out charucoIds);
          }
          else
          {
            arucoCharucoBoard.InterpolatedCorners = Functions.InterpolateCornersCharuco(arucoTracker.MarkerCorners[cameraId][dictionary],
              arucoTracker.MarkerIds[cameraId][dictionary], arucoTracker.ArucoCamera.Images[cameraId], arucoCharucoBoard.Board, out charucoCorners,
              out charucoIds, cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs);
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
    public override void EstimateTranforms(int cameraId, Dictionary dictionary)
    {
      CameraParameters[] cameraParameters = arucoTracker.ArucoCamera.CameraParameters;
      if (cameraParameters == null)
      {
        return;
      }

      foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
      {
        Vec3d rvec, tvec;
        arucoCharucoBoard.ValidTransform = Functions.EstimatePoseCharucoBoard(arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds,
          arucoCharucoBoard.Board, cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs, out rvec, out tvec);

        arucoCharucoBoard.Rvec = rvec;
        arucoCharucoBoard.Tvec = tvec;
      }
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Draw(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Draw(int cameraId, Dictionary dictionary)
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = arucoTracker.ArucoCamera.Images;
      CameraParameters[] cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

      foreach (var arucoCharucoBoard in arucoTracker.GetArucoObjects<ArucoCharucoBoard>(dictionary))
      {
        if (arucoCharucoBoard.InterpolatedCorners > 0 && arucoCharucoBoard.Rvec != null)
        {
          if (arucoTracker.DrawDetectedCharucoMarkers)
          {
            Functions.DrawDetectedCornersCharuco(cameraImages[cameraId], arucoCharucoBoard.DetectedCorners, arucoCharucoBoard.DetectedIds);
            updatedCameraImage = true;
          }

          if (arucoTracker.DrawAxes && cameraParameters != null && arucoCharucoBoard.ValidTransform)
          {
            Functions.DrawAxis(cameraImages[cameraId], cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs, 
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
    public override void Place(int cameraId, Dictionary dictionary)
    {
    }
  }

  /// \} aruco_unity_package
}