#include <iostream>
#include "aruco_unity.hpp"
#include "aruco_unity/dictionary.hpp"

int main() {
  cv::aruco::Dictionary* dictionary = auGetPredefinedDictionary(cv::aruco::DICT_4X4_100);

  std::cout << auGetDictionaryMarkerSize(dictionary) << std::endl;

  auDeleteDictionary(dictionary);

  system("pause");

  return 0;
}