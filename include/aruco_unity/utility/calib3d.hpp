#ifndef __ARUCO_UNITY_CALIB3D_HPP__
#define __ARUCO_UNITY_CALIB3D_HPP__

#include <opencv2\calib3d.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_calib3d
//! \brief Camera Calibration and 3D Reconstruction module.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html
//! @{

extern "C" {
  //! \name Static Member Functions
  //! @{

  //! \brief Converts a rotation matrix to a rotation vector or vice versa. 
  //! \param src Input rotation vector (3x1 or 1x3) or rotation matrix (3x3). 
  //! \param dst Output rotation matrix (3x3) or rotation vector (3x1 or 1x3), respectively.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga61585db663d9da06b68e70cfbf6a1eac
  ARUCO_UNITY_API void au_calib3d_Rodrigues(cv::Vec3d* src, cv::Mat** dst, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} utility_calib3d

#endif