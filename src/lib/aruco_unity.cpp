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

void detectMarkers(char* filename)
{
  try
  {
    aruco::MarkerDetector MDetector;
    std::vector<aruco::Marker> Markers;

    //read the input image
    cv::Mat InImage;
    InImage = cv::imread(filename);

    //Ok, let's detect
    MDetector.detect(InImage,Markers);

    //for each marker, draw info and its boundaries in the image
    for (unsigned int i = 0; i < Markers.size(); i++) {
      std::cout << Markers[i] << std::endl;
      Markers[i].draw(InImage, cv::Scalar(0,0,255), 2);
    }
    
    cv::imshow("in", InImage);
    cv::waitKey(0); //wait for key to be pressed
  } 
  catch (std::exception &exception)
  {
    std::cout << "Exception: " << exception.what() << std::endl;
  }
}

}