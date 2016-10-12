#include "aruco_unity/utility/vector_int.hpp"
#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<int>* au_vectorInt_new() {
    return new std::vector<int>();
  }

  void au_vectorInt_delete(std::vector<int>* vector) {
    delete vector;
  }

  // Functions
  int au_vectorInt_at(std::vector<int>* vector, size_t pos, cv::Exception* exception) {
    int element = 0;
    try {
      element = vector->at(pos);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }

  int* au_vectorInt_data(std::vector<int>* vector) {
    return vector->data();
  }

  void au_vectorInt_push_back(std::vector<int>* vector, int value) {
    vector->push_back(value);
  }

  void au_vectorInt_reserve(std::vector<int>* vector, size_t new_cap, cv::Exception* exception) {
    try {
      vector->reserve(new_cap);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    }
  }

  size_t au_vectorInt_size(std::vector<int>* vector) {
    return vector->size();
  }
}