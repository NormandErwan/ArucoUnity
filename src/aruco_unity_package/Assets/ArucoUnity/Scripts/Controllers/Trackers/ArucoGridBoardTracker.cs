using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Utility;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  public class ArucoGridBoardTracker : ArucoObjectTracker
  {
    public ArucoGridBoardTracker(ArucoTracker arucoTracker) : base(arucoTracker)
    {
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Detect(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Detect(int cameraId, Dictionary dictionary)
    {
      if (arucoTracker.RefineDetectedMarkers)
      {
        foreach (var arucoBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
        {
          Functions.RefineDetectedMarkers(arucoTracker.ArucoCamera.Images[cameraId], arucoBoard.Board, arucoTracker.MarkerCorners[cameraId][dictionary], 
            arucoTracker.MarkerIds[cameraId][dictionary], arucoTracker.RejectedCandidateCorners[cameraId][dictionary]);
          arucoTracker.DetectedMarkers[cameraId][dictionary] = (int)arucoTracker.MarkerIds[cameraId][dictionary].Size();
        }
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

      foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
      {
        Vec3d rvec = null, tvec = null;
        // TODO: fix crash
        //arucoGridBoard.MarkersUsedForEstimation = Functions.EstimatePoseBoard(MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary], 
        //  arucoGridBoard.Board, cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs, out rvec, out tvec);

        arucoGridBoard.Rvec = rvec;
        arucoGridBoard.Tvec = tvec;
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

      foreach (var arucoGridBoard in arucoTracker.GetArucoObjects<ArucoGridBoard>(dictionary))
      {
        if (arucoTracker.DrawAxes && cameraParameters != null && arucoGridBoard.MarkersUsedForEstimation > 0 
          && arucoGridBoard.Rvec != null)
        {
          Functions.DrawAxis(cameraImages[cameraId], cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs, 
            arucoGridBoard.Rvec, arucoGridBoard.Tvec, arucoGridBoard.AxisLength);
          updatedCameraImage = true;
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