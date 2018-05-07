#ifndef __ARUCO_UNITY_PLUGIN_SIZE_HPP__
#define __ARUCO_UNITY_PLUGIN_SIZE_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup size
//! \brief Class for specifying the size of an image or rectangle.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d6/d50/classcv_1_1Size__.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Size.
  ARUCO_UNITY_API cv::Size* au_cv_Size_new1();

  //! \brief Creates a Size.
  //! \param width The width value. 
  //! \param height The height value. 
  ARUCO_UNITY_API cv::Size* au_cv_Size_new2(int width, int height);

  //! \brief Deletes any Size.
  //! \param size The Size used.
  ARUCO_UNITY_API void au_cv_Size_delete(cv::Size* size);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Returns the area (width*height).
  //! \param size The Size used.
  ARUCO_UNITY_API int au_cv_Size_area(cv::Size* size);

  //! @} Member Functions

  //! \name Attributes
  //! @{

  //! \brief Returns the height.
  //! \param size The Size used.
  ARUCO_UNITY_API int au_cv_Size_getHeight(cv::Size* size);

  //! \brief Sets the height.
  //! \param size The Size used.
  //! \param height The new value.
  ARUCO_UNITY_API void au_cv_Size_setHeight(cv::Size* size, int height);

  //! \brief Returns the width.
  //! \param size The Size used.
  ARUCO_UNITY_API int au_cv_Size_getWidth(cv::Size* size);

  //! \brief Sets the width.
  //! \param size The Size used.
  //! \param width The new value.
  ARUCO_UNITY_API void au_cv_Size_setWidth(cv::Size* size, int width);

  //! @} Attributes
}

//! @} size

#endif