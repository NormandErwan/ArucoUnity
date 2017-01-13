#include "aruco_unity/utility/cv/term_criteria.hpp"

extern "C" {
  // Constructors & Destructors
  cv::TermCriteria* au_TermCriteria_new1() {
    return new cv::TermCriteria();
  }

  cv::TermCriteria* au_TermCriteria_new2(int type, int maxCount, double epsilon) {
    return new cv::TermCriteria(type, maxCount, epsilon);
  }
  
  void au_TermCriteria_delete(cv::TermCriteria* termCriteria) {
    delete termCriteria;
  }

  // Variables
  double au_TermCriteria_getEpsilon(cv::TermCriteria* termCriteria) {
    return termCriteria->epsilon;
  }

  void au_TermCriteria_setEpsilon(cv::TermCriteria* termCriteria, double epsilon) {
    termCriteria->epsilon = epsilon;
  }

  int au_TermCriteria_getMaxCount(cv::TermCriteria* termCriteria) {
    return termCriteria->maxCount;
  }

  void au_TermCriteria_setMaxCount(cv::TermCriteria* termCriteria, int maxCount) {
    termCriteria->maxCount = maxCount;
  }

  int au_TermCriteria_getType(cv::TermCriteria* termCriteria) {
    return termCriteria->type;
  }

  void au_TermCriteria_setType(cv::TermCriteria* termCriteria, int type) {
    termCriteria->type = type;
  }
}