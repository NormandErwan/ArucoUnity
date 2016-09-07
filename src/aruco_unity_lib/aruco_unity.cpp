#include "aruco_unity.hpp"
#include <unordered_map>

extern "C" {
  // Detector parameters
  cv::aruco::DetectorParameters* auCreateDetectorParameters() {
    return cv::aruco::DetectorParameters::create().get();
  }

  void auDeleteDetectorParameters(cv::aruco::DetectorParameters* parameters) {
    delete parameters;
  }


  // Dictionary
  cv::aruco::Dictionary* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    return cv::aruco::getPredefinedDictionary(name).get();
  }

  void auDeleteDictionary(cv::aruco::Dictionary* dictionary) {
    delete dictionary;
  }
}