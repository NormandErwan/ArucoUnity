#ifndef __ARUCO_UNITY_VECTOR_VEC4I_HPP__
#define __ARUCO_UNITY_VECTOR_VEC4I_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_vec4i
//! \brief Wrapper for std::vector<cv::Vec4i>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<cv::Vec4i>.
  ARUCO_UNITY_API std::vector<cv::Vec4i>* au_vectorVec4i_new();

  //! \brief Deletes any std::vector<cv::Vec4i>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorVec4i_delete(std::vector<cv::Vec4i>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //!
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API cv::Vec4i* au_vectorVec4i_at(std::vector<cv::Vec4i>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Vec4i* au_vectorVec4i_data(std::vector<cv::Vec4i>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_vectorVec4i_push_back(std::vector<cv::Vec4i>* vector, cv::Vec4i* value);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorVec4i_size(std::vector<cv::Vec4i>* vector);

  //! @} Functions
}

//! @} utility_vector_vec4i

#endif