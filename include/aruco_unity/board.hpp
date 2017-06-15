#ifndef __ARUCO_UNITY_BOARD_HPP__
#define __ARUCO_UNITY_BOARD_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup aruco_unity_lib
//! @{

//! @defgroup board Board
//! \brief Board of markers.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d4/db2/classcv_1_1aruco_1_1Board.html
//! @{

extern "C" {
  //! \name Attributes
  //! @{

  //! \brief Returns dictionary which indicates the type of markers of the board.
  //! \param board The Board used.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_Board_getDictionary(cv::Ptr<cv::aruco::Board>* board);

  //! \brief Sets the dictionary.
  //! \param board The Board used.
  //! \param dictionary The new value.
  ARUCO_UNITY_API void au_Board_setDictionary(cv::Ptr<cv::aruco::Board>* board, cv::Ptr<cv::aruco::Dictionary>* dictionary);

  //! \brief Returns the identifier of all the markers in the board.
  //! \param board The Board used.
  ARUCO_UNITY_API std::vector<int>* au_Board_getIds(cv::Ptr<cv::aruco::Board>* board);

  //! \brief Sets the identifier of all the markers in the board.
  //! \param board The Board used.
  //! \param ids The new value.
  ARUCO_UNITY_API void au_Board_setIds(cv::Ptr<cv::aruco::Board>* board, std::vector<int>* ids);

  //! \brief Returns the object points of the marker corners, i.e. their coordinates respect to the board system.
  //! \param board The Board used.
  ARUCO_UNITY_API std::vector<std::vector<cv::Point3f>>* au_Board_getObjPoints(cv::Ptr<cv::aruco::Board>* board);

  //! \brief Sets the object points.
  //! \param board The Board used.
  //! \param objPoints The new value.
  ARUCO_UNITY_API void au_Board_setObjPoints(cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point3f>>* objPoints);

  //! @} Attributes
}

//! @} board

//! @} aruco_unity_lib

#endif