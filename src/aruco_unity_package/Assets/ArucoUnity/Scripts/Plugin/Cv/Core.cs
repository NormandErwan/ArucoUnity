using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public static partial class Core
      {
        public enum Constants
        {
          CnShift = 3,
          DepthMax = 1 << CnShift,
          MatDepthMask = DepthMax - 1
        };

        public enum TYPE
        {
          CV_8U = 0,
          CV_8UC1 = (CV_8U & Constants.MatDepthMask) + (0 << Constants.CnShift),
          CV_8UC2 = (CV_8U & Constants.MatDepthMask) + (1 << Constants.CnShift),
          CV_8UC3 = (CV_8U & Constants.MatDepthMask) + (2 << Constants.CnShift),
          CV_8UC4 = (CV_8U & Constants.MatDepthMask) + (3 << Constants.CnShift),
          CV_16S = 3,
          CV_16SC1 = (CV_16S & Constants.MatDepthMask) + (0 << Constants.CnShift),
          CV_16SC2 = (CV_16S & Constants.MatDepthMask) + (1 << Constants.CnShift),
          CV_16SC3 = (CV_16S & Constants.MatDepthMask) + (2 << Constants.CnShift),
          CV_16SC4 = (CV_16S & Constants.MatDepthMask) + (3 << Constants.CnShift),
          CV_64F = 6,
          CV_64FC1 = (CV_64F & Constants.MatDepthMask) + (0 << Constants.CnShift),
          CV_64FC2 = (CV_64F & Constants.MatDepthMask) + (1 << Constants.CnShift),
          CV_64FC3 = (CV_64F & Constants.MatDepthMask) + (2 << Constants.CnShift),
          CV_64FC4 = (CV_64F & Constants.MatDepthMask) + (3 << Constants.CnShift),
        };

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