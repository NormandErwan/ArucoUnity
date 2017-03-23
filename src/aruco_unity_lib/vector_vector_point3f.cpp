#include "aruco_unity/std/vector_vector_point3f.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<std::vector<cv::Point3f>>* au_std_vectorVectorPoint3f_new() {
    return new std::vector<std::vector<cv::Point3f>>();
  }

  void au_std_vectorVectorPoint3f_delete(std::vector<std::vector<cv::Point3f>>* vector) {
    delete vector;
  }

  // Functions
  std::vector<cv::Point3f>* au_std_vectorVectorPoint3f_at(std::vector<std::vector<cv::Point3f>>* vector, size_t pos, cv::Exception* exception) {
    std::vector<cv::Point3f>* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }

  std::vector<cv::Point3f>* au_std_vectorVectorPoint3f_data(std::vector<std::vector<cv::Point3f>>* vector) {
    return vector->data();
  }

  void au_std_vectorVectorPoint3f_push_back(std::vector<std::vector<cv::Point3f>>* vector, std::vector<cv::Point3f>* value) {
    vector->push_back(std::vector<cv::Point3f>(*value));
  }

  size_t au_std_vectorVectorPoint3f_size(std::vector<std::vector<cv::Point3f>>* vector) {
    return vector->size();
  }
}