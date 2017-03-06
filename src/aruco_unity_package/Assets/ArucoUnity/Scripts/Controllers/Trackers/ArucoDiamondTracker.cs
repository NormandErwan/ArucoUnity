using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  public class ArucoDiamondTracker : ArucoObjectTracker
  {
    // Constants

    protected readonly float DETECT_SQUARE_MARKER_LENGTH_RATE = 2f;
    protected readonly float ESTIMATE_POSE_MARKER_LENGTH = 1f;

    // Properties

    public VectorVectorPoint2f DiamondCorners { get; set; }

    public VectorVec4i DiamondIds { get; set; }

    public int DetectedDiamonds { get; set; }

    public VectorVec3d DiamondRvecs { get; set; }

    public VectorVec3d DiamondTvecs { get; set; }

    // Constructor

    public ArucoDiamondTracker(ArucoTracker arucoTracker) : base(arucoTracker)
    {
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Detect(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Detect(int cameraId, Dictionary dictionary)
    {
      CameraParameters[] cameraParameters = arucoTracker.ArucoCamera.CameraParameters;
      
      VectorVectorPoint2f diamondCorners = null;
      VectorVec4i diamondIds = null;

      if (arucoTracker.DetectedMarkers[cameraId][dictionary] > 0)
      {
        if (cameraParameters == null)
        {
          Functions.DetectCharucoDiamond(arucoTracker.ArucoCamera.Images[cameraId], arucoTracker.MarkerCorners[cameraId][dictionary],
            arucoTracker.MarkerIds[cameraId][dictionary], DETECT_SQUARE_MARKER_LENGTH_RATE, out diamondCorners, out diamondIds);
        }
        else
        {
          Functions.DetectCharucoDiamond(arucoTracker.ArucoCamera.Images[cameraId], arucoTracker.MarkerCorners[cameraId][dictionary],
            arucoTracker.MarkerIds[cameraId][dictionary], DETECT_SQUARE_MARKER_LENGTH_RATE, out diamondCorners, out diamondIds, 
            cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs);
        }
      }

      DiamondCorners = diamondCorners;
      DiamondIds = diamondIds;
      DetectedDiamonds = (diamondIds != null) ? (int)diamondIds.Size() : 0;
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

      // TODO: add autoscale feature (see: https://github.com/opencv/opencv_contrib/blob/master/modules/aruco/samples/detect_diamonds.cpp#L203)
      VectorVec3d diamondRvecs, diamondTvecs;
      Functions.EstimatePoseSingleMarkers(DiamondCorners, ESTIMATE_POSE_MARKER_LENGTH, cameraParameters[cameraId].CameraMatrix,
        cameraParameters[cameraId].DistCoeffs, out diamondRvecs, out diamondTvecs);

      DiamondRvecs = diamondRvecs;
      DiamondTvecs = diamondTvecs;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Draw(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Draw(int cameraId, Dictionary dictionary)
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = arucoTracker.ArucoCamera.Images;
      CameraParameters[] cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

      if (DetectedDiamonds > 0)
      {
        if (arucoTracker.DrawDetectedDiamonds)
        {
          Functions.DrawDetectedDiamonds(cameraImages[cameraId], DiamondCorners, DiamondIds);
          updatedCameraImage = true;
        }

        if (arucoTracker.DrawAxes && cameraParameters != null && DiamondRvecs != null)
        {
          for (uint i = 0; i < DetectedDiamonds; i++)
          {
            // TODO: match the detected markers [i] with a ArucoDiamond on the list to get the AxisLength
            //Functions.DrawAxis(cameraImages[cameraId], cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs,
            //  Rvecs.At(i), Tvecs.At(i), arucoDiamond.AxisLength);
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