#ifndef __ARUCO_UNITY_VEC3D_HPP__
#define __ARUCO_UNITY_VEC3D_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup vec3d
//! \brief Numerical vector of four double elements.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d6/dcf/classcv_1_1Vec.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates a Vec3d.
  ARUCO_UNITY_API cv::Vec3d* au_cv_Vec3d_new(double v0, double v1, double v2);

  //! \brief Deletes any Vec3d.
  //! \param vec3d The Vec3d used.
  ARUCO_UNITY_API void au_cv_Vec3d_delete(cv::Vec3d* vec3d);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Returns the value of the element i.
  //!
  //! \param vec3d The Vec3d used.
  //! \param i The element number.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API double au_cv_Vec3d_get(cv::Vec3d* vec3d, int i, cv::Exception* exception);

  //! \brief Sets the value of the element i.
  //!
  //! \param vec3d The Vec3d used.
  //! \param i The element number.
  //! \param value The new value.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_cv_Vec3d_set(cv::Vec3d* vec3d, int i, double value, cv::Exception* exception);

  //! @} Member Functions
}

//! @} vec3d

#endif