#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Exception* au_Exception_New() {
    return new cv::Exception();
  }

  void au_Exception_Delete(cv::Exception* exception) {
    delete exception;
  }

  // Functions
  void au_Exception_What(cv::Exception* exception, char* what) {
    std::strcpy(what, exception->what());
  }

  // Variables
  int au_Exception_GetCode(cv::Exception* exception) {
    return exception->code;
  }
}