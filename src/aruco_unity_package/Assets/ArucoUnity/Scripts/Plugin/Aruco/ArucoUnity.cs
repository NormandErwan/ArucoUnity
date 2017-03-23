using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity
{
  /// \defgroup aruco_unity_package ArUco Unity package
  /// \brief Unity 5 package that provide the OpenCV's ArUco Marker Detection extra module features using the ArUco Unity library.
  ///
  /// See the OpenCV documentation for more information about its ArUco Marker Detection extra module: http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      /// \addtogroup aruco_unity_package
      /// \{

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraAruco1(System.IntPtr corners, System.IntPtr ids, System.IntPtr counter, System.IntPtr board,
      System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
      System.IntPtr criteria, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraAruco2(System.IntPtr corners, System.IntPtr ids, System.IntPtr counter, System.IntPtr board,
      System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
      System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraAruco3(System.IntPtr corners, System.IntPtr ids, System.IntPtr counter, System.IntPtr board,
      System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs,
      System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraAruco4(System.IntPtr corners, System.IntPtr ids, System.IntPtr counter, System.IntPtr board,
      System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraAruco5(System.IntPtr corners, System.IntPtr ids, System.IntPtr counter, System.IntPtr board,
      System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectMarkers1(System.IntPtr image, System.IntPtr dictionary, out System.IntPtr corners, out System.IntPtr ids,
        System.IntPtr parameters, out System.IntPtr rejectedImgPoints, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectMarkers2(System.IntPtr image, System.IntPtr dictionary, out System.IntPtr corners, out System.IntPtr ids,
        System.IntPtr parameters, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectMarkers3(System.IntPtr image, System.IntPtr dictionary, out System.IntPtr corners, out System.IntPtr ids,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawAxis(System.IntPtr image, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr rvec,
        System.IntPtr tvec, float length, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedMarkers1(System.IntPtr image, System.IntPtr corners, System.IntPtr ids, System.IntPtr borderColor,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedMarkers2(System.IntPtr image, System.IntPtr corners, System.IntPtr ids, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedMarkers3(System.IntPtr image, System.IntPtr corners, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedMarkers4(System.IntPtr image, System.IntPtr corners, System.IntPtr borderColor, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern int au_estimatePoseBoard(System.IntPtr corners, System.IntPtr ids, System.IntPtr board, System.IntPtr cameraMatrix,
        System.IntPtr distCoeffs, out System.IntPtr rvec, out System.IntPtr tvec, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers1(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, System.IntPtr recoveredIdxs, System.IntPtr parameters, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers2(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, System.IntPtr recoveredIdxs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers3(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers4(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, float minRepDistance, float errorCorrectionRate,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers5(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, float minRepDistance, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers6(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers7(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers8(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr exception);

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs, Cv.CALIB flags, Cv.TermCriteria criteria)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco1(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
        rvecs = new Std.VectorMat(rvecsPtr);
        tvecs = new Std.VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs, Cv.CALIB flags)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco2(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, exception.cppPtr);
        rvecs = new Std.VectorMat(rvecsPtr);
        tvecs = new Std.VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco3(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, exception.cppPtr);
        rvecs = new Std.VectorMat(rvecsPtr);
        tvecs = new Std.VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out Std.VectorMat rvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr;

        double reProjectionError = au_calibrateCameraAruco4(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, exception.cppPtr);
        rvecs = new Std.VectorMat(rvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();

        double reProjectionError = au_calibrateCameraAruco5(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, exception.cppPtr);

        exception.Check();
        return reProjectionError;
      }

      public static void DetectMarkers(Cv.Mat image, Dictionary dictionary, out Std.VectorVectorPoint2f corners, out Std.VectorInt ids,
        DetectorParameters parameters, out Std.VectorVectorPoint2f rejectedImgPoints)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr cornersPtr, idsPtr, rejectedPtr;

        au_detectMarkers1(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, parameters.cppPtr, out rejectedPtr, exception.cppPtr);
        corners = new Std.VectorVectorPoint2f(cornersPtr);
        ids = new Std.VectorInt(idsPtr);
        rejectedImgPoints = new Std.VectorVectorPoint2f(rejectedPtr);

        exception.Check();
      }

      public static void DetectMarkers(Cv.Mat image, Dictionary dictionary, out Std.VectorVectorPoint2f corners, out Std.VectorInt ids,
        DetectorParameters parameters)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr cornersPtr, idsPtr;

        au_detectMarkers2(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, parameters.cppPtr, exception.cppPtr);
        corners = new Std.VectorVectorPoint2f(cornersPtr);
        ids = new Std.VectorInt(idsPtr);

        exception.Check();
      }

      public static void DetectMarkers(Cv.Mat image, Dictionary dictionary, out Std.VectorVectorPoint2f corners, out Std.VectorInt ids)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr cornersPtr, idsPtr;

        au_detectMarkers3(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, exception.cppPtr);
        corners = new Std.VectorVectorPoint2f(cornersPtr);
        ids = new Std.VectorInt(idsPtr);

        exception.Check();
      }

      public static void DrawAxis(Cv.Mat image, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, Cv.Vec3d rvec, Cv.Vec3d tvec, float length)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawAxis(image.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, rvec.cppPtr, tvec.cppPtr, length, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, Std.VectorVectorPoint2f corners, Std.VectorInt ids, Color borderColor)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Scalar borderColorScalar = borderColor;
        au_drawDetectedMarkers1(image.cppPtr, corners.cppPtr, ids.cppPtr, borderColorScalar.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, Std.VectorVectorPoint2f corners, Std.VectorInt ids)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedMarkers2(image.cppPtr, corners.cppPtr, ids.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, Std.VectorVectorPoint2f corners)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedMarkers3(image.cppPtr, corners.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, Std.VectorVectorPoint2f corners, Color borderColor)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Scalar borderColorScalar = borderColor;
        au_drawDetectedMarkers4(image.cppPtr, corners.cppPtr, borderColorScalar.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static int EstimatePoseBoard(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Board board, Cv.Mat cameraMatrix, Cv.Mat distCoeffs,
        out Cv.Vec3d rvec, out Cv.Vec3d tvec)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecPtr, tvecPtr;

        int valid = au_estimatePoseBoard(corners.cppPtr, ids.cppPtr, board.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, out rvecPtr, out tvecPtr,
          exception.cppPtr);
        rvec = new Cv.Vec3d(rvecPtr);
        tvec = new Cv.Vec3d(tvecPtr);

        exception.Check();
        return valid;
      }

      public static void EstimatePoseSingleMarkers(Std.VectorVectorPoint2f corners, float markerLength, Cv.Mat cameraMatrix, Cv.Mat distCoeffs,
        out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        au_estimatePoseSingleMarkers(corners.cppPtr, markerLength, cameraMatrix.cppPtr, distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, exception.cppPtr);
        rvecs = new Std.VectorVec3d(rvecsPtr);
        tvecs = new Std.VectorVec3d(tvecsPtr);

        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, Std.VectorInt recoveredIdxs, DetectorParameters parameters)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers1(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cppPtr, parameters.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, Std.VectorInt recoveredIdxs)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers2(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers3(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers4(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers5(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers6(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers7(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers8(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, exception.cppPtr);
        exception.Check();
      }

      /// \} aruco_unity_package functions

    }
  }

  /// \} aruco_unity_package
}