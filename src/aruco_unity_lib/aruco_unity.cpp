#include "aruco_unity.hpp"
#include <unordered_map>

std::unordered_map<void*, cv::Ptr<cv::aruco::Dictionary>> dictionaries;

extern "C" {
  void* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::Ptr<cv::aruco::Dictionary> dictionary = cv::aruco::getPredefinedDictionary(name);
    void* dictionary_ptr = static_cast<void*>(dictionary);
    dictionaries[dictionary_ptr] = dictionary;
    return dictionary_ptr;
  }

  void auDestroyDictionary(void* dictionary) {
    dictionaries.erase(dictionary);
  }
}