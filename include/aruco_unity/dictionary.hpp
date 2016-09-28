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
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_GetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);

  //! \brief Deletes any Dictionary.
  //! \param dictionary The Dictionary used.
  ARUCO_UNITY_API void au_Dictionary_Delete(cv::Ptr<cv::aruco::Dictionary>* dictionary);
  
  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \brief Draw a canonical marker image.
  //! \param dictionary The Dictionary used.
  //! \param id The marker id. 
  //! \param sidePixels The number of pixel per side of the marker. 
  //! \param img The marker's pixels returned.
  //! \param borderBits The number of bits forming the marker border.
  //! \param exception The exception threw by the CV_ASSERT if it has been trigerred.
  ARUCO_UNITY_API void au_Dictionary_DrawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat* img, int borderBits, 
    cv::Exception* exception);
  
  //! @} Functions

  //! \name Variables
  //! @{

  //! \brief Returns the marker size.
  //! \param dictionary The Dictionary used.
  ARUCO_UNITY_API int au_Dictionary_GetMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary);

  //! @} Variables
}

//! @} dictionary

#endif