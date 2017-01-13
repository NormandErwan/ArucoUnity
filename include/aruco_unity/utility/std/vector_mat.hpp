#ifndef __ARUCO_UNITY_VECTOR_MAT_HPP__
#define __ARUCO_UNITY_VECTOR_MAT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup utility_vector_mat
//! \brief Wrapper for std::vector<cv::Mat>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<cv::Mat>.
  ARUCO_UNITY_API std::vector<cv::Mat>* au_std_vectorMat_new();

  //! \brief Deletes any std::vector<cv::Mat>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_std_vectorMat_delete(std::vector<cv::Mat>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //!
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API cv::Mat* au_std_vectorMat_at(std::vector<cv::Mat>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Mat* au_std_vectorMat_data(std::vector<cv::Mat>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_std_vectorMat_push_back(std::vector<cv::Mat>* vector, cv::Mat* value);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_std_vectorMat_size(std::vector<cv::Mat>* vector);

  //! @} Functions
}

//! @} utility_vector_mat

#endif