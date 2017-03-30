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

  //! \brief Computes the undistortion and rectification transformation map. 
  //! \param cameraMatrix Input camera matrix. 
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param R Optional rectification transformation in the object space (3x3 matrix).
  //! \param newCameraMatrix New camera matrix.
  //! \param size Undistorted image size.
  //! \param m1type Type of the first output map that can be CV_32FC1 or CV_16SC2.
  //! \param map1 The first output map.
  //! \param map2 The second output map.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/da/d54/group__imgproc__transform.html#ga7dfb72c9cf9780a347fbe3d1c47e5d5a
  ARUCO_UNITY_API void au_cv_imgproc_initUndistortRectifyMap(cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* R, cv::Mat* newCameraMatrix,
    cv::Size* size, int m1type, cv::Mat** map1, cv::Mat** map2, cv::Exception* exception);

  //! \brief Applies a generic geometrical transformation to an image. 
  //! \param src Source image.
  //! \param dst Destination image.
  //! \param map1 The first map of either (x,y) points or just x values having the type CV_16SC2 , CV_32FC1, or CV_32FC2.
  //! \param map2 The second map of y values having the type CV_16UC1, CV_32FC1, or none (empty map if map1 is (x,y) points), respectively.
  //! \param interpolation Interpolation method (see cv::InterpolationFlags).
  //! \param borderType Pixel extrapolation method (see cv::BorderTypes).
  //! \param borderValue Value used in case of a constant border.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/da/d54/group__imgproc__transform.html#gab75ef31ce5cdfb5c44b6da5f3b908ea4
  ARUCO_UNITY_API void au_cv_imgproc_remap1(cv::Mat* src, cv::Mat* dst, cv::Mat* map1, cv::Mat* map2, int interpolation, int borderType,
    cv::Scalar* borderValue, cv::Exception* exception);

  //! \see au_cv_imgproc_remap1().
  ARUCO_UNITY_API void au_cv_imgproc_remap2(cv::Mat* src, cv::Mat* dst, cv::Mat* map1, cv::Mat* map2, int interpolation, int borderType,
    cv::Exception* exception);

  //! \see au_cv_imgproc_remap1().
  ARUCO_UNITY_API void au_cv_imgproc_remap3(cv::Mat* src, cv::Mat* dst, cv::Mat* map1, cv::Mat* map2, int interpolation,
    cv::Exception* exception);

  //! \brief Transforms an image to compensate for lens distortion.
  //! \param src Input (distorted) image. 
  //! \param dst Output (corrected) image that has the same size and type as src.
  //! \param cameraMatrix Input camera matrix.
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param newCameraMatrix Camera matrix of the distorted image.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/da/d54/group__imgproc__transform.html#ga69f2545a8b62a6b0fc2ee060dc30559d
  ARUCO_UNITY_API void au_cv_imgproc_undistort1(cv::Mat* src, cv::Mat** dst, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* newCameraMatrix, 
    cv::Exception* exception);

  //! \see au_cv_imgproc_undistort1().
  ARUCO_UNITY_API void au_cv_imgproc_undistort2(cv::Mat* src, cv::Mat** dst, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} imgproc

#endif