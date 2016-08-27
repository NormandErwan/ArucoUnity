#include <iostream>
#include "aruco_unity.hpp"
#include "aruco_unity/dictionary.hpp"

int main() {
  void* dictionary = auGetPredefinedDictionary(cv::aruco::DICT_4X4_100);

  std::cout << auGetDictionaryMarkerSize(dictionary) << std::endl;

  auDestroyDictionary(dictionary);

  std::cout << auGetDictionaryMarkerSize(dictionary) << std::endl;

  system("pause");

  return 0;
}