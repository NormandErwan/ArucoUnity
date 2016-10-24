#include "aruco_unity.hpp"
#include "aruco_unity/utility/exception.hpp"
#include <opencv2/imgproc.hpp>

extern "C" {
  double au_calibrateCameraAruco1(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, flags, 
        *criteria);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraAruco2(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    std::vector<cv::Mat>** tvecs, int flags, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, flags);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraAruco3(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    std::vector<cv::Mat>** tvecs, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraAruco4(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraAruco5(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception) {
    double error = 0;
    try {
      error = cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize, *cameraMatrix, *distCoeffs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraCharuco1(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraCharuco(*charucoCorners, *charucoIds, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, 
        flags, *criteria);
    } catch(const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  double au_calibrateCameraCharuco2(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    std::vector<cv::Mat>** tvecs, int flags, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraCharuco(*charucoCorners, *charucoIds, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs, 
        flags);
    } catch(const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }
  
  double au_calibrateCameraCharuco3(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    std::vector<cv::Mat>** tvecs, cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();
      *tvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraCharuco(*charucoCorners, *charucoIds, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs, **tvecs);
    } catch(const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }
  
  double au_calibrateCameraCharuco4(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs, 
    cv::Exception* exception) {
    double error = 0;
    try {
      *rvecs = new std::vector<cv::Mat>();

      error = cv::aruco::calibrateCameraCharuco(*charucoCorners, *charucoIds, *board, *imageSize, *cameraMatrix, *distCoeffs, **rvecs);
    } catch(const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }
  
  double au_calibrateCameraCharuco5(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception) {
    double error = 0;
    try {
      error = cv::aruco::calibrateCameraCharuco(*charucoCorners, *charucoIds, *board, *imageSize, *cameraMatrix, *distCoeffs);
    } catch(const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return error;
    }
    return error;
  }

  void au_detectCharucoDiamond1(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* markerCorners, 
    std::vector<int>* markerIds, float squareMarkerLengthRate, std::vector<std::vector<cv::Point2f>>** diamondCorners, 
    std::vector<cv::Vec4i>** diamondIds, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception) {
    try {
      *diamondCorners = new std::vector<std::vector<cv::Point2f>>();
      *diamondIds = new std::vector<cv::Vec4i>();

      cv::aruco::detectCharucoDiamond(*image, *markerCorners, *markerIds, squareMarkerLengthRate, **diamondCorners, **diamondIds, 
        *cameraMatrix, *distCoeffs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }    
  }

  void au_detectCharucoDiamond2(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* markerCorners, 
    std::vector<int>* markerIds, float squareMarkerLengthRate, std::vector<std::vector<cv::Point2f>>** diamondCorners, 
    std::vector<cv::Vec4i>** diamondIds, cv::Mat* cameraMatrix, cv::Exception* exception) {
    try {
      *diamondCorners = new std::vector<std::vector<cv::Point2f>>();
      *diamondIds = new std::vector<cv::Vec4i>();

      cv::aruco::detectCharucoDiamond(*image, *markerCorners, *markerIds, squareMarkerLengthRate, **diamondCorners, **diamondIds, 
        *cameraMatrix);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_detectCharucoDiamond3(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* markerCorners, 
    std::vector<int>* markerIds, float squareMarkerLengthRate, std::vector<std::vector<cv::Point2f>>** diamondCorners, 
    std::vector<cv::Vec4i>** diamondIds, cv::Exception* exception) {
    try {
      *diamondCorners = new std::vector<std::vector<cv::Point2f>>();
      *diamondIds = new std::vector<cv::Vec4i>();

      cv::aruco::detectCharucoDiamond(*image, *markerCorners, *markerIds, squareMarkerLengthRate, **diamondCorners, **diamondIds);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_detectMarkers1(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, std::vector<std::vector<cv::Point2f>>** rejectedImgPoints,
    cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *rejectedImgPoints = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids, *parameters, **rejectedImgPoints);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_detectMarkers2(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids, *parameters);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_detectMarkers3(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawCharucoDiamond1(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Vec4i* ids, int squareLength, int markerLength, 
    cv::Mat** img, int marginSize, int borderBits, cv::Exception* exception) {
    try {
      *img = new cv::Mat();

      cv::aruco::drawCharucoDiamond(*dictionary, *ids, squareLength, markerLength, **img, marginSize, borderBits);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    };
    cv::cvtColor(**img, **img, CV_GRAY2RGB);
  }

  void au_drawCharucoDiamond2(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Vec4i* ids, int squareLength, int markerLength, 
    cv::Mat** img, int marginSize, cv::Exception* exception) {
    try {
      *img = new cv::Mat();

      cv::aruco::drawCharucoDiamond(*dictionary, *ids, squareLength, markerLength, **img, marginSize);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    };
    cv::cvtColor(**img, **img, CV_GRAY2RGB);
  }

  void au_drawCharucoDiamond3(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Vec4i* ids, int squareLength, int markerLength, 
    cv::Mat** img, cv::Exception* exception) {
    try {
      *img = new cv::Mat();

      cv::aruco::drawCharucoDiamond(*dictionary, *ids, squareLength, markerLength, **img);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    };
    cv::cvtColor(**img, **img, CV_GRAY2RGB);
  }

  void au_drawDetectedCornersCharuco1(cv::Mat* image, std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds,
    cv::Scalar* cornerColor, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedCornersCharuco(*image, *charucoCorners, *charucoIds, *cornerColor);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedCornersCharuco2(cv::Mat* image, std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, 
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedCornersCharuco(*image, *charucoCorners, *charucoIds);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedCornersCharuco3(cv::Mat* image, std::vector<cv::Point2f>* charucoCorners, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedCornersCharuco(*image, *charucoCorners);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedDiamonds1(cv::Mat* image, std::vector<cv::Vec4i>* diamondCorners, std::vector<int>* diamondIds,
    cv::Scalar* borderColor, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedDiamonds(*image, *diamondCorners, *diamondIds, *borderColor);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedDiamonds2(cv::Mat* image, std::vector<cv::Vec4i>* diamondCorners, std::vector<int>* diamondIds,
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedDiamonds(*image, *diamondCorners, *diamondIds);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedDiamonds3(cv::Mat* image, std::vector<cv::Vec4i>* diamondCorners, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedDiamonds(*image, *diamondCorners);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers1(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids,
    cv::Scalar* borderColor, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, *ids, *borderColor);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers2(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids,
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, *ids);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers3(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers4(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, cv::Scalar* borderColor, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, cv::noArray(), *borderColor);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  int au_estimatePoseBoard(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, cv::Ptr<cv::aruco::Board>* board,
    std::vector<cv::Mat>* cameraMatrix, std::vector<cv::Mat>* distCoeffs, cv::Mat** rvec, cv::Mat** tvec, cv::Exception* exception) {
    int valid = 0;
    try {
      *rvec = new cv::Mat();
      *tvec = new cv::Mat();

      valid = cv::aruco::estimatePoseBoard(*corners, *ids, *board, *cameraMatrix, *distCoeffs, **rvec, **tvec);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return valid;
    }
    return valid;
  }
  
  bool au_estimatePoseCharucoBoard(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds, cv::Ptr<cv::aruco::CharucoBoard>* board,
    cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Mat** rvec, cv::Mat** tvec, cv::Exception* exception) {
    bool validPose = 0;
    try {
      *rvec = new cv::Mat();
      *tvec = new cv::Mat();

      validPose = cv::aruco::estimatePoseCharucoBoard(*charucoCorners, *charucoIds, *board, *cameraMatrix, *distCoeffs, **rvec, **tvec);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return validPose;
    }
    return validPose;
  }

  void au_estimatePoseSingleMarkers(std::vector<std::vector<cv::Point2f>>* corners, float markerLength, cv::Mat* cameraMatrix, 
    cv::Mat* distCoeffs, std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, cv::Exception* exception) {
    try {
      *rvecs = new std::vector<cv::Vec3d>();
      *tvecs = new std::vector<cv::Vec3d>();

      cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, **rvecs, **tvecs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  int au_interpolateCornersCharuco1(std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds, cv::Mat* image, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, std::vector<cv::Point2f>** charucoCorners, std::vector<int>** charucoIds, cv::Mat* cameraMatrix, 
    cv::Mat* distCoeffs, cv::Exception* exception) {
    int interpolatedCorners = 0;
    try {
      *charucoCorners = new std::vector<cv::Point2f>();
      *charucoIds = new std::vector<int>();

      interpolatedCorners = cv::aruco::interpolateCornersCharuco(*markerCorners, *markerIds, *image, *board, **charucoCorners, **charucoIds, *cameraMatrix, 
        *distCoeffs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return interpolatedCorners;
    }
    return interpolatedCorners;
  }

  int au_interpolateCornersCharuco2(std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds, cv::Mat* image, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, std::vector<cv::Point2f>** charucoCorners, std::vector<int>** charucoIds, cv::Mat* cameraMatrix, 
    cv::Exception* exception) {
    int interpolatedCorners = 0;
    try {
      *charucoCorners = new std::vector<cv::Point2f>();
      *charucoIds = new std::vector<int>();

      interpolatedCorners = cv::aruco::interpolateCornersCharuco(*markerCorners, *markerIds, *image, *board, **charucoCorners, **charucoIds, *cameraMatrix);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return interpolatedCorners;
    }
    return interpolatedCorners;
  }

  int au_interpolateCornersCharuco3(std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds, cv::Mat* image, 
    cv::Ptr<cv::aruco::CharucoBoard>* board, std::vector<cv::Point2f>** charucoCorners, std::vector<int>** charucoIds, cv::Exception* exception) {
    int interpolatedCorners = 0;
    try {
      *charucoCorners = new std::vector<cv::Point2f>();
      *charucoIds = new std::vector<int>();

      interpolatedCorners = cv::aruco::interpolateCornersCharuco(*markerCorners, *markerIds, *image, *board, **charucoCorners, **charucoIds);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return interpolatedCorners;
    }
    return interpolatedCorners;
  }

  void au_refineDetectedMarkers1(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners, 
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, 
    float minRepDistance, float errorCorrectionRate, bool checkAllOrders, std::vector<int>* recoveredIdxs, 
    const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix,
        *distCoeffs, minRepDistance, errorCorrectionRate, checkAllOrders, *recoveredIdxs, *parameters);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers2(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    float minRepDistance, float errorCorrectionRate, bool checkAllOrders, std::vector<int>* recoveredIdxs, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix,
        *distCoeffs, minRepDistance, errorCorrectionRate, checkAllOrders, *recoveredIdxs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers3(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    float minRepDistance, float errorCorrectionRate, bool checkAllOrders, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix,
        *distCoeffs, minRepDistance, errorCorrectionRate, checkAllOrders);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers4(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    float minRepDistance, float errorCorrectionRate, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix,
        *distCoeffs, minRepDistance, errorCorrectionRate);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers5(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    float minRepDistance, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix,
        *distCoeffs, minRepDistance);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers6(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs,
    cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix,
        *distCoeffs);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers7(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Mat* cameraMatrix, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners, *cameraMatrix);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_refineDetectedMarkers8(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point2f>>* detectedCorners,
    std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners, cv::Exception* exception) {
    try {
      cv::aruco::refineDetectedMarkers(*image, *board, *detectedCorners, *detectedIds, *rejectedCorners);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }
}