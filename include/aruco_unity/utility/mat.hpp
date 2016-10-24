#ifndef __ARUCO_UNITY_MAT_HPP__
#define __ARUCO_UNITY_MAT_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_mat
//! \brief n-dimensional dense array class.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d3/d63/classcv_1_1Mat.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty Mat.
  ARUCO_UNITY_API cv::Mat* au_Mat_new1();

  //! \brief Creates a Mat.
  //!
  //! \param rows Number of rows. 
  //! \param cols Number of columns. 
  //! \param type Array type. 
  //! \param data Pointer to the data. The external data is not automatically deallocated, so you should take care of it. 
  ARUCO_UNITY_API cv::Mat* au_Mat_new2_uchar(int rows, int cols, int type, uchar* data);

  //! \brief Creates a Mat.
  //!
  //! \param rows Number of rows. 
  //! \param cols Number of columns. 
  //! \param type Array type. 
  //! \param data Pointer to the data. The external data is not automatically deallocated, so you should take care of it. 
  ARUCO_UNITY_API cv::Mat* au_Mat_new2_double(int rows, int cols, int type, double* data);

  //! \brief Deletes any Mat.
  //! \param mat The Mat used.
  ARUCO_UNITY_API void au_Mat_delete(cv::Mat* mat);

  //! @} Constructors & Destructors

  //! \name Member Functions
  //! @{

  //! \brief Returns the specified array element as an int.
  //!
  //! \param mat The Mat used.
  //! \param i0 Index along the dimension 0.
  //! \param i1 Index along the dimension 1.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API int au_Mat_at_int_get(cv::Mat* mat, int i0, int i1, cv::Exception* exception);

  //! \brief Sets the specified array element as an int.
  //!
  //! \param mat The Mat used.
  //! \param i0 Index along the dimension 0.
  //! \param i1 Index along the dimension 1.
  //! \param value The new value.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_Mat_at_int_set(cv::Mat* mat, int i0, int i1, int value, cv::Exception* exception);

  //! \brief Returns the specified array element as a double.
  //!
  //! \param mat The Mat used.
  //! \param i0 Index along the dimension 0.
  //! \param i1 Index along the dimension 1.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API double au_Mat_at_double_get(cv::Mat* mat, int i0, int i1, cv::Exception* exception);

  //! \brief Sets the specified array element as a double.
  //!
  //! \param mat The Mat used.
  //! \param i0 Index along the dimension 0.
  //! \param i1 Index along the dimension 1.
  //! \param value The new value.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_Mat_at_double_set(cv::Mat* mat, int i0, int i1, double value, cv::Exception* exception);

  //! \brief Allocates new array data if needed.
  //!
  //! \param mat The Mat used.
  //! \param rows Number of rows.
  //! \param cols Number of columns.
  //! \param type Array type.
  //! \param exception The first exception threw by any trigerred CV_ASSERT.
  ARUCO_UNITY_API void au_Mat_create(cv::Mat* mat, int rows, int cols, int type, cv::Exception* exception);

  //! \brief Returns the matrix element size in bytes.
  //! \param mat The Mat used.
  ARUCO_UNITY_API size_t au_Mat_elemSize(cv::Mat* mat);

  //! \brief Returns the total number of array elements.
  //! \param mat The Mat used.
  ARUCO_UNITY_API size_t au_Mat_total(cv::Mat* mat);

  //! \brief Returns the type of a matrix element.
  //! \param mat The Mat used.
  ARUCO_UNITY_API int au_Mat_type(cv::Mat* mat);

  //! @} Member Functions
  
  //! \name Variables
  //! @{

  //! \brief Returns the number of columns.
  //! \param mat The Mat used.
  ARUCO_UNITY_API int au_Mat_getCols(cv::Mat* mat);

  //! \brief Returns a pointer to the data.
  //! \param mat The Mat used.
  ARUCO_UNITY_API uchar* au_Mat_getData(cv::Mat* mat);

  //! \brief Returns the number of rows.
  //! \param mat The Mat used.
  ARUCO_UNITY_API int au_Mat_getRows(cv::Mat* mat);

  //! \brief TODO
  //! \param mat The Mat used.
  ARUCO_UNITY_API cv::Size* au_Mat_getSize(cv::Mat* mat);

  //! @} Variables
}

//! @} utility_mat

#endif