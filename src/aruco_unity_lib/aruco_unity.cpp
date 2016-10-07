#include "aruco_unity.hpp"
#include "aruco_unity/utility/exception.hpp"
#include "aruco_unity/utility/vector_vector_point2f.hpp"

extern "C" {
  void au_detectMarkers1(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, std::vector<std::vector<cv::Point2f>>** rejectedImgPoints,
    cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *rejectedImgPoints = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids, *parameters, **rejectedImgPoints);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_detectMarkers2(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, const cv::Ptr<cv::aruco::DetectorParameters>* parameters, cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids, *parameters);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_detectMarkers3(cv::Mat* image, cv::Ptr<cv::aruco::Dictionary>* dictionary, std::vector<std::vector<cv::Point2f>>** corners,
    std::vector<int>** ids, cv::Exception* exception) {
    try {
      *corners = new std::vector<std::vector<cv::Point2f>>();
      *ids = new std::vector<int>();

      cv::aruco::detectMarkers(*image, *dictionary, **corners, **ids);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers1(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids,
    cv::Scalar* borderColor, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, *ids, *borderColor);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers2(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, std::vector<int>* ids,
    cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, *ids);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers3(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }

  void au_drawDetectedMarkers4(cv::Mat* image, std::vector<std::vector<cv::Point2f>>* corners, cv::Scalar* borderColor, cv::Exception* exception) {
    try {
      cv::aruco::drawDetectedMarkers(*image, *corners, cv::noArray(), *borderColor);
    }
    catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return;
    }
  }
}