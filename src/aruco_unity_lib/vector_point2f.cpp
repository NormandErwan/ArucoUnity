#include "aruco_unity/std/vector_point2f.hpp"
#include "aruco_unity/cv/exception.hpp"

extern "C" {
  // Constructors & Destructors
  std::vector<cv::Point2f>* au_std_vectorPoint2f_new() {
    return new std::vector<cv::Point2f>();
  }

  void au_std_vectorPoint2f_delete(std::vector<cv::Point2f>* vector) {
    delete vector;
  }

  // Functions
  cv::Point2f* au_std_vectorPoint2f_at(std::vector<cv::Point2f>* vector, size_t pos, cv::Exception* exception) {
    cv::Point2f* element = NULL;
    try {
      element = &(vector->at(pos));
    } catch (const cv::Exception& e) {
      ARUCO_UNITY_COPY_EXCEPTION(exception, e);
      return element;
    }
    return element;
  }
  
  cv::Point2f* au_std_vectorPoint2f_data(std::vector<cv::Point2f>* vector) {
    return vector->data();
  }

  void au_std_vectorPoint2f_push_back(std::vector<cv::Point2f>* vector, cv::Point2f* value) {
    vector->push_back(cv::Point2f(*value));
  }

  size_t au_std_vectorPoint2f_size(std::vector<cv::Point2f>* vector) {
    return vector->size();
  }
}