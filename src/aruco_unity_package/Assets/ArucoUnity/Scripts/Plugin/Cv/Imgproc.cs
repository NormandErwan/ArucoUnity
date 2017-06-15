using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      // Enums

      /// <summary>
      /// See the OpenCV documentation for more information: 
      /// http://docs.opencv.org/3.2.0/d2/de8/group__core__array.html#ga209f2f4869e304c82d07739337eae7c5
      /// </summary>
      public enum BorderTypes
      {
        Constant = 0, /// `iiiiii|abcdefgh|iiiiiii` with some specified `i`
        Replicate = 1, /// `aaaaaa|abcdefgh|hhhhhhh`
        Reflect = 2, /// `fedcba|abcdefgh|hgfedcb`
        Wrap = 3, /// `cdefgh|abcdefgh|abcdefg`
        Reflect101 = 4, /// `gfedcb|abcdefgh|gfedcba`
        Transparent = 5, /// `uvwxyz|absdefgh|ijklmno`
        Default = Reflect101, /// Same as BORDER_REFLECT_101.
        Isolated = 16 /// Do not look outside of ROI.
      }

      /// <summary>
      /// See the OpenCV documentation for more information: 
      /// http://docs.opencv.org/3.2.0/da/d54/group__imgproc__transform.html#gga5bb5a1fea74ea38e1a5445ca803ff121ac97d8e4880d8b5d509e96825c7522deb
      /// </summary>
      public enum InterpolationFlags
      {
        Nearest = 0, /// Nearest neighbor interpolation.
        Linear = 1, /// Bilinear interpolation.
        Cubic = 2, /// Bicubic interpolation.
        Area = 3, /// Resampling using pixel area relation.
        Lanczos4 = 4, /// Lanczos interpolation over 8x8 neighborhood.
        Max = 7, /// Mask for interpolation codes.
        WarpFillOutliers = 8, /// Fills all of the destination image pixels.
        WarpInverseMap = 16 /// Inverse transformation.
      }

      // Native functions

      [DllImport("ArucoUnity")]
      static extern void au_cv_imgproc_initUndistortRectifyMap(System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr R,
        System.IntPtr newCameraMatrix, System.IntPtr size, int m1type, out System.IntPtr map1, out System.IntPtr map2, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_cv_imgproc_remap(System.IntPtr src, System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation,
        int borderType, System.IntPtr borderValue, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_cv_imgproc_undistort(System.IntPtr rotationVector, out System.IntPtr rotationMatrix, System.IntPtr cameraMatrix,
        System.IntPtr distCoeffs, System.IntPtr newCameraMatrix, System.IntPtr exception);

      // Methods

      public static void InitUndistortRectifyMap(Mat cameraMatrix, Mat distCoeffs, Mat R, Mat newCameraMatrix, Size size, Type m1type, out Mat map1,
        out Mat map2)
      {
        Exception exception = new Exception();
        System.IntPtr map1Ptr, map2Ptr;

        au_cv_imgproc_initUndistortRectifyMap(cameraMatrix.CppPtr, distCoeffs.CppPtr, R.CppPtr, newCameraMatrix.CppPtr, size.CppPtr, (int)m1type,
          out map1Ptr, out map2Ptr, exception.CppPtr);
        map1 = new Mat(map1Ptr);
        map2 = new Mat(map2Ptr);

        exception.Check();
      }

      public static void Remap(Mat src, Mat dst, Mat map1, Mat map2, InterpolationFlags interpolation, BorderTypes borderType, Scalar borderValue)
      {
        Exception exception = new Exception();
        au_cv_imgproc_remap(src.CppPtr, dst.CppPtr, map1.CppPtr, map2.CppPtr, (int)interpolation, (int)borderType, borderValue.CppPtr,
          exception.CppPtr);
        exception.Check();
      }

      public static void Remap(Mat src, Mat dst, Mat map1, Mat map2, InterpolationFlags interpolation, BorderTypes borderType = BorderTypes.Constant)
      {
        Scalar borderValue = new Scalar(0, 0, 0);
        Remap(src, dst, map1, map2, interpolation, borderType, borderValue);
      }

      public static void Undistort(Mat inputImage, out Mat outputImage, Mat cameraMatrix, Mat distCoeffs, Mat newCameraMatrix)
      {
        Exception exception = new Exception();
        System.IntPtr outputImagePtr;

        au_cv_imgproc_undistort(inputImage.CppPtr, out outputImagePtr, cameraMatrix.CppPtr, distCoeffs.CppPtr, newCameraMatrix.CppPtr,
          exception.CppPtr);
        outputImage = new Mat(outputImagePtr);

        exception.Check();
      }

      public static void Undistort(Mat inputImage, out Mat outputImage, Mat cameraMatrix, Mat distCoeffs)
      {
        Mat newCameraMatrix = new Mat();
        Undistort(inputImage, out outputImage, cameraMatrix, distCoeffs, newCameraMatrix);
      }
    }
  }

  /// \} aruco_unity_package
}