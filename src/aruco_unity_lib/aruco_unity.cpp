#include "aruco_unity.hpp"
#include <unordered_map>

// Map for storing the shared pointers of allocated objects
std::unordered_map<void*, cv::Ptr<cv::aruco::DetectorParameters>> _detectorParametersMap;
std::unordered_map<void*, cv::Ptr<cv::aruco::Dictionary>> _dictionaryMap;

extern "C" {
  // Detector parameters
  void* auCreateDetectorParameters() {
    cv::Ptr<cv::aruco::DetectorParameters> detectorParameters = cv::aruco::DetectorParameters::create();
    void* detectorParametersPtr = static_cast<void*>(detectorParameters);
    _detectorParametersMap[detectorParametersPtr] = detectorParameters;
    return detectorParametersPtr;
  }

  void auDestroyDetectorParameters(void* detectorParameter) {
    _detectorParametersMap.erase(detectorParameter);
  }


  // Dictionnary
  void* auGetPredefinedDictionary(cv::aruco::PREDEFINED_DICTIONARY_NAME name) {
    cv::Ptr<cv::aruco::Dictionary> dictionary = cv::aruco::getPredefinedDictionary(name);
    void* dictionaryPtr = static_cast<void*>(dictionary);
    _dictionaryMap[dictionaryPtr] = dictionary;
    return dictionaryPtr;
  }

  void auDestroyDictionary(void* dictionary) {
    _dictionaryMap.erase(dictionary);
  }
}