#include "aruco_unity/utility/cv/mat.hpp"
#include "aruco_unity/utility/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Mat* au_cv_Mat_new1() {
    return new cv::Mat();
  }

  cv::Mat* au_cv_Mat_new2_uchar(int rows, int cols, int type, uchar* data) {
    return new cv::Mat(rows, cols, type, data);
  }

  cv::Mat* au_cv_Mat_new2_double(int rows, int cols, int type, double* data) {
    return new cv::Mat(rows, cols, type, data);
  }
  
  void au_cv_Mat_delete(cv::Mat* mat) {
    delete mat;
  }

  // Member Functions
  int au_cv_Mat_at_int_get(cv::Mat* mat, int i0, int i1, cv::Exception* exception) {
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

  void au_cv_Mat_at_int_set(cv::Mat* mat, int i0, int i1, int value, cv::Exception* exception) {
    try {
      mat->at<int>(i0, i1) = value;
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    };
  }

  double au_cv_Mat_at_double_get(cv::Mat* mat, int i0, int i1, cv::Exception* exception) {
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

  void au_cv_Mat_at_double_set(cv::Mat* mat, int i0, int i1, double value, cv::Exception* exception) {
    try {
      mat->at<double>(i0, i1) = value;
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    };
  }

  void au_cv_Mat_create(cv::Mat* mat, int rows, int cols, int type, cv::Exception* exception) {
    try {
      mat->create(rows, cols, type);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    };
  }

  size_t au_cv_Mat_elemSize(cv::Mat* mat) {
    return mat->elemSize();
  }

  size_t au_cv_Mat_total(cv::Mat* mat) {
    return mat->total();
  }

  int au_cv_Mat_type(cv::Mat* mat) {
    return mat->type();
  }

  // Variables
  int au_cv_Mat_getCols(cv::Mat* mat) {
    return mat->cols;
  }

  uchar* au_cv_Mat_getData_void(cv::Mat* mat) {
	  return mat->data;
  }

  void au_cv_Mat_setData_void(cv::Mat* mat, uchar* value) {
	  mat->data = value;
  }

  uchar* au_cv_Mat_getData_uchar(cv::Mat* mat) {
    return au_cv_Mat_getData_void(mat);
  }

  void au_cv_Mat_setData_uchar(cv::Mat* mat, uchar* value) {
	  au_cv_Mat_setData_void(mat, value);
  }

  int au_cv_Mat_getRows(cv::Mat* mat) {
    return mat->rows;
  }

  cv::Size* au_cv_Mat_getSize(cv::Mat* mat) {
    return new cv::Size(mat->size());
  }
}