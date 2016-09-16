#include "aruco_unity/dictionary.hpp"
#include <opencv2/imgproc.hpp>

extern "C" {
  int auGetDictionaryMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    return dictionary->get()->markerSize;
  }

  void auDictionaryDrawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat* img, int borderBits) {
    dictionary->get()->drawMarker(id, sidePixels, *img, borderBits);
    cv::cvtColor(*img, *img, CV_GRAY2RGB);
  }
}