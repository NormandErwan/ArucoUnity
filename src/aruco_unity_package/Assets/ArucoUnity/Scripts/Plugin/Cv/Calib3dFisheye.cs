using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public static partial class Fisheye
      {
        // Enums

        public enum Calib
        {
          UseIntrinsicGuess = 1 << 0,
          RecomputeExtrinsic = 1 << 1,
          CheckCond = 1 << 2,
          FixSkew = 1 << 3,
          FixK1 = 1 << 4,
          FixK2 = 1 << 5,
          FixK3 = 1 << 6,
          FixK4 = 1 << 7,
          FixIntrinsic = 1 << 8,
          FixPrincipalPoint = 1 << 9
        };

        // Native functions

        [DllImport("ArucoUnity")]
        static extern double au_cv_calib3d_fisheye_calibrate(System.IntPtr objectPoints, System.IntPtr imagePoints, System.IntPtr imageSize,
          System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
          System.IntPtr criteria, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify(System.IntPtr cameraMatrix, System.IntPtr distCoeffs,
          System.IntPtr image_size, System.IntPtr R, out System.IntPtr P, double balance, System.IntPtr newSize, double fov_scale,
          System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_calib3d_fisheye_initUndistortRectifyMap(System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr R,
          System.IntPtr newCameraMatrix, System.IntPtr size, int m1type, out System.IntPtr map1, out System.IntPtr map2, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern double au_cv_calib3d_fisheye_stereoCalibrate(System.IntPtr objectPoints, System.IntPtr imagePoints1,
          System.IntPtr imagePoints2, System.IntPtr cameraMatrix1, System.IntPtr distCoeffs1, System.IntPtr cameraMatrix2,
          System.IntPtr distCoeffs2, System.IntPtr imageSize, out System.IntPtr R, out System.IntPtr T, int flags, System.IntPtr criteria,
          System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_calib3d_fisheye_stereoRectify(System.IntPtr cameraMatrix1, System.IntPtr distCoeffs1,
          System.IntPtr cameraMatrix2, System.IntPtr distCoeffs2, System.IntPtr imageSize, System.IntPtr R, System.IntPtr tvec,
          out System.IntPtr R1, out System.IntPtr R2, out System.IntPtr P1, out System.IntPtr P2, out System.IntPtr Q, int flags,
          System.IntPtr newImageSize, double balance, double fovScale, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_calib3d_fisheye_undistortImage(System.IntPtr distorted, out System.IntPtr undistorted,
          System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr newCameraMatrix, System.IntPtr newSize, System.IntPtr exception);

        // Static methods

        public static double Calibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize, Mat cameraMatrix,
          Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags, TermCriteria criteria)
        {
          Exception exception = new Exception();
          System.IntPtr rvecsPtr, tvecsPtr;

          double error = au_cv_calib3d_fisheye_calibrate(objectPoints.CppPtr, imagePoints.CppPtr, imageSize.CppPtr, cameraMatrix.CppPtr,
            distCoeffs.CppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.CppPtr, exception.CppPtr);
          rvecs = new Std.VectorVec3d(rvecsPtr);
          tvecs = new Std.VectorVec3d(tvecsPtr);

          exception.Check();
          return error;
        }

        public static double Calibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize, Mat cameraMatrix,
          Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags = 0)
        {
          TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 100, EPSILON);
          return Calibrate(objectPoints, imagePoints, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, flags, criteria);
        }

        public static void EstimateNewCameraMatrixForUndistortRectify(Mat cameraMatrix, Mat distCoeffs, Size imageSize, Mat R,
          out Mat newCameraMatrix, double balance, Size newSize, double fovScale = 1.0)
        {
          Exception exception = new Exception();
          System.IntPtr newCameraMatrixPtr;

          au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify(cameraMatrix.CppPtr, distCoeffs.CppPtr, imageSize.CppPtr, R.CppPtr,
            out newCameraMatrixPtr, balance, newSize.CppPtr, fovScale, exception.CppPtr);
          newCameraMatrix = new Mat(newCameraMatrixPtr);

          exception.Check();
        }

        public static void EstimateNewCameraMatrixForUndistortRectify(Mat cameraMatrix, Mat distCoeffs, Size imageSize, Mat R,
          out Mat newCameraMatrix, double balance = 1.0)
        {
          Size newSize = new Size();
          EstimateNewCameraMatrixForUndistortRectify(cameraMatrix, distCoeffs, imageSize, R, out newCameraMatrix, balance, newSize);
        }

        public static void InitUndistortRectifyMap(Mat cameraMatrix, Mat distCoeffs, Mat R, Mat newCameraMatrix, Size size, Type m1type,
          out Mat map1, out Mat map2)
        {
          Exception exception = new Exception();
          System.IntPtr map1Ptr, map2Ptr;

          au_cv_calib3d_fisheye_initUndistortRectifyMap(cameraMatrix.CppPtr, distCoeffs.CppPtr, R.CppPtr, newCameraMatrix.CppPtr, size.CppPtr,
            (int)m1type, out map1Ptr, out map2Ptr, exception.CppPtr);
          map1 = new Mat(map1Ptr);
          map2 = new Mat(map2Ptr);

          exception.Check();
        }

        public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
          Std.VectorVectorPoint2f imagePoints2, Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2,
          Size imageSize, out Mat rvec, out Mat tvec, Calib flags, TermCriteria criteria)
        {
          criteria = (criteria != null) ? criteria : new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 100, EPSILON);
          Exception exception = new Exception();
          System.IntPtr rvecPtr, tvecPtr;

          double error = au_cv_calib3d_fisheye_stereoCalibrate(objectPoints.CppPtr, imagePoints1.CppPtr, imagePoints2.CppPtr, cameraMatrix1.CppPtr,
            distCoeffs1.CppPtr, cameraMatrix2.CppPtr, distCoeffs2.CppPtr, imageSize.CppPtr, out rvecPtr, out tvecPtr, (int)flags, criteria.CppPtr,
            exception.CppPtr);
          rvec = new Mat(rvecPtr);
          tvec = new Mat(tvecPtr);

          exception.Check();
          return error;
        }

        public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
          Std.VectorVectorPoint2f imagePoints2, Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2,
          Size imageSize, out Mat rvec, out Mat tvec, Calib flags = Calib.FixIntrinsic)
        {
          TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 100, EPSILON);
          return StereoCalibrate(objectPoints, imagePoints1, imagePoints2, cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize,
            out rvec, out tvec, flags, criteria);
        }

        public static void StereoRectify(Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize, Mat rvec, Mat tvec,
          out Mat R1, out Mat R2, out Mat P1, out Mat P2, out Mat Q, StereoRectifyFlags flags, Size newImageSize, double balance = 0.0,
          double fovScale = 1.0)
        {
          newImageSize = (newImageSize != null) ? newImageSize : new Size();
          Exception exception = new Exception();
          System.IntPtr R1Ptr, R2Ptr, P1Ptr, P2Ptr, QPtr;

          au_cv_calib3d_fisheye_stereoRectify(cameraMatrix1.CppPtr, distCoeffs1.CppPtr, cameraMatrix2.CppPtr, distCoeffs2.CppPtr, imageSize.CppPtr,
            rvec.CppPtr, tvec.CppPtr, out R1Ptr, out R2Ptr, out P1Ptr, out P2Ptr, out QPtr, (int)flags, newImageSize.CppPtr, balance, fovScale,
            exception.CppPtr);
          R1 = new Mat(R1Ptr);
          R2 = new Mat(R2Ptr);
          P1 = new Mat(P1Ptr);
          P2 = new Mat(P2Ptr);
          Q = new Mat(QPtr);

          exception.Check();
        }

        public static void StereoRectify(Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize, Mat rvec,
          Mat tvec, out Mat R1, out Mat R2, out Mat P1, out Mat P2, out Mat Q, StereoRectifyFlags flags)
        {
          Size newImageSize = new Size();
          StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rvec, tvec, out R1, out R2, out P1, out P2, out Q,
            flags, newImageSize);
        }

        public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs, Mat newCameraMatrix, Size newSize)
        {
          Exception exception = new Exception();
          System.IntPtr undistortedPtr;

          au_cv_calib3d_fisheye_undistortImage(distorted.CppPtr, out undistortedPtr, cameraMatrix.CppPtr, distCoeffs.CppPtr,
            newCameraMatrix.CppPtr, newSize.CppPtr, exception.CppPtr);
          undistorted = new Mat(undistortedPtr);

          exception.Check();
        }

        public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs, Mat newCameraMatrix)
        {
          Size newSize = new Size();
          UndistortImage(distorted, out undistorted, cameraMatrix, distorted, newCameraMatrix, newSize);
        }

        public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs)
        {
          Mat newCameraMatrix = new Mat();
          UndistortImage(distorted, out undistorted, cameraMatrix, distorted, newCameraMatrix);
        }
      }
    }
  }

  /// \} aruco_unity_package
}