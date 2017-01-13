#include "aruco_unity/utility/std/vector_mat.hpp"
#include "aruco_unity/utility/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<cv::Mat>* au_vectorMat_new() {
    return new std::vector<cv::Mat>();
  }

  void au_vectorMat_delete(std::vector<cv::Mat>* vector) {
    delete vector;
  }

  // Functions
  cv::Mat* au_vectorMat_at(std::vector<cv::Mat>* vector, size_t pos, cv::Exception* exception) {
    cv::Mat* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }
  
  cv::Mat* au_vectorMat_data(std::vector<cv::Mat>* vector) {
    return vector->data();
  }

  void au_vectorMat_push_back(std::vector<cv::Mat>* vector, cv::Mat* value) {
    vector->push_back(cv::Mat(*value));
  }

  size_t au_vectorMat_size(std::vector<cv::Mat>* vector) {
    return vector->size();
  }
}