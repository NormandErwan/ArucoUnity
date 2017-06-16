using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.ObjectTrackers
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
        for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CameraNumber; cameraId++)
        {
          DiamondIds[cameraId].Add(dictionary, new Std.VectorVec4i());
          DetectedDiamonds[cameraId].Add(dictionary, 0);
          DiamondRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
          DiamondTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
        }
      }

      protected override void ArucoObjectsController_DictionaryRemoved(Aruco.Dictionary dictionary)
      {
        for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CameraNumber; cameraId++)
        {
          DiamondIds[cameraId].Remove(dictionary);
          DetectedDiamonds[cameraId].Remove(dictionary);
          DiamondRvecs[cameraId].Remove(dictionary);
          DiamondTvecs[cameraId].Remove(dictionary);
        }
      }

      // ArucoObjectTracker methods

      public override void Activate(ArucoTracker arucoTracker)
      {
        base.Activate(arucoTracker);

        DiamondCorners = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[arucoTracker.ArucoCamera.CameraNumber];
        DiamondIds = new Dictionary<Aruco.Dictionary, Std.VectorVec4i>[arucoTracker.ArucoCamera.CameraNumber];
        DetectedDiamonds = new Dictionary<Aruco.Dictionary, int>[arucoTracker.ArucoCamera.CameraNumber];
        DiamondRvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoTracker.ArucoCamera.CameraNumber];
        DiamondTvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoTracker.ArucoCamera.CameraNumber];

        for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CameraNumber; cameraId++)
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
        ArucoMarkerTracker markerTracker = arucoTracker.MarkerTracker;

        Std.VectorVectorPoint2f diamondCorners = null;
        Std.VectorVec4i diamondIds = null;

        if (markerTracker.DetectedMarkers[cameraId][dictionary] > 0)
        {
          if (cameraParameters == null)
          {
            Aruco.DetectCharucoDiamond(image, markerTracker.MarkerCorners[cameraId][dictionary], markerTracker.MarkerIds[cameraId][dictionary],
              DetectSquareMarkerLengthRate, out diamondCorners, out diamondIds);
          }
          else
          {
            Aruco.DetectCharucoDiamond(image, markerTracker.MarkerCorners[cameraId][dictionary], markerTracker.MarkerIds[cameraId][dictionary],
              DetectSquareMarkerLengthRate, out diamondCorners, out diamondIds, cameraParameters.CameraMatrices[cameraId],
              cameraParameters.DistCoeffs[cameraId]);
          }
        }

        DiamondCorners[cameraId][dictionary] = diamondCorners;
        DiamondIds[cameraId][dictionary] = diamondIds;
        DetectedDiamonds[cameraId][dictionary] = (diamondIds != null) ? (int)diamondIds.Size() : 0;
      }

      public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
      {
        // TODO: add autoscale feature (see: https://github.com/opencv/opencv_contrib/blob/master/modules/aruco/samples/detect_diamonds.cpp#L203)
        Std.VectorVec3d diamondRvecs = null, diamondTvecs = null;

        if (DetectedDiamonds[cameraId][dictionary] > 0 && cameraParameters != null)
        {
          Aruco.EstimatePoseSingleMarkers(DiamondCorners[cameraId][dictionary], EstimatePoseSquareLength, cameraParameters.CameraMatrices[cameraId],
            cameraParameters.DistCoeffs[cameraId], out diamondRvecs, out diamondTvecs);
        }

        DiamondRvecs[cameraId][dictionary] = diamondRvecs;
        DiamondTvecs[cameraId][dictionary] = diamondTvecs;
      }

      public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        if (DetectedDiamonds[cameraId][dictionary] > 0)
        {
          // Draw detected diamonds
          if (arucoTracker.DrawDetectedDiamonds)
          {
            Aruco.DrawDetectedDiamonds(image, DiamondCorners[cameraId][dictionary], DiamondIds[cameraId][dictionary]);
          }

          // Draw axes of detected diamonds
          if (arucoTracker.DrawAxes && cameraParameters != null && DiamondRvecs[cameraId][dictionary] != null)
          {
            for (uint i = 0; i < DetectedDiamonds[cameraId][dictionary]; i++)
            {
              Aruco.DrawAxis(image, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
              DiamondRvecs[cameraId][dictionary].At(i), DiamondTvecs[cameraId][dictionary].At(i), DrawAxisLength);
            }
          }
        }
      }

      public override void Place(int cameraId, Aruco.Dictionary dictionary)
      {
        if (DiamondRvecs[cameraId][dictionary] != null)
        {
          for (uint i = 0; i < DetectedDiamonds[cameraId][dictionary]; i++)
          {
            ArucoDiamond foundArucoDiamond;
            if (TryGetArucoDiamond(cameraId, dictionary, i, out foundArucoDiamond))
            {
              float positionFactor = foundArucoDiamond.SquareSideLength * EstimatePoseSquareLength / DetectSquareMarkerLengthRate; // Equal to marker lenght
              PlaceArucoObject(foundArucoDiamond, arucoTracker.MarkerTracker.MarkerRvecs[cameraId][dictionary].At(i),
                arucoTracker.MarkerTracker.MarkerTvecs[cameraId][dictionary].At(i), cameraId, positionFactor);
            }
          }
        }
      }

      protected bool TryGetArucoDiamond(int cameraId, Aruco.Dictionary dictionary, uint arucoObjectId, out ArucoDiamond arucoDiamond)
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

  /// \} aruco_unity_package
}