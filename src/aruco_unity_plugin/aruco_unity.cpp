#include "aruco_unity.hpp"
#include "aruco_unity/cv/exception.hpp"
#include <opencv2/imgproc.hpp>

extern "C" {
  double au_calibrateCameraAruco(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    const cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();
      error = cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, flags,
        *criteria);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraCharuco(std::vector<std::vector<cv::Point2f>>* charucoCorners, std::vector<std::vector<int>>* charucoIds,
    const cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();
      error = cv::aruco::calibrateCameraCharuco(*charucoCorners, *charucoIds, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs,
        flags, *criteria);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  void au_detectCharucoDiamond(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds,
    float squareMarkerLengthRate, std::vector<std::vector<cv::Point2f>>** diamondCorners, std::vector<cv::Vec4i>** diamondIds,
    cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception) {
    try {
      *diamondCorners = new std::vector<std::vector<cv::Point2f>>();
      *diamondIds = new std::vector<cv::Vec4i>();
      cv::aruco::detectCharucoDiamond(*image, *markerCorners, *markerIds, squareMarkerLengthRate, **diamondCorners, **diamondIds,
        *cameraMatrix, *distCoeffs);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_detectMarkers(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, std::vector<std::vector<cv::Point2f>>** rejectedImgPoints,
    cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *rejectedImgPoints = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids, *parameters, **rejectedImgPoints);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_drawAxis(cv::Mat* image, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Vec3d* rvec, cv::Vec3d* tvec, float length,
    cv::Exception* exception)
  {
    try {
      cv::aruco::drawAxis(*image, *cameraMatrix, *distCoeffs, *rvec, *tvec, length);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); return; }
  }

  void au_drawCharucoDiamond(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Vec4i* ids, int squareLength, int markerLength, cv::Mat** img,
    int marginSize, int borderBits, cv::Exception* exception) {
    try {
      *img = new cv::Mat();
      cv::aruco::drawCharucoDiamond(*dictionary, *ids, squareLength, markerLength, **img, marginSize, borderBits);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    };
    cv::cvtColor(**img, **img, cv::COLOR_GRAY2RGB);
  }

  void au_drawDetectedCornersCharuco(cv::Mat* image, std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, cv::Scalar* cornerColor,
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedCornersCharuco(*image, *charucoCorners, *charucoIds, *cornerColor);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_drawDetectedDiamonds(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* diamondCorners, std::vector<cv::Vec4i>* diamondIds,
    cv::Scalar* borderColor,
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedDiamonds(*image, *diamondCorners, *diamondIds, *borderColor);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  void au_drawDetectedMarkers(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, cv::Scalar* borderColor,
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, *ids, *borderColor);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  int au_estimatePoseBoard(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, const cv::Ptr<cv::aruco::Board>* board,
    cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Vec3d** rvec, cv::Vec3d** tvec, cv::Exception* exception) {
    int valid = 0;
    try {
      *rvec = new cv::Vec3d();
      *tvec = new cv::Vec3d();
      valid = cv::aruco::estimatePoseBoard(*corners, *ids, *board, *cameraMatrix, *distCoeffs, **rvec, **tvec);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return valid;
    }
    return valid;
  }
  
  bool au_estimatePoseCharucoBoard(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds,
    const cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Vec3d** rvec, cv::Vec3d** tvec,
    cv::Exception* exception) {
    bool validPose = 0;
    try {
      *rvec = new cv::Vec3d();
      *tvec = new cv::Vec3d();
      validPose = cv::aruco::estimatePoseCharucoBoard(*charucoCorners, *charucoIds, *board, *cameraMatrix, *distCoeffs, **rvec, **tvec);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return validPose;
    }
    return validPose;
  }

  void au_estimatePoseSingleMarkers(std::vector<std::vector<cv::Point2f>>* corners, float markerLength, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, 
    std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, cv::Exception* exception) {
    try {
      *rvecs = new std::vector<cv::Vec3d>();
      *tvecs = new std::vector<cv::Vec3d>();
      cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, **rvecs, **tvecs);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  cv::Ptr<cv::aruco::Dictionary>* au_generateCustomDictionary(int nMarkers, int markerSize, cv::Ptr<cv::aruco::Dictionary>* baseDictionary,
    cv::Exception* exception) {
    cv::Ptr<cv::aruco::Dictionary> dictionary;
    try {
      dictionary = cv::aruco::generateCustomDictionary(nMarkers, markerSize, *baseDictionary);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return NULL;
    }
    return new cv::Ptr<cv::aruco::Dictionary>(dictionary);
  }

  void au_getBoardObjectAndImagePoints(const cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<cv::Point3f>** objPoints, std::vector<cv::Point2f>** imgPoints, cv::Exception* exception) {
    try {
      *objPoints = new std::vector<cv::Point3f>();
      *imgPoints = new std::vector<cv::Point2f>();
      cv::aruco::getBoardObjectAndImagePoints(*board, *detectedCorners, *detectedIds, **objPoints, **imgPoints);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }

  cv::Ptr<cv::aruco::Dictionary>* au_getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::getPredefinedDictionary(name);
    return new cv::Ptr<cv::aruco::Dictionary>(ptr);
  }

  int au_interpolateCornersCharuco(std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds, cv::Mat* image,
    const cv::Ptr<cv::aruco::CharucoBoard>* board, std::vector<cv::Point2f>** charucoCorners, std::vector<int>** charucoIds, cv::Mat* cameraMatrix,
    cv::Mat* distCoeffs, cv::Exception* exception) {
    int interpolatedCorners = 0;
    try {
      *charucoCorners = new std::vector<cv::Point2f>();
      *charucoIds = new std::vector<int>();
      interpolatedCorners = cv::aruco::interpolateCornersCharuco(*markerCorners, *markerIds, *image, *board, **charucoCorners, **charucoIds,
        *cameraMatrix, *distCoeffs);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return interpolatedCorners;
    }
    return interpolatedCorners;
  }

  void au_refineDetectedMarkers(cv::Mat* image, const cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    float minRepDistance, float errorCorrectionRate, bool checkAllOrders, std::vector<int>* recoveredIdxs,
    const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix, *distCoeffs, minRepDistance,
        errorCorrectionRate, checkAllOrders, *recoveredIdxs, *parameters);
    } catch (const cv::Exception& e) { ARUCO_UNITY_COPY_EXCEPTION(exception, e); }
  }
}