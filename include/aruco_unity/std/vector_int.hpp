#ifndef __ARUCO_UNITY_VECTOR_INT_HPP__
#define __ARUCO_UNITY_VECTOR_INT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup vector_int
//! \brief Wrapper for std::vector<int>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<int>.
  ARUCO_UNITY_API std::vector<int>* au_std_vectorInt_new();

  //! \brief Deletes any std::vector<int>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_std_vectorInt_delete(std::vector<int>* vector);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //!
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API int au_std_vectorInt_at(std::vector<int>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API int* au_std_vectorInt_data(std::vector<int>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_std_vectorInt_push_back(std::vector<int>* vector, int value);

  //! \brief Reserves storage.
  //!
  //! \param vector The vector used.
  //! \param new_cap New capacity of the container.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_std_vectorInt_reserve(std::vector<int>* vector, size_t new_cap, cv::Exception* exception);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_std_vectorInt_size(std::vector<int>* vector);

  //! @} Member Functions
}

//! @} vector_int

#endif