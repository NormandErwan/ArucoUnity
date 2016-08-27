#include "aruco_unity/dictionary.hpp"

extern "C" {
  int auGetDictionaryMarkerSize(void* dictionary) {
    return static_cast<cv::aruco::Dictionary*>(dictionary)->markerSize;
  }
}