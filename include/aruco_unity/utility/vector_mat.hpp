#ifndef __ARUCO_UNITY_VECTOR_MAT_HPP__
#define __ARUCO_UNITY_VECTOR_MAT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_mat
//! \brief Wrapper for std::vector<cv::Mat>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Deletes any std::vector<cv::Mat>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorMat_delete(std::vector<cv::Mat>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Mat* au_vectorMat_data(std::vector<cv::Mat>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorMat_push_back(std::vector<cv::Mat>* vector, cv::Mat* value);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorMat_size(std::vector<cv::Mat>* vector);

  //! @} Functions
}

//! @} utility_vector_mat

#endif