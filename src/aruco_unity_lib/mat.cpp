#include "aruco_unity/utility/mat.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Mat* au_Mat_New() {
    return new cv::Mat();
  }
  
  void au_Mat_Delete(cv::Mat* mat) {
    delete mat;
  }

  // Functions
  size_t au_Mat_ElemSize(cv::Mat* mat) {
    return mat->elemSize();
  }

  size_t au_Mat_Total(cv::Mat* mat) {
    return mat->total();
  }

  // Variables
  int au_Mat_GetCols(cv::Mat* mat) {
    return mat->cols;
  }

  uchar* au_Mat_GetData(cv::Mat* mat) {
    return mat->data;
  }

  int au_Mat_GetRows(cv::Mat* mat) {
    return mat->rows;
  }
}