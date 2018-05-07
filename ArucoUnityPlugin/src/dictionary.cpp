#include "aruco_unity/dictionary.hpp"
#include "aruco_unity/cv/exception.hpp"
#include <opencv2/imgproc.hpp>

extern "C" {
  // Constructors & Destructors

  cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new1(const cv::Mat* bytesList, int markerSize, int maxCorrectionBits) {
    cv::aruco::Dictionary dictionary = cv::aruco::Dictionary(*bytesList, markerSize, maxCorrectionBits);
    cv::Ptr<cv::aruco::Dictionary> ptr = cv::makePtr<cv::aruco::Dictionary>(dictionary);
    return new cv::Ptr<cv::aruco::Dictionary>(ptr);
  }

  cv::Ptr<cv::aruco::Dictionary>* au_Dictionary_new2(const cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    cv::aruco::Dictionary _dictionary = cv::aruco::Dictionary(*dictionary);
    cv::Ptr<cv::aruco::Dictionary> ptr = cv::makePtr<cv::aruco::Dictionary>(_dictionary);
    return new cv::Ptr<cv::aruco::Dictionary>(ptr);
  }
  
  void au_Dictionary_delete(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    delete dictionary;
  }

  // Member Functions

  void au_Dictionary_drawMarker(cv::Ptr<cv::aruco::Dictionary>* dictionary, int id, int sidePixels, cv::Mat** img, int borderBits,
    cv::Exception* exception) {
    try {
      *img = new cv::Mat();
      dictionary->get()->drawMarker(id, sidePixels, **img, borderBits);
      cv::cvtColor(**img, **img, cv::COLOR_GRAY2RGB);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    };
  }

  int au_Dictionary_getDistanceToId(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Mat* bits, int id, bool allRotations,
    cv::Exception* exception) {
    int currentMinDistance = 0;
    try {
      currentMinDistance = dictionary->get()->getDistanceToId(*bits, id, allRotations);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return currentMinDistance;
    };
    return currentMinDistance;
  }

  bool au_Dictionary_identify(cv::Ptr<cv::aruco::Dictionary>* dictionary, const cv::Mat* onlyBits, int* idx, int* rotation, double maxCorrectionRate,
    cv::Exception* exception) {
    return dictionary->get()->identify(*onlyBits, *idx, *rotation, maxCorrectionRate);
  }

  // Static Member Functions

  cv::Mat* au_Dictionary_getBitsFromByteList(const cv::Mat* byteList, int markerSize, cv::Exception* exception) {
    cv::Mat bits;
    try {
      bits = cv::aruco::Dictionary::getBitsFromByteList(*byteList, markerSize);
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return NULL;
    };
    return new cv::Mat(bits);
  }

  cv::Mat* au_Dictionary_getByteListFromBits(const cv::Mat* bits) {
    cv::Mat byteList = cv::aruco::Dictionary::getByteListFromBits(*bits);
    return new cv::Mat(byteList);
  }

  // Attributes

  cv::Mat* au_Dictionary_getBytesList(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    return &(dictionary->get()->bytesList);
  }

  void au_Dictionary_setBytesList(cv::Ptr<cv::aruco::Dictionary>* dictionary, cv::Mat* bytesList) {
    dictionary->get()->bytesList = cv::Mat(*bytesList);
  }

  int au_Dictionary_getMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    return dictionary->get()->markerSize;
  }

  void au_Dictionary_setMarkerSize(cv::Ptr<cv::aruco::Dictionary>* dictionary, int markerSize) {
    dictionary->get()->markerSize = markerSize;
  }

  int au_Dictionary_getMaxCorrectionBits(cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    return dictionary->get()->maxCorrectionBits;
  }

  void au_Dictionary_setMaxCorrectionBits(cv::Ptr<cv::aruco::Dictionary>* dictionary, int maxCorrectionBits) {
    dictionary->get()->maxCorrectionBits = maxCorrectionBits;
  }
}