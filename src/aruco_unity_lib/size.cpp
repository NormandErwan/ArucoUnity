#include "aruco_unity/cv/size.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Size* au_cv_Size_new() {
    return new cv::Size();
  }
  
  void au_cv_Size_delete(cv::Size* size) {
    delete size;
  }

  // Member functions
  int au_cv_Size_area(cv::Size* size) {
    return size->area();
  }

  // Variables
  int au_cv_Size_getHeight(cv::Size* size) {
    return size->height;
  }

  void au_cv_Size_setHeight(cv::Size* size, int height) {
    size->height = height;
  }

  int au_cv_Size_getWidth(cv::Size* size) {
    return size->width;
  }

  void au_cv_Size_setWidth(cv::Size* size, int width) {
    size->width = width;
  }
}