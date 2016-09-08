#include "aruco_unity/dictionary.hpp"

extern "C" {
  int auGetDictionaryMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    return dictionary->get()->markerSize;
  }
}