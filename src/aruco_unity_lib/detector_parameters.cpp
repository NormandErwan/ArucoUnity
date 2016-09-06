#include "aruco_unity/detector_parameters.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshWinSizeMin;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(void* parameters, int adaptiveThreshWinSizeMin) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshWinSizeMin = adaptiveThreshWinSizeMin;
  }

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshWinSizeMax;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(void* parameters, int adaptiveThreshWinSizeMax) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshWinSizeMax = adaptiveThreshWinSizeMax;
  }

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshWinSizeStep;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(void* parameters, int adaptiveThreshWinSizeStep) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshWinSizeStep = adaptiveThreshWinSizeStep;
  }

  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshConstant;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(void* parameters, double adaptiveThreshConstant) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->adaptiveThreshConstant = adaptiveThreshConstant;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->minMarkerPerimeterRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(void* parameters, double minMarkerPerimeterRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->minMarkerPerimeterRate = minMarkerPerimeterRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->maxMarkerPerimeterRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(void* parameters, double maxMarkerPerimeterRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->maxMarkerPerimeterRate = maxMarkerPerimeterRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->polygonalApproxAccuracyRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(void* parameters, double polygonalApproxAccuracyRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->polygonalApproxAccuracyRate = polygonalApproxAccuracyRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->minCornerDistanceRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(void* parameters, double minCornerDistanceRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->minCornerDistanceRate = minCornerDistanceRate;
  }

  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->minDistanceToBorder;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(void* parameters, int minDistanceToBorder) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->minDistanceToBorder = minDistanceToBorder;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->minMarkerDistanceRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(void* parameters, double minMarkerDistanceRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->minMarkerDistanceRate = minMarkerDistanceRate;
  }

  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->doCornerRefinement;
  }

  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(void* parameters, bool doCornerRefinement) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->doCornerRefinement = doCornerRefinement;
  }

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->cornerRefinementWinSize;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(void* parameters, int cornerRefinementWinSize) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->cornerRefinementWinSize = cornerRefinementWinSize;
  }

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->cornerRefinementMaxIterations;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(void* parameters, int cornerRefinementMaxIterations) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->cornerRefinementMaxIterations = cornerRefinementMaxIterations;
  }

  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->cornerRefinementMinAccuracy;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(void* parameters, double cornerRefinementMinAccuracy) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->cornerRefinementMinAccuracy = cornerRefinementMinAccuracy;
  }

  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->markerBorderBits;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(void* parameters, int markerBorderBits) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->markerBorderBits = markerBorderBits;
  }

  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->perspectiveRemovePixelPerCell;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(void* parameters, int perspectiveRemovePixelPerCell) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->perspectiveRemovePixelPerCell = perspectiveRemovePixelPerCell;
  }

  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->perspectiveRemoveIgnoredMarginPerCell;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(void* parameters, double perspectiveRemoveIgnoredMarginPerCell) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->perspectiveRemoveIgnoredMarginPerCell = perspectiveRemoveIgnoredMarginPerCell;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->maxErroneousBitsInBorderRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(void* parameters, double maxErroneousBitsInBorderRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->maxErroneousBitsInBorderRate = maxErroneousBitsInBorderRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->minOtsuStdDev;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(void* parameters, double minOtsuStdDev) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->minOtsuStdDev = minOtsuStdDev;
  }

  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(void* parameters) {
    return static_cast<cv::aruco::DetectorParameters*>(parameters)->errorCorrectionRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(void* parameters, double errorCorrectionRate) {
    static_cast<cv::aruco::DetectorParameters*>(parameters)->errorCorrectionRate = errorCorrectionRate;
  }
}