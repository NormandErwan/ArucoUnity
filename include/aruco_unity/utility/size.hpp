#ifndef __ARUCO_UNITY_SIZE_HPP__
#define __ARUCO_UNITY_SIZE_HPP__

#include <opencv2\core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_size
//! \brief Class for specifying the size of an image or rectangle.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d6/d50/classcv_1_1Size__.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Size.
  ARUCO_UNITY_API cv::Size* au_Size_new();

  //! \brief Deletes any Size.
  //! \param size The Size used.
  ARUCO_UNITY_API void au_Size_delete(cv::Size* size);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Returns the area (width*height).
  //! \param size The Size used.
  ARUCO_UNITY_API int au_Size_area(cv::Size* size);

  //! @} Member Functions

  //! \name Variables
  //! @{

  //! \brief Returns the height.
  //! \param size The Size used.
  ARUCO_UNITY_API int au_Size_getHeight(cv::Size* size);

  //! \brief Sets the height.
  //! \param size The Size used.
  //! \param height The new value.
  ARUCO_UNITY_API void au_Size_setHeight(cv::Size* size, int height);

  //! \brief Returns the width.
  //! \param size The Size used.
  ARUCO_UNITY_API int au_Size_getWidth(cv::Size* size);

  //! \brief Sets the width.
  //! \param size The Size used.
  //! \param width The new value.
  ARUCO_UNITY_API void au_Size_setWidth(cv::Size* size, int width);

  //! @} Variables
}

//! @} utility_size

#endif