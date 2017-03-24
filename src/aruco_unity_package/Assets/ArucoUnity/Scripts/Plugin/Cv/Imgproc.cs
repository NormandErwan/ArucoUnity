using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public static partial class Imgproc
      {
        /// <summary>
        /// See the OpenCV documentation for more information: 
        /// http://docs.opencv.org/3.1.0/d2/de8/group__core__array.html#ga209f2f4869e304c82d07739337eae7c5
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
        /// http://docs.opencv.org/3.1.0/da/d54/group__imgproc__transform.html#gga5bb5a1fea74ea38e1a5445ca803ff121ac97d8e4880d8b5d509e96825c7522deb
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

        // Static Member Functions
        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_initUndistortRectifyMap(System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr R,
          System.IntPtr newCameraMatrix, System.IntPtr size, int m1type, out System.IntPtr map1, out System.IntPtr map2, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_remap1(System.IntPtr src, System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation,
          int borderType, System.IntPtr borderValue, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_remap2(System.IntPtr src, System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation,
          int borderType, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_remap3(System.IntPtr src, System.IntPtr dst, System.IntPtr map1, System.IntPtr map2, int interpolation,
          System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_undistort2(System.IntPtr rotationVector, out System.IntPtr rotationMatrix, System.IntPtr cameraMatrix,
          System.IntPtr distCoeffs, System.IntPtr exception);

        public static void InitUndistortRectifyMap(Core.Mat cameraMatrix, Core.Mat distCoeffs, Core.Mat R, Core.Mat newCameraMatrix, Core.Size size, 
          Core.TYPE m1type, out Core.Mat map1, out Core.Mat map2)
        {
          Core.Exception exception = new Core.Exception();
          System.IntPtr map1Ptr, map2Ptr;

          au_cv_imgproc_initUndistortRectifyMap(cameraMatrix.cppPtr, distCoeffs.cppPtr, R.cppPtr, newCameraMatrix.cppPtr, size.cppPtr, (int)m1type,
            out map1Ptr, out map2Ptr, exception.cppPtr);
          map1 = new Core.Mat(map1Ptr);
          map2 = new Core.Mat(map2Ptr);

          exception.Check();
        }

        public static void Remap(Core.Mat src, Core.Mat dst, Core.Mat map1, Core.Mat map2, InterpolationFlags interpolation, BorderTypes borderType,
          Core.Scalar borderValue)
        {
          Core.Exception exception = new Core.Exception();
          au_cv_imgproc_remap1(src.cppPtr, dst.cppPtr, map1.cppPtr, map2.cppPtr, (int)interpolation, (int)borderType, borderValue.cppPtr, exception.cppPtr);
          exception.Check();
        }

        public static void Remap(Core.Mat src, Core.Mat dst, Core.Mat map1, Core.Mat map2, InterpolationFlags interpolation, BorderTypes borderType)
        {
          Core.Exception exception = new Core.Exception();
          au_cv_imgproc_remap2(src.cppPtr, dst.cppPtr, map1.cppPtr, map2.cppPtr, (int)interpolation, (int)borderType, exception.cppPtr);
          exception.Check();
        }

        public static void Remap(Core.Mat src, Core.Mat dst, Core.Mat map1, Core.Mat map2, InterpolationFlags interpolation)
        {
          Core.Exception exception = new Core.Exception();
          au_cv_imgproc_remap3(src.cppPtr, dst.cppPtr, map1.cppPtr, map2.cppPtr, (int)interpolation, exception.cppPtr);
          exception.Check();
        }

        // TODO: add the other version of undistord
        public static void Undistort(Core.Mat inputImage, out Core.Mat outputImage, Core.Mat cameraMatrix, Core.Mat distCoeffs)
        {
          Core.Exception exception = new Core.Exception();
          System.IntPtr outputImagePtr;

          au_cv_imgproc_undistort2(inputImage.cppPtr, out outputImagePtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, exception.cppPtr);
          outputImage = new Core.Mat(outputImagePtr);

          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}