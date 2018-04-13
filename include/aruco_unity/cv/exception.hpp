#ifndef __ARUCO_UNITY_EXCEPTION_HPP__
#define __ARUCO_UNITY_EXCEPTION_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

#define ARUCO_UNITY_COPY_EXCEPTION(exceptionDestination, exceptionSource) \
  exception->code = e.code;                                               \
  exception->err = e.err;                                                 \
  exception->file = e.file;                                               \
  exception->func = e.func;                                               \
  exception->line = e.line;                                               \
  exception->msg = e.msg;                                                 \

//! @addtogroup exception
//! \brief Class passed to an error. 
//! 
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d1/dee/classcv_1_1Exception.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates empty Exception.
  ARUCO_UNITY_API cv::Exception* au_cv_Exception_new();

  //! \brief Deletes any Exception.
  //! \param exception The Exception used.
  ARUCO_UNITY_API void au_cv_Exception_delete(cv::Exception* exception);

  //! @} Constructors & Destructors

  //! \name Functions
  //! @{

  //! \return Returns the error description and the context as a text string.
  //! \param exception The Exception used.
  //! \param what The error description.
  //! \param whatLength The max length of the error description.
  ARUCO_UNITY_API void au_cv_Exception_what(cv::Exception* exception, char* what, int whatLength);

  //! @} Functions

  //! \name Variables
  //! @{

  //! \return Returns the error code.
  //! \param exception The Exception used.
  ARUCO_UNITY_API int au_cv_Exception_getCode(cv::Exception* exception);

  //! @} Variables
}

//! @} exception

#endif