using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public static partial class Calib3d
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

          public enum StereoRectifyFlags
          {
            ZeroDisparity = 1024
          };

          // Native functions

          [DllImport("ArucoUnity")]
          static extern double au_cv_calib3d_fisheye_calibrate(System.IntPtr objectPoints, System.IntPtr imagePoints, System.IntPtr image_size,
            System.IntPtr K, System.IntPtr D, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags, System.IntPtr criteria,
            System.IntPtr exception);

          [DllImport("ArucoUnity")]
          static extern void au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify(System.IntPtr K, System.IntPtr D,
            System.IntPtr image_size, System.IntPtr R, out System.IntPtr P, double balance, System.IntPtr new_size, double fov_scale,
            System.IntPtr exception);

          [DllImport("ArucoUnity")]
          static extern void au_cv_calib3d_fisheye_initUndistortRectifyMap(System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr R,
            System.IntPtr newCameraMatrix, System.IntPtr size, int m1type, out System.IntPtr map1, out System.IntPtr map2, System.IntPtr exception);

          [DllImport("ArucoUnity")]
          static extern double au_cv_calib3d_fisheye_stereoCalibrate(System.IntPtr objectPoints, System.IntPtr imagePoints1,
            System.IntPtr imagePoints2, out System.IntPtr K1, out System.IntPtr D1, out System.IntPtr K2, out System.IntPtr D2,
            System.IntPtr imageSize, out System.IntPtr R, out System.IntPtr T, int flags, System.IntPtr criteria, System.IntPtr exception);

          [DllImport("ArucoUnity")]
          static extern void au_cv_calib3d_fisheye_stereoRectify(System.IntPtr K1, System.IntPtr D1, System.IntPtr K2, System.IntPtr D2,
            System.IntPtr imageSize, System.IntPtr R, System.IntPtr tvec, out System.IntPtr R1, out System.IntPtr R2, out System.IntPtr P1,
            out System.IntPtr P2, out System.IntPtr Q, int flags, System.IntPtr newImageSize, double balance, double fov_scale,
            System.IntPtr exception);

          [DllImport("ArucoUnity")]
          static extern void au_cv_calib3d_fisheye_undistortImage(System.IntPtr distorted, out System.IntPtr undistorted,
            System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr newCameraMatrix, System.IntPtr newSize, System.IntPtr exception);

          // Static methods

          public static double Calibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Core.Size imageSize, 
            Core.Mat cameraMatrix, Core.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs, Calib flags = default(Calib), 
            Core.TermCriteria criteria = null)
          {
            Core.Exception exception = new Core.Exception();
            System.IntPtr rvecsPtr, tvecsPtr;

            double error = au_cv_calib3d_fisheye_calibrate(objectPoints.cppPtr, imagePoints.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr,
              out rvecsPtr, out tvecsPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
            rvecs = new Std.VectorMat(rvecsPtr);
            tvecs = new Std.VectorMat(tvecsPtr);

            exception.Check();
            return error;
          }

          public static void EstimateNewCameraMatrixForUndistortRectify(Core.Mat cameraMatrix, Core.Mat distCoeffs, Core.Size imageSize, Core.Mat R,
            out Core.Mat newCameraMatrix, double balance = 0.0, Core.Size newSize = null, double fovScale = 1.0)
          {
            newSize = (newSize != null) ? newSize : new Core.Size();
            Core.Exception exception = new Core.Exception();
            System.IntPtr newCameraMatrixPtr;

            au_cv_calib3d_fisheye_estimateNewCameraMatrixForUndistortRectify(cameraMatrix.cppPtr, distCoeffs.cppPtr, imageSize.cppPtr, R.cppPtr, 
              out newCameraMatrixPtr, balance, newSize.cppPtr, fovScale, exception.cppPtr);
            newCameraMatrix = new Core.Mat(newCameraMatrixPtr);

            exception.Check();
          }

          public static void InitUndistortRectifyMap(Core.Mat cameraMatrix, Core.Mat distCoeffs, Core.Mat R, Core.Mat newCameraMatrix,
            Core.Size size, int m1type, out Core.Mat map1, out Core.Mat map2)
          {
            Core.Exception exception = new Core.Exception();
            System.IntPtr map1Ptr, map2Ptr;
            au_cv_calib3d_fisheye_initUndistortRectifyMap(cameraMatrix.cppPtr, distCoeffs.cppPtr, R.cppPtr, newCameraMatrix.cppPtr, size.cppPtr,
              m1type, out map1Ptr, out map2Ptr, exception.cppPtr);
            map1 = new Core.Mat(map1Ptr);
            map2 = new Core.Mat(map2Ptr);
            exception.Check();
          }

          public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
            Std.VectorVectorPoint2f imagePoints2, out Core.Mat K1, out Core.Mat D1, out Core.Mat K2, out Core.Mat D2, Core.Size imageSize, 
            out Core.Mat rvec, out Core.Mat tvec, Calib flags = Calib.FixIntrinsic, Core.TermCriteria criteria = null)
          {
            criteria = (criteria != null) ? criteria : new Core.TermCriteria(Core.TermCriteria.Type.Count | Core.TermCriteria.Type.Eps, 100, Core.EPSILON);
            Core.Exception exception = new Core.Exception();
            System.IntPtr K1Ptr, D1Ptr, K2Ptr, D2Ptr, rvecPtr, tvecPtr;

            double error = au_cv_calib3d_fisheye_stereoCalibrate(objectPoints.cppPtr, imagePoints1.cppPtr, imagePoints2.cppPtr, out K1Ptr, 
              out D1Ptr, out K2Ptr, out D2Ptr, imageSize.cppPtr, out rvecPtr, out tvecPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
            K1 = new Core.Mat(K1Ptr);
            D1 = new Core.Mat(D1Ptr);
            K2 = new Core.Mat(K2Ptr);
            D2 = new Core.Mat(D2Ptr);
            rvec = new Core.Mat(rvecPtr);
            tvec = new Core.Mat(tvecPtr);

            exception.Check();
            return error;
          }

          public static void StereoRectify(Core.Mat K1, Core.Mat D1, Core.Mat K2, Core.Mat D2, Core.Size imageSize, Core.Mat rvec, Core.Mat tvec,
            out Core.Mat R1, out Core.Mat R2, out Core.Mat P1, out Core.Mat P2, out Core.Mat Q, StereoRectifyFlags flags,
            Core.Size newImageSize = null, double balance = 0.0, double fovScale = 1.0)
          {
            newImageSize = (newImageSize != null) ? newImageSize : new Core.Size();
            Core.Exception exception = new Core.Exception();
            System.IntPtr R1Ptr, R2Ptr, P1Ptr, P2Ptr, QPtr;

            au_cv_calib3d_fisheye_stereoRectify(K1.cppPtr, D1.cppPtr, K2.cppPtr, D2.cppPtr, imageSize.cppPtr, rvec.cppPtr, tvec.cppPtr, out R1Ptr, 
              out R2Ptr, out P1Ptr, out P2Ptr, out QPtr, (int)flags, newImageSize.cppPtr, balance, fovScale, exception.cppPtr);
            R1 = new Core.Mat(R1Ptr);
            R2 = new Core.Mat(R2Ptr);
            P1 = new Core.Mat(P1Ptr);
            P2 = new Core.Mat(P2Ptr);
            Q = new Core.Mat(QPtr);

            exception.Check();
          }

          public static void UndistortImage(Core.Mat distorted, out Core.Mat undistorted, Core.Mat cameraMatrix, Core.Mat distCoeffs, 
            Core.Mat newCameraMatrix = null, Core.Size newSize = null)
          {
            newCameraMatrix = (newCameraMatrix != null) ? newCameraMatrix : new Core.Mat();
            newSize = (newSize != null) ? newSize : new Core.Size();
            Core.Exception exception = new Core.Exception();
            System.IntPtr undistortedPtr;

            au_cv_calib3d_fisheye_undistortImage(distorted.cppPtr, out undistortedPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, 
              newCameraMatrix.cppPtr, newSize.cppPtr, exception.cppPtr);
            undistorted = new Core.Mat(undistortedPtr);

            exception.Check();
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}