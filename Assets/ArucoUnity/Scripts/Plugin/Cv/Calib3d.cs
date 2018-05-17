using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    // Enums

    public enum Calib
    {
      UseIntrinsicGuess = 0x00001,
      FixAspectRatio = 0x00002,
      FixPrincipalPoint = 0x00004,
      ZeroTangentDist = 0x00008,
      FixK1 = 0x00020,
      FixK2 = 0x00040,
      FixK3 = 0x00080,
      FixK4 = 0x00800,
      FixK5 = 0x01000,
      FixK6 = 0x02000,
      RationalModel = 0x04000,
      ThinPrismModel = 0x08000,
      FixS1S2S3S4 = 0x10000,
      TiltedModel = 0x40000,
      FixTauxTauy = 0x80000,
      // Following are only for stereo
      FixFocalLength = 0x00010,
      FixIntrinsic = 0x00100,
      SameFocalLength = 0x00200
    };

    public enum StereoRectifyFlags
    {
      ZeroDisparity = 1024
    };

    // Native functions

    [DllImport("ArucoUnityPlugin")]
    static extern double au_cv_calib3d_calibrateCamera1(IntPtr objectPoints, IntPtr imagePoints, IntPtr imageSize,
      IntPtr cameraMatrix, IntPtr distCoeffs, out IntPtr rvecs, out IntPtr tvecs,
      IntPtr stdDeviationsIntrinsics, IntPtr stdDeviationsExtrinsics, IntPtr perViewErrors, int flags,
      IntPtr criteria, IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern double au_cv_calib3d_calibrateCamera2(IntPtr objectPoints, IntPtr imagePoints, IntPtr imageSize,
      IntPtr cameraMatrix, IntPtr distCoeffs, out IntPtr rvecs, out IntPtr tvecs, int flags, IntPtr criteria,
      IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern IntPtr au_cv_calib3d_initCameraMatrix2D(IntPtr objectPoints, IntPtr imagePoints, IntPtr imageSize,
      double aspectRatio, IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern void au_cv_calib3d_Rodrigues1(IntPtr rotationVector, out IntPtr rotationMatrix, IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern void au_cv_calib3d_Rodrigues2(IntPtr rotationMatrix, out IntPtr rotationVector, IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern double au_cv_calib3d_stereoCalibrate(IntPtr objectPoints, IntPtr imagePoints1, IntPtr imagePoints2,
      IntPtr cameraMatrix1, IntPtr distCoeffs1, IntPtr cameraMatrix2, IntPtr distCoeffs2, IntPtr imageSize,
      out IntPtr R, out IntPtr T, out IntPtr E, out IntPtr F, int flags, IntPtr criteria,
      IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern void au_cv_calib3d_stereoRectify(IntPtr cameraMatrix1, IntPtr distCoeffs1, IntPtr cameraMatrix2,
      IntPtr distCoeffs2, IntPtr imageSize, IntPtr R, IntPtr T, out IntPtr R1, out IntPtr R2,
      out IntPtr P1, out IntPtr P2, out IntPtr Q, int flags, double alpha, IntPtr newImageSize,
      IntPtr validPixROI1, IntPtr validPixROI2, IntPtr exception);

    [DllImport("ArucoUnityPlugin")]
    static extern IntPtr au_cv_calib3d_getOptimalNewCameraMatrix(IntPtr cameraMatrix, IntPtr distCoeffs,
      IntPtr imageSize, double scalingFactor, IntPtr newImageSize, IntPtr validPixROI, bool centerPrincipalPoint,
      IntPtr exception);

    // Static methods

    public static double CalibrateCamera(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Std.VectorDouble stdDeviationsIntrinsics,
      Std.VectorDouble stdDeviationsExtrinsics, Std.VectorDouble perViewErrors, Calib flags, TermCriteria criteria)
    {
      Exception exception = new Exception();
      IntPtr rvecsPtr, tvecsPtr;

      double error = au_cv_calib3d_calibrateCamera1(objectPoints.CppPtr, imagePoints.CppPtr, imageSize.CppPtr, cameraMatrix.CppPtr,
        distCoeffs.CppPtr, out rvecsPtr, out tvecsPtr, stdDeviationsIntrinsics.CppPtr, stdDeviationsExtrinsics.CppPtr, perViewErrors.CppPtr,
        (int)flags, criteria.CppPtr, exception.CppPtr);
      rvecs = new Std.VectorVec3d(rvecsPtr);
      tvecs = new Std.VectorVec3d(tvecsPtr);
      exception.Check();

      return error;
    }

    public static double CalibrateCamera(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Std.VectorDouble stdDeviationsIntrinsics,
      Std.VectorDouble stdDeviationsExtrinsics, Std.VectorDouble perViewErrors, Calib flags = 0)
    {
      TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 30, EPSILON);
      return CalibrateCamera(objectPoints, imagePoints, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, stdDeviationsIntrinsics,
        stdDeviationsExtrinsics, perViewErrors, flags, criteria);
    }

    public static double CalibrateCamera(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags, TermCriteria criteria)
    {
      Exception exception = new Exception();
      IntPtr rvecsPtr, tvecsPtr;

      double error = au_cv_calib3d_calibrateCamera2(objectPoints.CppPtr, imagePoints.CppPtr, imageSize.CppPtr, cameraMatrix.CppPtr,
        distCoeffs.CppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.CppPtr, exception.CppPtr);
      rvecs = new Std.VectorVec3d(rvecsPtr);
      tvecs = new Std.VectorVec3d(tvecsPtr);

      exception.Check();
      return error;
    }

    public static double CalibrateCamera(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs, Calib flags = 0)
    {
      TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 30, EPSILON);
      return CalibrateCamera(objectPoints, imagePoints, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, flags, criteria);
    }

    public static Mat InitCameraMatrix2D(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Size imageSize,
      double aspectRatio = 1.0)
    {
      Exception exception = new Exception();
      IntPtr cameraMatrixPtr = au_cv_calib3d_initCameraMatrix2D(objectPoints.CppPtr, imagePoints.CppPtr, imageSize.CppPtr, aspectRatio,
        exception.CppPtr);
      exception.Check();
      return new Mat(cameraMatrixPtr);
    }

    public static void Rodrigues(Vec3d rotationVector, out Mat rotationMatrix)
    {
      Exception exception = new Exception();
      IntPtr rotationMatrixPtr;
      au_cv_calib3d_Rodrigues1(rotationVector.CppPtr, out rotationMatrixPtr, exception.CppPtr);
      rotationMatrix = new Mat(rotationMatrixPtr);
      exception.Check();
    }

    public static void Rodrigues(Mat rotationMatrix, out Vec3d rotationVector)
    {
      Exception exception = new Exception();
      IntPtr rotationVectorPtr;
      au_cv_calib3d_Rodrigues2(rotationMatrix.CppPtr, out rotationVectorPtr, exception.CppPtr);
      rotationVector = new Vec3d(rotationVectorPtr);
      exception.Check();
    }

    public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
      Std.VectorVectorPoint2f imagePoints2, Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize,
      out Mat rotationMatrix, out Vec3d tvec, out Mat essentialMatrix, out Mat fundamentalMatrix, Calib flags, TermCriteria criteria)
    {
      Exception exception = new Exception();
      IntPtr rotationMatrixPtr, tvecPtr, essentialMatrixPtr, fundamentalMatrixPtr;

      double error = au_cv_calib3d_stereoCalibrate(objectPoints.CppPtr, imagePoints1.CppPtr, imagePoints2.CppPtr, cameraMatrix1.CppPtr,
        distCoeffs1.CppPtr, cameraMatrix2.CppPtr, distCoeffs2.CppPtr, imageSize.CppPtr, out rotationMatrixPtr, out tvecPtr, out essentialMatrixPtr,
        out fundamentalMatrixPtr, (int)flags, criteria.CppPtr, exception.CppPtr);
      rotationMatrix = new Mat(rotationMatrixPtr);
      tvec = new Vec3d(tvecPtr);
      essentialMatrix = new Mat(essentialMatrixPtr);
      fundamentalMatrix = new Mat(fundamentalMatrixPtr);

      exception.Check();
      return error;
    }

    public static double StereoCalibrate(Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints1,
      Std.VectorVectorPoint2f imagePoints2, Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize,
      out Mat rotationMatrix, out Vec3d tvec, out Mat essentialMatrix, out Mat fundamentalMatrix, Calib flags = Calib.FixIntrinsic)
    {
      TermCriteria criteria = new TermCriteria(TermCriteria.Type.Count | TermCriteria.Type.Eps, 30, 1e-6);
      return StereoCalibrate(objectPoints, imagePoints1, imagePoints2, cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize,
        out rotationMatrix, out tvec, out essentialMatrix, out fundamentalMatrix, flags, criteria);
    }

    public static void StereoRectify(Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize, Mat rotationMatrix,
      Vec3d tvec, out Mat rectificationMatrix1, out Mat rectificationMatrix2, out Mat projectionMatrix1, out Mat projectionMatrix2,
      out Mat disparityMatrix, StereoRectifyFlags flags, double scalingFactor, Size newImageSize, Rect validPixROI1, Rect validPixROI2)
    {
      Exception exception = new Exception();
      IntPtr rectificationMatrix1Ptr, rectificationMatrix2Ptr, projectionMatrix1Ptr, projectionMatrix2Ptr, disparityMatrixPtr;

      au_cv_calib3d_stereoRectify(cameraMatrix1.CppPtr, distCoeffs1.CppPtr, cameraMatrix2.CppPtr, distCoeffs2.CppPtr, imageSize.CppPtr,
        rotationMatrix.CppPtr, tvec.CppPtr, out rectificationMatrix1Ptr, out rectificationMatrix2Ptr, out projectionMatrix1Ptr,
        out projectionMatrix2Ptr, out disparityMatrixPtr, (int)flags, scalingFactor, newImageSize.CppPtr, validPixROI1.CppPtr,
        validPixROI2.CppPtr, exception.CppPtr);
      rectificationMatrix1 = new Mat(rectificationMatrix1Ptr);
      rectificationMatrix2 = new Mat(rectificationMatrix2Ptr);
      projectionMatrix1 = new Mat(projectionMatrix1Ptr);
      projectionMatrix2 = new Mat(projectionMatrix2Ptr);
      disparityMatrix = new Mat(disparityMatrixPtr);

      exception.Check();
    }

    public static void StereoRectify(Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize, Mat rotationMatrix,
      Vec3d tvec, out Mat rectificationMatrix1, out Mat rectificationMatrix2, out Mat projectionMatrix1, out Mat projectionMatrix2,
      out Mat disparityMatrix, StereoRectifyFlags flags, double scalingFactor, Size newImageSize, Rect validPixROI1)
    {
      Rect validPixROI2 = new Rect();
      StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rotationMatrix, tvec, out rectificationMatrix1,
        out rectificationMatrix2, out projectionMatrix1, out projectionMatrix2, out disparityMatrix, flags, scalingFactor, newImageSize,
        validPixROI1, validPixROI2);
    }

    public static void StereoRectify(Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize, Mat rotationMatrix,
      Vec3d tvec, out Mat rectificationMatrix1, out Mat rectificationMatrix2, out Mat projectionMatrix1, out Mat projectionMatrix2,
      out Mat disparityMatrix, StereoRectifyFlags flags, double scalingFactor, Size newImageSize)
    {
      Rect validPixROI1 = new Rect();
      StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rotationMatrix, tvec, out rectificationMatrix1,
        out rectificationMatrix2, out projectionMatrix1, out projectionMatrix2, out disparityMatrix, flags, scalingFactor, newImageSize,
        validPixROI1);
    }

    public static void StereoRectify(Mat cameraMatrix1, Mat distCoeffs1, Mat cameraMatrix2, Mat distCoeffs2, Size imageSize, Mat rotationMatrix,
      Vec3d tvec, out Mat rectificationMatrix1, out Mat rectificationMatrix2, out Mat projectionMatrix1, out Mat projectionMatrix2,
      out Mat disparityMatrix, StereoRectifyFlags flags = StereoRectifyFlags.ZeroDisparity, double scalingFactor = -1)
    {
      Size newImageSize = new Size();
      StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rotationMatrix, tvec, out rectificationMatrix1,
        out rectificationMatrix2, out projectionMatrix1, out projectionMatrix2, out disparityMatrix, flags, scalingFactor, newImageSize);
    }

    public static Mat GetOptimalNewCameraMatrix(Mat cameraMatrix, Mat distCoeffs, Size imageSize, double scalingFactor, Size newImageSize,
      Rect validPixRoi, bool centerPrincipalPoint = false)
    {
      Exception exception = new Exception();
      IntPtr newCameraMatrixPtr;

      newCameraMatrixPtr = au_cv_calib3d_getOptimalNewCameraMatrix(cameraMatrix.CppPtr, distCoeffs.CppPtr, imageSize.CppPtr, scalingFactor,
        newImageSize.CppPtr, validPixRoi.CppPtr, centerPrincipalPoint, exception.CppPtr);

      exception.Check();
      return (newCameraMatrixPtr != IntPtr.Zero) ? new Mat(newCameraMatrixPtr) : null;
    }

    public static Mat GetOptimalNewCameraMatrix(Mat cameraMatrix, Mat distCoeffs, Size imageSize, double scalingFactor, Size newImageSize)
    {
      Rect validPixROI = new Rect();
      return GetOptimalNewCameraMatrix(cameraMatrix, distCoeffs, imageSize, scalingFactor, newImageSize, validPixROI);
    }

    public static Mat GetOptimalNewCameraMatrix(Mat cameraMatrix, Mat distCoeffs, Size imageSize, double scalingFactor)
    {
      Size newImageSize = imageSize;
      return GetOptimalNewCameraMatrix(cameraMatrix, distCoeffs, imageSize, scalingFactor, newImageSize);
    }
  }
}