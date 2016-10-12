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

  size_t au_vectorMat_size(std::vector<cv::Mat>* vector) {
    return vector->size();
  }
}