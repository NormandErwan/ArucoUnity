#include "aruco_unity/utility/mat.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Mat* auNewMat() {
    return new cv::Mat();
  }
  
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
  int auGetMatCols(cv::Mat* mat) {
    return mat->cols;
  }

  uchar* auGetMatData(cv::Mat* mat) {
    return mat->data;
  }

  int auGetMatRows(cv::Mat* mat) {
    return mat->rows;
  }
}