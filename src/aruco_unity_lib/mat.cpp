#include "aruco_unity/utility/mat.hpp"

extern "C" {
  // Constructors
  cv::Mat* auNewMat() {
    return new cv::Mat();
  }

  // Destructor
  void auDeleteMat(cv::Mat* mat) {
    delete mat;
  }

  // Functions
  size_t auMatElemSize(cv::Mat* mat) {
    return mat->elemSize();
  }

  size_t auMatTotal(cv::Mat* mat) {
    return mat->total();
  }

  // Variables
  uchar* auGetMatData(cv::Mat* mat) {
    return mat->data;
  }
}