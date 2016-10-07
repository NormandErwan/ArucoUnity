#ifndef __ARUCO_UNITY_HPP__
#define __ARUCO_UNITY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

extern "C" {
  //! \brief Basic marker detection.
  //!
  //! \param image Input image.
  //! \param dictionary Indicates the type of markers that will be searched.
  //! \param corners Vector of detected marker corners. For each marker, its four corners
  //! are provided, (e.g std::vector<std::vector<cv::Point2f> > ). For N detected markers,
  //! the dimensions of this array is Nx4. The order of the corners is clockwise.
  //! \param ids Vector of identifiers of the detected markers. The identifier is of type int
  //! (e.g. std::vector<int>). For N detected markers, the size of ids is also N.
  //! The identifiers have the same order than the markers in the imgPoints array.
  //! \param parameters Marker detection parameters.
  //! \param rejectedImgPoints Contains the imgPoints of those squares whose inner code has not a
  //! correct codification. Useful for debugging purposes.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga306791ee1aab1513bc2c2b40d774f370.
  ARUCO_UNITY_API void au_detectMarkers1(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, std::vector<std::vector<cv::Point2f>>** rejectedImgPoints,
    cv::Exception* exception);

  //! \see au_detectMarkers1().
  ARUCO_UNITY_API void au_detectMarkers2(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
      std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception);

  //! \see au_detectMarkers1().
  ARUCO_UNITY_API void au_detectMarkers3(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, cv::Exception* exception);

  //! \brief Draw detected markers in image.
  //! \param image Input/output image. It must have 1 or 3 channels.The number of channels is not altered.
  //! \param corners Positions of marker corners on input image (e.g std::vector<std::vector<cv::Point2f>>).
  //! For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.
  //! \param ids Vector of identifiers for markers in markersCorners. Optional, if not provided, ids are not painted.
  //! \param borderColor Color of marker borders. Rest of colors(text color and first corner color) are calculated based on this one to improve visualization.
  ARUCO_UNITY_API void au_drawDetectedMarkers1(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, 
    cv::Scalar* borderColor, cv::Exception* exception);

  //! \see au_drawDetectedMarkers1().
  ARUCO_UNITY_API void au_drawDetectedMarkers2(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, 
    cv::Exception* exception);

  //! \see au_drawDetectedMarkers1().
  ARUCO_UNITY_API void au_drawDetectedMarkers3(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, cv::Exception* exception);

  //! \see au_drawDetectedMarkers1().
  ARUCO_UNITY_API void au_drawDetectedMarkers4(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, cv::Scalar* borderColor, 
    cv::Exception* exception);
}

#endif