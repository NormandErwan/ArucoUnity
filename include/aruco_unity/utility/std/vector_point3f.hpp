#ifndef __ARUCO_UNITY_VECTOR_POINT3F_HPP__
#define __ARUCO_UNITY_VECTOR_POINT3F_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup vector_point3f
//! \brief Wrapper for std::vector<cv::Point3f>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<cv::Point3f>.
  ARUCO_UNITY_API std::vector<cv::Point3f>* au_vectorPoint3f_new();

  //! \brief Deletes any std::vector<cv::Point3f>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_vectorPoint3f_delete(std::vector<cv::Point3f>* vector);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{
  
  //! \brief Access specified element with bounds checking. 
  //!
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API cv::Point3f* au_vectorPoint3f_at(std::vector<cv::Point3f>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Point3f* au_vectorPoint3f_data(std::vector<cv::Point3f>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_vectorPoint3f_push_back(std::vector<cv::Point3f>* vector, cv::Point3f* value);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_vectorPoint3f_size(std::vector<cv::Point3f>* vector);

  //! @} Functions
}

//! @} vector_point3f

#endif