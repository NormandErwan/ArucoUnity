#include "aruco_unity/utility/exception.hpp"

extern "C" {
  // Constructors & Destructors
  cv::Exception* auNewException() {
    return new cv::Exception();
  }

  void auDeleteException(cv::Exception* exception) {
    delete exception;
  }

  // Functions
  void auExceptionWhat(cv::Exception* exception, char* what) {
    std::strcpy(what, exception->what());
  }

  // Variables
  int auGetExceptionCode(cv::Exception* exception) {
    return exception->code;
  }
}