#include "aruco_unity/utility/cv/vec3d.hpp"
#include "aruco_unity/utility/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Vec3d* au_Vec3d_new() {
    return new cv::Vec3d();
  }
  
  void au_Vec3d_delete(cv::Vec3d* vec3d) {
    delete vec3d;
  }

  // Variables
  double au_Vec3d_get(cv::Vec3d* vec3d, int i, cv::Exception* exception) {
    double value = 0;
    try {
      value = (*vec3d)[i];
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return value;
    }
    return value;
  }

  void au_Vec3d_set(cv::Vec3d* vec3d, int i, double value, cv::Exception* exception) {
    try {
      (*vec3d)[i] = value;
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    }
  }
}