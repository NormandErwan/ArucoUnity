#ifndef __ARUCO_UNITY_VECTOR_DOUBLE_HPP__
#define __ARUCO_UNITY_VECTOR_DOUBLE_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup vector_double
//! \brief Wrapper for std::vector<double>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<double>.
  ARUCO_UNITY_API std::vector<double>* au_std_vectorDouble_new();

  //! \brief Deletes any std::vector<double>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_std_vectorDouble_delete(std::vector<double>* vector);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //!
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API double au_std_vectorDouble_at(std::vector<double>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API double* au_std_vectorDouble_data(std::vector<double>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_std_vectorDouble_push_back(std::vector<double>* vector, double value);

  //! \brief Reserves storage.
  //!
  //! \param vector The vector used.
  //! \param new_cap New capacity of the container.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_std_vectorDouble_reserve(std::vector<double>* vector, size_t new_cap, cv::Exception* exception);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_std_vectorDouble_size(std::vector<double>* vector);

  //! @} Member Functions
}

//! @} vector_double

#endif