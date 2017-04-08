#ifndef __ARUCO_UNITY_CORE_HPP__
#define __ARUCO_UNITY_CORE_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup core
//! \brief Core module.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d0/de1/group__core.html
//! @{

extern "C" {
  //! \name Static Member Functions
  //! @{

  //! \brief Flips a 2D array around vertical, horizontal, or both axes.
  //! \param src Input array.
  //! \param dst Output array of the same size and type as src.
  //! \param flipCode A flag to specify how to flip the array; 0 means flipping around the x-axis and positive value (for example, 1) means
  //! flipping around y-axis. Negative value (for example, -1) means flipping around both axes. 
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.2.0/d2/de8/group__core__array.html#gaca7be533e3dac7feb70fc60635adf441
  ARUCO_UNITY_API void au_cv_core_flip(cv::Mat* src, cv::Mat* dst, int flipCode, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} core

#endif