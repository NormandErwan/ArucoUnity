#ifndef __ARUCO_UNITY_VEC3D_HPP__
#define __ARUCO_UNITY_VEC3D_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_vec3d
//! \brief Template class for short numerical vectors, a partial case of Matx.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d6/dcf/classcv_1_1Vec.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Vec3d.
  ARUCO_UNITY_API cv::Vec3d* au_Vec3d_new();

  //! \brief Deletes any Vec3d.
  //! \param vec3d The Vec3d used.
  ARUCO_UNITY_API void au_Vec3d_delete(cv::Vec3d* vec3d);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Returns the value of the element i.
  //! \param vec3d The Vec3d used.
  //! \param i The element number.
  ARUCO_UNITY_API double au_Vec3d_get(cv::Vec3d* vec3d, int i, cv::Exception* exception);

  //! \brief Sets the value of the element i.
  //! \param vec3d The Vec3d used.
  //! \param i The element number.
  //! \param value The new value.
  ARUCO_UNITY_API void au_Vec3d_set(cv::Vec3d* vec3d, int i, double value, cv::Exception* exception);

  //! @} Member Functions
}

//! @} utility_vec3d

#endif