#ifndef __ARUCO_UNITY_CCALIB_HPP__
#define __ARUCO_UNITY_CCALIB_HPP__

#include <opencv2/ccalib/omnidir.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup ccalib
//! \brief Custom Calibration Pattern for 3D reconstruction module.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d3/ddc/group__ccalib.html
//! @{

extern "C" {
  //! \name Static Member Functions
  //! @{

  //! \brief Perform omnidirectional camera calibration, the default depth of outputs is CV_64F. 
  //!
  //! \param objectPoints Vector of vector of Vec3f object points in world (pattern) coordinate.
  //! \param imagePoints Vector of vector of Vec2f corresponding image points of objectPoints.
  //! \param imageSize Image size of calibration images.
  //! \param cameraMatrix Output calibrated camera matrix.
  //! \param xi Output parameter xi for CMei's model.
  //! \param distCoeffs Output distortion parameters.
  //! \param rvecs Output rotations for each calibration images.
  //! \param tvecs Output translation for each calibration images.
  //! \param flags The flags that control calibrate.
  //! \param criteria Termination criteria for optimization.
  //! \param idx Indices of images that pass initialization, which are really used in calibration.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The overall RMS re-projection error.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.2.0/d3/ddc/group__ccalib.html#gac31505e43f856154a0fd7b65c1fc9ce9
  ARUCO_UNITY_API double au_cv_ccalib_omnidir_calibrate(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* xi, cv::Mat* distCoeffs,
    std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, int flags, cv::TermCriteria* criteria, cv::Mat** idx, cv::Exception* exception);

  //! \brief Computes undistortion and rectification maps for omnidirectional camera image transform by a rotation R.
  //!
  //! \param cameraMatrix Input camera matrix. 
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param xi The parameter xi for CMei's model.
  //! \param R Rotation transform between the original and object space.
  //! \param newCameraMatrix New camera matrix (3x3) or new projection matrix (3x4).
  //! \param size Undistorted image size.
  //! \param m1type Type of the first output map that can be CV_32FC1 or CV_16SC2.
  //! \param map1 The first output map.
  //! \param map2 The second output map.
  //! \param flags Flags indicates the rectification type.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.2.0/d3/ddc/group__ccalib.html#ga0d0b216ff9c9c2cee1ab9cc13cc20faa
  ARUCO_UNITY_API void au_cv_ccalib_omnidir_initUndistortRectifyMap(cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* xi, cv::Mat* R,
    cv::Mat* newCameraMatrix, cv::Size* size, int m1type, cv::Mat** map1, cv::Mat** map2, int flags, cv::Exception* exception);

  //! \brief Stereo calibration for omnidirectional camera model.
  //! 
  //! \param objectPoints Object points in world (pattern) coordinate.
  //! \param imagePoints1 The corresponding image points of the first camera.
  //! \param imagePoints2 The corresponding image points of the second camera.
  //! \param imageSize1 Image size of calibration images of the first camera.
  //! \param imageSize2 Image size of calibration images of the second camera.
  //! \param cameraMatrix1 Output camera matrix for the first camera.
  //! \param xi1 Output parameter xi of Mei's model for the first camera.
  //! \param distCoeffs1 Output vector of distortion coefficients for the first camera.
  //! \param cameraMatrix2 Output camera matrix for the second camera.
  //! \param xi2 Output parameter xi of Mei's model for the second camera.
  //! \param distCoeffs2 Output vector of distortion coefficients for the second camera.
  //! \param rvec Output rotation between the first and second camera.
  //! \param tvec Output translation between the first and second camera.
  //! \param rvecsL Output rotation for each image of the first camera.
  //! \param tvecsL Output translation for each image of the first camera.
  //! \param flags The flags that control stereoCalibrate.
  //! \param criteria Termination criteria for optimization.
  //! \param idx Indices of image pairs that pass initialization, which are really used in calibration.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.2.0/d3/ddc/group__ccalib.html#ga6f1aa828b02e7263394acb5f6821cc84
  ARUCO_UNITY_API double au_cv_ccalib_omnidir_stereoCalibrate(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Size* imageSize1,
    cv::Size* imageSize2, cv::Mat* cameraMatrix1, cv::Mat xi1, cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* xi2, cv::Mat* distCoeffs2,
    cv::Vec3d** rvec, cv::Vec3d** tvec, cv::Mat** rvecsL, cv::Mat** tvecsL, int flags, cv::TermCriteria* criteria, cv::Mat** idx,
    cv::Exception* exception);

  //! \brief Stereo rectification for omnidirectional camera model. It computes the rectification rotations for two cameras. 
  //! 
  //! \param rvec Rotation between the first and the second camera.
  //! \param tvec Translation between the first and the second camera.
  //! \param R1 Output 3x3 rotation matrix for the first camera.
  //! \param R2 Output 3x3 rotation matrix for the second camera.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.2.0/d3/ddc/group__ccalib.html#gaf055863d589cb166c23cc26fcaa6ce98
  ARUCO_UNITY_API void au_cv_ccalib_omnidir_stereoRectify(cv::Vec3d* rvec, cv::Vec3d* tvec, cv::Mat** R1, cv::Mat** R2, cv::Exception* exception);

  //! \brief Undistort omnidirectional images to perspective images.
  //!
  //! \param distorted The input omnidirectional image. 
  //! \param undistorted The output undistorted image.
  //! \param cameraMatrix Input camera matrix.
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param xi The parameter xi for CMei's model.
  //! \param flags Flags indicates the rectification type.
  //! \param newCameraMatrix Camera matrix of the distorted image.
  //! \param newSize The new image size.
  //! \param R Rotation matrix between the input and output images.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.2.0/d3/ddc/group__ccalib.html#gafe4f53d9b64bfe15b86e75a4699cbba4
  ARUCO_UNITY_API void au_cv_ccalib_omnidir_undistortImage(cv::Mat* distorted, cv::Mat** undistorted, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Mat* xi, int flags, cv::Mat* newCameraMatrix, cv::Size* newSize, cv::Mat* R, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} ccalib

#endif