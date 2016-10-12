#include "aruco_unity/utility/vector_int.hpp"

extern "C" {
  // Constructors & Destructors
  void au_vectorInt_delete(std::vector<int>* vector) {
    delete vector;
  }

  // Functions
  int* au_vectorInt_data(std::vector<int>* vector) {
    return vector->data();
  }

  void au_vectorInt_push_back(std::vector<int>* vector, int value) {
    vector->push_back(value);
  }

  void au_vectorInt_reserve(std::vector<int>* vector, size_t new_cap) {
    vector->reserve(new_cap);
  }

  size_t au_vectorInt_size(std::vector<int>* vector) {
    return vector->size();
  }
}