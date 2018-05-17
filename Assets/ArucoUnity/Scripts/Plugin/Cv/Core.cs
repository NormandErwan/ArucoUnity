using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    // Constants

    public const int CN_SHIFT = 3;
    public const int DEPTH_MAX = 1 << CN_SHIFT;
    public const int MAT_DEPTH_MAX = DEPTH_MAX - 1;
    public const double EPSILON = 2.2204460492503131e-016;

    public const int horizontalFlipCode = 1;
    public const int verticalFlipCode = 0;
    public const int bothAxesFlipCode = -1;

    // Enums

    public enum Type
    {
      CV_8U = 0,
      CV_8UC1 = (CV_8U & MAT_DEPTH_MAX) + (0 << CN_SHIFT),
      CV_8UC2 = (CV_8U & MAT_DEPTH_MAX) + (1 << CN_SHIFT),
      CV_8UC3 = (CV_8U & MAT_DEPTH_MAX) + (2 << CN_SHIFT),
      CV_8UC4 = (CV_8U & MAT_DEPTH_MAX) + (3 << CN_SHIFT),
      CV_16S = 3,
      CV_16SC1 = (CV_16S & MAT_DEPTH_MAX) + (0 << CN_SHIFT),
      CV_16SC2 = (CV_16S & MAT_DEPTH_MAX) + (1 << CN_SHIFT),
      CV_16SC3 = (CV_16S & MAT_DEPTH_MAX) + (2 << CN_SHIFT),
      CV_16SC4 = (CV_16S & MAT_DEPTH_MAX) + (3 << CN_SHIFT),
      CV_64F = 6,
      CV_64FC1 = (CV_64F & MAT_DEPTH_MAX) + (0 << CN_SHIFT),
      CV_64FC2 = (CV_64F & MAT_DEPTH_MAX) + (1 << CN_SHIFT),
      CV_64FC3 = (CV_64F & MAT_DEPTH_MAX) + (2 << CN_SHIFT),
      CV_64FC4 = (CV_64F & MAT_DEPTH_MAX) + (3 << CN_SHIFT),
    };

    // Native functions

    [DllImport("ArucoUnityPlugin")]
    static extern void au_cv_core_flip(IntPtr src, IntPtr dst, int flipCode, IntPtr exception);

    // Static methods

    public static void Flip(Mat src, Mat dst, int flipCode)
    {
      Exception exception = new Exception();
      au_cv_core_flip(src.CppPtr, dst.CppPtr, flipCode, exception.CppPtr);
      exception.Check();
    }
  }
}