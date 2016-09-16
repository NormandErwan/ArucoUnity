#include "aruco_unity/utility/mat.hpp"

extern "C" {
  cv::Mat* auNewMat() {
    return new cv::Mat();
  }

  void auDeleteMat(cv::Mat* mat) {
    delete mat;
  }

  size_t auMatElemSize(cv::Mat* mat) {
    return mat->elemSize();
  }

  size_t auMatTotal(cv::Mat* mat) {
    return mat->total();
  }

  uchar* auGetMatData(cv::Mat* mat) {
    return mat->data;
  }
}