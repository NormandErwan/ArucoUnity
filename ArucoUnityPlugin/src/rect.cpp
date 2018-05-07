#include "aruco_unity/cv/rect.hpp"

extern "C" {
  // Constructors & Destructors

  cv::Rect* au_cv_Rect_new1() {
    return new cv::Rect();
  }

  cv::Rect* au_cv_Rect_new2(int x, int y, int width, int height) {
    return new cv::Rect(x, y, width, height);
  }
  
  void au_cv_Rect_delete(cv::Rect* rect) {
    delete rect;
  }

  // Attributes

  int au_cv_Rect_getX(cv::Rect* rect) {
    return rect->x;
  }

  void au_cv_Rect_setX(cv::Rect* rect, int x) {
    rect->x = x;
  }

  int au_cv_Rect_getY(cv::Rect* rect) {
    return rect->y;
  }

  void au_cv_Rect_setY(cv::Rect* rect, int y) {
    rect->y = y;
  }

  int au_cv_Rect_getWidth(cv::Rect* rect) {
    return rect->width;
  }

  void au_cv_Rect_setWidth(cv::Rect* rect, int width) {
    rect->width = width;
  }

  int au_cv_Rect_getHeight(cv::Rect* rect) {
    return rect->height;
  }

  void au_cv_Rect_setHeight(cv::Rect* rect, int height) {
    rect->height = height;
  }
}