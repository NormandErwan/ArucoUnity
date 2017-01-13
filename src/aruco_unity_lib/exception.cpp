#include "aruco_unity/utility/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Exception* au_cv_Exception_new() {
    return new cv::Exception();
  }

  void au_cv_Exception_delete(cv::Exception* exception) {
    delete exception;
  }

  // Functions
  void au_cv_Exception_what(cv::Exception* exception, char* what) {
    std::strcpy(what, exception->what());
  }

  // Variables
  int au_cv_Exception_getCode(cv::Exception* exception) {
    return exception->code;
  }
}