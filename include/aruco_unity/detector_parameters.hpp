#ifndef __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__
#define __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(cv::aruco::DetectorParameters* parameters, int adaptiveThreshWinSizeMin);

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(cv::aruco::DetectorParameters* parameters, int adaptiveThreshWinSizeMax);

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(cv::aruco::DetectorParameters* parameters, int adaptiveThreshWinSizeStep);

  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(cv::aruco::DetectorParameters* parameters, double adaptiveThreshConstant);

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters, double minMarkerPerimeterRate);

  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(cv::aruco::DetectorParameters* parameters, double maxMarkerPerimeterRate);

  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(cv::aruco::DetectorParameters* parameters, double polygonalApproxAccuracyRate);

  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(cv::aruco::DetectorParameters* parameters, double minCornerDistanceRate);

  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(cv::aruco::DetectorParameters* parameters, int minDistanceToBorder);

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(cv::aruco::DetectorParameters* parameters, double minMarkerDistanceRate);

  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(cv::aruco::DetectorParameters* parameters, bool doCornerRefinement);

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(cv::aruco::DetectorParameters* parameters, int cornerRefinementWinSize);

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(cv::aruco::DetectorParameters* parameters, int cornerRefinementMaxIterations);

  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(cv::aruco::DetectorParameters* parameters, double cornerRefinementMinAccuracy);

  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(cv::aruco::DetectorParameters* parameters, int markerBorderBits);

  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(cv::aruco::DetectorParameters* parameters, int perspectiveRemovePixelPerCell);

  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::aruco::DetectorParameters* parameters, double perspectiveRemoveIgnoredMarginPerCell);

  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(cv::aruco::DetectorParameters* parameters, double maxErroneousBitsInBorderRate);

  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(cv::aruco::DetectorParameters* parameters, double minOtsuStdDev);

  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(cv::aruco::DetectorParameters* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(cv::aruco::DetectorParameters* parameters, double errorCorrectionRate);
}

#endif