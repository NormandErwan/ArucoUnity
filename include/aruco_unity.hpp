#ifndef _ARUCO_UNITY_HPP_
#define _ARUCO_UNITY_HPP_

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API void* getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);
  ARUCO_UNITY_API void destroyDictionary(void* dictionary);
}

#endif