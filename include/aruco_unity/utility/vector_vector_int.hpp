#ifndef __ARUCO_UNITY_VECTOR_VECTOR_INT_HPP__
#define __ARUCO_UNITY_VECTOR_VECTOR_INT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_vector_int
//! \brief Wrapper for std::vector<std::vector<int>>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<std::vector<int>>.
  ARUCO_UNITY_API std::vector<std::vector<int>>* au_vectorVectorInt_new();

  //! \brief Deletes any std::vector<std::vector<int>>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorVectorInt_delete(std::vector<std::vector<int>>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API std::vector<int>* au_vectorVectorInt_at(std::vector<std::vector<int>>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API std::vector<int>* au_vectorVectorInt_data(std::vector<std::vector<int>>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_vectorVectorInt_push_back(std::vector<std::vector<int>>* vector, std::vector<int>* value);
  
  //! \brief Returns the number of vector elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorVectorInt_size(std::vector<std::vector<int>>* vector);

  //! @} Functions
}

//! @} utility_vector_vector_int

#endif