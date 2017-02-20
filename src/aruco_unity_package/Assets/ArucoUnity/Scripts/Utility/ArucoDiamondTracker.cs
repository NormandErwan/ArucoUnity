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
    public ArucoDiamondTracker(ArucoTracker arucoTracker) : base(arucoTracker)
    {
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Detect(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Detect(int cameraId, Dictionary dictionary)
    {
      CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters[cameraId];

      foreach (var arucoDiamond in arucoTracker.GetArucoObjects<ArucoDiamond>(dictionary))
      {
        VectorVectorPoint2f diamondCorners = null;
        VectorVec4i diamondIds = null;

        if (arucoTracker.DetectedMarkers[cameraId][dictionary] > 0)
        {
          if (cameraParameters == null)
          {
            // TODO: handle multiple diamond detection (can do only one detection for all the diamonds?)
            Functions.DetectCharucoDiamond(arucoTracker.ArucoCamera.Images[cameraId], arucoTracker.MarkerCorners[cameraId][dictionary],
              arucoTracker.MarkerIds[cameraId][dictionary], arucoDiamond.SquareSideLength / arucoDiamond.MarkerSideLength, out diamondCorners,
              out diamondIds);
          }
          else
          {
            Functions.DetectCharucoDiamond(arucoTracker.ArucoCamera.Images[cameraId], arucoTracker.MarkerCorners[cameraId][dictionary], arucoTracker.MarkerIds[cameraId][dictionary],
              arucoDiamond.SquareSideLength / arucoDiamond.MarkerSideLength, out diamondCorners, out diamondIds, cameraParameters.CameraMatrix, 
              cameraParameters.DistCoeffs);
          }
        }

        arucoDiamond.DetectedCorners = diamondCorners;
        arucoDiamond.DetectedIds = diamondIds;
        arucoDiamond.DetectedMarkers = (diamondIds != null) ? (int)diamondIds.Size() : 0;
      }
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.EstimateTranforms(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void EstimateTranforms(int cameraId, Dictionary dictionary)
    {
      CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters[cameraId];

      // TODO: add autoscale feature (see: https://github.com/opencv/opencv_contrib/blob/master/modules/aruco/samples/detect_diamonds.cpp#L203)
      foreach (var arucoDiamond in arucoTracker.GetArucoObjects<ArucoDiamond>(dictionary))
      {
        VectorVec3d adRvecs, adTvecs;
        Functions.EstimatePoseSingleMarkers(arucoDiamond.DetectedCorners, arucoDiamond.SquareSideLength, cameraParameters.CameraMatrix,
          cameraParameters.DistCoeffs, out adRvecs, out adTvecs);

        arucoDiamond.Rvecs = adRvecs;
        arucoDiamond.Tvecs = adTvecs;
      }
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Draw(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Draw(int cameraId, Dictionary dictionary)
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = arucoTracker.ArucoCamera.Images;

      foreach (var arucoDiamond in arucoTracker.GetArucoObjects<ArucoDiamond>(dictionary))
      {
        if (arucoDiamond.DetectedMarkers > 0)
        {
          if (arucoTracker.DrawDetectedDiamonds)
          {
            // TODO: fix
            //Functions.DrawDetectedDiamonds(cameraImages[cameraId], arucoDiamond.DetectedCorners, arucoDiamond.DetectedIds);
            updatedCameraImage = true;
          }

          if (arucoTracker.DrawAxes && arucoDiamond.Rvecs != null)
          {
            for (uint i = 0; i < arucoDiamond.DetectedMarkers; i++)
            {
              // TODO: fix
              //Functions.DrawAxis(cameraImages[cameraId], cameraParameters[cameraId].CameraMatrix, cameraParameters[cameraId].DistCoeffs,
              //  arucoDiamond.Rvecs.At(i), arucoDiamond.Tvecs.At(i), arucoDiamond.AxisLength);
              updatedCameraImage = true;
            }
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