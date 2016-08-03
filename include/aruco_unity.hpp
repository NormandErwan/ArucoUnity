#include <iostream>
#include <aruco/aruco.h>
#include <aruco/cvdrawingutils.h>
#include <opencv2/highgui/highgui.hpp>

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
  ARUCO_UNITY_API int testMult(int a, int b);
  ARUCO_UNITY_API int opencvAbs(int n);
  ARUCO_UNITY_API void detectMarkers(char* filename);
}