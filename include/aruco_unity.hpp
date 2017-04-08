#ifndef __ARUCO_UNITY_HPP__
#define __ARUCO_UNITY_HPP__

#include <opencv2/aruco.hpp>
#include <opencv2/aruco/charuco.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @defgroup aruco_unity_lib ArUco Unity library
//! \brief C interface for the OpenCV's ArUco Marker Detection extra module.
//!
//! See the OpenCV documentation for more information about its ArUco Marker Detection extra module:
//! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html
//! @{

extern "C" {
  //! \brief Calibrate a camera using aruco markers
  //!
  //! \param corners Vector of detected marker corners in all frames. The corners should have the same format returned by detectMarkers
  //! (\see au_detectMarkers1).
  //! \param ids List of identifiers for each marker in corners.
  //! \param counter Number of markers in each frame so that corners and ids can be split.
  //! \param board Marker board layout.
  //! \param imageSize Size of the image used only to initialize the intrinsic camera matrix.
  //! \param cameraMatrix Output 3x3 floating-point camera matrix.
  //! \param distCoeffs Output vector of distortion coefficients.
  //! \param rvecs Output vector of rotation vectors (see Rodrigues) estimated for each board view (e.g. std::vector<cv::Mat>>).
  //! \param tvecs Output vector of translation vectors estimated for each pattern view.
  //! \param flags Different flags for the calibration process (See 
  //! http://docs.opencv.org/3.1.0/d9/d0c/group__calib3d.html#ga687a1ab946686f0d85ae0363b5af1d7b)
  //! \param criteria Termination criteria for the iterative optimization algorithm.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#gaee8294f02fb752562096aadb2e62a00f.
  ARUCO_UNITY_API double au_calibrateCameraAruco(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids, std::vector<int>* counter,
    const cv::Ptr<cv::aruco::Board>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception);
  
  //! \brief Calibrate a camera using Charuco corners
  //!
  //! \param charucoCorners Vector of detected charuco corners per frame.
  //! \param charucoIds List of identifiers for each corner in charucoCorners per frame.
  //! \param board Marker Board layout.
  //! \param imageSize Input image size.
  //! \param cameraMatrix Output 3x3 floating-point camera matrix.
  //! \param distCoeffs Output vector of distortion coefficients.
  //! \param rvecs Output vector of rotation vectors (see Rodrigues) estimated for each board view (e.g. std::vector<cv::Mat>>).
  //! \param tvecs Output vector of translation vectors estimated for each pattern view.
  //! \param flags Different flags for the calibration process (see calibrateCamera).
  //! \param criteria Termination criteria for the iterative optimization algorithm.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API double au_calibrateCameraCharuco(std::vector<std::vector<cv::Point2f>>* charucoCorners, std::vector<std::vector<int>>* charucoIds,
    const cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Size* imageSize, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, std::vector<cv::Mat>** rvecs,
    std::vector<cv::Mat>** tvecs, int flags, cv::TermCriteria* criteria, cv::Exception* exception);

  //! \brief Detect ChArUco Diamond markers
  //!
  //! \param image Input image necessary for corner subpixel.
  //! \param markerCorners List of detected marker corners from detectMarkers function.
  //! \param markerIds List of marker ids in markerCorners.
  //! \param squareMarkerLengthRate Rate between square and marker length: squareMarkerLengthRate = squareLength/markerLength.
  //! \param diamondCorners Output list of detected diamond corners (4 corners per diamond).
  //! \param diamondIds Ids of the diamonds in diamondCorners.
  //! \param cameraMatrix Optional camera calibration matrix.
  //! \param distCoeffs Optional camera distortion coefficients.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API void au_detectCharucoDiamond(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds,
    float squareMarkerLengthRate, std::vector<std::vector<cv::Point2f>>** diamondCorners, std::vector<cv::Vec4i>** diamondIds, 
    cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception);

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
  ARUCO_UNITY_API void au_detectMarkers(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, std::vector<std::vector<cv::Point2f>>** rejectedImgPoints,
    cv::Exception* exception);

  //! \brief Draw coordinate system axis from pose estimation. 
  //!
  //! \param image Input/output image.
  //! \param cameraMatrix Input 3x3 floating-point camera matrix.
  //! \param distCoeffs Vector of distortion coefficients.
  //! \param rvec Rotation vector of the coordinate system that will be drawn.
  //! \param tvec Translation vector of the coordinate system that will be drawn.
  //! \param exception Length of the painted axis in the same unit than tvec (usually in meters).
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga16fda651a4e6a8f5747a85cbb6b400a2
  ARUCO_UNITY_API void au_drawAxis(cv::Mat* image, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Vec3d* rvec, cv::Vec3d* tvec, float length,
    cv::Exception* exception);

  //! \brief Draw a ChArUco Diamond marker.
  //!
  //! \param dictionary Dictionary of markers indicating the type of markers.
  //! \param ids List of 4 ids for each ArUco marker in the ChArUco marker.
  //! \param squareLength Size of the chessboard squares in pixels.
  //! \param markerLength Size of the markers in pixels.
  //! \param img Output Image with the marker.
  //! \param marginSize Minimum margins (in pixels) of the marker in the output image.
  //! \param borderBits Width of the marker borders.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API void au_drawCharucoDiamond(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Vec4i* ids, int squareLength, int markerLength,
    cv::Mat** img, int marginSize, int borderBits, cv::Exception* exception);

  //! \brief Draws a set of Charuco corners
  //! \param image Input/output image.
  //! \param charucoCorners Vector of detected charuco corners
  //! \param charucoIds List of identifiers for each corner in charucoCorners
  //! \param cornerColor Color of the square surrounding each corner
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API void au_drawDetectedCornersCharuco(cv::Mat* image, std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds,
    cv::Scalar* cornerColor, cv::Exception* exception);

  //! \brief Draw a set of detected ChArUco Diamond markers
  //!
  //! \param image Input/output image.
  //! \param diamondCorners Positions of diamond corners in the same format returned by detectCharucoDiamond().
  //! \param diamondIds Vector of identifiers for diamonds in diamondCorners, in the same format returned by detectCharucoDiamond() (e.g. 
  //! std::vector<Vec4i>).
  //! \param borderColor Color of marker borders.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API void au_drawDetectedDiamonds(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* diamondCorners,
    std::vector<cv::Vec4i>* diamondIds, cv::Scalar* borderColor, cv::Exception* exception);

  //! \brief Draw detected markers in image.
  //!
  //! \param image Input/output image. It must have 1 or 3 channels.The number of channels is not altered.
  //! \param corners Positions of marker corners on input image (e.g std::vector<std::vector<cv::Point2f>>).
  //! For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.
  //! \param ids Vector of identifiers for markers in markersCorners. Optional, if not provided, ids are not painted.
  //! \param borderColor Color of marker borders. Rest of colors(text color and first corner color) are calculated based on this one to improve
  //! visualization.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga2ad34b0f277edebb6a132d3069ed2909
  ARUCO_UNITY_API void au_drawDetectedMarkers(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids,
    cv::Scalar* borderColor, cv::Exception* exception);

  //! \brief Pose estimation for a board of markers.
  //!
  //! \param corners Vector of already detected markers corners.
  //! \param ids List of identifiers for each marker in corners.
  //! \param board Layout of markers in the board.
  //! \param cameraMatrix Input 3x3 floating-point camera matrix.
  //! \param distCoeffs Vector of distortion coefficients.
  //! \param rvec Output vector (e.g. cv::Mat) corresponding to the rotation vector of the board (see Rodrigues).
  //! \param tvec Output vector (e.g. cv::Mat) corresponding to the translation vector of the board.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#gae89235944f3bdbaad69d8dbac5340f1c
  ARUCO_UNITY_API int au_estimatePoseBoard(std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids,
    const cv::Ptr<cv::aruco::Board>* board, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Vec3d** rvec, cv::Vec3d** tvec,
    cv::Exception* exception);

  //! \brief Pose estimation for a ChArUco board given some of their corners
  //!
  //! \param charucoCorners Vector of detected charuco corners.
  //! \param charucoIds List of identifiers for each corner in charucoCorners.
  //! \param board Layout of ChArUco board.
  //! \param cameraMatrix Input 3x3 floating-point camera matrix.
  //! \param distCoeffs Vector of distortion coefficients.
  //! \param rvec Output vector (e.g. cv::Mat) corresponding to the rotation vector of the board (see Rodrigues).
  //! \param tvec Output vector (e.g. cv::Mat) corresponding to the translation vector of the board.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API bool au_estimatePoseCharucoBoard(std::vector<cv::Point2f>* charucoCorners, std::vector<int>* charucoIds,
    const cv::Ptr<cv::aruco::CharucoBoard>* board, cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Vec3d** rvec, cv::Vec3d** tvec,
    cv::Exception* exception);

  //! \brief Pose estimation for single markers.
  //!
  //! \param corners Vector of already detected markers corners.
  //! \param markerLength The length of the markers' side.
  //! \param cameraMatrix Input 3x3 floating-point camera matrix.
  //! \param distCoeffs Vector of distortion coefficients.
  //! \param rvecs Array of output rotation vectors (see Rodrigues) (e.g. std::vector<cv::Vec3d>).
  //! \param tvecs Array of output translation vectors (e.g. std::vector<cv::Vec3d>).
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#gafdd609e5c251dc7b8197323657a874c3
  ARUCO_UNITY_API void au_estimatePoseSingleMarkers(std::vector<std::vector<cv::Point2f>>* corners, float markerLength, cv::Mat* cameraMatrix,
    cv::Mat* distCoeffs, std::vector<cv::Vec3d>** rvecs, std::vector<cv::Vec3d>** tvecs, cv::Exception* exception);

  //! \brief Generates a new customizable marker Dictionary.
  //!
  //! \param nMarkers number of markers in the Dictionary.
  //! \param markerSize number of bits per dimension of each markers.
  //! \param baseDictionary Include the markers in this Dictionary at the beginning (optional).
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The generated Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_generateCustomDictionary(int nMarkers, int markerSize,
    cv::Ptr<cv::aruco::Dictionary>* baseDictionary, cv::Exception* exception);

  //! \brief Returns the corresponding the image points and object points of a board configuration and a set of markers.
  //!
  //! \param board Marker board layout.
  //! \param detectedCorners List of identifiers for each marker.
  //! \param detectedIds List of detected marker corners of the board.
  //! \param objPoints Vector of vectors of the projections of board marker corner points.
  //! \param imgPoints Vector of vectors of board marker points in the board coordinate space.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_getBoardObjectAndImagePoints(const cv::Ptr<cv::aruco::Board>* board,
    std::vector<std::vector<cv::Point2f>>* detectedCorners, std::vector<int>* detectedIds, std::vector<cv::Point3f>** objPoints,
    std::vector<cv::Point2f>** imgPoints, cv::Exception* exception);

  //! \brief Returns one of the predefined dictionaries defined in PREDEFINED_DICTIONARY_NAME.
  //! \return The Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);

  //! \brief Interpolate position of ChArUco board corners
  //!
  //! \param markerCorners Vector of already detected markers corners.
  //! \param markerIds List of identifiers for each marker in corners.
  //! \param image Input image necesary for corner refinement.
  //! \param board Layout of ChArUco board.
  //! \param charucoCorners Interpolated chessboard corners
  //! \param charucoIds Interpolated chessboard corners identifiers.
  //! \param cameraMatrix Optional 3x3 floating-point camera matrix.
  //! \param distCoeffs Optional vector of distortion coefficients.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API int au_interpolateCornersCharuco(std::vector<std::vector<cv::Point2f>>* markerCorners, std::vector<int>* markerIds,
    cv::Mat* image, const cv::Ptr<cv::aruco::CharucoBoard>* board, std::vector<cv::Point2f>** charucoCorners, std::vector<int>** charucoIds,
    cv::Mat* cameraMatrix, cv::Mat* distCoeffs, cv::Exception* exception);

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
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //!
  //! See the OpenCV documentation for more information: 
  //! http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html#ga90374a799f1da566e5de16f277b12463
  ARUCO_UNITY_API void au_refineDetectedMarkers(cv::Mat* image, const cv::Ptr<cv::aruco::Board>* board,
    std::vector<std::vector<cv::Point2f>>* detectedCorners, std::vector<int>* detectedIds, std::vector<std::vector<cv::Point2f>>* rejectedCorners,
    cv::Mat* cameraMatrix, cv::Mat* distCoeffs, float minRepDistance, float errorCorrectionRate, bool checkAllOrders,
    std::vector<int>* recoveredIdxs, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception);
}

//! @} aruco_unity_lib

#endif