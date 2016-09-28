#ifndef __ARUCO_UNITY_EXCEPTION_HPP__
#define __ARUCO_UNITY_EXCEPTION_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/exports.hpp"

#define ARUCO_UNITY_TRY_CATCH(exceptionObject, trySection) \
try {                                                      \
  trySection                                               \
}                                                          \
catch (const cv::Exception& e) {                           \
  exception->code = e.code;                                \
  exception->err = e.err;                                  \
  exception->file = e.file;                                \
  exception->func = e.func;                                \
  exception->line = e.line;                                \
  exception->msg = e.msg;                                  \
  return;                                                  \
}                                                          \

//! @addtogroup utility_exception
//! \brief Class passed to an error. 
//! 
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d1/dee/classcv_1_1Exception.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates empty Exception.
  ARUCO_UNITY_API cv::Exception* au_Exception_New();

  //! \brief Deletes any Exception.
  //! \param e The Exception used.
  ARUCO_UNITY_API void au_Exception_Delete(cv::Exception* exception);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \param mat The Exception used.
  //! \return The error description and the context as a text string.
  ARUCO_UNITY_API void au_Exception_What(cv::Exception* exception, char* what);

  //! @} Functions

  //! \name Variables
  //! @{

  //! \param mat The Exception used.
  //! \return The error code.
  ARUCO_UNITY_API int au_Exception_GetCode(cv::Exception* exception);

  //! @} Variables
}

//! @} utility_mat

#endif