using UnityEngine;

public class ArucoUnityManager : MonoBehaviour {

  public GameObject markerPlane;
  
  ArucoUnity.Dictionary dictionary;
  ArucoUnity.DetectorParameters detectorParameters;
  ArucoUnity.Mat marker;

  void Start()
  {
    dictionary = ArucoUnity.GetPredefinedDictionary(ArucoUnity.PREDEFINED_DICTIONARY_NAME.DICT_4X4_100);
    testDictionary(dictionary);

    detectorParameters = ArucoUnity.CreateDetectorParameters();
    testDetectorParameters(detectorParameters);

    marker = new ArucoUnity.Mat();
    testMarker();
  }

  void testMarker()
  {
    int markerId = 1,
        markerSize = 100,
        markerBorderBits = 1;
    dictionary.DrawMarker(markerId, markerSize, ref marker, markerBorderBits);

    Texture2D markerPlaneTexture = new Texture2D(markerSize, markerSize, TextureFormat.RGB24, false);
    markerPlane.GetComponent<Renderer>().material.mainTexture = markerPlaneTexture;

    int markerDataSize = (int)(marker.ElemSize() * marker.Total());
    markerPlaneTexture.LoadRawTextureData(marker.data, markerDataSize);
    markerPlaneTexture.Apply();
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
