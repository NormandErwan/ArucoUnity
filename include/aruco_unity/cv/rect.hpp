#ifndef __ARUCO_UNITY_PLUGIN_RECT_HPP__
#define __ARUCO_UNITY_PLUGIN_RECT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup rect
//! \brief Class for int 2D rectangles.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d2/d44/classcv_1_1Rect__.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Rect.
  ARUCO_UNITY_API cv::Rect* au_cv_Rect_new1();

  //! \brief Creates a Rect.
  ARUCO_UNITY_API cv::Rect* au_cv_Rect_new2(int x, int y, int width, int height);

  //! \brief Deletes any Rect.
  //! \param rect The Rect used.
  ARUCO_UNITY_API void au_cv_Rect_delete(cv::Rect* rect);

  //! @} Constructors & Destructors

  //! \name Attributes
  //! @{

  //! \brief Returns the x value.
  //! \param rect The Rect used.
  ARUCO_UNITY_API int au_cv_Rect_getX(cv::Rect* rect);

  //! \brief Sets the x value.
  //! \param rect The Rect used.
  //! \param x The new value.
  ARUCO_UNITY_API void au_cv_Rect_setX(cv::Rect* rect, int x);

  //! \brief Returns the y value.
  //! \param rect The Rect used.
  ARUCO_UNITY_API int au_cv_Rect_getY(cv::Rect* rect);

  //! \brief Sets the y value.
  //! \param rect The Rect used.
  //! \param y The new value.
  ARUCO_UNITY_API void au_cv_Rect_setY(cv::Rect* rect, int y);

  //! \brief Returns the width value.
  //! \param rect The Rect used.
  ARUCO_UNITY_API int au_cv_Rect_getWidth(cv::Rect* rect);

  //! \brief Sets the width value.
  //! \param rect The Rect used.
  //! \param width The new value.
  ARUCO_UNITY_API void au_cv_Rect_setWidth(cv::Rect* rect, int width);

  //! \brief Returns the height value.
  //! \param rect The Rect used.
  ARUCO_UNITY_API int au_cv_Rect_getHeight(cv::Rect* rect);

  //! \brief Sets the height value.
  //! \param rect The Rect used.
  //! \param height The new value.
  ARUCO_UNITY_API void au_cv_Rect_setHeight(cv::Rect* rect, int height);

  //! @} Attributes
}

//! @} rect

#endif