#include "aruco_unity/utility/vector_int.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<int>* au_vectorInt_new() {
    return new std::vector<int>();
  }

  void au_vectorInt_delete(std::vector<int>* vector) {
    delete vector;
  }

  // Functions
  int* au_vectorInt_data(std::vector<int>* vector) {
    return vector->data();
  }

  int au_vectorInt_size(std::vector<int>* vector) {
    return (int)vector->size();
  }
}