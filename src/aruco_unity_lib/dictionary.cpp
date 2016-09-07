#include "aruco_unity/dictionary.hpp"

extern "C" {
  int auGetDictionaryMarkerSize(cv::aruco::Dictionary* dictionary) {
    return dictionary->markerSize;
  }
}