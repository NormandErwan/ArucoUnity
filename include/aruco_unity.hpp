#ifndef __ARUCO_UNITY_HPP__
#define __ARUCO_UNITY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  // Detector parameters
  ARUCO_UNITY_API cv::aruco::DetectorParameters* auCreateDetectorParameters();
  ARUCO_UNITY_API void auDeleteDetectorParameters(cv::aruco::DetectorParameters* parameters);

  // Dictionary
  ARUCO_UNITY_API cv::aruco::Dictionary* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);
  ARUCO_UNITY_API void auDeleteDictionary(cv::aruco::Dictionary* dictionary);
}

#endif