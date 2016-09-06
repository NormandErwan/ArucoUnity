#ifndef __ARUCO_UNITY_HPP__
#define __ARUCO_UNITY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  // Detector parameters
  ARUCO_UNITY_API void* auCreateDetectorParameters();
  ARUCO_UNITY_API void auDestroyDetectorParameters(void* parameters);

  // Dictionnary
  ARUCO_UNITY_API void* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);
  ARUCO_UNITY_API void auDestroyDictionary(void* dictionary);
}

#endif