#ifndef __ARUCO_UNITY_DICTIONARY_HPP__
#define __ARUCO_UNITY_DICTIONARY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDictionaryMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary);
}

#endif