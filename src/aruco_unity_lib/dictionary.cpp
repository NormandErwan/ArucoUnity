#include "aruco_unity/dictionary.hpp"
#include <opencv2/imgproc.hpp>

extern "C" {
  // Constructors & Destructors
  cv::Ptr<cv::aruco::Dictionary>* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::Ptr<cv::aruco::Dictionary> ptr = cv::aruco::getPredefinedDictionary(name);
    return new cv::Ptr<cv::aruco::Dictionary>(ptr);
  }
  
  void auDeleteDictionary(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    delete dictionary;
  }

  // Functions
  void auDictionaryDrawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat* img, int borderBits) {
    dictionary->get()->drawMarker(id, sidePixels, *img, borderBits);
    cv::cvtColor(*img, *img, CV_GRAY2RGB);
  }

  // Variables
  int auGetDictionaryMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    return dictionary->get()->markerSize;
  }
}