using System.Runtime.InteropServices;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
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

  public enum CALIB
  {
    USE_INTRINSIC_GUESS = 0x00001,
    FIX_ASPECT_RATIO = 0x00002,
    FIX_PRINCIPAL_POINT = 0x00004,
    ZERO_TANGENT_DIST = 0x00008,
    FIX_FOCAL_LENGTH = 0x00010,
    FIX_K1 = 0x00020,
    FIX_K2 = 0x00040,
    FIX_K3 = 0x00080,
    FIX_K4 = 0x00800,
    FIX_K5 = 0x01000,
    FIX_K6 = 0x02000,
    RATIONAL_MODEL = 0x04000,
    THIN_PRISM_MODEL = 0x08000,
    FIX_S1_S2_S3_S4 = 0x10000,
    TILTED_MODEL = 0x40000,
    FIX_TAUX_TAUY = 0x80000,
    FIX_INTRINSIC = 0x00100,
    SAME_FOCAL_LENGTH = 0x00200,
    ZERO_DISPARITY = 0x00400,
    USE_LU = (1 << 17)
  };

  public enum CV 
  {
    CN_SHIFT = 3
  };

  public enum TYPE
  {
    CV_8U = 0,
    CV_8UC1 = (CV_8U + 0) << CV.CN_SHIFT,
    CV_8UC2 = (CV_8U + 1) << CV.CN_SHIFT,
    CV_8UC3 = (CV_8U + 2) << CV.CN_SHIFT,
    CV_8UC4 = (CV_8U + 3) << CV.CN_SHIFT,
    CV_64F = 6,
    CV_64F1 = (CV_64F + 0) << CV.CN_SHIFT,
    CV_64F2 = (CV_64F + 1) << CV.CN_SHIFT,
    CV_64F3 = (CV_64F + 2) << CV.CN_SHIFT,
    CV_64F4 = (CV_64F + 3) << CV.CN_SHIFT,
  };

  public class Methods
  {
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
      float squareMarkerLengthRate, System.IntPtr diamondCorners, System.IntPtr diamondIds, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, 
      System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_detectCharucoDiamond2(System.IntPtr image, System.IntPtr markerCorners, System.IntPtr markerIds, 
      float squareMarkerLengthRate, System.IntPtr diamondCorners, System.IntPtr diamondIds, System.IntPtr cameraMatrix, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_detectCharucoDiamond3(System.IntPtr image, System.IntPtr markerCorners, System.IntPtr markerIds, 
      float squareMarkerLengthRate, System.IntPtr diamondCorners, System.IntPtr diamondIds, System.IntPtr exception);

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
    static extern void au_drawCharucoDiamond1(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength, out System.IntPtr img, 
      int marginSize, int borderBits, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_drawCharucoDiamond2(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength, out System.IntPtr img, 
      int marginSize, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_drawCharucoDiamond3(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength, out System.IntPtr img, 
      System.IntPtr exception);

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

    public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, GridBoard board, Size imageSize, 
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, int flags, TermCriteria criteria) 
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;

      double reProjectionError = au_calibrateCameraAruco1(corners.cvPtr, ids.cvPtr, counter.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, flags, criteria.cvPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return reProjectionError;
    }

    public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, GridBoard board, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, int flags) 
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;

      double reProjectionError = au_calibrateCameraAruco2(corners.cvPtr, ids.cvPtr, counter.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, flags, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return reProjectionError;
    }

    public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, GridBoard board, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs) 
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;

      double reProjectionError = au_calibrateCameraAruco3(corners.cvPtr, ids.cvPtr, counter.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return reProjectionError;
    }

    public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, GridBoard board, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs) 
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr;

      double reProjectionError = au_calibrateCameraAruco4(corners.cvPtr, ids.cvPtr, counter.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      
      exception.Check();
      return reProjectionError;
    }

    public static double CalibrateCameraAruco(VectorVectorPoint2f corners, VectorInt ids, VectorInt counter, GridBoard board, Size imageSize,
      Mat cameraMatrix, Mat distCoeffs) 
    {
      Exception exception = new Exception();
      
      double reProjectionError = au_calibrateCameraAruco5(corners.cvPtr, ids.cvPtr, counter.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, exception.cvPtr);
      
      exception.Check();
      return reProjectionError;
    }

    public static double CalibrateCameraCharuco(VectorMat charucoCorners, VectorInt charucoIds, CharucoBoard board, Size imageSize, 
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, int flags, TermCriteria criteria)
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;
      
      double reProjectionError = au_calibrateCameraCharuco1(charucoCorners.cvPtr, charucoIds.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, flags, criteria.cvPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return reProjectionError;
    }

    public static double CalibrateCameraCharuco(VectorMat charucoCorners, VectorInt charucoIds, CharucoBoard board, Size imageSize, 
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs, int flags)
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;
      
      double reProjectionError = au_calibrateCameraCharuco2(charucoCorners.cvPtr, charucoIds.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, flags, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return reProjectionError;
    }             
    
    public static double CalibrateCameraCharuco(VectorMat charucoCorners, VectorInt charucoIds, CharucoBoard board, Size imageSize, 
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs)
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;
      
      double reProjectionError = au_calibrateCameraCharuco3(charucoCorners.cvPtr, charucoIds.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return reProjectionError;
    }       
    
    public static double CalibrateCameraCharuco(VectorMat charucoCorners, VectorInt charucoIds, CharucoBoard board, Size imageSize, 
      Mat cameraMatrix, Mat distCoeffs, out VectorMat rvecs)
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr;
      
      double reProjectionError = au_calibrateCameraCharuco4(charucoCorners.cvPtr, charucoIds.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, out rvecsPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);

      exception.Check();
      return reProjectionError;
    }       
    
    public static double CalibrateCameraCharuco(VectorMat charucoCorners, VectorInt charucoIds, CharucoBoard board, Size imageSize, 
      Mat cameraMatrix, Mat distCoeffs)
    {
      Exception exception = new Exception();
      
      double reProjectionError = au_calibrateCameraCharuco5(charucoCorners.cvPtr, charucoIds.cvPtr, board.cvPtr, imageSize.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, exception.cvPtr);

      exception.Check();
      return reProjectionError;
    }       

    public static void DetectCharucoDiamond(Mat image, VectorVectorPoint2f markerCorners, VectorInt markerIds, float squareMarkerLengthRate,
      VectorVectorPoint2f diamondCorners, VectorVec4i diamondIds, Mat cameraMatrix, Mat distCoeffs)
    {
      Exception exception = new Exception();
      au_detectCharucoDiamond1(image.cvPtr, markerCorners.cvPtr, markerIds.cvPtr, squareMarkerLengthRate, diamondCorners.cvPtr, diamondIds.cvPtr, 
        cameraMatrix.cvPtr, distCoeffs.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DetectCharucoDiamond(Mat image, VectorVectorPoint2f markerCorners, VectorInt markerIds, float squareMarkerLengthRate,
      VectorVectorPoint2f diamondCorners, VectorVec4i diamondIds, Mat cameraMatrix)
    {
      Exception exception = new Exception();
      au_detectCharucoDiamond2(image.cvPtr, markerCorners.cvPtr, markerIds.cvPtr, squareMarkerLengthRate, diamondCorners.cvPtr, diamondIds.cvPtr, 
        cameraMatrix.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DetectCharucoDiamond(Mat image, VectorVectorPoint2f markerCorners, VectorInt markerIds, float squareMarkerLengthRate,
      VectorVectorPoint2f diamondCorners, VectorVec4i diamondIds)
    {
      Exception exception = new Exception();
      au_detectCharucoDiamond3(image.cvPtr, markerCorners.cvPtr, markerIds.cvPtr, squareMarkerLengthRate, diamondCorners.cvPtr, diamondIds.cvPtr, 
        exception.cvPtr);
      exception.Check();
    }

    public static void DetectMarkers(Mat image, Dictionary dictionary, out VectorVectorPoint2f corners, out VectorInt ids, 
      DetectorParameters parameters, out VectorVectorPoint2f rejectedImgPoints)
    {
      Exception exception = new Exception();
      System.IntPtr cornersPtr, idsPtr, rejectedPtr;

      au_detectMarkers1(image.cvPtr, dictionary.cvPtr, out cornersPtr, out idsPtr, parameters.cvPtr, out rejectedPtr, exception.cvPtr);
      corners = new VectorVectorPoint2f(cornersPtr);
      ids = new VectorInt(idsPtr);
      rejectedImgPoints = new VectorVectorPoint2f(rejectedPtr);

      exception.Check();
    }

    public static void DetectMarkers(Mat image, Dictionary dictionary, out VectorVectorPoint2f corners, out VectorInt ids, 
      DetectorParameters parameters)
    {
      Exception exception = new Exception();
      System.IntPtr cornersPtr, idsPtr;

      au_detectMarkers2(image.cvPtr, dictionary.cvPtr, out cornersPtr, out idsPtr, parameters.cvPtr, exception.cvPtr);
      corners = new VectorVectorPoint2f(cornersPtr);
      ids = new VectorInt(idsPtr);

      exception.Check();
    }

    public static void DetectMarkers(Mat image, Dictionary dictionary, out VectorVectorPoint2f corners, out VectorInt ids)
    {
      Exception exception = new Exception();
      System.IntPtr cornersPtr, idsPtr;

      au_detectMarkers3(image.cvPtr, dictionary.cvPtr, out cornersPtr, out idsPtr, exception.cvPtr);
      corners = new VectorVectorPoint2f(cornersPtr);
      ids = new VectorInt(idsPtr);

      exception.Check();
    }

    public static void DrawCharucoDiamond(Dictionary dictionary, Vec4i ids, int squareLength, int markerLength, out Mat image, int marginSize, 
      int borderBits)
    {
      Exception exception = new Exception();
      System.IntPtr imagePtr;

      au_drawCharucoDiamond1(dictionary.cvPtr, ids.cvPtr, squareLength, markerLength, out imagePtr, marginSize, borderBits, exception.cvPtr);
      image = new Mat(imagePtr);

      exception.Check();
    }

    public static void DrawCharucoDiamond(Dictionary dictionary, Vec4i ids, int squareLength, int markerLength, out Mat image, int marginSize)
    {
      Exception exception = new Exception();
      System.IntPtr imagePtr;

      au_drawCharucoDiamond2(dictionary.cvPtr, ids.cvPtr, squareLength, markerLength, out imagePtr, marginSize, exception.cvPtr);
      image = new Mat(imagePtr);

      exception.Check();
    }

    public static void DrawCharucoDiamond(Dictionary dictionary, Vec4i ids, int squareLength, int markerLength, out Mat image)
    {
      Exception exception = new Exception();
      System.IntPtr imagePtr;

      au_drawCharucoDiamond3(dictionary.cvPtr, ids.cvPtr, squareLength, markerLength, out imagePtr, exception.cvPtr);
      image = new Mat(imagePtr);

      exception.Check();
    }

    public static void DrawDetectedCornersCharuco(Mat image, VectorPoint2f charucoCorners, VectorInt charucoIds, Scalar cornerColor)
    {
      Exception exception = new Exception();
      au_drawDetectedCornersCharuco1(image.cvPtr, charucoCorners.cvPtr, charucoIds.cvPtr, cornerColor.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedCornersCharuco(Mat image, VectorPoint2f charucoCorners, VectorInt charucoIds)
    {
      Exception exception = new Exception();
      au_drawDetectedCornersCharuco2(image.cvPtr, charucoCorners.cvPtr, charucoIds.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedCornersCharuco(Mat image, VectorPoint2f charucoCorners)
    {
      Exception exception = new Exception();
      au_drawDetectedCornersCharuco3(image.cvPtr, charucoCorners.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedDiamonds(Dictionary dictionary, VectorVec4i diamondCorners, VectorInt diamondIds, Scalar borderColor)
    {
      Exception exception = new Exception();
      au_drawDetectedDiamonds1(dictionary.cvPtr, diamondCorners.cvPtr, diamondIds.cvPtr, borderColor.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedDiamonds(Dictionary dictionary, VectorVec4i diamondCorners, VectorInt diamondIds)
    {
      Exception exception = new Exception();
      au_drawDetectedDiamonds2(dictionary.cvPtr, diamondCorners.cvPtr, diamondIds.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedDiamonds(Dictionary dictionary, VectorVec4i diamondCorners)
    {
      Exception exception = new Exception();
      au_drawDetectedDiamonds3(dictionary.cvPtr, diamondCorners.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedMarkers(Mat image, VectorVectorPoint2f corners, VectorInt ids, Color borderColor)
    {
      Exception exception = new Exception();
      Scalar borderColorScalar = borderColor;
      au_drawDetectedMarkers1(image.cvPtr, corners.cvPtr, ids.cvPtr, borderColorScalar.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedMarkers(Mat image, VectorVectorPoint2f corners, VectorInt ids)
    {
      Exception exception = new Exception();
      au_drawDetectedMarkers2(image.cvPtr, corners.cvPtr, ids.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedMarkers(Mat image, VectorVectorPoint2f corners)
    {
      Exception exception = new Exception();
      au_drawDetectedMarkers3(image.cvPtr, corners.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void DrawDetectedMarkers(Mat image, VectorVectorPoint2f corners, Color borderColor)
    {
      Exception exception = new Exception();
      Scalar borderColorScalar = borderColor;
      au_drawDetectedMarkers4(image.cvPtr, corners.cvPtr, borderColorScalar.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static int EstimatePoseBoard(VectorVectorPoint2f corners, VectorInt ids, GridBoard board, VectorMat cameraMatrix, VectorMat distCoeffs,
      out Mat rvec, out Mat tvec)
    {
      Exception exception = new Exception();
      System.IntPtr rvecPtr, tvecPtr;

      int valid = au_estimatePoseBoard(corners.cvPtr, ids.cvPtr, board.cvPtr, cameraMatrix.cvPtr, distCoeffs.cvPtr, out rvecPtr, out tvecPtr, 
        exception.cvPtr);
      rvec = new Mat(rvecPtr);
      tvec = new Mat(tvecPtr);

      exception.Check();
      return valid;
    }

    public static bool EstimatePoseCharucoBoard(VectorPoint2f charucoCorners, VectorInt charucoIds, CharucoBoard board, Mat cameraMatrix,
      Mat distCoeffs, out VectorMat rvecs, out VectorMat tvecs)
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;
      
      bool valid = au_estimatePoseCharucoBoard(charucoCorners.cvPtr, charucoIds.cvPtr, board.cvPtr, cameraMatrix.cvPtr, distCoeffs.cvPtr, 
        out rvecsPtr, out tvecsPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
      return valid;
    }

    public static void EstimatePoseSingleMarkers(VectorVectorPoint2f corners, float markerLength, VectorMat cameraMatrix, VectorMat distCoeffs,
      out VectorMat rvecs, out VectorMat tvecs)
    {
      Exception exception = new Exception();
      System.IntPtr rvecsPtr, tvecsPtr;
      
      au_estimatePoseSingleMarkers(corners.cvPtr, markerLength, cameraMatrix.cvPtr, distCoeffs.cvPtr, out rvecsPtr, out tvecsPtr, exception.cvPtr);
      rvecs = new VectorMat(rvecsPtr);
      tvecs = new VectorMat(tvecsPtr);

      exception.Check();
    }

    public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize)
    {
      Exception exception = new Exception();
      System.IntPtr dictionaryPtr = au_generateCustomDictionary1(nMarkers, markerSize, exception.cvPtr);
      exception.Check();
      return new Dictionary(dictionaryPtr);
    }

    public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize, Dictionary baseDictionary)
    {
      Exception exception = new Exception();
      System.IntPtr dictionaryPtr = au_generateCustomDictionary2(nMarkers, markerSize, baseDictionary.cvPtr, exception.cvPtr);
      exception.Check();
      return new Dictionary(dictionaryPtr);
    }

    public static Dictionary GetPredefinedDictionary(PREDEFINED_DICTIONARY_NAME name)
    {
      return new Dictionary(au_getPredefinedDictionary(name));
    }

    public static int InterpolateCornersCharuco(VectorVectorPoint2f markerCorners, VectorInt markerIds, Mat image, CharucoBoard board,
      out VectorPoint2f charucoCorners, out VectorInt charucoIds, Mat cameraMatrix, Mat distCoeffs)
    {
      Exception exception = new Exception();
      System.IntPtr charucoCornersPtr, charucoIdsPtr;

      int interpolateCorners = au_interpolateCornersCharuco1(markerCorners.cvPtr, markerIds.cvPtr, image.cvPtr, board.cvPtr,
        out charucoCornersPtr, out charucoIdsPtr, cameraMatrix.cvPtr, distCoeffs.cvPtr, exception.cvPtr);
      charucoCorners = new VectorPoint2f(charucoCornersPtr);
      charucoIds = new VectorInt(charucoIdsPtr);
      exception.Check();

      return interpolateCorners;
    }

    public static int InterpolateCornersCharuco(VectorVectorPoint2f markerCorners, VectorInt markerIds, Mat image, CharucoBoard board,
      out VectorPoint2f charucoCorners, out VectorInt charucoIds, Mat cameraMatrix)
    {
      Exception exception = new Exception();
      System.IntPtr charucoCornersPtr, charucoIdsPtr;

      int interpolateCorners = au_interpolateCornersCharuco2(markerCorners.cvPtr, markerIds.cvPtr, image.cvPtr, board.cvPtr,
        out charucoCornersPtr, out charucoIdsPtr, cameraMatrix.cvPtr, exception.cvPtr);
      charucoCorners = new VectorPoint2f(charucoCornersPtr);
      charucoIds = new VectorInt(charucoIdsPtr);
      exception.Check();

      return interpolateCorners;
    }

    public static int InterpolateCornersCharuco(VectorVectorPoint2f markerCorners, VectorInt markerIds, Mat image, CharucoBoard board,
      out VectorPoint2f charucoCorners, out VectorInt charucoIds)
    {
      Exception exception = new Exception();
      System.IntPtr charucoCornersPtr, charucoIdsPtr;

      int interpolateCorners = au_interpolateCornersCharuco3(markerCorners.cvPtr, markerIds.cvPtr, image.cvPtr, board.cvPtr,
        out charucoCornersPtr, out charucoIdsPtr, exception.cvPtr);
      charucoCorners = new VectorPoint2f(charucoCornersPtr);
      charucoIds = new VectorInt(charucoIdsPtr);
      exception.Check();

      return interpolateCorners;
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds, 
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate, 
      bool checkAllOrders, VectorInt recoveredIdxs, DetectorParameters parameters) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers1(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cvPtr, parameters.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
      bool checkAllOrders, VectorInt recoveredIdxs) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers2(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
      bool checkAllOrders) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers3(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, checkAllOrders, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers4(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix, Mat distCoeffs, float minRepDistance) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers5(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix, Mat distCoeffs) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers6(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners, Mat cameraMatrix) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers7(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, GridBoard board, VectorVectorPoint2f detectedCorners, VectorInt detectedIds,
      VectorVectorPoint2f rejectedCorners) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers8(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, exception.cvPtr);
      exception.Check();
    }
  }
}