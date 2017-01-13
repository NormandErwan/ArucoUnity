#ifndef __ARUCO_UNITY_IMGPROC_HPP__
#define __ARUCO_UNITY_IMGPROC_HPP__

#include <opencv2/imgproc.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup imgproc
//! \brief Image processing module.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d7/dbd/group__imgproc.html
//! @{

extern "C" {
  //! \name Static Member Functions
  //! @{

  //! \brief Transforms an image to compensate for lens distortion.
  //! \param src Input (distorted) image. 
  //! \param dst Output (corrected) image that has the same size and type as src.
  //! \param cameraMatrix Input camera matrix.
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/da/d54/group__imgproc__transform.html#ga69f2545a8b62a6b0fc2ee060dc30559d
  // TODO: au_cv_imgproc_undistord1
  ARUCO_UNITY_API void au_cv_imgproc_undistord2(cv::Mat* src, cv::Mat** dst, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} imgproc

#endif