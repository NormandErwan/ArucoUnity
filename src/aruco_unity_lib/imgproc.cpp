#include "aruco_unity/utility/imgproc.hpp"
#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Static member functions
  void au_imgproc_undistord2(cv::Mat* src, cv::Mat** dst, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception) {
    try {
      *dst = new cv::Mat();

      cv::undistort(*src, **dst, *cameraMatrix, *distCoeffs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }
}