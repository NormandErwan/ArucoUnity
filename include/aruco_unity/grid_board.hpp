#ifndef __ARUCO_UNITY_GRID_BOARD_HPP__
#define __ARUCO_UNITY_GRID_BOARD_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

//! @defgroup grid_board GridBoard
//! \brief Planar board with grid arrangement of markers.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/de/d05/classcv_1_1aruco_1_1GridBoard.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Deletes any GridBoard.
  //! \param gridBoard The GridBoard used.
  ARUCO_UNITY_API void au_GridBoard_delete(cv::Ptr<cv::aruco::GridBoard>* gridBoard);
  
  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Draw a GridBoard.
  //! \param gridBoard The GridBoard used.
  //! \param outSize Size of the output image in pixels.
  //! \param img Output image with the board. The size of this image will be outSize 
  //!   and the board will be on the center, keeping the board proportions.
  //! \param marginSize Minimum margins (in pixels) of the board in the output image.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \param borderBits Width of the marker borders.
  ARUCO_UNITY_API void au_GridBoard_draw1(cv::Ptr<cv::aruco::GridBoard>* gridBoard, cv::Size* outSize, cv::Mat* img, int marginSize, int borderBits,
    cv::Exception* exception);

  //! \see au_GridBoard_draw1().
  ARUCO_UNITY_API void au_GridBoard_draw2(cv::Ptr<cv::aruco::GridBoard>* gridBoard, cv::Size* outSize, cv::Mat* img, int marginSize, 
    cv::Exception* exception);

  //! \see au_GridBoard_draw1().
  ARUCO_UNITY_API void au_GridBoard_draw3(cv::Ptr<cv::aruco::GridBoard>* gridBoard, cv::Size* outSize, cv::Mat* img, cv::Exception* exception);

  //! \brief Returns the size of the GridBoard.
  //! \param gridBoard The GridBoard used.
  ARUCO_UNITY_API cv::Size* au_GridBoard_getGridSize(cv::Ptr<cv::aruco::GridBoard>* gridBoard);

  //! \brief Returns the length of a marker of the GridBoard.
  //! \param gridBoard The GridBoard used.
  ARUCO_UNITY_API float au_GridBoard_getMarkerLength(cv::Ptr<cv::aruco::GridBoard>* gridBoard);

  //! \brief Returns the separation distance between two markers of the GridBoard.
  //! \param gridBoard The GridBoard used.
  ARUCO_UNITY_API float au_GridBoard_getMarkerSeparation(cv::Ptr<cv::aruco::GridBoard>* gridBoard);

  //! @} Member Functions

  //! \name Static Member Functions
  //! @{

  //! \brief Create a GridBoard object.
  //!
  //! \param markersX Number of markers in X direction.
  //! \param markersY Number of markers in Y direction.
  //! \param markerLength Marker side length (normally in meters).
  //! \param markerSeparation Separation between two markers (same unit as markerLength).
  //! \param dictionary Dictionary of markers indicating the type of markers.
  //! \param firstMarker Id of first marker in dictionary to use on board.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The output GridBoard object.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::GridBoard>* au_GridBoard_create1(int markersX, int markersY, float markerLength, float markerSeparation, 
    cv::Ptr<cv::aruco::Dictionary>* dictionary, int firstMarker, cv::Exception* exception);

  //! \see au_GridBoard_create1().
  ARUCO_UNITY_API cv::Ptr<cv::aruco::GridBoard>* au_GridBoard_create2(int markersX, int markersY, float markerLength, float markerSeparation, 
    cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Exception* exception);

  //! @} Static Member Functions
}

//! @} grid_board

#endif