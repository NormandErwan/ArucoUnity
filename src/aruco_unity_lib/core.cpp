#include "aruco_unity/cv/core.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  void au_cv_core_flip(cv::Mat* src, cv::Mat* dst, int flipCode, cv::Exception* exception) {
    try {
      cv::flip(*src, *dst, flipCode);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }
}