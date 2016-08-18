#ifndef _ARUCO_UNITY_HPP_
#define _ARUCO_UNITY_HPP_

#include <opencv2/aruco.hpp>

#ifdef WIN32
  #ifdef aruco_unity_EXPORTS
    #define ARUCO_UNITY_API __declspec(dllexport)
  #else
    #define ARUCO_UNITY_API __declspec(dllimport)
  #endif
#else
  #define ARUCO_UNITY_API
#endif

extern "C" {
  ARUCO_UNITY_API void* createMat(int i);
  ARUCO_UNITY_API void destroyMat(void* mat);
  ARUCO_UNITY_API int displayMat(void* mat);
}

#endif