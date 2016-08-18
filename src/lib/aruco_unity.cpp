#include "aruco_unity.hpp"

extern "C" {

  void* createMat(int i) {
    cv::Mat* mat = new cv::Mat(2, 2, CV_32SC1, cv::Scalar(i));
    return mat;
  }

  void destroyMat(void* mat) {
    delete mat;
  }

  int displayMat(void* mat) {
    cv::Mat* _mat = static_cast<cv::Mat*>(mat);
    return _mat->at<int>(0,0);
  }

}