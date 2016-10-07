#include "aruco_unity/utility/vector_vector_point2f.hpp"

extern "C" {
  // Constructors & Destructors
  void au_vectorVectorPoint2f_delete(std::vector<std::vector<cv::Point2f>>* vector) {
    delete vector;
  }

  // Functions
  cv::Point2f** au_vectorVectorPoint2f_data(std::vector<std::vector<cv::Point2f>>* vector) {
    size_t size1 = au_vectorVectorPoint2f_size1(vector),
           size2 = au_vectorVectorPoint2f_size2(vector);
    cv::Point2f** data = new cv::Point2f*[size1];

    for (int i = 0; i < size1; i++) {
      for (int j = 0; j < size2; j++) {
        data[i * size2 + j] = new cv::Point2f(vector->at(i).at(j));
      }
    }

    return data;
  }

  void au_vectorVectorPoint2f_data_delete(cv::Point2f** vector_data) {
    delete vector_data;
  }

  size_t au_vectorVectorPoint2f_size1(std::vector<std::vector<cv::Point2f>>* vector) {
    return vector->size();
  }

  size_t au_vectorVectorPoint2f_size2(std::vector<std::vector<cv::Point2f>>* vector) {
    return vector->at(0).size();
  }
}