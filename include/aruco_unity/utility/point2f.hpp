#ifndef __ARUCO_UNITY_POINT2F_HPP__
#define __ARUCO_UNITY_POINT2F_HPP__

#include <opencv2\core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_point2f
//! \brief Class for 2D float points specified by its coordinates x and y.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/dc/d84/group__core__basic.html#ga7d080aa40de011e4410bca63385ffe2a
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Point2f.
  ARUCO_UNITY_API cv::Point2f* au_Point2f_new();

  //! \brief Deletes any Point2f.
  //! \param point2f The Point2f used.
  ARUCO_UNITY_API void au_Point2f_delete(cv::Point2f* point2f);

  //! @} Constructors & Destructors

  //! \name Variables
  //! @{

  //! \brief Returns the x value.
  //! \param point2f The Point2f used.
  ARUCO_UNITY_API float au_Point2f_getX(cv::Point2f* point2f);

  //! \brief Sets the x value.
  //! \param point2f The Point2f used.
  //! \param x The new value.
  ARUCO_UNITY_API void au_Point2f_setX(cv::Point2f* point2f, float x);

  //! \brief Returns the y value.
  //! \param point2f The Point2f used.
  ARUCO_UNITY_API float au_Point2f_getY(cv::Point2f* point2f);

  //! \brief Sets the y value.
  //! \param point2f The Point2f used.
  //! \param y The new value.
  ARUCO_UNITY_API void au_Point2f_setY(cv::Point2f* point2f, float y);

  //! @} Variables
}

//! @} utility_point2f

#endif