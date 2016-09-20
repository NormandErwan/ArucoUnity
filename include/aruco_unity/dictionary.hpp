#ifndef __ARUCO_UNITY_DICTIONARY_HPP__
#define __ARUCO_UNITY_DICTIONARY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/exports.hpp"

//! @defgroup dictionary Dictionary
//! \brief Set of markers. 
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d5/d0b/classcv_1_1aruco_1_1Dictionary.html.
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! @brief Returns one of the predefined dictionaries defined in PREDEFINED_DICTIONARY_NAME.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);

  //! \brief Deletes any Dictionary.
  //! \param parameters The Dictionary used.
  ARUCO_UNITY_API void auDeleteDictionary(cv::Ptr<cv::aruco::Dictionary>* dictionary);
  
  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \brief Draw a canonical marker image.
  //! \param parameters The Dictionary used.
  ARUCO_UNITY_API void auDictionaryDrawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat* img, int borderBits);
  
  //! @} Functions

  //! \name Variables
  //! @{

  //! \brief Returns the marker size.
  //! \param parameters The Dictionary used.
  ARUCO_UNITY_API int auGetDictionaryMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary);

  //! @} Variables
}

//! @} dictionary

#endif