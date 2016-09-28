#include "aruco_unity/detector_parameters.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Ptr<cv::aruco::DetectorParameters>* au_DetectorParameters_create() {
    cv::Ptr<cv::aruco::DetectorParameters> ptr = cv::aruco::DetectorParameters::create();
    return new cv::Ptr<cv::aruco::DetectorParameters>(ptr);
  }

  void au_DetectorParameters_delete(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    delete parameters;
  }

  // Variables
  int au_DetectorParameters_getAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMin;
  }

  void au_DetectorParameters_setAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMin) {
    parameters->get()->adaptiveThreshWinSizeMin = adaptiveThreshWinSizeMin;
  }

  int au_DetectorParameters_getAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMax;
  }

  void au_DetectorParameters_setAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMax) {
    parameters->get()->adaptiveThreshWinSizeMax = adaptiveThreshWinSizeMax;
  }

  int au_DetectorParameters_getAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeStep;
  }

  void au_DetectorParameters_setAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeStep) {
    parameters->get()->adaptiveThreshWinSizeStep = adaptiveThreshWinSizeStep;
  }

  double au_DetectorParameters_getAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshConstant;
  }

  void au_DetectorParameters_setAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double adaptiveThreshConstant) {
    parameters->get()->adaptiveThreshConstant = adaptiveThreshConstant;
  }

  double au_DetectorParameters_getMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerPerimeterRate;
  }

  void au_DetectorParameters_setMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerPerimeterRate) {
    parameters->get()->minMarkerPerimeterRate = minMarkerPerimeterRate;
  }

  double au_DetectorParameters_getMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxMarkerPerimeterRate;
  }

  void au_DetectorParameters_setMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxMarkerPerimeterRate) {
    parameters->get()->maxMarkerPerimeterRate = maxMarkerPerimeterRate;
  }

  double au_DetectorParameters_getPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->polygonalApproxAccuracyRate;
  }

  void au_DetectorParameters_setPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double polygonalApproxAccuracyRate) {
    parameters->get()->polygonalApproxAccuracyRate = polygonalApproxAccuracyRate;
  }

  double au_DetectorParameters_getMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minCornerDistanceRate;
  }

  void au_DetectorParameters_setMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minCornerDistanceRate) {
    parameters->get()->minCornerDistanceRate = minCornerDistanceRate;
  }

  int au_DetectorParameters_getMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minDistanceToBorder;
  }

  void au_DetectorParameters_setMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int minDistanceToBorder) {
    parameters->get()->minDistanceToBorder = minDistanceToBorder;
  }

  double au_DetectorParameters_getMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerDistanceRate;
  }

  void au_DetectorParameters_setMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerDistanceRate) {
    parameters->get()->minMarkerDistanceRate = minMarkerDistanceRate;
  }

  bool au_DetectorParameters_getDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->doCornerRefinement;
  }

  void au_DetectorParameters_setDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters, bool doCornerRefinement) {
    parameters->get()->doCornerRefinement = doCornerRefinement;
  }

  int au_DetectorParameters_getCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementWinSize;
  }

  void au_DetectorParameters_setCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementWinSize) {
    parameters->get()->cornerRefinementWinSize = cornerRefinementWinSize;
  }

  int au_DetectorParameters_getCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMaxIterations;
  }

  void au_DetectorParameters_setCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementMaxIterations) {
    parameters->get()->cornerRefinementMaxIterations = cornerRefinementMaxIterations;
  }

  double au_DetectorParameters_getCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMinAccuracy;
  }

  void au_DetectorParameters_setCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double cornerRefinementMinAccuracy) {
    parameters->get()->cornerRefinementMinAccuracy = cornerRefinementMinAccuracy;
  }

  int au_DetectorParameters_getMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->markerBorderBits;
  }

  void au_DetectorParameters_setMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int markerBorderBits) {
    parameters->get()->markerBorderBits = markerBorderBits;
  }

  int au_DetectorParameters_getPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemovePixelPerCell;
  }

  void au_DetectorParameters_setPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int perspectiveRemovePixelPerCell) {
    parameters->get()->perspectiveRemovePixelPerCell = perspectiveRemovePixelPerCell;
  }

  double au_DetectorParameters_getPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemoveIgnoredMarginPerCell;
  }

  void au_DetectorParameters_setPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double perspectiveRemoveIgnoredMarginPerCell) {
    parameters->get()->perspectiveRemoveIgnoredMarginPerCell = perspectiveRemoveIgnoredMarginPerCell;
  }

  double au_DetectorParameters_getMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxErroneousBitsInBorderRate;
  }

  void au_DetectorParameters_setMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxErroneousBitsInBorderRate) {
    parameters->get()->maxErroneousBitsInBorderRate = maxErroneousBitsInBorderRate;
  }

  double au_DetectorParameters_getMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minOtsuStdDev;
  }

  void au_DetectorParameters_setMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minOtsuStdDev) {
    parameters->get()->minOtsuStdDev = minOtsuStdDev;
  }

  double au_DetectorParameters_getErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->errorCorrectionRate;
  }

  void au_DetectorParameters_setErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double errorCorrectionRate) {
    parameters->get()->errorCorrectionRate = errorCorrectionRate;
  }
}