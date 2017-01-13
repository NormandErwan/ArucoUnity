#ifndef __ARUCO_UNITY_DICTIONARY_HPP__
#define __ARUCO_UNITY_DICTIONARY_HPP__

#include <opencv2/aruco.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup aruco_unity_lib
//! @{

//! @defgroup dictionary Dictionary
//! \brief Set of markers. 
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d5/d0b/classcv_1_1aruco_1_1Dictionary.html.
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates a Dictionary.
  //!
  //! \param bytesList Marker code information.
  //! \param markerSize Number of bits per dimension.
  //! \param maxCorrectionBits Maximum number of bits that can be reached.
  //! \return The new Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new1(const cv::Mat* bytesList, int markerSize, int maxCorrectionBits);

  //! \see au_Dictionary_new1().
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new2(const cv::Mat* bytesList, int markerSize);

  //! \see au_Dictionary_new1().
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new3(const cv::Mat* bytesList);

  //! \see au_Dictionary_new1().
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new4();

  //! \brief Copy a Dictionary.
  //! \param dictionary The Dictionary copied.
  //! \return The new Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new5(const cv::Ptr<cv::aruco::Dictionary>* dictionary);

// TODO: move it to aruco_unity.hpp?
  //! \brief Returns one of the predefined dictionaries defined in PREDEFINED_DICTIONARY_NAME.
  //! \return The Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name);

  //! \brief Generates a new customizable marker Dictionary.
  //!
  //! \param nMarkers number of markers in the Dictionary.
  //! \param markerSize number of bits per dimension of each markers.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The generated Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_generateCustomDictionary1(int nMarkers, int markerSize, cv::Exception* exception);

  //! \brief Generates a new customizable marker Dictionary.
  //!
  //! \param nMarkers number of markers in the Dictionary.
  //! \param markerSize number of bits per dimension of each markers.
  //! \param baseDictionary Include the markers in this Dictionary at the beginning (optional).
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The generated Dictionary.
  ARUCO_UNITY_API cv::Ptr<cv::aruco::Dictionary>* au_generateCustomDictionary2(int nMarkers, int markerSize, 
    cv::Ptr<cv::aruco::Dictionary>* baseDictionary, cv::Exception* exception);

  //! \brief Deletes any Dictionary.
  //! \param dictionary The Dictionary used.
  ARUCO_UNITY_API void au_Dictionary_delete(cv::Ptr<cv::aruco::Dictionary>* dictionary);
  
  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Draw a canonical marker image.
  //!
  //! \param dictionary The Dictionary used.
  //! \param id The marker id. 
  //! \param sidePixels The number of pixel per side of the marker. 
  //! \param img The marker's pixels returned.
  //! \param borderBits The number of bits forming the marker border.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_Dictionary_drawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat* img, int borderBits, 
    cv::Exception* exception);

  //! \brief Returns the distance of the input bits to the specific id. If allRotations is true, the four posible bits rotation are considered.
  //!
  //! \param dictionary The Dictionary used.
  //! \param bits
  //! \param id
  //! \param allRotations
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API int au_Dictionary_getDistanceToId1(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Mat* bits, int id, bool allRotations, 
    cv::Exception* exception);

  //! \see au_Dictionary_getDistanceToId1().
  ARUCO_UNITY_API int au_Dictionary_getDistanceToId2(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Mat* bits, int id, cv::Exception* exception);
  
  //! \brief Given a matrix of bits. Returns whether if marker is identified or not. It returns by reference the correct id (if any) and the correct rotation.
  //!
  //! \param dictionary The Dictionary used.
  //! \param onlyBits The matrix of bits.
  //! \param idx The id of the identified marker.
  //! \param rotation The rotation of the identified marker.
  //! \param maxCorrectionRate
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return Is the marker identified.
  ARUCO_UNITY_API bool au_Dictionary_identify(cv::Ptr<cv::aruco::Dictionary>* dictionary, const cv::Mat* onlyBits, int* idx, int* rotation, 
    double maxCorrectionRate, cv::Exception* exception);

  //! @} Member Functions
  
  //! \name Static Member Functions
  //! @{

  //! \brief Transform list of bytes to matrix of bits.
  //!
  //! \param byteList The list of bytes.
  //! \param markerSize The size of a side of the matrix of bits.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  //! \return The matrix of bits.
  ARUCO_UNITY_API cv::Mat* au_Dictionary_getBitsFromByteList(const cv::Mat* byteList, int markerSize, cv::Exception* exception);

  //! \brief Transform matrix of bits to list of bytes in the 4 rotations.
  //! \param bits The matrix of bits.
  //! \return The list of bytes.
  ARUCO_UNITY_API cv::Mat* au_Dictionary_getByteListFromBits(const cv::Mat* bits);

  //! @} Member Functions

  //! \name Attributes
  //! @{

  //! \brief Returns the marker code information.
  //! \param dictionary The Dictionary used.
  ARUCO_UNITY_API cv::Mat* au_Dictionary_getBytesList(cv::Ptr<cv::aruco::Dictionary>* dictionary);

  //! \brief Sets the marker code information.
  //! \param dictionary The Dictionary used.
  //! \param bytesList The new value.
  ARUCO_UNITY_API void au_Dictionary_setBytesList(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Mat* bytesList);

  //! \brief Returns the number of bits per dimension.
  //! \param dictionary The Dictionary used.
  ARUCO_UNITY_API int au_Dictionary_getMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary);

  //! \brief Sets the number of bits per dimension.
  //! \param dictionary The Dictionary used.
  //! \param markerSize The new value.
  ARUCO_UNITY_API void au_Dictionary_setMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary, int markerSize);

  //! \brief Returns the maximum number of bits that can be corrected.
  //! \param dictionary The Dictionary used.
  ARUCO_UNITY_API int au_Dictionary_getMaxCorrectionBits(cv::Ptr<cv::aruco::Dictionary>* dictionary);

  //! \brief Sets the maximum number of bits that can be corrected.
  //! \param dictionary The Dictionary used.
  //! \param maxCorrectionBits The new value.
  ARUCO_UNITY_API void au_Dictionary_setMaxCorrectionBits(cv::Ptr<cv::aruco::Dictionary>* dictionary, int maxCorrectionBits);

  //! @} Attributes
}

//! @} dictionary

//! @} aruco_unity_lib

#endif