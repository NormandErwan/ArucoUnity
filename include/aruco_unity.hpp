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
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
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
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga2ad34b0f277edebb6a132d3069ed2909
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

  //! \brief Refind not detected markers based on the already detected and the board layout.
  //!
  //! \param image Input image.
  //! \param board Layout of markers in the board.
  //! \param detectedCorners VECTOR of already detected marker corners.
  //! \param detectedIds Vector of already detected marker identifiers.
  //! \param rejectedCorners Vector of rejected candidates during the marker detection process.
  //! \param cameraMatrix Optional input 3x3 floating-point camera matrix.
  //! \param distCoeffs Optional vector of distortion coefficients of 4, 5, 8 or 12 elements.
  //! \param minRepDistance Minimum distance between the corners of the rejected candidate and the
  //! reprojected Marker in order to consider it as a correspondence.
  //! \param errorCorrectionRate Rate of allowed erroneous bits respect to the error correction
  //! capability of the used dictionary. -1 ignores the error correction step.
  //! \param checkAllOrders Consider the four posible corner orders in the rejectedCorners array.
  //! If it set to false, only the provided corner order is considered (default true).
  //! \param recoveredIdxs Optional array to returns the indexes of the recovered candidates in the
  //! original rejectedCorners array.
  //! \param parameters Marker detection parameters.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API void au_refineDetectedMarkers1(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, float minRepDistance, 
    float errorCorrectionRate, bool checkAllOrders, cv::Mat* recoveredIdxs, 
    const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers2(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, float minRepDistance, 
    float errorCorrectionRate, bool checkAllOrders, cv::Mat* recoveredIdxs, cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers3(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, float minRepDistance, 
    float errorCorrectionRate, bool checkAllOrders, cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers4(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, float minRepDistance, 
    float errorCorrectionRate, cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers5(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, float minRepDistance, 
    cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers6(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, 
    cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers7(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Mat* cameraMatrix, cv::Exception* exception);

  //! \see au_refineDetectedMarkers1().
  ARUCO_UNITY_API void au_refineDetectedMarkers8(cv::Mat* image, cv::Ptr<cv::aruco::Board>* board, cv::Mat* detectedCorners,
    cv::Mat* detectedIds, cv::Mat* rejectedCorners, cv::Exception* exception);
}

#endif