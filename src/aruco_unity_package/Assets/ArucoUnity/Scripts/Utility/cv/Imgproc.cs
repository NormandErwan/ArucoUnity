using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      public static class Imgproc
      {
        // Static Member Functions
        [DllImport("ArucoUnity")]
        static extern void au_cv_imgproc_undistord2(System.IntPtr rotationVector, out System.IntPtr rotationMatrix, System.IntPtr cameraMatrix,
          System.IntPtr distCoeffs, System.IntPtr exception);

        // TODO: add the other version of undistord
        public static void Undistord(Mat inputImage, out Mat outputImage, Mat cameraMatrix, Mat distCoeffs)
        {
          Exception exception = new Exception();
          System.IntPtr outputImagePtr;

          au_cv_imgproc_undistord2(inputImage.cvPtr, out outputImagePtr, cameraMatrix.cvPtr, distCoeffs.cvPtr, exception.cvPtr);
          outputImage = new Mat(outputImagePtr);

          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}