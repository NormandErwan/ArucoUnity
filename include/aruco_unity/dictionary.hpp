#ifndef __ARUCO_UNITY_DICTIONARY_HPP__
#define __ARUCO_UNITY_DICTIONARY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  // Constructors
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);

  // Destructor
  ARUCO_UNITY_API void auDeleteDictionary(cv::Ptr<cv::aruco::Dictionary>* dictionary);

  // Functions
  ARUCO_UNITY_API void auDictionaryDrawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat* img, int borderBits);

  // Variables
  ARUCO_UNITY_API int auGetDictionaryMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary);
}

#endif