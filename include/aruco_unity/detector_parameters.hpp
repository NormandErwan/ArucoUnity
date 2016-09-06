#ifndef __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__
#define __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(void* parameters, int adaptiveThreshWinSizeMin);

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(void* parameters, int adaptiveThreshWinSizeMax);

  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(void* parameters, int adaptiveThreshWinSizeStep);

  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(void* parameters, double adaptiveThreshConstant);

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(void* parameters, double minMarkerPerimeterRate);

  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(void* parameters, double maxMarkerPerimeterRate);

  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(void* parameters, double polygonalApproxAccuracyRate);

  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(void* parameters, double minCornerDistanceRate);

  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(void* parameters, int minDistanceToBorder);

  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(void* parameters, double minMarkerDistanceRate);

  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(void* parameters, bool doCornerRefinement);

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(void* parameters, int cornerRefinementWinSize);

  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(void* parameters, int cornerRefinementMaxIterations);

  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(void* parameters, double cornerRefinementMinAccuracy);

  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(void* parameters, int markerBorderBits);

  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(void* parameters, int perspectiveRemovePixelPerCell);

  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(void* parameters, double perspectiveRemoveIgnoredMarginPerCell);

  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(void* parameters, double maxErroneousBitsInBorderRate);

  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(void* parameters, double minOtsuStdDev);

  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(void* parameters);
  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(void* parameters, double errorCorrectionRate);
}

#endif