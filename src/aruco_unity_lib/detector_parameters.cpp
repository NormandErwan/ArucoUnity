#include "aruco_unity/detector_parameters.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Ptr<cv::aruco::DetectorParameters>* au_DetectorParameters_Create() {
    cv::Ptr<cv::aruco::DetectorParameters> ptr = cv::aruco::DetectorParameters::create();
    return new cv::Ptr<cv::aruco::DetectorParameters>(ptr);
  }

  void au_DetectorParameters_Delete(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    delete parameters;
  }

  // Variables
  int au_DetectorParameters_GetAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMin;
  }

  void au_DetectorParameters_SetAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMin) {
    parameters->get()->adaptiveThreshWinSizeMin = adaptiveThreshWinSizeMin;
  }

  int au_DetectorParameters_GetAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeMax;
  }

  void au_DetectorParameters_SetAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMax) {
    parameters->get()->adaptiveThreshWinSizeMax = adaptiveThreshWinSizeMax;
  }

  int au_DetectorParameters_GetAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshWinSizeStep;
  }

  void au_DetectorParameters_SetAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeStep) {
    parameters->get()->adaptiveThreshWinSizeStep = adaptiveThreshWinSizeStep;
  }

  double au_DetectorParameters_GetAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->adaptiveThreshConstant;
  }

  void au_DetectorParameters_SetAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double adaptiveThreshConstant) {
    parameters->get()->adaptiveThreshConstant = adaptiveThreshConstant;
  }

  double au_DetectorParameters_GetMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerPerimeterRate;
  }

  void au_DetectorParameters_SetMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerPerimeterRate) {
    parameters->get()->minMarkerPerimeterRate = minMarkerPerimeterRate;
  }

  double au_DetectorParameters_GetMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxMarkerPerimeterRate;
  }

  void au_DetectorParameters_SetMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxMarkerPerimeterRate) {
    parameters->get()->maxMarkerPerimeterRate = maxMarkerPerimeterRate;
  }

  double au_DetectorParameters_GetPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->polygonalApproxAccuracyRate;
  }

  void au_DetectorParameters_SetPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double polygonalApproxAccuracyRate) {
    parameters->get()->polygonalApproxAccuracyRate = polygonalApproxAccuracyRate;
  }

  double au_DetectorParameters_GetMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minCornerDistanceRate;
  }

  void au_DetectorParameters_SetMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minCornerDistanceRate) {
    parameters->get()->minCornerDistanceRate = minCornerDistanceRate;
  }

  int au_DetectorParameters_GetMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minDistanceToBorder;
  }

  void au_DetectorParameters_SetMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int minDistanceToBorder) {
    parameters->get()->minDistanceToBorder = minDistanceToBorder;
  }

  double au_DetectorParameters_GetMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minMarkerDistanceRate;
  }

  void au_DetectorParameters_SetMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerDistanceRate) {
    parameters->get()->minMarkerDistanceRate = minMarkerDistanceRate;
  }

  bool au_DetectorParameters_GetDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->doCornerRefinement;
  }

  void au_DetectorParameters_SetDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters, bool doCornerRefinement) {
    parameters->get()->doCornerRefinement = doCornerRefinement;
  }

  int au_DetectorParameters_GetCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementWinSize;
  }

  void au_DetectorParameters_SetCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementWinSize) {
    parameters->get()->cornerRefinementWinSize = cornerRefinementWinSize;
  }

  int au_DetectorParameters_GetCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMaxIterations;
  }

  void au_DetectorParameters_SetCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementMaxIterations) {
    parameters->get()->cornerRefinementMaxIterations = cornerRefinementMaxIterations;
  }

  double au_DetectorParameters_GetCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->cornerRefinementMinAccuracy;
  }

  void au_DetectorParameters_SetCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double cornerRefinementMinAccuracy) {
    parameters->get()->cornerRefinementMinAccuracy = cornerRefinementMinAccuracy;
  }

  int au_DetectorParameters_GetMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->markerBorderBits;
  }

  void au_DetectorParameters_SetMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int markerBorderBits) {
    parameters->get()->markerBorderBits = markerBorderBits;
  }

  int au_DetectorParameters_GetPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemovePixelPerCell;
  }

  void au_DetectorParameters_SetPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int perspectiveRemovePixelPerCell) {
    parameters->get()->perspectiveRemovePixelPerCell = perspectiveRemovePixelPerCell;
  }

  double au_DetectorParameters_GetPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->perspectiveRemoveIgnoredMarginPerCell;
  }

  void au_DetectorParameters_SetPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double perspectiveRemoveIgnoredMarginPerCell) {
    parameters->get()->perspectiveRemoveIgnoredMarginPerCell = perspectiveRemoveIgnoredMarginPerCell;
  }

  double au_DetectorParameters_GetMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->maxErroneousBitsInBorderRate;
  }

  void au_DetectorParameters_SetMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxErroneousBitsInBorderRate) {
    parameters->get()->maxErroneousBitsInBorderRate = maxErroneousBitsInBorderRate;
  }

  double au_DetectorParameters_GetMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->minOtsuStdDev;
  }

  void au_DetectorParameters_SetMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minOtsuStdDev) {
    parameters->get()->minOtsuStdDev = minOtsuStdDev;
  }

  double au_DetectorParameters_GetErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    return parameters->get()->errorCorrectionRate;
  }

  void au_DetectorParameters_SetErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double errorCorrectionRate) {
    parameters->get()->errorCorrectionRate = errorCorrectionRate;
  }
}