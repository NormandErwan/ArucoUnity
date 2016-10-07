#ifndef __ARUCO_UNITY_POINT2F_HPP__
#define __ARUCO_UNITY_POINT2F_HPP__

#include <opencv2\core.hpp>
#include "aruco_unity/exports.hpp"

//! @addtogroup utility_scalar
//! \brief Template class for a 4-element vector derived from Vec.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.1.0/d1/da0/classcv_1_1Scalar__.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates a Scalar.
  ARUCO_UNITY_API cv::Scalar* au_Scalar_new(double v0, double v1, double v2);

  //! \brief Deletes any Scalar.
  //! \param scalar The Scalar used.
  ARUCO_UNITY_API void au_Scalar_delete(cv::Scalar* scalar);

  //! @} Constructors & Destructors
}

//! @} utility_scalar

#endif