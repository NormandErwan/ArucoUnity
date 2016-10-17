#include "aruco_unity/utility/vec4i.hpp"
#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Vec4i* au_Vec4i_new() {
    return new cv::Vec4i();
  }
  
  void au_Vec4i_delete(cv::Vec4i* vec4i) {
    delete vec4i;
  }

  // Variables
  int au_Vec4i_get(cv::Vec4i* vec4i, int i, cv::Exception* exception) {
    int value = 0;
    try {
      value = (*vec4i)[i];
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return value;
    }
    return value;
  }

  void au_Vec4i_set(cv::Vec4i* vec4i, int i, int value, cv::Exception* exception) {
    try {
      (*vec4i)[i] = value;
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    }
  }
}