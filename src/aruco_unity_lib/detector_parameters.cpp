#include "aruco_unity/detector_parameters.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Ptr<cv::aruco::DetectorParameters>* auCreateDetectorParameters() {
    cv::Ptr<cv::aruco::DetectorParameters> ptr = cv::aruco::DetectorParameters::create();
    return new cv::Ptr<cv::aruco::DetectorParameters>(ptr);
  }

  void auDeleteDetectorParameters(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    delete parameters;
  }

  // Variables
  int auGetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMin;
  }

  void auSetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMin) {
    parameters->get()->adaptiveThreshWinSizeMin = adaptiveThreshWinSizeMin;
  }

  int auGetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMax;
  }

  void auSetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMax) {
    parameters->get()->adaptiveThreshWinSizeMax = adaptiveThreshWinSizeMax;
  }

  int auGetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeStep;
  }

  void auSetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeStep) {
    parameters->get()->adaptiveThreshWinSizeStep = adaptiveThreshWinSizeStep;
  }

  double auGetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshConstant;
  }

  void auSetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double adaptiveThreshConstant) {
    parameters->get()->adaptiveThreshConstant = adaptiveThreshConstant;
  }

  double auGetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerPerimeterRate;
  }

  void auSetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerPerimeterRate) {
    parameters->get()->minMarkerPerimeterRate = minMarkerPerimeterRate;
  }

  double auGetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxMarkerPerimeterRate;
  }

  void auSetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxMarkerPerimeterRate) {
    parameters->get()->maxMarkerPerimeterRate = maxMarkerPerimeterRate;
  }

  double auGetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->polygonalApproxAccuracyRate;
  }

  void auSetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double polygonalApproxAccuracyRate) {
    parameters->get()->polygonalApproxAccuracyRate = polygonalApproxAccuracyRate;
  }

  double auGetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minCornerDistanceRate;
  }

  void auSetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minCornerDistanceRate) {
    parameters->get()->minCornerDistanceRate = minCornerDistanceRate;
  }

  int auGetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minDistanceToBorder;
  }

  void auSetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int minDistanceToBorder) {
    parameters->get()->minDistanceToBorder = minDistanceToBorder;
  }

  double auGetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerDistanceRate;
  }

  void auSetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerDistanceRate) {
    parameters->get()->minMarkerDistanceRate = minMarkerDistanceRate;
  }

  bool auGetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->doCornerRefinement;
  }

  void auSetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters, bool doCornerRefinement) {
    parameters->get()->doCornerRefinement = doCornerRefinement;
  }

  int auGetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementWinSize;
  }

  void auSetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementWinSize) {
    parameters->get()->cornerRefinementWinSize = cornerRefinementWinSize;
  }

  int auGetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMaxIterations;
  }

  void auSetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementMaxIterations) {
    parameters->get()->cornerRefinementMaxIterations = cornerRefinementMaxIterations;
  }

  double auGetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMinAccuracy;
  }

  void auSetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double cornerRefinementMinAccuracy) {
    parameters->get()->cornerRefinementMinAccuracy = cornerRefinementMinAccuracy;
  }

  int auGetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->markerBorderBits;
  }

  void auSetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int markerBorderBits) {
    parameters->get()->markerBorderBits = markerBorderBits;
  }

  int auGetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemovePixelPerCell;
  }

  void auSetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int perspectiveRemovePixelPerCell) {
    parameters->get()->perspectiveRemovePixelPerCell = perspectiveRemovePixelPerCell;
  }

  double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemoveIgnoredMarginPerCell;
  }

  void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double perspectiveRemoveIgnoredMarginPerCell) {
    parameters->get()->perspectiveRemoveIgnoredMarginPerCell = perspectiveRemoveIgnoredMarginPerCell;
  }

  double auGetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxErroneousBitsInBorderRate;
  }

  void auSetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxErroneousBitsInBorderRate) {
    parameters->get()->maxErroneousBitsInBorderRate = maxErroneousBitsInBorderRate;
  }

  double auGetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minOtsuStdDev;
  }

  void auSetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minOtsuStdDev) {
    parameters->get()->minOtsuStdDev = minOtsuStdDev;
  }

  double auGetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->errorCorrectionRate;
  }

  void auSetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double errorCorrectionRate) {
    parameters->get()->errorCorrectionRate = errorCorrectionRate;
  }
}