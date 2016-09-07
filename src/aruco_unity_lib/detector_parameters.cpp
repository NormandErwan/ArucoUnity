#include "aruco_unity/detector_parameters.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(cv::aruco::DetectorParameters* parameters) {
    return parameters->adaptiveThreshWinSizeMin;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(cv::aruco::DetectorParameters* parameters, int adaptiveThreshWinSizeMin) {
    parameters->adaptiveThreshWinSizeMin = adaptiveThreshWinSizeMin;
  }

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(cv::aruco::DetectorParameters* parameters) {
    return parameters->adaptiveThreshWinSizeMax;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(cv::aruco::DetectorParameters* parameters, int adaptiveThreshWinSizeMax) {
    parameters->adaptiveThreshWinSizeMax = adaptiveThreshWinSizeMax;
  }

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(cv::aruco::DetectorParameters* parameters) {
    return parameters->adaptiveThreshWinSizeStep;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(cv::aruco::DetectorParameters* parameters, int adaptiveThreshWinSizeStep) {
    parameters->adaptiveThreshWinSizeStep = adaptiveThreshWinSizeStep;
  }

  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(cv::aruco::DetectorParameters* parameters) {
    return parameters->adaptiveThreshConstant;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(cv::aruco::DetectorParameters* parameters, double adaptiveThreshConstant) {
    parameters->adaptiveThreshConstant = adaptiveThreshConstant;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->minMarkerPerimeterRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters, double minMarkerPerimeterRate) {
    parameters->minMarkerPerimeterRate = minMarkerPerimeterRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->maxMarkerPerimeterRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters, double maxMarkerPerimeterRate) {
    parameters->maxMarkerPerimeterRate = maxMarkerPerimeterRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->polygonalApproxAccuracyRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(cv::aruco::DetectorParameters* parameters, double polygonalApproxAccuracyRate) {
    parameters->polygonalApproxAccuracyRate = polygonalApproxAccuracyRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->minCornerDistanceRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(cv::aruco::DetectorParameters* parameters, double minCornerDistanceRate) {
    parameters->minCornerDistanceRate = minCornerDistanceRate;
  }

  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(cv::aruco::DetectorParameters* parameters) {
    return parameters->minDistanceToBorder;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(cv::aruco::DetectorParameters* parameters, int minDistanceToBorder) {
    parameters->minDistanceToBorder = minDistanceToBorder;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->minMarkerDistanceRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(cv::aruco::DetectorParameters* parameters, double minMarkerDistanceRate) {
    parameters->minMarkerDistanceRate = minMarkerDistanceRate;
  }

  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(cv::aruco::DetectorParameters* parameters) {
    return parameters->doCornerRefinement;
  }

  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(cv::aruco::DetectorParameters* parameters, bool doCornerRefinement) {
    parameters->doCornerRefinement = doCornerRefinement;
  }

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(cv::aruco::DetectorParameters* parameters) {
    return parameters->cornerRefinementWinSize;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(cv::aruco::DetectorParameters* parameters, int cornerRefinementWinSize) {
    parameters->cornerRefinementWinSize = cornerRefinementWinSize;
  }

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(cv::aruco::DetectorParameters* parameters) {
    return parameters->cornerRefinementMaxIterations;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(cv::aruco::DetectorParameters* parameters, int cornerRefinementMaxIterations) {
    parameters->cornerRefinementMaxIterations = cornerRefinementMaxIterations;
  }

  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(cv::aruco::DetectorParameters* parameters) {
    return parameters->cornerRefinementMinAccuracy;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(cv::aruco::DetectorParameters* parameters, double cornerRefinementMinAccuracy) {
    parameters->cornerRefinementMinAccuracy = cornerRefinementMinAccuracy;
  }

  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(cv::aruco::DetectorParameters* parameters) {
    return parameters->markerBorderBits;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(cv::aruco::DetectorParameters* parameters, int markerBorderBits) {
    parameters->markerBorderBits = markerBorderBits;
  }

  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(cv::aruco::DetectorParameters* parameters) {
    return parameters->perspectiveRemovePixelPerCell;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(cv::aruco::DetectorParameters* parameters, int perspectiveRemovePixelPerCell) {
    parameters->perspectiveRemovePixelPerCell = perspectiveRemovePixelPerCell;
  }

  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::aruco::DetectorParameters* parameters) {
    return parameters->perspectiveRemoveIgnoredMarginPerCell;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::aruco::DetectorParameters* parameters, double perspectiveRemoveIgnoredMarginPerCell) {
    parameters->perspectiveRemoveIgnoredMarginPerCell = perspectiveRemoveIgnoredMarginPerCell;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->maxErroneousBitsInBorderRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(cv::aruco::DetectorParameters* parameters, double maxErroneousBitsInBorderRate) {
    parameters->maxErroneousBitsInBorderRate = maxErroneousBitsInBorderRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(cv::aruco::DetectorParameters* parameters) {
    return parameters->minOtsuStdDev;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(cv::aruco::DetectorParameters* parameters, double minOtsuStdDev) {
    parameters->minOtsuStdDev = minOtsuStdDev;
  }

  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(cv::aruco::DetectorParameters* parameters) {
    return parameters->errorCorrectionRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(cv::aruco::DetectorParameters* parameters, double errorCorrectionRate) {
    parameters->errorCorrectionRate = errorCorrectionRate;
  }
}