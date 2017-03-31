#include "aruco_unity/cv/calib3d.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Static member functions
  void au_cv_calib3d_Rodrigues(cv::Vec3d* src, cv::Mat** dst, cv::Exception* exception) {
    try {
      *dst = new cv::Mat();
      cv::Rodrigues(*src, **dst);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  double au_cv_calib3d_fisheye_calibrate1(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* image_size, cv::Mat* K, cv::Mat* D, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>(), *tvecs = new std::vector<cv::Mat>();
      error = cv::fisheye::calibrate(*objectPoints, *imagePoints, *image_size, *K, *D, **rvecs, **tvecs, flags, *criteria);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_cv_calib3d_fisheye_calibrate2(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* image_size, cv::Mat* K, cv::Mat* D, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, int flags, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>(), *tvecs = new std::vector<cv::Mat>();
      error = cv::fisheye::calibrate(*objectPoints, *imagePoints, *image_size, *K, *D, **rvecs, **tvecs, flags);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_cv_calib3d_fisheye_calibrate3(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints, cv::Size* image_size, cv::Mat* K, cv::Mat* D, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>(), *tvecs = new std::vector<cv::Mat>();
      error = cv::fisheye::calibrate(*objectPoints, *imagePoints, *image_size, *K, *D, **rvecs, **tvecs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify1(cv::Mat* K, cv::Mat* D, cv::Size* image_size, cv::Mat* R,
    cv::Mat** P, double balance, cv::Size* new_size, double fov_scale, cv::Exception* exception) {
    try {
      *P = new cv::Mat();
      cv::fisheye::estimateNewCameraMatrixForUndistortRectify(*K, *D, *image_size, *R, **P, balance, *new_size, fov_scale);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify2(cv::Mat* K, cv::Mat* D, cv::Size* image_size, cv::Mat* R,
    cv::Mat** P, double balance, cv::Size* new_size, cv::Exception* exception) {
    try {
      *P = new cv::Mat();
      cv::fisheye::estimateNewCameraMatrixForUndistortRectify(*K, *D, *image_size, *R, **P, balance, *new_size);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify3(cv::Mat* K, cv::Mat* D, cv::Size* image_size, cv::Mat* R,
    cv::Mat** P, double balance, cv::Exception* exception) {
    try {
      *P = new cv::Mat();
      cv::fisheye::estimateNewCameraMatrixForUndistortRectify(*K, *D, *image_size, *R, **P, balance);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify4(cv::Mat* K, cv::Mat* D, cv::Size* image_size, cv::Mat* R,
    cv::Mat** P, cv::Exception* exception) {
    try {
      *P = new cv::Mat();
      cv::fisheye::estimateNewCameraMatrixForUndistortRectify(*K, *D, *image_size, *R, **P);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_initUndistortRectifyMap(cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* R, cv::Mat* newCameraMatrix,
    cv::Size* size, int m1type, cv::Mat** map1, cv::Mat** map2, cv::Exception* exception) {
    try {
      *map1 = new cv::Mat(), *map2 = new cv::Mat();
      cv::fisheye::initUndistortRectifyMap(*cameraMatrix, *distCoeffs, *R, *newCameraMatrix, *size, m1type, **map1, **map2);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  double au_cv_calib3d_fisheye_stereoCalibrate1(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Mat** K1, cv::Mat** D1,
    cv::Mat** K2, cv::Mat** D2, cv::Size* imageSize, cv::Mat** R, cv::Mat** T, int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *K1 = new cv::Mat(), *D1 = new cv::Mat(), *K2 = new cv::Mat(), *D2 = new cv::Mat(), *R = new cv::Mat();
      error = cv::fisheye::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, **K1, **D1, **K2, **D2, *imageSize, **R, **R, flags, 
        *criteria);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_cv_calib3d_fisheye_stereoCalibrate2(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Mat** K1, cv::Mat** D1,
    cv::Mat** K2, cv::Mat** D2, cv::Size* imageSize, cv::Mat** R, cv::Mat** T, int flags, cv::Exception* exception) {
    double error = 0;
    try {
      *K1 = new cv::Mat(), *D1 = new cv::Mat(), *K2 = new cv::Mat(), *D2 = new cv::Mat(), *R = new cv::Mat();
      error = cv::fisheye::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, **K1, **D1, **K2, **D2, *imageSize, **R, **R, flags);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_cv_calib3d_fisheye_stereoCalibrate3(std::vector<std::vector<cv::Point3f>>* objectPoints,
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Mat** K1, cv::Mat** D1,
    cv::Mat** K2, cv::Mat** D2, cv::Size* imageSize, cv::Mat** R, cv::Mat** T, cv::Exception* exception) {
    double error = 0;
    try {
      *K1 = new cv::Mat(), *D1 = new cv::Mat(), *K2 = new cv::Mat(), *D2 = new cv::Mat(), *R = new cv::Mat();
      error = cv::fisheye::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, **K1, **D1, **K2, **D2, *imageSize, **R, **R);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  void au_cv_calib3d_fisheye_stereoRectify1(cv::Mat* K1, cv::Mat* D1, cv::Mat* K2, cv::Mat* D2, cv::Size* imageSize, cv::Mat* R,
    cv::Mat* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q, int flags, cv::Size* newImageSize, double balance,
    double fov_scale, cv::Exception* exception) {
    try {
      *R1 = new cv::Mat(), *R2 = new cv::Mat(), *P1 = new cv::Mat(), *P2 = new cv::Mat(), *Q = new cv::Mat();
      cv::fisheye::stereoRectify(*K1, *D1, *K2, *D2, *imageSize, *R, *tvec, **R1, **R2, **P1, **P2, **Q, flags, *newImageSize,
        balance, fov_scale);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_stereoRectify2(cv::Mat* K1, cv::Mat* D1, cv::Mat* K2, cv::Mat* D2, cv::Size* imageSize, cv::Mat* R,
    cv::Mat* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q, int flags, cv::Size* newImageSize, double balance,
    cv::Exception* exception) {
    try {
      *R1 = new cv::Mat(), *R2 = new cv::Mat(), *P1 = new cv::Mat(), *P2 = new cv::Mat(), *Q = new cv::Mat();
      cv::fisheye::stereoRectify(*K1, *D1, *K2, *D2, *imageSize, *R, *tvec, **R1, **R2, **P1, **P2, **Q, flags, *newImageSize,
        balance);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_stereoRectify3(cv::Mat* K1, cv::Mat* D1, cv::Mat* K2, cv::Mat* D2, cv::Size* imageSize, cv::Mat* R,
    cv::Mat* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q, int flags, cv::Size* newImageSize,
    cv::Exception* exception) {
    try {
      *R1 = new cv::Mat(), *R2 = new cv::Mat(), *P1 = new cv::Mat(), *P2 = new cv::Mat(), *Q = new cv::Mat();
      cv::fisheye::stereoRectify(*K1, *D1, *K2, *D2, *imageSize, *R, *tvec, **R1, **R2, **P1, **P2, **Q, flags, *newImageSize);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_stereoRectify4(cv::Mat* K1, cv::Mat* D1, cv::Mat* K2, cv::Mat* D2, cv::Size* imageSize, cv::Mat* R,
    cv::Mat* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q, int flags, cv::Exception* exception) {
    try {
      *R1 = new cv::Mat(), *R2 = new cv::Mat(), *P1 = new cv::Mat(), *P2 = new cv::Mat(), *Q = new cv::Mat();
      cv::fisheye::stereoRectify(*K1, *D1, *K2, *D2, *imageSize, *R, *tvec, **R1, **R2, **P1, **P2, **Q, flags);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_undistortImage1(cv::Mat* distorted, cv::Mat** undistorted, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Mat* newCameraMatrix, cv::Size* newSize, cv::Exception* exception) {
    try {
      *undistorted = new cv::Mat();
      cv::fisheye::undistortImage(*distorted, **undistorted, *cameraMatrix, *distCoeffs, *newCameraMatrix, *newSize);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_undistortImage2(cv::Mat* distorted, cv::Mat** undistorted, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Mat* newCameraMatrix, cv::Exception* exception) {
    try {
      *undistorted = new cv::Mat();
      cv::fisheye::undistortImage(*distorted, **undistorted, *cameraMatrix, *distCoeffs, *newCameraMatrix);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_calib3d_fisheye_undistortImage3(cv::Mat* distorted, cv::Mat** undistorted, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Exception* exception) {
    try {
      *undistorted = new cv::Mat();
      cv::fisheye::undistortImage(*distorted, **undistorted, *cameraMatrix, *distCoeffs);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }
}