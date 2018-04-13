#include "aruco_unity/charuco_board.hpp"
#include "aruco_unity/cv/exception.hpp"
#include <opencv2/imgproc.hpp>

extern "C" {
  // Constructors & Destructors

  void au_CharucoBoard_delete(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    delete charucoBoard;
  }

  // Member Functions

  void au_CharucoBoard_draw(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, cv::Size* outSize, cv::Mat** img, int marginSize, int borderBits,
    cv::Exception* exception) {
    try {
      *img = new cv::Mat();
      charucoBoard->get()->draw(*outSize, **img, marginSize, borderBits);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    };
    cv::cvtColor(**img, **img, cv::COLOR_GRAY2RGB);
  }

  cv::Size* au_CharucoBoard_getChessboardSize(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    return new cv::Size(charucoBoard->get()->getChessboardSize());
  }

  float au_CharucoBoard_getMarkerLength(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    return charucoBoard->get()->getMarkerLength();
  }

  float au_CharucoBoard_getSquareLength(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    return charucoBoard->get()->getSquareLength();
  }

  // Static Member Functions

  cv::Ptr<cv::aruco::CharucoBoard>* au_CharucoBoard_create(int squaresX, int squaresY, float squareLength, float markerLength, 
    cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Exception* exception) {
    cv::Ptr<cv::aruco::CharucoBoard> charucoBoard;
    try {
      charucoBoard = cv::aruco::CharucoBoard::create(squaresX, squaresY, squareLength, markerLength, *dictionary);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return NULL;
    };
    return new cv::Ptr<cv::aruco::CharucoBoard>(charucoBoard);
  }

  // Attributes

  std::vector<cv::Point3f>* au_CharucoBoard_getChessboardCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    return &(charucoBoard->get()->chessboardCorners);
  }

  void au_CharucoBoard_setChessboardCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, std::vector<cv::Point3f>* chessboardCorners) {
    charucoBoard->get()->chessboardCorners = std::vector<cv::Point3f>(*chessboardCorners);
  }

  std::vector<std::vector<int>>* au_CharucoBoard_getNearestMarkerCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    return &(charucoBoard->get()->nearestMarkerCorners);
  }

  void au_CharucoBoard_setNearestMarkerCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, std::vector<std::vector<int>>* nearestMarkerCorners) {
    charucoBoard->get()->nearestMarkerCorners = std::vector<std::vector<int>>(*nearestMarkerCorners);
  }

  std::vector<std::vector<int>>* au_CharucoBoard_getNearestMarkerIdx(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard) {
    return &(charucoBoard->get()->nearestMarkerIdx);
  }

  void au_CharucoBoard_setNearestMarkerIdx(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, std::vector<std::vector<int>>* nearestMarkerIdx) {
    charucoBoard->get()->nearestMarkerIdx = std::vector<std::vector<int>>(*nearestMarkerIdx);
  }
}