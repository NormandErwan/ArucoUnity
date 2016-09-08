#ifndef __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__
#define __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMin);

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMax);

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeStep);

  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double adaptiveThreshConstant);

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerPerimeterRate);

  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxMarkerPerimeterRate);

  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double polygonalApproxAccuracyRate);

  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minCornerDistanceRate);

  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int minDistanceToBorder);

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerDistanceRate);

  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters, bool doCornerRefinement);

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementWinSize);

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementMaxIterations);

  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double cornerRefinementMinAccuracy);

  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int markerBorderBits);

  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int perspectiveRemovePixelPerCell);

  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double perspectiveRemoveIgnoredMarginPerCell);

  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxErroneousBitsInBorderRate);

  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minOtsuStdDev);

  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double errorCorrectionRate);
}

#endif