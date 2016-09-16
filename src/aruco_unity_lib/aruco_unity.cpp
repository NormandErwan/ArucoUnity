#include "aruco_unity.hpp"

extern "C" {
  // Detector parameters
  cv::Ptr<cv::aruco::DetectorParameters>* auCreateDetectorParameters() {
    cv::Ptr<cv::aruco::DetectorParameters> ptr = cv::aruco::DetectorParameters::create();
    return new cv::Ptr<cv::aruco::DetectorParameters>(ptr);
  }

  void auDeleteDetectorParameters(cv::Ptr<cv::aruco::DetectorParameters>* parameters) {
    delete parameters;
  }


  // Dictionary
  cv::Ptr<cv::aruco::Dictionary>* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::getPredefinedDictionary(name);
    return new cv::Ptr<cv::aruco::Dictionary>(ptr);
  }

  void auDeleteDictionary(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    delete dictionary;
  }
}