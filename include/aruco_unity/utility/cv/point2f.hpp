#ifndef __ARUCO_UNITY_POINT2F_HPP__
#define __ARUCO_UNITY_POINT2F_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup point2f
//! \brief 2D float points specified by its coordinates x and y.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/db/d4e/classcv_1_1Point__.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Point2f.
  ARUCO_UNITY_API cv::Point2f* au_cv_Point2f_new();

  //! \brief Deletes any Point2f.
  //! \param point2f The Point2f used.
  ARUCO_UNITY_API void au_cv_Point2f_delete(cv::Point2f* point2f);

  //! @} Constructors & Destructors

  //! \name Variables
  //! @{

  //! \brief Returns the x value.
  //! \param point2f The Point2f used.
  ARUCO_UNITY_API float au_cv_Point2f_getX(cv::Point2f* point2f);

  //! \brief Sets the x value.
  //! \param point2f The Point2f used.
  //! \param x The new value.
  ARUCO_UNITY_API void au_cv_Point2f_setX(cv::Point2f* point2f, float x);

  //! \brief Returns the y value.
  //! \param point2f The Point2f used.
  ARUCO_UNITY_API float au_cv_Point2f_getY(cv::Point2f* point2f);

  //! \brief Sets the y value.
  //! \param point2f The Point2f used.
  //! \param y The new value.
  ARUCO_UNITY_API void au_cv_Point2f_setY(cv::Point2f* point2f, float y);

  //! @} Variables
}

//! @} point2f

#endif