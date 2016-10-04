#ifndef __ARUCO_UNITY_VECTOR_INT_HPP__
#define __ARUCO_UNITY_VECTOR_INT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_int
//! \brief TODO
//!
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  ARUCO_UNITY_API std::vector<int>* au_vectorInt_new();

  ARUCO_UNITY_API void au_vectorInt_delete(std::vector<int>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  ARUCO_UNITY_API int* au_vectorInt_data(std::vector<int>* vector);

  ARUCO_UNITY_API int au_vectorInt_size(std::vector<int>* vector);

  //! @} Functions
}

//! @} utility_vector_int

#endif