#ifndef __ARUCO_UNITY_VEC4I_HPP__
#define __ARUCO_UNITY_VEC4I_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup vec4i
//! \brief Numerical vector of four int elements.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d6/dcf/classcv_1_1Vec.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Vec4i.
  ARUCO_UNITY_API cv::Vec4i* au_cv_Vec4i_new();

  //! \brief Deletes any Vec4i.
  //! \param vec4i The Vec4i used.
  ARUCO_UNITY_API void au_cv_Vec4i_delete(cv::Vec4i* vec4i);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Returns the value of the element i.
  //! \param vec4i The Vec4i used.
  //! \param i The element number.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API int au_cv_Vec4i_get(cv::Vec4i* vec4i, int i, cv::Exception* exception);

  //! \brief Sets the value of the element i.
  //!
  //! \param vec4i The Vec4i used.
  //! \param i The element number.
  //! \param value The new value.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_cv_Vec4i_set(cv::Vec4i* vec4i, int i, int value, cv::Exception* exception);

  //! @} Member Functions
}

//! @} vec4i

#endif