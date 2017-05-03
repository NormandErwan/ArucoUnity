#ifndef __ARUCO_UNITY_CALIB3D_HPP__
#define __ARUCO_UNITY_CALIB3D_HPP__

#include <opencv2/calib3d.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup calib3d
//! \brief Camera Calibration and 3D Reconstruction module.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html
//! @{

extern "C" {
  //! \name Static Member Functions
  //! @{

  //! \brief Finds the camera intrinsic and extrinsic parameters from several views of a calibration pattern.
  //!
  //! \param objectPoints Vector of vectors of calibration pattern points in the calibration pattern coordinate space.
  //! \param imagePoints Vector of vectors of the projections of calibration pattern points.
  //! \param imageSize Size of the image used only to initialize the intrinsic camera matrix.
  //! \param cameraMatrix Output 3x3 floating-point camera matrix.
  //! \param distCoeffs Output vector of distortion coefficients.
  //! \param rvecs Output vector of rotation vectors (see Rodrigues) estimated for each pattern view.
  //! \param tvecs Output vector of translation vectors estimated for each pattern view.
  //! \param stdDeviationsIntrinsics Output vector of standard deviations estimated for intrinsic parameters.
  //! \param stdDeviationsExtrinsics Output vector of standard deviations estimated for extrinsic parameters.
  //! \param perViewErrors Output vector of the RMS re-projection error estimated for each pattern view.
  //! \param flags Different flags that may be zero or a combination.
  //! \param criteria Termination criteria for the iterative optimization algorithm.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The overall RMS re-projection error.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga3207604e4b1a1758aa66acb6ed5aa65d
  ARUCO_UNITY_API double au_cv_calib3d_calibrateCamera1(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, std::vector<double>* stdDeviationsIntrinsics,
    std::vector<double>* stdDeviationsExtrinsics, std::vector<double>* perViewErrors, int flags, cv::TermCriteria* criteria,
    cv::Exception* exception);

  //! \see au_cv_calib3d_calibrateCamera1().
  ARUCO_UNITY_API double au_cv_calib3d_calibrateCamera2(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception);

  //! \brief Finds an initial camera matrix from 3D-2D point correspondences.
  //! 
  //! \param objectPoints Vector of vectors of the calibration pattern points in the calibration pattern coordinate space.
  //! \param imagePoints Vector of vectors of the projections of the calibration pattern points.
  //! \param imageSize Image size in pixels used to initialize the principal point.
  //! \param aspectRatio
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga8132c7dbbb61738cc3510bebbdffde55
  ARUCO_UNITY_API cv::Mat* au_cv_calib3d_initCameraMatrix2D(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* imageSize, double aspectRatio, cv::Exception* exception);

  //! \brief Converts a rotation matrix to a rotation vector or vice versa. 
  //! \param src Input rotation vector (3x1 or 1x3) or rotation matrix (3x3). 
  //! \param dst Output rotation matrix (3x3) or rotation vector (3x1 or 1x3), respectively.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga61585db663d9da06b68e70cfbf6a1eac
  ARUCO_UNITY_API void au_cv_calib3d_Rodrigues(cv::Vec3d* src, cv::Mat** dst, cv::Exception* exception);

  //! \brief Calibrates the stereo camera.
  //! 
  //! \param objectPoints Vector of vectors of the calibration pattern points.
  //! \param imagePoints1 Vector of vectors of the projections of the calibration pattern points, observed by the first camera.
  //! \param imagePoints2 Vector of vectors of the projections of the calibration pattern points, observed by the second camera.
  //! \param cameraMatrix1 Input/output first camera matrix.
  //! \param distCoeffs1 Input/output vector of distortion coefficients.
  //! \param cameraMatrix2 Input/output second camera matrix.
  //! \param distCoeffs2 Input/output lens distortion coefficients for the second camera.
  //! \param imageSize Size of the image used only to initialize intrinsic camera matrix.
  //! \param rvec Output rotation vector between the 1st and the 2nd camera coordinate systems.
  //! \param tvec Output translation vector between the coordinate systems of the cameras.
  //! \param E Output essential matrix.
  //! \param F Output fundamental matrix.
  //! \param flags Different flags that may be zero or a combination.
  //! \param criteria Termination criteria for the iterative optimization algorithm.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga246253dcc6de2e0376c599e7d692303a
  ARUCO_UNITY_API double au_cv_calib3d_stereoCalibrate(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Mat* cameraMatrix1, 
    cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* distCoeffs2, cv::Size* imageSize, cv::Vec3d** rvec, cv::Vec3d** tvec, cv::Mat** E,
    cv::Mat** F, int flags, cv::TermCriteria* criteria, cv::Exception* exception);

  //! \brief Computes rectification transforms for each head of a calibrated stereo camera.
  //! 
  //! \param cameraMatrix1 First camera matrix.
  //! \param distCoeffs1 First camera distortion parameters.
  //! \param cameraMatrix2 Second camera matrix.
  //! \param distCoeffs2 Second camera distortion parameters.
  //! \param imageSize Size of the image used for stereo calibration.
  //! \param rvec Rotation vector between the coordinate systems of the first and the second cameras.
  //! \param tvec Translation vector between coordinate systems of the cameras.
  //! \param R1 Output 3x3 rectification transform (rotation matrix) for the first camera.
  //! \param R2 Output 3x3 rectification transform (rotation matrix) for the second camera.
  //! \param P1 Output 3x4 projection matrix in the new (rectified) coordinate systems for the first camera.
  //! \param P2 Output 3x4 projection matrix in the new (rectified) coordinate systems for the second camera.
  //! \param Q Output 4x4 disparity-to-depth mapping matrix (see reprojectImageTo3D).
  //! \param flags Operation flags that may be zero or CV_CALIB_ZERO_DISPARITY.
  //! \param alpha Free scaling parameter.
  //! \param newImageSize New image resolution after rectification.
  //! \param validPixROI1 Optional output rectangles inside the rectified images where all the pixels are valid.
  //! \param validPixROI2 Optional output rectangles inside the rectified images where all the pixels are valid.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga617b1685d4059c6040827800e72ad2b6
  ARUCO_UNITY_API void au_cv_calib3d_stereoRectify(cv::Mat* cameraMatrix1, cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* distCoeffs2,
    cv::Size* imageSize, cv::Vec3d* rvec, cv::Vec3d* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q, int flags,
    double alpha, cv::Size* newImageSize, cv::Rect* validPixROI1, cv::Rect* validPixROI2, cv::Exception* exception);

  //! \brief Performs camera calibaration.
  //!
  //! \param objectPoints Vector of vectors of calibration pattern points in the calibration pattern coordinate space.
  //! \param imagePoints Vector of vectors of the projections of calibration pattern points.
  //! \param imageSize Size of the image used only to initialize the intrinsic camera matrix.
  //! \param cameraMatrix Output 3x3 floating-point camera matrix.
  //! \param distCoeffs Output vector of distortion coefficients.
  //! \param rvecs Output vector of rotation vectors (see Rodrigues) estimated for each pattern view.
  //! \param tvecs Output vector of translation vectors estimated for each pattern view.
  //! \param flags Different flags for the calibration process (see 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#gad626a78de2b1dae7489e152a5a5a89e1).
  //! \param criteria Termination criteria for the iterative optimization algorithm.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#gad626a78de2b1dae7489e152a5a5a89e1
  ARUCO_UNITY_API double au_cv_calib3d_fisheye_calibrate(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception);

  //! \brief Estimates new camera matrix for undistortion or rectification.
  //!
  //! \param cameraMatrix Camera matrix.
  //! \param imageSize
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param R Rectification transformation in the object space.
  //! \param P New camera matrix (3x3) or new projection matrix (3x4).
  //! \param balance Sets the new focal length in range between the min focal length and the max focal length. Balance is in range of [0, 1].
  //! \param newSize
  //! \param fovScale Divisor for new focal length.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#ga384940fdf04c03e362e94b6eb9b673c9
  ARUCO_UNITY_API void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify(cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Size* imageSize, cv::Mat* R, cv::Mat** P, double balance, cv::Size* newSize, double fovScale, cv::Exception* exception);

  //! \brief Computes undistortion and rectification maps for image transform by cv::remap().
  //!
  //! \param cameraMatrix Camera matrix.
  //! \param distCoeffs Input vector of distortion coefficients.
  //! \param R Rectification transformation in the object space.
  //! \param P New camera matrix (3x3) or new projection matrix (3x4)
  //! \param size Undistorted image size.
  //! \param m1type Type of the first output map that can be CV_32FC1 or CV_16SC2.
  //! \param map1 The first output map.
  //! \param map2 The second output map.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#ga0d37b45f780b32f63ed19c21aa9fd333
  ARUCO_UNITY_API void au_cv_calib3d_fisheye_initUndistortRectifyMap(cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* R,
    cv::Mat* newCameraMatrix, cv::Size* size, int m1type, cv::Mat** map1, cv::Mat** map2, cv::Exception* exception);

  //! \brief Performs stereo calibration
  //!
  //! \param objectPoints Vector of vectors of the calibration pattern points.
  //! \param imagePoints1 Vector of vectors of the projections of the calibration pattern points, observed by the first camera.
  //! \param imagePoints2 Vector of vectors of the projections of the calibration pattern points, observed by the second camera.
  //! \param cameraMatrix1 Input/output first camera matrix.
  //! \param distCoeffs1 Input/output vector of distortion coefficients.
  //! \param cameraMatrix2 Input/output second camera matrix.
  //! \param distCoeffs2 Input/output lens distortion coefficients for the second camera
  //! \param imageSize Size of the image used only to initialize intrinsic camera matrix.
  //! \param rvec Output rotation vector between the 1st and the 2nd camera coordinate systems.
  //! \param tvec Output translation vector between the coordinate systems of the cameras.
  //! \param flags Different flags for the calibration process (see 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#gadbb3a6ca6429528ef302c784df47949b)
  //! \param criteria Termination criteria for the iterative optimization algorithm.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#gadbb3a6ca6429528ef302c784df47949b
  ARUCO_UNITY_API double au_cv_calib3d_fisheye_stereoCalibrate(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Mat* cameraMatrix1,
    cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* distCoeffs2, cv::Size* imageSize, cv::Vec3d** rvec, cv::Vec3d** tvec, int flags,
    cv::TermCriteria* criteria, cv::Exception* exception);

  //! \brief Stereo rectification for fisheye camera model
  //!
  //! \param cameraMatrix1 First camera matrix.
  //! \param distCoeffs1 First camera distortion parameters.
  //! \param cameraMatrix2 Second camera matrix.
  //! \param distCoeffs2 Second camera distortion parameters.
  //! \param imageSize Size of the image used for stereo calibration.
  //! \param rvec Rotation vector between the coordinate systems of the first and the second cameras.
  //! \param tvec Translation vector between coordinate systems of the cameras.
  //! \param R1 Output 3x3 rectification transform (rotation matrix) for the first camera.
  //! \param R2 Output 3x3 rectification transform (rotation matrix) for the second camera.
  //! \param P1 Output 3x4 projection matrix in the new (rectified) coordinate systems for the first camera.
  //! \param P2 Output 3x4 projection matrix in the new (rectified) coordinate systems for the second camera.
  //! \param Q Output 4x4 disparity-to-depth mapping matrix (see reprojectImageTo3D).
  //! \param flags Operation flags that may be zero or CV_CALIB_ZERO_DISPARITY.
  //! \param newImageSize New image resolution after rectification.
  //! \param balance Sets the new focal length in range between the min focal length and the max focal length. Balance is in range of [0, 1].
  //! \param fovScale Divisor for new focal length.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#gac1af58774006689056b0f2ef1db55ecc
  ARUCO_UNITY_API void au_cv_calib3d_fisheye_stereoRectify(cv::Mat* cameraMatrix1, cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2,
    cv::Mat* distCoeffs2, cv::Size* imageSize, cv::Vec3d* rvec, cv::Vec3d* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q,
    int flags, cv::Size* newImageSize, double balance, double fovScale, cv::Exception* exception);

  //! \brief Transforms an image to compensate for fisheye lens distortion.
  //!
  //! \param distorted Image with fisheye lens distortion.
  //! \param undistorted Output image with compensated fisheye lens distortion.
  //! \param K Camera matrix.
  //! \param D Input vector of distortion coefficients.
  //! \param Knew Camera matrix of the distorted image.
  //! \param new_size
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/db/d58/group__calib3d__fisheye.html#ga167df4b00a6fd55287ba829fbf9913b9
  ARUCO_UNITY_API void au_cv_calib3d_fisheye_undistortImage(cv::Mat* distorted, cv::Mat** undistorted, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Mat* newCameraMatrix, cv::Size* newSize, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} calib3d

#endif