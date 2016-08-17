#include "aruco_unity.hpp"

extern "C" {

int testMult(int a, int b) 
{
  return a * b;
}

int opencvAbs(int n)
{
  return cv::abs(n);
}

int createMarkers()
{
  int dictionaryId = cv::aruco::DICT_4X4_50;
  int markerId = 0;
  int borderBits = 1;
  int markerSize = 400;
  cv::String out = "dict_4x4_50_0.png";

  cv::Ptr<cv::aruco::Dictionary> dictionary =
    cv::aruco::getPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME(dictionaryId));

  cv::Mat markerImg;
  cv::aruco::drawMarker(dictionary, markerId, markerSize, markerImg, borderBits);

  return 0;
  //cv::imwrite(out, markerImg);
}

}