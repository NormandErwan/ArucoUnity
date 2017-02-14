using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      /// <summary>
      /// See the OpenCV documentation for more information: 
      /// http://docs.opencv.org/3.1.0/d2/de8/group__core__array.html#ga209f2f4869e304c82d07739337eae7c5
      /// </summary>
      public enum BorderTypes
      {
        BORDER_CONSTANT = 0, /// `iiiiii|abcdefgh|iiiiiii` with some specified `i`
        BORDER_REPLICATE = 1, /// `aaaaaa|abcdefgh|hhhhhhh`
        BORDER_REFLECT = 2, /// `fedcba|abcdefgh|hgfedcb`
        BORDER_WRAP = 3, /// `cdefgh|abcdefgh|abcdefg`
        BORDER_REFLECT_101 = 4, /// `gfedcb|abcdefgh|gfedcba`
        BORDER_TRANSPARENT = 5, /// `uvwxyz|absdefgh|ijklmno`
        BORDER_REFLECT101 = BORDER_REFLECT_101, /// Same as BORDER_REFLECT_101.
        BORDER_DEFAULT = BORDER_REFLECT_101, /// Same as BORDER_REFLECT_101.
        BORDER_ISOLATED = 16 /// Do not look outside of ROI.
      }

      /// <summary>
      /// See the OpenCV documentation for more information: 
      /// http://docs.opencv.org/3.1.0/da/d54/group__imgproc__transform.html#gga5bb5a1fea74ea38e1a5445ca803ff121ac97d8e4880d8b5d509e96825c7522deb
      /// </summary>
      public enum InterpolationFlags
      {
        INTER_NEAREST = 0, /// Nearest neighbor interpolation.
        INTER_LINEAR = 1, /// Bilinear interpolation.
        INTER_CUBIC = 2, /// Bicubic interpolation.
        INTER_AREA = 3, /// Resampling using pixel area relation.
        INTER_LANCZOS4 = 4, /// Lanczos interpolation over 8x8 neighborhood.
        INTER_MAX = 7, /// Mask for interpolation codes.
        WARP_FILL_OUTLIERS = 8, /// Fills all of the destination image pixels.
        WARP_INVERSE_MAP = 16 /// Inverse transformation.
      }

      public static class Imgproc
      {
        // Static Member Functions
        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_initUndistortRectifyMap(System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr R, 
          System.IntPtr newCameraMatrix, System.IntPtr size, int m1type, out System.IntPtr map1, out System.IntPtr map2, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_remap1(System.IntPtr src, out System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation, 
          int borderType, System.IntPtr borderValue, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_remap2(System.IntPtr src, out System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation,
          int borderType, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_remap3(System.IntPtr src, out System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation, 
          System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_undistort2(System.IntPtr rotationVector, out System.IntPtr rotationMatrix, System.IntPtr cameraMatrix,
          System.IntPtr distCoeffs, System.IntPtr exception);

        public static void InitUndistortRectifyMap(Mat cameraMatrix, Mat distCoeffs, Mat R, Mat newCameraMatrix, Size size, TYPE m1type, out Mat map1,
          out Mat map2)
        {
          Exception exception = new Exception();
          System.IntPtr map1Ptr, map2Ptr;

          au_cv_imgproc_initUndistortRectifyMap(cameraMatrix.cppPtr, distCoeffs.cppPtr, R.cppPtr, newCameraMatrix.cppPtr, size.cppPtr, (int)m1type,
            out map1Ptr, out map2Ptr, exception.cppPtr);
          map1 = new Mat(map1Ptr);
          map2 = new Mat(map2Ptr);

          exception.Check();
        }

        public static void Remap(Mat src, out Mat dst, Mat map1, Mat map2, InterpolationFlags interpolation, BorderTypes borderType, 
          Scalar borderValue)
        {
          Exception exception = new Exception();
          System.IntPtr dstPtr;

          au_cv_imgproc_remap1(src.cppPtr, out dstPtr, map1.cppPtr, map2.cppPtr, (int)interpolation, (int) borderType, borderValue.cppPtr,
            exception.cppPtr);
          dst = new Mat(dstPtr);

          exception.Check();
        }

        public static void Remap(Mat src, out Mat dst, Mat map1, Mat map2, InterpolationFlags interpolation, BorderTypes borderType)
        {
          Exception exception = new Exception();
          System.IntPtr dstPtr;

          au_cv_imgproc_remap2(src.cppPtr, out dstPtr, map1.cppPtr, map2.cppPtr, (int)interpolation, (int)borderType, exception.cppPtr);
          dst = new Mat(dstPtr);

          exception.Check();
        }

        public static void Remap(Mat src, out Mat dst, Mat map1, Mat map2, InterpolationFlags interpolation)
        {
          Exception exception = new Exception();
          System.IntPtr dstPtr;

          au_cv_imgproc_remap3(src.cppPtr, out dstPtr, map1.cppPtr, map2.cppPtr, (int)interpolation, exception.cppPtr);
          dst = new Mat(dstPtr);

          exception.Check();
        }

        // TODO: add the other version of undistord
        public static void Undistort(Mat inputImage, out Mat outputImage, Mat cameraMatrix, Mat distCoeffs)
        {
          Exception exception = new Exception();
          System.IntPtr outputImagePtr;

          au_cv_imgproc_undistort2(inputImage.cppPtr, out outputImagePtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, exception.cppPtr);
          outputImage = new Mat(outputImagePtr);

          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}