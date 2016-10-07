#ifndef __ARUCO_UNITY_VECTOR_VECTOR_POINT2F_HPP__
#define __ARUCO_UNITY_VECTOR_VECTOR_POINT2F_HPP__

#include <unordered_map>
#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_vector_point2f
//! \brief Wrapper for std::vector<std::vector<cv::Point2f>>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Deletes any std::vector<std::vector<cv::Point2f>>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorVectorPoint2f_delete(std::vector<std::vector<cv::Point2f>>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Point2f** au_vectorVectorPoint2f_data(std::vector<std::vector<cv::Point2f>>* vector);

  //! \brief Deletes returned data.
  //! \param vector_data The vector data used.
  ARUCO_UNITY_API void au_vectorVectorPoint2f_data_delete(cv::Point2f** vector_data);
  
  //! \brief Returns the number of vector elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorVectorPoint2f_size1(std::vector<std::vector<cv::Point2f>>* vector);

  //! \brief Returns the number of Point2f in a vector element.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorVectorPoint2f_size2(std::vector<std::vector<cv::Point2f>>* vector);

  //! @} Functions
}

//! @} utility_vector_vector_point2f

#endif