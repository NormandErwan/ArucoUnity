#include "aruco_unity/std/vector_vec3d.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors

  std::vector<cv::Vec3d>* au_std_vectorVec3d_new() {
    return new std::vector<cv::Vec3d>();
  }

  void au_std_vectorVec3d_delete(std::vector<cv::Vec3d>* vector) {
    delete vector;
  }

  // Functions

  cv::Vec3d* au_std_vectorVec3d_at(std::vector<cv::Vec3d>* vector, size_t pos, cv::Exception* exception) {
    cv::Vec3d* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }
  
  cv::Vec3d* au_std_vectorVec3d_data(std::vector<cv::Vec3d>* vector) {
    return vector->data();
  }

  void au_std_vectorVec3d_push_back(std::vector<cv::Vec3d>* vector, cv::Vec3d* value) {
    vector->push_back(cv::Vec3d(*value));
  }

  size_t au_std_vectorVec3d_size(std::vector<cv::Vec3d>* vector) {
    return vector->size();
  }
}