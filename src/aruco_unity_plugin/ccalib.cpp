#include "aruco_unity/cv/ccalib.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  double au_cv_ccalib_omnidir_calibrate(std::vector<std::vector<cv::Point3f>>* objectPoints, std::vector<std::vector<cv::Point2f>>* imagePoints,
    cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* xi, cv::Mat* distCoeffs, std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs,
    int flags, cv::TermCriteria* criteria, cv::Mat** idx, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Vec3d>(), *tvecs = new std::vector<cv::Vec3d>();
      *idx = new cv::Mat();
      error = cv::omnidir::calibrate(*objectPoints, *imagePoints, *imageSize, *cameraMatrix, *xi, *distCoeffs, **rvecs, **tvecs, flags, *criteria,
        **idx);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
    return error;
  }

  void au_cv_ccalib_omnidir_initUndistortRectifyMap(cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* xi, cv::Mat* R, cv::Mat* newCameraMatrix,
    cv::Size* size, int m1type, cv::Mat** map1, cv::Mat** map2, int flags, cv::Exception* exception) {
    try {
      *map1 = new cv::Mat(), *map2 = new cv::Mat();
      cv::omnidir::initUndistortRectifyMap(*cameraMatrix, *distCoeffs, *xi, *R, *newCameraMatrix, *size, m1type, **map1, **map2, flags);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  double au_cv_ccalib_omnidir_stereoCalibrate(std::vector<std::vector<cv::Point3f>>* objectPoints, 
    std::vector<std::vector<cv::Point2f>>* imagePoints1, std::vector<std::vector<cv::Point2f>>* imagePoints2, cv::Size* imageSize1,
    cv::Size* imageSize2, cv::Mat* cameraMatrix1, cv::Mat xi1, cv::Mat* distCoeffs1, cv::Mat* cameraMatrix2, cv::Mat* xi2, cv::Mat* distCoeffs2,
    cv::Vec3d** rvec, cv::Vec3d** tvec, std::vector<cv::Vec3d>** rvecsL, std::vector<cv::Vec3d>** tvecsL, int flags, cv::TermCriteria* criteria, cv::Mat** idx,
    cv::Exception* exception) {
    double error = 0;
    try {
      *rvec = new cv::Vec3d(), *tvec = new cv::Vec3d(), *rvecsL = new std::vector<cv::Vec3d>(), *tvecsL = new std::vector<cv::Vec3d>(),
        *idx = new cv::Mat();
      error = cv::omnidir::stereoCalibrate(*objectPoints, *imagePoints1, *imagePoints2, *imageSize1, *imageSize2, *cameraMatrix1, *xi2, *distCoeffs1,
        *cameraMatrix2, *xi2, *distCoeffs2, **rvec, **tvec, **rvecsL, **tvecsL, flags, *criteria, **idx);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
    return error;
  }

  void au_cv_ccalib_omnidir_stereoRectify(cv::Vec3d* rvec, cv::Vec3d* tvec, cv::Mat** R1, cv::Mat** R2, cv::Exception* exception) {
    try {
      *R1 = new cv::Mat(), *R2 = new cv::Mat();
      cv::omnidir::stereoRectify(*rvec, *tvec, **R1, **R2);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_cv_ccalib_omnidir_undistortImage(cv::Mat* distorted, cv::Mat** undistorted, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat* xi,
    int flags, cv::Mat* newCameraMatrix, cv::Size* newSize, cv::Mat* R, cv::Exception* exception) {
    try {
      *undistorted = new cv::Mat();
      cv::omnidir::undistortImage(*distorted, **undistorted, *cameraMatrix, *distCoeffs, *xi, flags, *newCameraMatrix, *newSize, *R);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }
}