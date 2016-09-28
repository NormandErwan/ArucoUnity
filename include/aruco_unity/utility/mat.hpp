#ifndef __ARUCO_UNITY_MAT_HPP__
#define __ARUCO_UNITY_MAT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_mat
//! \brief n-dimensional dense array class
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d3/d63/classcv_1_1Mat.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Mat.
  ARUCO_UNITY_API cv::Mat* au_Mat_New();

  //! \brief Deletes any Mat.
  //! \param mat The Mat used.
  ARUCO_UNITY_API void au_Mat_Delete(cv::Mat* mat);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \brief Returns the matrix element size in bytes.
  //! \param mat The Mat used.
  ARUCO_UNITY_API size_t au_Mat_ElemSize(cv::Mat* mat);

  //! \brief Returns the total number of array elements.
  //! \param mat The Mat used.
  ARUCO_UNITY_API size_t au_Mat_Total(cv::Mat* mat);

  //! @} Functions

  //! \name Variables
  //! @{

  //! \brief Returns the number of columns.
  //! \param mat The Mat used.
  ARUCO_UNITY_API int au_Mat_GetCols(cv::Mat* mat);

  //! \brief Returns a pointer to the data.
  //! \param mat The Mat used.
  ARUCO_UNITY_API uchar* au_Mat_GetData(cv::Mat* mat);

  //! \brief Returns the number of rows.
  //! \param mat The Mat used.
  ARUCO_UNITY_API int au_Mat_GetRows(cv::Mat* mat);

  //! @} Variables
}

//! @} utility_mat

#endif