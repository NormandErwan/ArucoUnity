using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      public static class Core
      {
        // Static Member Functions
        [DllImport("ArucoUnity")]
        static extern void au_cv_core_flip(System.IntPtr src, System.IntPtr dst, int flipCode, System.IntPtr exception);

        public static void Flip(Mat src, Mat dst, int flipCode)
        {
          Exception exception = new Exception();
          au_cv_core_flip(src.cppPtr, dst.cppPtr, flipCode, exception.cppPtr);
          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}