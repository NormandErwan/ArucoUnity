using ArucoUnity.Plugin;
using System.Collections.Generic;

namespace ArucoUnity.Objects.Trackers
{
  public class ArucoDiamondTracker : ArucoObjectTracker
  {
    // Constants

    protected const float DetectSquareMarkerLengthRate = 2f;
    protected const float EstimatePoseSquareLength = 1f;
    protected const float DrawAxisLength = EstimatePoseSquareLength / 2f;

    // Properties

    public Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[] DiamondCorners { get; set; }

    public Dictionary<Aruco.Dictionary, Std.VectorVec4i>[] DiamondIds { get; set; }

    public Dictionary<Aruco.Dictionary, int>[] DetectedDiamonds { get; set; }

    public Dictionary<Aruco.Dictionary, Std.VectorVec3d>[] DiamondRvecs { get; set; }

    public Dictionary<Aruco.Dictionary, Std.VectorVec3d>[] DiamondTvecs { get; set; }

    // ArucoObjectsController related methods

    protected override void ArucoObjectsController_DictionaryAdded(Aruco.Dictionary dictionary)
    {
      for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
      {
        DiamondIds[cameraId].Add(dictionary, new Std.VectorVec4i());
        DetectedDiamonds[cameraId].Add(dictionary, 0);
        DiamondRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
        DiamondTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
      }
    }

    protected override void ArucoObjectsController_DictionaryRemoved(Aruco.Dictionary dictionary)
    {
      for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
      {
        DiamondIds[cameraId].Remove(dictionary);
        DetectedDiamonds[cameraId].Remove(dictionary);
        DiamondRvecs[cameraId].Remove(dictionary);
        DiamondTvecs[cameraId].Remove(dictionary);
      }
    }

    // ArucoObjectTracker methods

    public override void Activate(IArucoObjectsTracker arucoTracker)
    {
      base.Activate(arucoTracker);

      DiamondCorners = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[arucoCamera.CameraNumber];
      DiamondIds = new Dictionary<Aruco.Dictionary, Std.VectorVec4i>[arucoCamera.CameraNumber];
      DetectedDiamonds = new Dictionary<Aruco.Dictionary, int>[arucoCamera.CameraNumber];
      DiamondRvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoCamera.CameraNumber];
      DiamondTvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoCamera.CameraNumber];

      for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
      {
        DiamondCorners[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>();
        DiamondIds[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec4i>();
        DetectedDiamonds[cameraId] = new Dictionary<Aruco.Dictionary, int>();
        DiamondRvecs[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>();
        DiamondTvecs[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>();

        foreach (var arucoObjectDictionary in arucoTracker.ArucoObjects)
        {
          Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

          DiamondCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
          DiamondIds[cameraId].Add(dictionary, new Std.VectorVec4i());
          DetectedDiamonds[cameraId].Add(dictionary, 0);
          DiamondRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
          DiamondTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
        }
      }
    }

    public override void Deactivate()
    {
      base.Deactivate();

      DiamondCorners = null;
      DiamondIds = null;
      DetectedDiamonds = null;
      DiamondRvecs = null;
      DiamondTvecs = null;
    }

    public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Detect(cameraId, dictionary, image);

      ArucoMarkerTracker markerTracker = arucoTracker.MarkerTracker;

      Std.VectorVectorPoint2f diamondCorners = null;
      Std.VectorVec4i diamondIds = null;

      if (markerTracker.DetectedMarkers[cameraId][dictionary] > 0)
      {
        if (arucoCameraUndistortion == null)
        {
          Aruco.DetectCharucoDiamond(image, markerTracker.MarkerCorners[cameraId][dictionary], markerTracker.MarkerIds[cameraId][dictionary],
            DetectSquareMarkerLengthRate, out diamondCorners, out diamondIds);
        }
        else
        {
          Aruco.DetectCharucoDiamond(image, markerTracker.MarkerCorners[cameraId][dictionary], markerTracker.MarkerIds[cameraId][dictionary],
            DetectSquareMarkerLengthRate, out diamondCorners, out diamondIds, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId],
            arucoCameraUndistortion.UndistortedDistCoeffs[cameraId]);
        }
      }

      DiamondCorners[cameraId][dictionary] = diamondCorners;
      DiamondIds[cameraId][dictionary] = diamondIds;
      DetectedDiamonds[cameraId][dictionary] = (diamondIds != null) ? (int)diamondIds.Size() : 0;
    }

    public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
    {
      base.Draw(cameraId, dictionary, image);

      if (DetectedDiamonds[cameraId][dictionary] > 0)
      {
        // Draw detected diamonds
        if (arucoTracker.DrawDetectedDiamonds)
        {
          Aruco.DrawDetectedDiamonds(image, DiamondCorners[cameraId][dictionary], DiamondIds[cameraId][dictionary]);
        }

        // Draw axes of detected diamonds
        if (arucoTracker.DrawAxes && arucoCameraUndistortion != null && DiamondRvecs[cameraId][dictionary] != null)
        {
          for (uint i = 0; i < DetectedDiamonds[cameraId][dictionary]; i++)
          {
            Aruco.DrawAxis(image, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId], arucoCameraUndistortion.UndistortedDistCoeffs[cameraId],
            DiamondRvecs[cameraId][dictionary].At(i), DiamondTvecs[cameraId][dictionary].At(i), DrawAxisLength);
          }
        }
      }
    }

