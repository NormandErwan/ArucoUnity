#ifndef __ARUCO_UNITY_VECTOR_VECTOR_POINT2F_HPP__
#define __ARUCO_UNITY_VECTOR_VECTOR_POINT2F_HPP__

#include <unordered_map>
#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_vector_point2f
//! \brief TODO
//!
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  ARUCO_UNITY_API void au_vectorVectorPoint2f_delete(std::vector<std::vector<cv::Point2f>>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  ARUCO_UNITY_API cv::Point2f** au_vectorVectorPoint2f_data(std::vector<std::vector<cv::Point2f>>* vector);

  ARUCO_UNITY_API void au_vectorVectorPoint2f_data_delete(cv::Point2f** vector_data);
  
  ARUCO_UNITY_API size_t au_vectorVectorPoint2f_size1(std::vector<std::vector<cv::Point2f>>* vector);

  ARUCO_UNITY_API size_t au_vectorVectorPoint2f_size2(std::vector<std::vector<cv::Point2f>>* vector);

  //! @} Functions
}

//! @} utility_vector_vector_point2f

#endif