#ifndef _ARUCO_UNITY_DICTIONARY_HPP_
#define _ARUCO_UNITY_DICTIONARY_HPP_

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API int auGetDictionaryMarkerSize(void* dictionary);
}

#endif