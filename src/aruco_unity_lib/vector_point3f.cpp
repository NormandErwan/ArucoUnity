#include "aruco_unity/utility/vector_point3f.hpp"
#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<cv::Point3f>* au_vectorPoint3f_new() {
    return new std::vector<cv::Point3f>();
  }

  void au_vectorPoint3f_delete(std::vector<cv::Point3f>* vector) {
    delete vector;
  }

  // Functions
  cv::Point3f* au_vectorPoint3f_at(std::vector<cv::Point3f>* vector, size_t pos, cv::Exception* exception) {
    cv::Point3f* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }
  
  cv::Point3f* au_vectorPoint3f_data(std::vector<cv::Point3f>* vector) {
    return vector->data();
  }

  void au_vectorPoint3f_push_back(std::vector<cv::Point3f>* vector, cv::Point3f* value) {
    vector->push_back(cv::Point3f(*value));
  }

  size_t au_vectorPoint3f_size(std::vector<cv::Point3f>* vector) {
    return vector->size();
  }
}