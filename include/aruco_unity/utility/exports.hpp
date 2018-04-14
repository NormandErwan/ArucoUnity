#ifndef __ARUCO_UNITY_PLUGIN_EXPORTS_HPP__
#define __ARUCO_UNITY_PLUGIN_EXPORTS_HPP__

#ifdef WIN32
  #ifdef ArucoUnityPlugin_EXPORTS
    #define ARUCO_UNITY_API __declspec(dllexport)
  #else
    #define ARUCO_UNITY_API __declspec(dllimport)
  #endif
#else
  #define ARUCO_UNITY_API
#endif

#endif