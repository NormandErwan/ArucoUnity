using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using System.Collections.Generic;

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

    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] DiamondCorners { get; set; }

    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec4i>[] DiamondIds { get; set; }

    public Dictionary<ArucoUnity.Plugin.Dictionary, int>[] DetectedDiamonds { get; set; }

    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] DiamondRvecs { get; set; }

    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] DiamondTvecs { get; set; }

    // ArucoObjectController related methods

    public override void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
    {
      base.ArucoObjectController_DictionaryAdded(dictionary);

      for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CamerasNumber; cameraId++)
      {
        DiamondIds[cameraId].Add(dictionary, new VectorVec4i());
        DetectedDiamonds[cameraId].Add(dictionary, 0);
        DiamondRvecs[cameraId].Add(dictionary, new VectorVec3d());
        DiamondTvecs[cameraId].Add(dictionary, new VectorVec3d());
      }
    }

    public override void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
    {
      base.ArucoObjectController_DictionaryRemoved(dictionary);

      for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CamerasNumber; cameraId++)
      {
        DiamondIds[cameraId].Remove(dictionary);
        DetectedDiamonds[cameraId].Remove(dictionary);
        DiamondRvecs[cameraId].Remove(dictionary);
        DiamondTvecs[cameraId].Remove(dictionary);
      }
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Configure()"/>
    /// </summary>
    public override void Configure(ArucoTracker arucoTracker)
    {
      base.Configure(arucoTracker);

      DiamondCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[arucoTracker.ArucoCamera.CamerasNumber];
      DiamondIds = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec4i>[arucoTracker.ArucoCamera.CamerasNumber];
      DetectedDiamonds = new Dictionary<ArucoUnity.Plugin.Dictionary, int>[arucoTracker.ArucoCamera.CamerasNumber];
      DiamondRvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[arucoTracker.ArucoCamera.CamerasNumber];
      DiamondTvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[arucoTracker.ArucoCamera.CamerasNumber];

      for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CamerasNumber; cameraId++)
      {
        DiamondCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        DiamondIds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec4i>();
        DetectedDiamonds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, int>();
        DiamondRvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        DiamondTvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();

        foreach (var arucoObjectDictionary in arucoTracker.ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          DiamondCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          DiamondIds[cameraId].Add(dictionary, new VectorVec4i());
          DetectedDiamonds[cameraId].Add(dictionary, 0);
          DiamondRvecs[cameraId].Add(dictionary, new VectorVec3d());
          DiamondTvecs[cameraId].Add(dictionary, new VectorVec3d());
        }
      }
    }

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

      DiamondCorners[cameraId][dictionary] = diamondCorners;
      DiamondIds[cameraId][dictionary] = diamondIds;
      DetectedDiamonds[cameraId][dictionary] = (diamondIds != null) ? (int)diamondIds.Size() : 0;
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
      Functions.EstimatePoseSingleMarkers(DiamondCorners[cameraId][dictionary], ESTIMATE_POSE_MARKER_LENGTH, cameraParameters[cameraId].CameraMatrix,
        cameraParameters[cameraId].DistCoeffs, out diamondRvecs, out diamondTvecs);

      DiamondRvecs[cameraId][dictionary] = diamondRvecs;
      DiamondTvecs[cameraId][dictionary] = diamondTvecs;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Draw(int, Dictionary, HashSet{ArucoObject})"/>
    /// </summary>
    public override void Draw(int cameraId, Dictionary dictionary)
    {
      bool updatedCameraImage = false;
      Mat[] cameraImages = arucoTracker.ArucoCamera.Images;
      CameraParameters[] cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

      if (DetectedDiamonds[cameraId][dictionary] > 0)
      {
        if (arucoTracker.DrawDetectedDiamonds)
        {
          Functions.DrawDetectedDiamonds(cameraImages[cameraId], DiamondCorners[cameraId][dictionary], DiamondIds[cameraId][dictionary]);
          updatedCameraImage = true;
        }

        if (arucoTracker.DrawAxes && cameraParameters != null && DiamondRvecs != null)
        {
          for (uint i = 0; i < DetectedDiamonds[cameraId][dictionary]; i++)
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