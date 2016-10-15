#ifndef __ARUCO_UNITY_CHARUCO_BOARD_HPP__
#define __ARUCO_UNITY_CHARUCO_BOARD_HPP__

#include <opencv2/aruco/charuco.hpp>
#include "aruco_unity/exports.hpp"

//! @defgroup charuco_board CharucoBoard
//! \brief A ChArUco board is a planar board where the markers are placed inside the white squares of a chessboard.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d0/d3c/classcv_1_1aruco_1_1CharucoBoard.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Deletes any CharucoBoard.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API void au_CharucoBoard_delete(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);
  
  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Draw a CharucoBoard.
  //! \param charucoBoard The CharucoBoard used.
  //! \param outSize Size of the output image in pixels.
  //! \param img Output image with the board. The size of this image will be outSize 
  //!   and the board will be on the center, keeping the board proportions.
  //! \param marginSize Minimum margins (in pixels) of the board in the output image.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \param borderBits Width of the marker borders.
  ARUCO_UNITY_API void au_CharucoBoard_draw1(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, cv::Size* outSize, cv::Mat** img, int marginSize, int borderBits,
    cv::Exception* exception);

  //! \see au_CharucoBoard_draw1().
  ARUCO_UNITY_API void au_CharucoBoard_draw2(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, cv::Size* outSize, cv::Mat** img, int marginSize, 
    cv::Exception* exception);

  //! \see au_CharucoBoard_draw1().
  ARUCO_UNITY_API void au_CharucoBoard_draw3(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, cv::Size* outSize, cv::Mat** img, cv::Exception* exception);

  //! \brief Returns the size chessboard of the CharucoBoard.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API cv::Size* au_CharucoBoard_getChessboardSize(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);

  //! \brief Returns the length of a marker of the CharucoBoard.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API float au_CharucoBoard_getMarkerLength(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);

  //! \brief Returns the square length.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API float au_CharucoBoard_getSquareLength(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);

  //! @} Member Functions

  //! \name Static Member Functions
  //! @{

  //! \brief Create a CharucoBoard object.
  //!
  //! \param squaresX Number of squares in X direction.
  //! \param squaresY Number of squares in Y direction.
  //! \param squareLength Chessboard square side length (normally in meters).
  //! \param markerLength Marker side length (normally in meters).
  //! \param dictionary Dictionary of markers indicating the type of markers.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The output CharucoBoard object.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::CharucoBoard>* au_CharucoBoard_create(int squaresX, int squaresY, float squareLength, float markerLength, 
    cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Exception* exception);

  //! @} Static Member Functions

  //! \name Attributes
  //! @{

  //! \brief Returns the chessboard corners.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API std::vector<cv::Point3f>* au_CharucoBoard_getChessboardCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);

  //! \brief Sets the chessboard corners.
  //! \param charucoBoard The CharucoBoard used.
  //! \param chessboardCorners The new value.
  ARUCO_UNITY_API void au_CharucoBoard_setChessboardCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, std::vector<cv::Point3f>* chessboardCorners);

  //! \brief Returns the nearest marker corners.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API std::vector<std::vector<int>> au_CharucoBoard_getNearestMarkerCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);

  //! \brief Sets the nearest marker corners.
  //! \param charucoBoard The CharucoBoard used.
  //! \param nearestMarkerCorners The new value.
  ARUCO_UNITY_API void au_CharucoBoard_setNearestMarkerCorners(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, std::vector<std::vector<int>>* nearestMarkerCorners);

  //! \brief Returns the nearest marker idx.
  //! \param charucoBoard The CharucoBoard used.
  ARUCO_UNITY_API std::vector<std::vector<int>> au_CharucoBoard_getNearestMarkerIdx(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard);

  //! \brief Sets the nearest marker idx.
  //! \param charucoBoard The CharucoBoard used.
  //! \param nearestMarkerIdx The new value.
  ARUCO_UNITY_API void au_CharucoBoard_setNearestMarkerIdx(cv::Ptr<cv::aruco::CharucoBoard>* charucoBoard, std::vector<std::vector<int>>* nearestMarkerIdx);

  //! @} Attributes
}

//! @} charuco_board

#endif