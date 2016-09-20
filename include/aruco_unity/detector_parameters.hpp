#ifndef __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__
#define __ARUCO_UNITY_DETECTOR_PARAMETERS_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

//! @defgroup detector_parameters DetectorParameters
//! \brief Parameters for the detectMarker process
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d1/dcd/structcv_1_1aruco_1_1DetectorParameters.html.
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{
  
  //! \brief Creates a new sets of DetectorParameters with default values.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::DetectorParameters>* auCreateDetectorParameters();
  
  //! \brief Deletes any DetectorParameters.
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API void auDeleteDetectorParameters(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! @} Constructor & Destructor

  //! \name Variables
  //! @{
  
  //! \brief Returns the minimum window size for adaptive thresholding before finding contours (default 3).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum window size for adaptive thresholding before finding contours (default 3).
  //! \param parameters The DetectorParameters used.
  //! \param adaptiveThreshWinSizeMin The new value.
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMin(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMin);

  //! \brief Returns the maximum window size for adaptive thresholding before finding contours (default 23).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the maximum window size for adaptive thresholding before finding contours (default 23).
  //! \param parameters The DetectorParameters used.
  //! \param adaptiveThreshWinSizeMax The new value.
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeMax(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeMax);

  //! \brief Returns the increments from adaptiveThreshWinSizeMin to adaptiveThreshWinSizeMax during the thresholding (default 10).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the increments from adaptiveThreshWinSizeMin to adaptiveThreshWinSizeMax during the thresholding (default 10).
  //! \param parameters The DetectorParameters used.
  //! \param adaptiveThreshWinSizeStep The new value.
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshWinSizeStep(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int adaptiveThreshWinSizeStep);

  //! \brief Returns the constant for adaptive thresholding before finding contours (default 7).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the constant for adaptive thresholding before finding contours (default 7).
  //! \param parameters The DetectorParameters used.
  //! \param adaptiveThreshConstant The new value.
  ARUCO_UNITY_API void auSetDetectorParametersAdaptiveThreshConstant(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double adaptiveThreshConstant);

  //! \brief Returns the minimum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 0.03).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 0.03).
  //! \param parameters The DetectorParameters used.
  //! \param minMarkerPerimeterRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerPerimeterRate);

  //! \brief Returns the maximum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 4.0).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the maximum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 4.0).
  //! \param parameters The DetectorParameters used.
  //! \param maxMarkerPerimeterRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMaxMarkerPerimeterRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxMarkerPerimeterRate);

  //! \brief Returns the minimum accuracy during the polygonal approximation process to determine which contours are squares.
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum accuracy during the polygonal approximation process to determine which contours are squares.
  //! \param parameters The DetectorParameters used.
  //! \param polygonalApproxAccuracyRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersPolygonalApproxAccuracyRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double polygonalApproxAccuracyRate);

  //! \brief Returns the minimum distance between corners for detected markers relative to its perimeter (default 0.05).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum distance between corners for detected markers relative to its perimeter (default 0.05).
  //! \param parameters The DetectorParameters used.
  //! \param minCornerDistanceRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMinCornerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minCornerDistanceRate);

  //! \brief Returns the minimum distance of any corner to the image border for detected markers (in pixels) (default 3).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum distance of any corner to the image border for detected markers (in pixels) (default 3).
  //! \param parameters The DetectorParameters used.
  //! \param minDistanceToBorder The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMinDistanceToBorder(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int minDistanceToBorder);

  //! \brief Returns the minimum mean distance beetween two marker corners to be considered similar, so that the smaller one is removed. The rate is relative to the smaller perimeter of the two markers (default 0.05).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum mean distance beetween two marker corners to be considered similar, so that the smaller one is removed. The rate is relative to the smaller perimeter of the two markers (default 0.05).
  //! \param parameters The DetectorParameters used.
  //! \param minMarkerDistanceRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMinMarkerDistanceRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minMarkerDistanceRate);

  //! \brief Returns if there is a subpixel refinement or not.
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API bool auGetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets if there is a subpixel refinement or not.
  //! \param parameters The DetectorParameters used.
  //! \param doCornerRefinement The new value.
  ARUCO_UNITY_API void auSetDetectorParametersDoCornerRefinement(cv::Ptr<cv::aruco::DetectorParameters>* parameters, bool doCornerRefinement);

  //! \brief Returns the window size for the corner refinement process (in pixels) (default 5).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the window size for the corner refinement process (in pixels) (default 5).
  //! \param parameters The DetectorParameters used.
  //! \param cornerRefinementWinSize The new value.
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementWinSize(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementWinSize);

  //! \brief Returns the maximum number of iterations for stop criteria of the corner refinement process (default 30).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the maximum number of iterations for stop criteria of the corner refinement process (default 30).
  //! \param parameters The DetectorParameters used.
  //! \param cornerRefinementMaxIterations The new value.
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMaxIterations(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int cornerRefinementMaxIterations);

  //! \brief Returns the minimum error for the stop cristeria of the corner refinement process (default: 0.1).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimum error for the stop cristeria of the corner refinement process (default: 0.1).
  //! \param parameters The DetectorParameters used.
  //! \param cornerRefinementMinAccuracy The new value.
  ARUCO_UNITY_API void auSetDetectorParametersCornerRefinementMinAccuracy(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double cornerRefinementMinAccuracy);

  //! \brief Returns the number of bits of the marker border, i.e. marker border width (default 1).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the number of bits of the marker border, i.e. marker border width (default 1).
  //! \param parameters The DetectorParameters used.
  //! \param markerBorderBits The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMarkerBorderBits(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int markerBorderBits);

  //! \brief Returns the number of bits (per dimension) for each cell of the marker when removing the perspective (default 8).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API int auGetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the number of bits (per dimension) for each cell of the marker when removing the perspective (default 8).
  //! \param parameters The DetectorParameters used.
  //! \param perspectiveRemovePixelPerCell The new value.
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemovePixelPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, int perspectiveRemovePixelPerCell);

  //! \brief Returns the width of the margin of pixels on each cell not considered for the determination of the cell bit. Represents the rate respect to the total size of the cell, i.e. perpectiveRemovePixelPerCell (default 0.13).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the width of the margin of pixels on each cell not considered for the determination of the cell bit. Represents the rate respect to the total size of the cell, i.e. perpectiveRemovePixelPerCell (default 0.13).
  //! \param parameters The DetectorParameters used.
  //! \param perspectiveRemoveIgnoredMarginPerCell The new value.
  ARUCO_UNITY_API void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double perspectiveRemoveIgnoredMarginPerCell);

  //! \brief Returns the maximum number of accepted erroneous bits in the border (i.e. number of allowed white bits in the border). Represented as a rate respect to the total number of bits per marker (default 0.35).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the maximum number of accepted erroneous bits in the border (i.e. number of allowed white bits in the border). Represented as a rate respect to the total number of bits per marker (default 0.35).
  //! \param parameters The DetectorParameters used.
  //! \param maxErroneousBitsInBorderRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMaxErroneousBitsInBorderRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double maxErroneousBitsInBorderRate);

  //! \brief Returns the minimun standard deviation in pixels values during the decodification step to apply Otsu thresholding (otherwise, all the bits are sets to 0 or 1 depending on mean higher than 128 or not) (default 5.0).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the minimun standard deviation in pixels values during the decodification step to apply Otsu thresholding (otherwise, all the bits are sets to 0 or 1 depending on mean higher than 128 or not) (default 5.0).
  //! \param parameters The DetectorParameters used.
  //! \param minOtsuStdDev The new value.
  ARUCO_UNITY_API void auSetDetectorParametersMinOtsuStdDev(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double minOtsuStdDev);

  //! \brief Returns the maximun error correction capability for each dictionary (default 0.6).
  //! \param parameters The DetectorParameters used.
  ARUCO_UNITY_API double auGetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters);

  //! \brief Sets the maximun error correction capability for each dictionary (default 0.6).
  //! \param parameters The DetectorParameters used.
  //! \param errorCorrectionRate The new value.
  ARUCO_UNITY_API void auSetDetectorParametersErrorCorrectionRate(cv::Ptr<cv::aruco::DetectorParameters>* parameters, double errorCorrectionRate);

  //! @} Variables
}

//! @} detector_parameters

#endif