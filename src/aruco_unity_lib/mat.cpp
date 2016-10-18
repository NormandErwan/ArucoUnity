#include "aruco_unity/utility/mat.hpp"
#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Mat* au_Mat_new1() {
    return new cv::Mat();
  }

  cv::Mat* au_Mat_new2(int rows, int cols, int type) {
    return new cv::Mat(rows, cols, type);
  }

  cv::Mat* au_Mat_new2_uchar(int rows, int cols, int type, uchar* data) {
    return new cv::Mat(rows, cols, type, data);
  }

  cv::Mat* au_Mat_new2_double(int rows, int cols, int type, double* data) {
    return new cv::Mat(rows, cols, type, data);
  }
  
  void au_Mat_delete(cv::Mat* mat) {
    delete mat;
  }

  // Member Functions
  int au_Mat_at_int_get(cv::Mat* mat, int i0, int i1, cv::Exception* exception) {
    int value = 0;
    try {
      value = mat->at<int>(i0, i1);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return value;
    };
    return value;
  }

  void au_Mat_at_int_set(cv::Mat* mat, int i0, int i1, int value, cv::Exception* exception) {
    try {
      mat->at<int>(i0, i1) = value;
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    };
  }

  double au_Mat_at_double_get(cv::Mat* mat, int i0, int i1, cv::Exception* exception) {
    double value = 0;
    try {
      value = mat->at<double>(i0, i1);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return value;
    };
    return value;
  }

  void au_Mat_at_double_set(cv::Mat* mat, int i0, int i1, double value, cv::Exception* exception) {
    try {
      mat->at<double>(i0, i1) = value;
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    };
  }

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