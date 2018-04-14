#ifndef __ARUCO_UNITY_PLUGIN_VECTOR_VEC3D_HPP__
#define __ARUCO_UNITY_PLUGIN_VECTOR_VEC3D_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup vector_vec3d
//! \brief Wrapper for std::vector<cv::Vec3d>.
//!
//! See the std documentation for more information: http://en.cppreference.com/w/cpp/container/vector
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Create a new std::vector<cv::Vec3d>.
  ARUCO_UNITY_API std::vector<cv::Vec3d>* au_std_vectorVec3d_new();

  //! \brief Deletes any std::vector<cv::Vec3d>.
  //! \param vector The vector used.
  ARUCO_UNITY_API void au_std_vectorVec3d_delete(std::vector<cv::Vec3d>* vector);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{
  
  //! \brief Access specified element with bounds checking.
  //!
  //! \param vector The vector used.
  //! \param pos Position of the element to return.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API cv::Vec3d* au_std_vectorVec3d_at(std::vector<cv::Vec3d>* vector, size_t pos, cv::Exception* exception);

  //! \brief Direct access to the underlying array.
  //! \param vector The vector used.
  ARUCO_UNITY_API cv::Vec3d* au_std_vectorVec3d_data(std::vector<cv::Vec3d>* vector);

  //! \brief Adds an element to the end.
  //! \param vector The vector used.
  //! \param value The value of the element to append.
  ARUCO_UNITY_API void au_std_vectorVec3d_push_back(std::vector<cv::Vec3d>* vector, cv::Vec3d* value);

  //! \brief Returns the number of elements.
  //! \param vector The vector used.
  ARUCO_UNITY_API size_t au_std_vectorVec3d_size(std::vector<cv::Vec3d>* vector);

  //! @} Member Functions
}

//! @} vector_vec3d

#endif