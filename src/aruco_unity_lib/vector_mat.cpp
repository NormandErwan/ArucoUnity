#include "aruco_unity/utility/vector_mat.hpp"

extern "C" {
  // Constructors & Destructors
  void au_vectorMat_delete(std::vector<cv::Mat>* vector) {
    delete vector;
  }

  // Functions
  cv::Mat* au_vectorMat_data(std::vector<cv::Mat>* vector) {
    return vector->data();
  }

  void au_vectorMat_push_back(std::vector<cv::Mat>* vector, cv::Mat* value) {
    vector->push_back(*value);
  }

  size_t au_vectorMat_size(std::vector<cv::Mat>* vector) {
    return vector->size();
  }
}