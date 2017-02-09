namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      public enum CONSTANTS
      {
        CN_SHIFT = 3
      };

      public enum TYPE
      {
        CV_8U = 0,
        CV_8UC1 = (CV_8U + 0) << CONSTANTS.CN_SHIFT,
        CV_8UC2 = (CV_8U + 1) << CONSTANTS.CN_SHIFT,
        CV_8UC3 = (CV_8U + 2) << CONSTANTS.CN_SHIFT,
        CV_8UC4 = (CV_8U + 3) << CONSTANTS.CN_SHIFT,
        CV_64F = 6,
        CV_64F1 = (CV_64F + 0) << CONSTANTS.CN_SHIFT,
        CV_64F2 = (CV_64F + 1) << CONSTANTS.CN_SHIFT,
        CV_64F3 = (CV_64F + 2) << CONSTANTS.CN_SHIFT,
        CV_64F4 = (CV_64F + 3) << CONSTANTS.CN_SHIFT,
      };
    }
  }

  /// \} aruco_unity_package
}