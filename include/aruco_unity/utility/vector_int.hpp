#ifndef __ARUCO_UNITY_VECTOR_INT_HPP__
#define __ARUCO_UNITY_VECTOR_INT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_int
//! \brief Wrapper for std::vector<int>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Deletes any std::vector<int>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorInt_delete(std::vector<int>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API int* au_vectorInt_data(std::vector<int>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorInt_push_back(std::vector<int>* vector, int value);

  //! \brief Reserves storage.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorInt_reserve(std::vector<int>* vector, size_t new_cap);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorInt_size(std::vector<int>* vector);

  //! @} Functions
}

//! @} utility_vector_int

#endif