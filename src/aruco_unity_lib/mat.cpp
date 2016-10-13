#include "aruco_unity/utility/mat.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Mat* au_Mat_new1() {
    return new cv::Mat();
  }

  cv::Mat* au_Mat_new2(int rows, int cols, uchar* data) {
    return new cv::Mat(rows, cols, CV_8UC3, data);
  }
  
  void au_Mat_delete(cv::Mat* mat) {
    delete mat;
  }

  // Functions
  size_t au_Mat_elemSize(cv::Mat* mat) {
    return mat->elemSize();
  }

  size_t au_Mat_total(cv::Mat* mat) {
    return mat->total();
  }

  // Variables
  int au_Mat_getCols(cv::Mat* mat) {
    return mat->cols;
  }

  uchar* au_Mat_getData(cv::Mat* mat) {
    return mat->data;
  }

  int au_Mat_getRows(cv::Mat* mat) {
    return mat->rows;
  }

  cv::Size* au_Mat_getSize(cv::Mat* mat) {
    return new cv::Size(mat->size());
  }
}