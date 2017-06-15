#include "aruco_unity/cv/calib3d.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  double au_cv_calib3d_calibrateCamera1(std::vector<std::vector<cv::Point3f>>* objectPoints, std::vector<std::vector<cv::Point2f>>* imagePoints,
    cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs,
    std::vector<double>* stdDeviationsIntrinsics, std::vector<double>* stdDeviationsExtrinsics, std::vector<double>* perViewErrors,
    int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Vec3d>(), *tvecs = new std::vector<cv::Vec3d>();
      error = cv::calibrateCamera(*objectPoints, *imagePoints, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, *stdDeviationsIntrinsics, 
        *stdDeviationsExtrinsics, *perViewErrors, flags, *criteria);
    } catch (const cv::Exception& e) { 
      ARUCO_UNITY_COPY_EXCEPTION(exception, e); 
      return error;
    }
    return error;
  }

  double au_cv_calib3d_calibrateCamera2(std::vector<std::vector<cv::Point3f>>* objectPoints, std::vector<std::vector<cv::Point2f>>* imagePoints,
    cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, int flags,
    cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Vec3d>(), *tvecs = new std::vector<cv::Vec3d>();
      error = cv::calibrateCamera(*objectPoints, *imagePoints, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, flags, *criteria);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  cv::Mat* au_cv_calib3d_initCameraMatrix2D(std::vector<std::vector<cv::Point3f>>* objectPoints, std::vector<std::vector<cv::Point2f>>* imagePoints,
    cv::Size* imageSize, double aspectRatio, cv::Exception* exception) {
    cv::Mat cameraMatrix;
    try {
      cameraMatrix = cv::initCameraMatrix2D(*objectPoints, *imagePoints, *imageSize, aspectRatio);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e); 
      return NULL; 
    }
    return new cv::Mat(cameraMatrix);
  }

  void au_cv_calib3d_Rodrigues(cv::Vec3d* src, cv::Mat** dst, cv::Exception* exception) {
    try {
      *dst = new cv::Mat();
      cv::Rodrigues(*src, **dst);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  double au_cv_calib3d_stereoCalibrate(std::vector<std::vector<cv::Point3f>>* objectPoints, std::vector<std::vector<cv::Point2f>>* imagePoints1,
    std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Mat* cameraMatrix1, cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* distCoeffs2,
    cv::Size* imageSize, cv::Vec3d** rvec, cv::Vec3d** tvec, cv::Mat** E, cv::Mat** F, int flags, cv::TermCriteria* criteria,
    cv::Exception* exception) {
    double error = 0;
    try {
      *rvec = new cv::Vec3d(), *tvec = new cv::Vec3d(), *E = new cv::Mat(), *F = new cv::Mat();
      error = cv::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, *cameraMatrix1, *distCoeffs1, *cameraMatrix2, *distCoeffs2,
        *imageSize, **rvec, **tvec, **E, **F, flags, *criteria);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  void au_cv_calib3d_stereoRectify(cv::Mat* cameraMatrix1, cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* distCoeffs2, cv::Size* imageSize,
    cv::Vec3d* rvec, cv::Vec3d* tvec, cv::Mat** R1, cv::Mat** R2, cv::Mat** P1, cv::Mat** P2, cv::Mat** Q, int flags, double alpha,
    cv::Size* newImageSize, cv::Rect* validPixROI1, cv::Rect* validPixROI2, cv::Exception* exception) {
    try {
      *R1 = new cv::Mat(), *R2 = new cv::Mat(), *P1 = new cv::Mat(), *P2 = new cv::Mat(), *Q = new cv::Mat();
      cv::stereoRectify(*cameraMatrix1, *distCoeffs1, *cameraMatrix2, *distCoeffs2, *imageSize, *rvec, *tvec, **R1, **R2, **P1, **P2, **Q, flags,
        alpha, *newImageSize, validPixROI1, validPixROI2);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }
}