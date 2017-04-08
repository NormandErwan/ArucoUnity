#include "aruco_unity/std/vector_double.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors

  std::vector<double>* au_std_vectorDouble_new() {
    return new std::vector<double>();
  }

  void au_std_vectorDouble_delete(std::vector<double>* vector) {
    delete vector;
  }

  // Member Functions

  double au_std_vectorDouble_at(std::vector<double>* vector, size_t pos, cv::Exception* exception) {
    double element = 0;
    try {
      element = vector->at(pos);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }

  double* au_std_vectorDouble_data(std::vector<double>* vector) {
    return vector->data();
  }

  void au_std_vectorDouble_push_back(std::vector<double>* vector, double value) {
    vector->push_back(value);
  }

  void au_std_vectorDouble_reserve(std::vector<double>* vector, size_t new_cap, cv::Exception* exception) {
    try {
      vector->reserve(new_cap);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
    }
  }

  size_t au_std_vectorDouble_size(std::vector<double>* vector) {
    return vector->size();
  }
}