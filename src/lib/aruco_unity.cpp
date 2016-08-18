#include "aruco_unity.hpp"
#include <unordered_map>

std::unordered_map<void*, cv::Ptr<cv::aruco::Dictionary>> dictionaries;

extern "C" {
  void* getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::Ptr<cv::aruco::Dictionary> dictionary = cv::aruco::getPredefinedDictionary(name);
    void* dictionary_ptr = static_cast<void*>(dictionary);
    dictionaries[dictionary_ptr] = dictionary;
    return dictionary_ptr;
  }

  void destroyDictionary(void* dictionary) {
    dictionaries.erase(dictionary);
  }

  int getDictionaryMarkerSize(void* dictionary) {
    return static_cast<cv::aruco::Dictionary*>(dictionary)->markerSize;
  }
}