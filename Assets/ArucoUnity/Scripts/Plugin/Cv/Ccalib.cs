using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public static partial class Omnidir
    {
      // Enums

      public enum Calib
      {
        UseGuess = 1,
        FixSkew = 2,
        FixK1 = 4,
        FixK2 = 8,
        FixP1 = 16,
        FixP2 = 32,
        FixXi = 64,
        FixGamma = 128,
        FixCenter = 256,
      };

      public enum Rectifify
      {
        Perspective = 1,
        Cylindrical = 2,
        Longlati = 3,
        Stereographic = 4
      };

      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern double au_cv_ccalib_omnidir_calibrate(IntPtr objectPoints, IntPtr imagePoints, IntPtr imageSize,
        IntPtr cameraMatrix, IntPtr xi, IntPtr distCoeffs, out IntPtr rvecs, out IntPtr tvecs, int flags,
        IntPtr criteria, out IntPtr idx, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_ccalib_omnidir_initUndistortRectifyMap(IntPtr cameraMatrix, IntPtr distCoeffs, IntPtr xi,
        IntPtr R, IntPtr newCameraMatrix, IntPtr size, int m1type, out IntPtr map1, out IntPtr map2, int flags,
        IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_cv_ccalib_omnidir_stereoCalibrate(IntPtr objectPoints, IntPtr imagePoints1, IntPtr imagePoints2,
        IntPtr imageSize1, IntPtr imageSize2, IntPtr cameraMatrix1, IntPtr xi1, IntPtr distCoeffs1,
         IntPtr cameraMatrix2, IntPtr xi2, IntPtr distCoeffs2, out IntPtr rvec, out IntPtr tvec,
         out IntPtr rvecsL, out IntPtr tvecsL, int flags, IntPtr criteria, out IntPtr idx, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_ccalib_omnidir_stereoRectify(IntPtr rvec, IntPtr tvec, out IntPtr R1, out IntPtr R2,
        IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_ccalib_omnidir_undistortImage(IntPtr distorted, out IntPtr undistorted, IntPtr cameraMatrix,
        IntPtr distCoeffs, IntPtr xi, int flags, IntPtr newCameraMatrix, IntPtr newSize, IntPtr R,
        IntPtr exception);

      // Static methods

      public static double Calibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
        Mat cameraMatrix, Mat xi, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags, TermCriteria criteria,
        out Mat idx)
      {
        Exception exception = new Exception();
        IntPtr rvecsPtr, tvecsPtr, idxPtr;

        double error = au_cv_ccalib_omnidir_calibrate(objectPoints.CppPtr, imagePoints.CppPtr, imageSize.CppPtr, cameraMatrix.CppPtr,
          xi.CppPtr, distCoeffs.CppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.CppPtr, out idxPtr, exception.CppPtr);
        rvecs = new Std.VectorVec3d(rvecsPtr);
        tvecs = new Std.VectorVec3d(tvecsPtr);
        idx = new Mat(idxPtr);

        exception.Check();
        return error;
      }

      public static double Calibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
        Mat cameraMatrix, Mat xi, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags, TermCriteria criteria)
      {
        Mat idx;
        return Calibrate(objectPoints, imagePoints, imageSize, cameraMatrix, xi, distCoeffs, out rvecs, out tvecs, flags, criteria, out idx);
      }

      public static double Calibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
        Mat cameraMatrix, Mat xi, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags)
      {
        TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 200, EPSILON);
        return Calibrate(objectPoints, imagePoints, imageSize, cameraMatrix, xi, distCoeffs, out rvecs, out tvecs, flags, criteria);
      }

      public static void InitUndistortRectifyMap(Mat cameraMatrix, Mat distCoeffs, Mat xi, Mat R, Mat newCameraMatrix, Size size, Type m1type,
        out Mat map1, out Mat map2, Rectifify flags)
      {
        Exception exception = new Exception();
        IntPtr map1Ptr, map2Ptr;

        au_cv_ccalib_omnidir_initUndistortRectifyMap(cameraMatrix.CppPtr, distCoeffs.CppPtr, xi.CppPtr, R.CppPtr, newCameraMatrix.CppPtr, size.CppPtr,
          (int)m1type, out map1Ptr, out map2Ptr, (int)flags, exception.CppPtr);
        map1 = new Mat(map1Ptr);
        map2 = new Mat(map2Ptr);

        exception.Check();
      }

      public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
        Std.VectorVectorPoint2f imagePoints2, Size imageSize1, Size imageSize2, Mat cameraMatrix1, Mat xi1, Mat distCoeffs1, Mat cameraMatrix2,
        Mat xi2, Mat distCoeffs2, out Vec3d rvec, out Vec3d tvec, out Std.VectorVec3d rvecsL, out Std.VectorVec3d tvecsL, Calib flags,
        TermCriteria criteria, out Mat idx)
      {
        Exception exception = new Exception();
        IntPtr rvecPtr, tvecPtr, rvecsLPtr, tvecsLPtr, idxPtr;

        double error = au_cv_ccalib_omnidir_stereoCalibrate(objectPoints.CppPtr, imagePoints1.CppPtr, imagePoints2.CppPtr, imageSize1.CppPtr,
          imageSize2.CppPtr, cameraMatrix1.CppPtr, xi1.CppPtr, distCoeffs1.CppPtr, cameraMatrix2.CppPtr, xi2.CppPtr, distCoeffs2.CppPtr, out rvecPtr,
          out tvecPtr, out rvecsLPtr, out tvecsLPtr, (int)flags, criteria.CppPtr, out idxPtr, exception.CppPtr);
        rvec = new Vec3d(rvecPtr);
        tvec = new Vec3d(tvecPtr);
        rvecsL = new Std.VectorVec3d(rvecsLPtr);
        tvecsL = new Std.VectorVec3d(tvecsLPtr);
        idx = new Mat(idxPtr);

        exception.Check();
        return error;
      }

      public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
        Std.VectorVectorPoint2f imagePoints2, Size imageSize1, Size imageSize2, Mat cameraMatrix1, Mat xi1, Mat distCoeffs1, Mat cameraMatrix2,
        Mat xi2, Mat distCoeffs2, out Vec3d rvec, out Vec3d tvec, out Std.VectorVec3d rvecsL, out Std.VectorVec3d tvecsL, Calib flags, TermCriteria criteria)
      {
        Mat idx;
        return StereoCalibrate(objectPoints, imagePoints1, imagePoints2, imageSize1, imageSize2, cameraMatrix1, xi1, distCoeffs1, cameraMatrix2,
          xi2, distCoeffs2, out rvec, out tvec, out rvecsL, out tvecsL, flags, criteria, out idx);
      }

      public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
        Std.VectorVectorPoint2f imagePoints2, Size imageSize1, Size imageSize2, Mat cameraMatrix1, Mat xi1, Mat distCoeffs1, Mat cameraMatrix2,
        Mat xi2, Mat distCoeffs2, out Vec3d rvec, out Vec3d tvec, out Std.VectorVec3d rvecsL, out Std.VectorVec3d tvecsL, Calib flags)
      {
        TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 200, EPSILON);
        return StereoCalibrate(objectPoints, imagePoints1, imagePoints2, imageSize1, imageSize2, cameraMatrix1, xi1, distCoeffs1, cameraMatrix2,
          xi2, distCoeffs2, out rvec, out tvec, out rvecsL, out tvecsL, flags, criteria);
      }

      public static void StereoRectify(Vec3d rvec, Vec3d tvec, out Mat R1, out Mat R2)
      {
        Exception exception = new Exception();
        IntPtr R1Ptr, R2Ptr;

        au_cv_ccalib_omnidir_stereoRectify(rvec.CppPtr, tvec.CppPtr, out R1Ptr, out R2Ptr, exception.CppPtr);
        R1 = new Mat(R1Ptr);
        R2 = new Mat(R2Ptr);

        exception.Check();
      }

      public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs, Mat xi, Rectifify flags,
        Mat newCameraMatrix, Size newSize, Mat R)
      {
        Exception exception = new Exception();
        IntPtr undistortedPtr;

        au_cv_ccalib_omnidir_undistortImage(distorted.CppPtr, out undistortedPtr, cameraMatrix.CppPtr, distCoeffs.CppPtr, xi.CppPtr, (int)flags,
          newCameraMatrix.CppPtr, newSize.CppPtr, R.CppPtr, exception.CppPtr);
        undistorted = new Mat(undistortedPtr);

        exception.Check();
      }

      public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs, Mat xi, Rectifify flags,
        Mat newCameraMatrix, Size newSize)
      {
        Mat R = new Mat(3, 3, Type.CV_64F, new double[9] { 1, 0, 0, 0, 1, 0, 0, 0, 1 });
        UndistortImage(distorted, out undistorted, cameraMatrix, distorted, xi, flags, newCameraMatrix, newSize, R);
      }

      public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs, Mat xi, Rectifify flags,
        Mat newCameraMatrix)
      {
        Size newSize = new Size();
        UndistortImage(distorted, out undistorted, cameraMatrix, distorted, xi, flags, newCameraMatrix, newSize);
      }

      public static void UndistortImage(Mat distorted, out Mat undistorted, Mat cameraMatrix, Mat distCoeffs, Mat xi, Rectifify flags)
      {
        UndistortImage(distorted, out undistorted, cameraMatrix, distorted, xi, flags, cameraMatrix);
      }
    }
  }
}