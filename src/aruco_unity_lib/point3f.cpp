#include "aruco_unity/utility/cv/point3f.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Point3f* au_cv_Point3f_new() {
    return new cv::Point3f();
  }
  
  void au_cv_Point3f_delete(cv::Point3f* point3f) {
    delete point3f;
  }

  // Variables
  float au_cv_Point3f_getX(cv::Point3f* point3f) {
    return point3f->x;
  }

  void au_cv_Point3f_setX(cv::Point3f* point3f, float x) {
    point3f->x = x;
  }

  float au_cv_Point3f_getY(cv::Point3f* point3f) {
    return point3f->y;
  }

  void au_cv_Point3f_setY(cv::Point3f* point3f, float y) {
    point3f->y = y;
  }

  float au_cv_Point3f_getZ(cv::Point3f* point3f) {
    return point3f->z;
  }

  void au_cv_Point3f_setZ(cv::Point3f* point3f, float z) {
    point3f->z = z;
  }
}