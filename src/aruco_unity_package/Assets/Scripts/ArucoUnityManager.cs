using UnityEngine;

public class ArucoUnityManager : MonoBehaviour {

  ArucoUnity.Dictionary dictionary;
  ArucoUnity.DetectorParameters detectorParameters;

  void Start()
  {
    dictionary = ArucoUnity.GetPredefinedDictionary(ArucoUnity.PREDEFINED_DICTIONARY_NAME.DICT_4X4_100);
    testDictionary(dictionary);

    detectorParameters = ArucoUnity.CreateDetectorParameters();
    testDetectorParameters(detectorParameters);
  }

  void testDictionary(ArucoUnity.Dictionary dictionary) {
    print(dictionary.markerSize);
  }

  void testDetectorParameters(ArucoUnity.DetectorParameters detectorParameters) {
    detectorParameters.AdaptiveThreshWinSizeMin = 1;
    detectorParameters.AdaptiveThreshWinSizeMax = 2;
    detectorParameters.AdaptiveThreshWinSizeStep = 3;
    detectorParameters.AdaptiveThreshConstant = 0.1;
    detectorParameters.MinMarkerPerimeterRate = 0.2;
    detectorParameters.MaxMarkerPerimeterRate = 0.3;
    detectorParameters.PolygonalApproxAccuracyRate = 0.4;
    detectorParameters.MinCornerDistanceRate = 0.5;
    detectorParameters.MinDistanceToBorder = 4;
    detectorParameters.MinMarkerDistanceRate = 0.6;
    detectorParameters.DoCornerRefinement = true;
    detectorParameters.CornerRefinementWinSize = 5;
    detectorParameters.CornerRefinementMaxIterations = 6;
    detectorParameters.CornerRefinementMinAccuracy = 0.7;
    detectorParameters.MarkerBorderBits = 7;
    detectorParameters.PerspectiveRemovePixelPerCell = 8;
    detectorParameters.PerspectiveRemoveIgnoredMarginPerCell = 0.8;
    detectorParameters.MaxErroneousBitsInBorderRate = 1.0;
    detectorParameters.MinOtsuStdDev = -0.1;
    detectorParameters.ErrorCorrectionRate = -1.0;

    print(detectorParameters.AdaptiveThreshWinSizeMin);
    print(detectorParameters.AdaptiveThreshWinSizeMax);
    print(detectorParameters.AdaptiveThreshWinSizeStep);
    print(detectorParameters.AdaptiveThreshConstant);
    print(detectorParameters.MinMarkerPerimeterRate);
    print(detectorParameters.MaxMarkerPerimeterRate);
    print(detectorParameters.PolygonalApproxAccuracyRate);
    print(detectorParameters.MinCornerDistanceRate);
    print(detectorParameters.MinDistanceToBorder);
    print(detectorParameters.MinMarkerDistanceRate);
    print(detectorParameters.DoCornerRefinement);
    print(detectorParameters.CornerRefinementWinSize);
    print(detectorParameters.CornerRefinementMaxIterations);
    print(detectorParameters.CornerRefinementMinAccuracy);
    print(detectorParameters.MarkerBorderBits);
    print(detectorParameters.PerspectiveRemovePixelPerCell);
    print(detectorParameters.PerspectiveRemoveIgnoredMarginPerCell);
    print(detectorParameters.MaxErroneousBitsInBorderRate);
    print(detectorParameters.MinOtsuStdDev);
    print(detectorParameters.ErrorCorrectionRate);
  }
}
