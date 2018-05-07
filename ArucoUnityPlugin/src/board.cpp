#include "aruco_unity/board.hpp"

extern "C" {
  // Attributes

  cv::Ptr<cv::aruco::Dictionary>* au_Board_getDictionary(cv::Ptr<cv::aruco::Board>* board) {
    return &(board->get()->dictionary);
  }

  void au_Board_setDictionary(cv::Ptr<cv::aruco::Board>* board, cv::Ptr<cv::aruco::Dictionary>* dictionary) {
    board->get()->dictionary = cv::Ptr<cv::aruco::Dictionary>(*dictionary);
  }

  std::vector<int>* au_Board_getIds(cv::Ptr<cv::aruco::Board>* board) {
    return &(board->get()->ids);
  }

  void au_Board_setIds(cv::Ptr<cv::aruco::Board>* board, std::vector<int>* ids) {
    board->get()->ids = std::vector<int>(*ids);
  }

  std::vector<std::vector<cv::Point3f>>* au_Board_getObjPoints(cv::Ptr<cv::aruco::Board>* board) {
    return &(board->get()->objPoints);
  }

  void au_Board_setObjPoints(cv::Ptr<cv::aruco::Board>* board, std::vector<std::vector<cv::Point3f>>* objPoints) {
    board->get()->objPoints = std::vector<std::vector<cv::Point3f>>(*objPoints);
  }
}