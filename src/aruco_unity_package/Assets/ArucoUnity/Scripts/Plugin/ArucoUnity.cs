using System.Runtime.InteropServices;
using UnityEngine;
using ArucoUnity.Plugin.Std;

namespace ArucoUnity
{
  /// \defgroup aruco_unity_package ArUco Unity package
  /// \brief Unity 5 package that provide the OpenCV's ArUco Marker Detection extra module features using the ArUco Unity library.
  ///
  /// See the OpenCV documentation for more information about its ArUco Marker Detection extra module: http://docs.opencv.org/3.1.0/d9/d6a/group__aruco.html
  /// \{

  namespace Plugin
  {
    public enum PREDEFINED_DICTIONARY_NAME
    {
      DICT_4X4_50 = 0,
      DICT_4X4_100,
      DICT_4X4_250,
      DICT_4X4_1000,
      DICT_5X5_50,
      DICT_5X5_100,
      DICT_5X5_250,
      DICT_5X5_1000,
      DICT_6X6_50,
      DICT_6X6_100,
      DICT_6X6_250,
      DICT_6X6_1000,
      DICT_7X7_50,
      DICT_7X7_100,
      DICT_7X7_250,
      DICT_7X7_1000,
      DICT_ARUCO_ORIGINAL
    }

    public class Aruco
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
      static extern double au_calibrateCameraCharuco1(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
        System.IntPtr criteria, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraCharuco2(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraCharuco3(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraCharuco4(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraCharuco5(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectCharucoDiamond1(System.IntPtr image, System.IntPtr markerCorners, System.IntPtr markerIds,
        float squareMarkerLengthRate, out System.IntPtr diamondCorners, out System.IntPtr diamondIds, System.IntPtr cameraMatrix, 
        System.IntPtr distCoeffs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectCharucoDiamond2(System.IntPtr image, System.IntPtr markerCorners, System.IntPtr markerIds,
        float squareMarkerLengthRate, out System.IntPtr diamondCorners, out System.IntPtr diamondIds, System.IntPtr cameraMatrix, 
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectCharucoDiamond3(System.IntPtr image, System.IntPtr markerCorners, System.IntPtr markerIds,
        float squareMarkerLengthRate, out System.IntPtr diamondCorners, out System.IntPtr diamondIds, System.IntPtr exception);

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
      static extern void au_drawCharucoDiamond1(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength, 
        out System.IntPtr img, int marginSize, int borderBits, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawCharucoDiamond2(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength, 
        out System.IntPtr img, int marginSize, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawCharucoDiamond3(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength, 
        out System.IntPtr img, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedCornersCharuco1(System.IntPtr image, System.IntPtr charucoCorners, System.IntPtr charucoIds,
        System.IntPtr cornerColor, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedCornersCharuco2(System.IntPtr image, System.IntPtr charucoCorners, System.IntPtr charucoIds,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedCornersCharuco3(System.IntPtr image, System.IntPtr charucoCorners, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedDiamonds1(System.IntPtr image, System.IntPtr diamondCorners, System.IntPtr diamondIds,
        System.IntPtr borderColor, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedDiamonds2(System.IntPtr image, System.IntPtr diamondCorners, System.IntPtr diamondIds, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedDiamonds3(System.IntPtr image, System.IntPtr diamondCorners, System.IntPtr exception);

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
      static extern bool au_estimatePoseCharucoBoard(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvec, out System.IntPtr tvec, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_estimatePoseSingleMarkers(System.IntPtr corners, float markerLength, System.IntPtr cameraMatrix, System.IntPtr distCoeffs,
        out System.IntPtr rvecs, out System.IntPtr tvecs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_generateCustomDictionary1(int nMarkers, int markerSize, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_generateCustomDictionary2(int nMarkers, int markerSize, System.IntPtr baseDictionary, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_getPredefinedDictionary(PREDEFINED_DICTIONARY_NAME name);

      [DllImport("ArucoUnity")]
      static extern int au_interpolateCornersCharuco1(System.IntPtr markerCorners, System.IntPtr markerIds, System.IntPtr image, System.IntPtr board,
        out System.IntPtr charucoCorners, out System.IntPtr charucoIds, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern int au_interpolateCornersCharuco2(System.IntPtr markerCorners, System.IntPtr markerIds, System.IntPtr image, System.IntPtr board,
        out System.IntPtr charucoCorners, out System.IntPtr charucoIds, System.IntPtr cameraMatrix, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern int au_interpolateCornersCharuco3(System.IntPtr markerCorners, System.IntPtr markerIds, System.IntPtr image, System.IntPtr board,
        out System.IntPtr charucoCorners, out System.IntPtr charucoIds, System.IntPtr exception);

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

      public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, Cv.CALIB flags, Cv.TermCriteria criteria)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco1(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);
        tvecs = new VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, Cv.CALIB flags)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco2(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);
        tvecs = new VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco3(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);
        tvecs = new VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr;

        double reProjectionError = au_calibrateCameraAruco4(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, Board board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();

        double reProjectionError = au_calibrateCameraAruco5(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, exception.cppPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraCharuco(VectorVectorPoint2f charucoCorners, VectorVectorInt charucoIds, CharucoBoard board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, Cv.CALIB flags, Cv.TermCriteria criteria)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraCharuco1(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);
        tvecs = new VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraCharuco(VectorVectorPoint2f charucoCorners, VectorVectorInt charucoIds, CharucoBoard board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, Cv.CALIB flags)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraCharuco2(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);
        tvecs = new VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraCharuco(VectorVectorPoint2f charucoCorners, VectorVectorInt charucoIds, CharucoBoard board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraCharuco3(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);
        tvecs = new VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraCharuco(VectorVectorPoint2f charucoCorners, VectorVectorInt charucoIds, CharucoBoard board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs, out VectorMat rvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr;

        double reProjectionError = au_calibrateCameraCharuco4(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, exception.cppPtr);
        rvecs = new VectorMat(rvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraCharuco(VectorVectorPoint2f charucoCorners, VectorVectorInt charucoIds, CharucoBoard board, Cv.Size imageSize,
        Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();

        double reProjectionError = au_calibrateCameraCharuco5(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, exception.cppPtr);

        exception.Check();
        return reProjectionError;
      }

      public static void DetectCharucoDiamond(Cv.Mat image, VectorVectorPoint2f markerCorners, VectorInt markerIds, float squareMarkerLengthRate,
        out VectorVectorPoint2f diamondCorners, out VectorVec4i diamondIds, Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr diamondCornersPtr, diamondIdsPtr;

        au_detectCharucoDiamond1(image.cppPtr, markerCorners.cppPtr, markerIds.cppPtr, squareMarkerLengthRate, out diamondCornersPtr, 
          out diamondIdsPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, exception.cppPtr);
        diamondCorners = new VectorVectorPoint2f(diamondCornersPtr);
        diamondIds = new VectorVec4i(diamondIdsPtr);

        exception.Check();
      }

      public static void DetectCharucoDiamond(Cv.Mat image, VectorVectorPoint2f markerCorners, VectorInt markerIds, float squareMarkerLengthRate,
        out VectorVectorPoint2f diamondCorners, out VectorVec4i diamondIds, Cv.Mat cameraMatrix)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr diamondCornersPtr, diamondIdsPtr;

        au_detectCharucoDiamond2(image.cppPtr, markerCorners.cppPtr, markerIds.cppPtr, squareMarkerLengthRate, out diamondCornersPtr, 
          out diamondIdsPtr, cameraMatrix.cppPtr, exception.cppPtr);
        diamondCorners = new VectorVectorPoint2f(diamondCornersPtr);
        diamondIds = new VectorVec4i(diamondIdsPtr);

        exception.Check();
      }

      public static void DetectCharucoDiamond(Cv.Mat image, VectorVectorPoint2f markerCorners, VectorInt markerIds, float squareMarkerLengthRate,
        out VectorVectorPoint2f diamondCorners, out VectorVec4i diamondIds)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr diamondCornersPtr, diamondIdsPtr;

        au_detectCharucoDiamond3(image.cppPtr, markerCorners.cppPtr, markerIds.cppPtr, squareMarkerLengthRate, out diamondCornersPtr, 
          out diamondIdsPtr, exception.cppPtr);
        diamondCorners = new VectorVectorPoint2f(diamondCornersPtr);
        diamondIds = new VectorVec4i(diamondIdsPtr);

        exception.Check();
      }

      public static void DetectMarkers(Cv.Mat image, Dictionary dictionary, out VectorVectorPoint2f corners, out VectorInt ids,
        DetectorParameters parameters, out VectorVectorPoint2f rejectedImgPoints)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr cornersPtr, idsPtr, rejectedPtr;

        au_detectMarkers1(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, parameters.cppPtr, out rejectedPtr, exception.cppPtr);
        corners = new VectorVectorPoint2f(cornersPtr);
        ids = new VectorInt(idsPtr);
        rejectedImgPoints = new VectorVectorPoint2f(rejectedPtr);

        exception.Check();
      }

      public static void DetectMarkers(Cv.Mat image, Dictionary dictionary, out VectorVectorPoint2f corners, out VectorInt ids,
        DetectorParameters parameters)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr cornersPtr, idsPtr;

        au_detectMarkers2(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, parameters.cppPtr, exception.cppPtr);
        corners = new VectorVectorPoint2f(cornersPtr);
        ids = new VectorInt(idsPtr);

        exception.Check();
      }

      public static void DetectMarkers(Cv.Mat image, Dictionary dictionary, out VectorVectorPoint2f corners, out VectorInt ids)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr cornersPtr, idsPtr;

        au_detectMarkers3(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, exception.cppPtr);
        corners = new VectorVectorPoint2f(cornersPtr);
        ids = new VectorInt(idsPtr);

        exception.Check();
      }

      public static void DrawAxis(Cv.Mat image, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, Cv.Vec3d rvec, Cv.Vec3d tvec, float length)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawAxis(image.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, rvec.cppPtr, tvec.cppPtr, length, exception.cppPtr);
        exception.Check();
      }

      public static void DrawCharucoDiamond(Dictionary dictionary, Cv.Vec4i ids, int squareLength, int markerLength, out Cv.Mat image, int marginSize,
        int borderBits)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr imagePtr;

        au_drawCharucoDiamond1(dictionary.cppPtr, ids.cppPtr, squareLength, markerLength, out imagePtr, marginSize, borderBits, exception.cppPtr);
        image = new Cv.Mat(imagePtr);

        exception.Check();
      }

      public static void DrawCharucoDiamond(Dictionary dictionary, Cv.Vec4i ids, int squareLength, int markerLength, out Cv.Mat image, int marginSize)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr imagePtr;

        au_drawCharucoDiamond2(dictionary.cppPtr, ids.cppPtr, squareLength, markerLength, out imagePtr, marginSize, exception.cppPtr);
        image = new Cv.Mat(imagePtr);

        exception.Check();
      }

      public static void DrawCharucoDiamond(Dictionary dictionary, Cv.Vec4i ids, int squareLength, int markerLength, out Cv.Mat image)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr imagePtr;

        au_drawCharucoDiamond3(dictionary.cppPtr, ids.cppPtr, squareLength, markerLength, out imagePtr, exception.cppPtr);
        image = new Cv.Mat(imagePtr);

        exception.Check();
      }

      public static void DrawDetectedCornersCharuco(Cv.Mat image, VectorPoint2f charucoCorners, VectorInt charucoIds, Cv.Scalar cornerColor)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedCornersCharuco1(image.cppPtr, charucoCorners.cppPtr, charucoIds.cppPtr, cornerColor.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedCornersCharuco(Cv.Mat image, VectorPoint2f charucoCorners, VectorInt charucoIds)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedCornersCharuco2(image.cppPtr, charucoCorners.cppPtr, charucoIds.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedCornersCharuco(Cv.Mat image, VectorPoint2f charucoCorners)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedCornersCharuco3(image.cppPtr, charucoCorners.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedDiamonds(Cv.Mat image, VectorVectorPoint2f diamondCorners, VectorVec4i diamondIds, Cv.Scalar borderColor)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedDiamonds1(image.cppPtr, diamondCorners.cppPtr, diamondIds.cppPtr, borderColor.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedDiamonds(Cv.Mat image, VectorVectorPoint2f diamondCorners, VectorVec4i diamondIds)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedDiamonds2(image.cppPtr, diamondCorners.cppPtr, diamondIds.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedDiamonds(Cv.Mat image, VectorVectorPoint2f diamondCorners)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedDiamonds3(image.cppPtr, diamondCorners.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, VectorVectorPoint2f corners, VectorInt ids, Color borderColor)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Scalar borderColorScalar = borderColor;
        au_drawDetectedMarkers1(image.cppPtr, corners.cppPtr, ids.cppPtr, borderColorScalar.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, VectorVectorPoint2f corners, VectorInt ids)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedMarkers2(image.cppPtr, corners.cppPtr, ids.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, VectorVectorPoint2f corners)
      {
        Cv.Exception exception = new Cv.Exception();
        au_drawDetectedMarkers3(image.cppPtr, corners.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Mat image, VectorVectorPoint2f corners, Color borderColor)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Scalar borderColorScalar = borderColor;
        au_drawDetectedMarkers4(image.cppPtr, corners.cppPtr, borderColorScalar.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static int EstimatePoseBoard(VectorVectorPoint2f corners, VectorInt ids, Board board, Cv.Mat cameraMatrix, Cv.Mat distCoeffs,
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

      public static bool EstimatePoseCharucoBoard(VectorPoint2f charucoCorners, VectorInt charucoIds, CharucoBoard board, Cv.Mat cameraMatrix,
        Cv.Mat distCoeffs, out Cv.Vec3d rvec, out Cv.Vec3d tvec)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecPtr, tvecPtr;

        bool valid = au_estimatePoseCharucoBoard(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr,
          out rvecPtr, out tvecPtr, exception.cppPtr);
        rvec = new Cv.Vec3d(rvecPtr);
        tvec = new Cv.Vec3d(tvecPtr);

        exception.Check();
        return valid;
      }

      public static void EstimatePoseSingleMarkers(VectorVectorPoint2f corners, float markerLength, Cv.Mat cameraMatrix, Cv.Mat distCoeffs,
        out VectorVec3d rvecs, out VectorVec3d tvecs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        au_estimatePoseSingleMarkers(corners.cppPtr, markerLength, cameraMatrix.cppPtr, distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, exception.cppPtr);
        rvecs = new VectorVec3d(rvecsPtr);
        tvecs = new VectorVec3d(tvecsPtr);

        exception.Check();
      }

      public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr dictionaryPtr = au_generateCustomDictionary1(nMarkers, markerSize, exception.cppPtr);
        exception.Check();
        return new Dictionary(dictionaryPtr);
      }

      public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize, Dictionary baseDictionary)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr dictionaryPtr = au_generateCustomDictionary2(nMarkers, markerSize, baseDictionary.cppPtr, exception.cppPtr);
        exception.Check();
        return new Dictionary(dictionaryPtr);
      }

      public static Dictionary GetPredefinedDictionary(PREDEFINED_DICTIONARY_NAME name)
      {
        Dictionary dictionary = new Dictionary(au_getPredefinedDictionary(name));
        dictionary.name = name;
        return dictionary;
      }

      public static int InterpolateCornersCharuco(VectorVectorPoint2f markerCorners, VectorInt markerIds, Cv.Mat image, CharucoBoard board,
        out VectorPoint2f charucoCorners, out VectorInt charucoIds, Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr charucoCornersPtr, charucoIdsPtr;

        int interpolateCorners = au_interpolateCornersCharuco1(markerCorners.cppPtr, markerIds.cppPtr, image.cppPtr, board.cppPtr,
          out charucoCornersPtr, out charucoIdsPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, exception.cppPtr);
        charucoCorners = new VectorPoint2f(charucoCornersPtr);
        charucoIds = new VectorInt(charucoIdsPtr);
        exception.Check();

        return interpolateCorners;
      }

      public static int InterpolateCornersCharuco(VectorVectorPoint2f markerCorners, VectorInt markerIds, Cv.Mat image, CharucoBoard board,
        out VectorPoint2f charucoCorners, out VectorInt charucoIds, Cv.Mat cameraMatrix)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr charucoCornersPtr, charucoIdsPtr;

        int interpolateCorners = au_interpolateCornersCharuco2(markerCorners.cppPtr, markerIds.cppPtr, image.cppPtr, board.cppPtr,
          out charucoCornersPtr, out charucoIdsPtr, cameraMatrix.cppPtr, exception.cppPtr);
        charucoCorners = new VectorPoint2f(charucoCornersPtr);
        charucoIds = new VectorInt(charucoIdsPtr);
        exception.Check();

        return interpolateCorners;
      }

      public static int InterpolateCornersCharuco(VectorVectorPoint2f markerCorners, VectorInt markerIds, Cv.Mat image, CharucoBoard board,
        out VectorPoint2f charucoCorners, out VectorInt charucoIds)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr charucoCornersPtr, charucoIdsPtr;

        int interpolateCorners = au_interpolateCornersCharuco3(markerCorners.cppPtr, markerIds.cppPtr, image.cppPtr, board.cppPtr,
          out charucoCornersPtr, out charucoIdsPtr, exception.cppPtr);
        charucoCorners = new VectorPoint2f(charucoCornersPtr);
        charucoIds = new VectorInt(charucoIdsPtr);
        exception.Check();

        return interpolateCorners;
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, VectorInt recoveredIdxs, DetectorParameters parameters)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers1(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cppPtr, parameters.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, VectorInt recoveredIdxs)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers2(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers3(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance, float errorCorrectionRate)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers4(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs, float minRepDistance)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers5(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix, Cv.Mat distCoeffs)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers6(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners, Cv.Mat cameraMatrix)
      {
        Cv.Exception exception = new Cv.Exception();
        au_refineDetectedMarkers7(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Mat image, Board board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
        VectorVectorPoint2f rejectedCorners)
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