    public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.EstimateTransforms(cameraId, dictionary);

      Std.VectorVec3d diamondRvecs = null, diamondTvecs = null;

      if (DetectedDiamonds[cameraId][dictionary] > 0 && arucoCameraUndistortion != null)
      {
        Aruco.EstimatePoseSingleMarkers(DiamondCorners[cameraId][dictionary], EstimatePoseSquareLength, arucoCameraUndistortion.RectifiedCameraMatrices[cameraId],
          arucoCameraUndistortion.UndistortedDistCoeffs[cameraId], out diamondRvecs, out diamondTvecs);
      }

      DiamondRvecs[cameraId][dictionary] = diamondRvecs;
      DiamondTvecs[cameraId][dictionary] = diamondTvecs;
    }

    public override void UpdateTransforms(int cameraId, Aruco.Dictionary dictionary)
    {
      base.UpdateTransforms(cameraId, dictionary);

      if (DiamondRvecs[cameraId][dictionary] != null)
      {
        for (uint i = 0; i < DetectedDiamonds[cameraId][dictionary]; i++)
        {
          ArucoDiamond foundArucoDiamond;
          if (TryGetArucoDiamond(cameraId, dictionary, i, out foundArucoDiamond))
          {
            float positionFactor = foundArucoDiamond.SquareSideLength * EstimatePoseSquareLength / DetectSquareMarkerLengthRate; // Equal to marker length
            arucoCameraDisplay.PlaceArucoObject(foundArucoDiamond.transform, cameraId,
              arucoTracker.MarkerTracker.MarkerTvecs[cameraId][dictionary].At(i).ToPosition() * positionFactor,
              arucoTracker.MarkerTracker.MarkerRvecs[cameraId][dictionary].At(i).ToRotation());
          }
        }
      }
    }

    protected virtual bool TryGetArucoDiamond(int cameraId, Aruco.Dictionary dictionary, uint arucoObjectId, out ArucoDiamond arucoDiamond)
    {
      int[] detectedDiamondIds = new int[4];
      for (int j = 0; j < 4; j++)
      {
        detectedDiamondIds[j] = DiamondIds[cameraId][dictionary].At(arucoObjectId).Get(j);
      }

      ArucoObject foundArucoObject;
      int detectedDiamondHashCode = ArucoDiamond.GetArucoHashCode(detectedDiamondIds);
      if (arucoTracker.ArucoObjects[dictionary].TryGetValue(detectedDiamondHashCode, out foundArucoObject))
      {
        arucoDiamond = foundArucoObject as ArucoDiamond;
        if (arucoDiamond != null)
        {
          return true;
        }
      }

      arucoDiamond = null;
      return false;
    }
  }
}