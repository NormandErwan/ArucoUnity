#ifndef __ARUCO_UNITY_EXPORTS_HPP__
#define __ARUCO_UNITY_EXPORTS_HPP__

#ifdef WIN32
  #ifdef ArucoUnity_EXPORTS
    #define ARUCO_UNITY_API __declspec(dllexport)
  #else
    #define ARUCO_UNITY_API __declspec(dllimport)
  #endif
#else
  #define ARUCO_UNITY_API
#endif

#endif