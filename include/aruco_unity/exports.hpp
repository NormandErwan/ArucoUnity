#ifndef _ARUCO_UNITY_EXPORTS_HPP_
#define _ARUCO_UNITY_EXPORTS_HPP_

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