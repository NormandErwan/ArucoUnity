#ifndef __ARUCO_UNITY_TERM_CRITERIA_HPP__
#define __ARUCO_UNITY_TERM_CRITERIA_HPP__

#include <opencv2/core.hpp>
#include "aruco_unity/utility/exports.hpp"

//! @addtogroup term_criteria
//! \brief The class defining termination criteria for iterative algorithms.
//!
//! See the OpenCV documentation for more information: http://docs.opencv.org/3.2.0/d9/d5d/classcv_1_1TermCriteria.html
//! @{

extern "C" {
  //! \name Constructors & Destructors
  //! @{

  //! \brief Creates an empty TermCriteria.
  ARUCO_UNITY_API cv::TermCriteria* au_cv_TermCriteria_new1();

  //! \brief Creates an new TermCriteria.
  //!
  //! \param type The type of termination criteria, one of TermCriteria::Type.
  //! \param maxCount The maximum number of iterations or elements to compute.
  //! \param epsilon The desired accuracy or change in parameters at which the iterative algorithm stops. 
  ARUCO_UNITY_API cv::TermCriteria* au_cv_TermCriteria_new2(int type, int maxCount, double epsilon);

  //! \brief Deletes any TermCriteria.
  //! \param termCriteria The TermCriteria used.
  ARUCO_UNITY_API void au_cv_TermCriteria_delete(cv::TermCriteria* termCriteria);

  //! @} Constructors & Destructors

  //! \name Attributes
  //! @{

  //! \brief Returns the epsilon value.
  //! \param termCriteria The TermCriteria used.
  ARUCO_UNITY_API double au_cv_TermCriteria_getEpsilon(cv::TermCriteria* termCriteria);

  //! \brief Sets the epsilon value.
  //! \param termCriteria The TermCriteria used.
  //! \param epsilon The new value.
  ARUCO_UNITY_API void au_cv_TermCriteria_setEpsilon(cv::TermCriteria* termCriteria, double epsilon);

  //! \brief Returns the maxCount value.
  //! \param termCriteria The TermCriteria used.
  ARUCO_UNITY_API int au_cv_TermCriteria_getMaxCount(cv::TermCriteria* termCriteria);

  //! \brief Sets the maxCount value.
  //! \param termCriteria The TermCriteria used.
  //! \param maxCount The new value.
  ARUCO_UNITY_API void au_cv_TermCriteria_setMaxCount(cv::TermCriteria* termCriteria, int maxCount);

  //! \brief Returns the type value.
  //! \param termCriteria The TermCriteria used.
  ARUCO_UNITY_API int au_cv_TermCriteria_getType(cv::TermCriteria* termCriteria);

  //! \brief Sets the type value.
  //! \param termCriteria The TermCriteria used.
  //! \param type The new value.
  ARUCO_UNITY_API void au_cv_TermCriteria_setType(cv::TermCriteria* termCriteria, int type);

  //! @} Attributes
}

//! @} term_criteria

#endif