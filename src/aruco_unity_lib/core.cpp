#include "aruco_unity/utility/cv/core.hpp"
#include "aruco_unity/utility/cv/exception.hpp"

extern "C" {
  // Static member functions
  void au_cv_core_flip(cv::Mat* src, cv::Mat* dst, int flipCode, cv::Exception* exception) {
    try {
      cv::flip(*src, *dst, flipCode);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }
}