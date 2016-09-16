#ifndef __ARUCO_UNITY_MAT_HPP__
#define __ARUCO_UNITY_MAT_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  ARUCO_UNITY_API cv::Mat* auNewMat();
  ARUCO_UNITY_API void auDeleteMat(cv::Mat* mat);

  ARUCO_UNITY_API size_t auMatElemSize(cv::Mat* mat);
  ARUCO_UNITY_API size_t auMatTotal(cv::Mat* mat);

  ARUCO_UNITY_API uchar* auGetMatData(cv::Mat* mat);
}

#endif