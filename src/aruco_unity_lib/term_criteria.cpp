#include "aruco_unity/cv/term_criteria.hpp"

extern "C" {
  // Constructors & Destructors

  cv::TermCriteria* au_cv_TermCriteria_new1() {
    return new cv::TermCriteria();
  }

  cv::TermCriteria* au_cv_TermCriteria_new2(int type, int maxCount, double epsilon) {
    return new cv::TermCriteria(type, maxCount, epsilon);
  }
  
  void au_cv_TermCriteria_delete(cv::TermCriteria* termCriteria) {
    delete termCriteria;
  }

  // Attributes

  double au_cv_TermCriteria_getEpsilon(cv::TermCriteria* termCriteria) {
    return termCriteria->epsilon;
  }

  void au_cv_TermCriteria_setEpsilon(cv::TermCriteria* termCriteria, double epsilon) {
    termCriteria->epsilon = epsilon;
  }

  int au_cv_TermCriteria_getMaxCount(cv::TermCriteria* termCriteria) {
    return termCriteria->maxCount;
  }

  void au_cv_TermCriteria_setMaxCount(cv::TermCriteria* termCriteria, int maxCount) {
    termCriteria->maxCount = maxCount;
  }

  int au_cv_TermCriteria_getType(cv::TermCriteria* termCriteria) {
    return termCriteria->type;
  }

  void au_cv_TermCriteria_setType(cv::TermCriteria* termCriteria, int type) {
    termCriteria->type = type;
  }
}