#include "aruco_unity/std/vector_vector_int.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<std::vector<int>>* au_std_vectorVectorInt_new() {
    return new std::vector<std::vector<int>>();
  }

  void au_std_vectorVectorInt_delete(std::vector<std::vector<int>>* vector) {
    delete vector;
  }

  // Functions
  std::vector<int>* au_std_vectorVectorInt_at(std::vector<std::vector<int>>* vector, size_t pos, cv::Exception* exception) {
    std::vector<int>* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }

  std::vector<int>* au_std_vectorVectorInt_data(std::vector<std::vector<int>>* vector) {
    return vector->data();
  }

  void au_std_vectorVectorInt_push_back(std::vector<std::vector<int>>* vector, std::vector<int>* value) {
    vector->push_back(std::vector<int>(*value));
  }

  void au_std_vectorVectorInt_reserve(std::vector<std::vector<int>>* vector, size_t new_cap) {
    vector->reserve(new_cap);
  }

  size_t au_std_vectorVectorInt_size(std::vector<std::vector<int>>* vector) {
    return vector->size();
  }
}