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

  size_t au_vectorInt_size(std::vector<int>* vector) {
    return vector->size();
  }
}