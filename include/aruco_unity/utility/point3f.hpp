#ifndef __ARUCO_UNITY_POINT3F_HPP__
#define __ARUCO_UNITY_POINT3F_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_point3f
//! \brief Template class for 3D float points specified by its coordinates x, y and z.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/df/d6c/classcv_1_1Point3__.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Point3f.
  ARUCO_UNITY_API cv::Point3f* au_Point3f_new();

  //! \brief Deletes any Point3f.
  //! \param point3f The Point3f used.
  ARUCO_UNITY_API void au_Point3f_delete(cv::Point3f* point3f);

  //! @} Constructors & Destructors

  //! \name Variables
  //! @{

  //! \brief Returns the x value.
  //! \param point3f The Point3f used.
  ARUCO_UNITY_API float au_Point3f_getX(cv::Point3f* point3f);

  //! \brief Sets the x value.
  //! \param point3f The Point3f used.
  //! \param x The new value.
  ARUCO_UNITY_API void au_Point3f_setX(cv::Point3f* point3f, float x);

  //! \brief Returns the y value.
  //! \param point3f The Point3f used.
  ARUCO_UNITY_API float au_Point3f_getY(cv::Point3f* point3f);

  //! \brief Sets the y value.
  //! \param point3f The Point3f used.
  //! \param y The new value.
  ARUCO_UNITY_API void au_Point3f_setY(cv::Point3f* point3f, float y);

  //! \brief Returns the y value.
  //! \param point3f The Point3f used.
  ARUCO_UNITY_API float au_Point3f_getZ(cv::Point3f* point3f);

  //! \brief Sets the y value.
  //! \param point3f The Point3f used.
  //! \param y The new value.
  ARUCO_UNITY_API void au_Point3f_setZ(cv::Point3f* point3f, float z);

  //! @} Variables
}

//! @} utility_point3f

#endif