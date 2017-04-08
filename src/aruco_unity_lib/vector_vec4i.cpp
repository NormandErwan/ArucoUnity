#include "aruco_unity/std/vector_vec4i.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors

  std::vector<cv::Vec4i>* au_std_vectorVec4i_new() {
    return new std::vector<cv::Vec4i>();
  }

  void au_std_vectorVec4i_delete(std::vector<cv::Vec4i>* vector) {
    delete vector;
  }

  // Member Functions

  cv::Vec4i* au_std_vectorVec4i_at(std::vector<cv::Vec4i>* vector, size_t pos, cv::Exception* exception) {
    cv::Vec4i* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }
  
  cv::Vec4i* au_std_vectorVec4i_data(std::vector<cv::Vec4i>* vector) {
    return vector->data();
  }

  void au_std_vectorVec4i_push_back(std::vector<cv::Vec4i>* vector, cv::Vec4i* value) {
    vector->push_back(cv::Vec4i(*value));
  }

  size_t au_std_vectorVec4i_size(std::vector<cv::Vec4i>* vector) {
    return vector->size();
  }
}