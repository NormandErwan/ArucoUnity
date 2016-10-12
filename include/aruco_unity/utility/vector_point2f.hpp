#ifndef __ARUCO_UNITY_VECTOR_POINT2F_HPP__
#define __ARUCO_UNITY_VECTOR_POINT2F_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vector_point2f
//! \brief Wrapper for std::vector<cv::Point2f>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<cv::Point2f>.
  ARUCO_UNITY_API std::vector<cv::Point2f>* au_vectorPoint2f_new();

  //! \brief Deletes any std::vector<cv::Point2f>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorPoint2f_delete(std::vector<cv::Point2f>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API cv::Point2f* au_vectorPoint2f_at(std::vector<cv::Point2f>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Point2f* au_vectorPoint2f_data(std::vector<cv::Point2f>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_vectorPoint2f_push_back(std::vector<cv::Point2f>* vector, cv::Point2f* value);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorPoint2f_size(std::vector<cv::Point2f>* vector);

  //! @} Functions
}

//! @} utility_vector_point2f

#endif