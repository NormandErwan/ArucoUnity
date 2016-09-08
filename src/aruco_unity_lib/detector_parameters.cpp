#include "aruco_unity/detector_parameters.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMin;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMin) {
    parameters->get()->adaptiveThreshWinSizeMin = adaptiveThreshWinSizeMin;
  }

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMax;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMax) {
    parameters->get()->adaptiveThreshWinSizeMax = adaptiveThreshWinSizeMax;
  }

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeStep;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeStep) {
    parameters->get()->adaptiveThreshWinSizeStep = adaptiveThreshWinSizeStep;
  }

  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshConstant;
  }

  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double adaptiveThreshConstant) {
    parameters->get()->adaptiveThreshConstant = adaptiveThreshConstant;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerPerimeterRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerPerimeterRate) {
    parameters->get()->minMarkerPerimeterRate = minMarkerPerimeterRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxMarkerPerimeterRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxMarkerPerimeterRate) {
    parameters->get()->maxMarkerPerimeterRate = maxMarkerPerimeterRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->polygonalApproxAccuracyRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double polygonalApproxAccuracyRate) {
    parameters->get()->polygonalApproxAccuracyRate = polygonalApproxAccuracyRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minCornerDistanceRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minCornerDistanceRate) {
    parameters->get()->minCornerDistanceRate = minCornerDistanceRate;
  }

  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minDistanceToBorder;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int minDistanceToBorder) {
    parameters->get()->minDistanceToBorder = minDistanceToBorder;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerDistanceRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerDistanceRate) {
    parameters->get()->minMarkerDistanceRate = minMarkerDistanceRate;
  }

  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->doCornerRefinement;
  }

  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters, bool doCornerRefinement) {
    parameters->get()->doCornerRefinement = doCornerRefinement;
  }

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementWinSize;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementWinSize) {
    parameters->get()->cornerRefinementWinSize = cornerRefinementWinSize;
  }

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMaxIterations;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementMaxIterations) {
    parameters->get()->cornerRefinementMaxIterations = cornerRefinementMaxIterations;
  }

  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMinAccuracy;
  }

  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double cornerRefinementMinAccuracy) {
    parameters->get()->cornerRefinementMinAccuracy = cornerRefinementMinAccuracy;
  }

  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->markerBorderBits;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int markerBorderBits) {
    parameters->get()->markerBorderBits = markerBorderBits;
  }

  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemovePixelPerCell;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int perspectiveRemovePixelPerCell) {
    parameters->get()->perspectiveRemovePixelPerCell = perspectiveRemovePixelPerCell;
  }

  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemoveIgnoredMarginPerCell;
  }

  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double perspectiveRemoveIgnoredMarginPerCell) {
    parameters->get()->perspectiveRemoveIgnoredMarginPerCell = perspectiveRemoveIgnoredMarginPerCell;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxErroneousBitsInBorderRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxErroneousBitsInBorderRate) {
    parameters->get()->maxErroneousBitsInBorderRate = maxErroneousBitsInBorderRate;
  }

  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minOtsuStdDev;
  }

  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minOtsuStdDev) {
    parameters->get()->minOtsuStdDev = minOtsuStdDev;
  }

  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->errorCorrectionRate;
  }

  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double errorCorrectionRate) {
    parameters->get()->errorCorrectionRate = errorCorrectionRate;
  }
}