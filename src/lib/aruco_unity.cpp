#include "aruco_unity.hpp"

extern "C" {
  void* getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::aruco::Dictionary* dictionary = cv::aruco::getPredefinedDictionary(name);
    return static_cast<void*>(dictionary);
  }

  void destroyDictionary(void* dictionary) {
    delete dictionary;
  }

  int getDictionaryMarkerSize(void* dictionary) {
    return static_cast<cv::aruco::Dictionary*>(dictionary)->markerSize;
  }
}