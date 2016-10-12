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

  public class Methods
  {
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
    static extern void au_drawDetectedMarkers1(System.IntPtr image, System.IntPtr corners, System.IntPtr ids, System.IntPtr borderColor, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_drawDetectedMarkers2(System.IntPtr image, System.IntPtr corners, System.IntPtr ids, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_drawDetectedMarkers3(System.IntPtr image, System.IntPtr corners, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_drawDetectedMarkers4(System.IntPtr image, System.IntPtr corners, System.IntPtr borderColor, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_generateCustomDictionary1(int nMarkers, int markerSize, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_generateCustomDictionary2(int nMarkers, int markerSize, System.IntPtr baseDictionary, System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_getPredefinedDictionary(PREDEFINED_DICTIONARY_NAME name);

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

    public static void DetectMarkers(Mat image, Dictionary dictionary, out VectorVectorPoint2f corners,
      out VectorInt ids, DetectorParameters parameters, out VectorVectorPoint2f rejectedImgPoints)
    {
      Exception exception = new Exception();
      System.IntPtr cornersPtr, idsPtr, rejectedPtr;

      au_detectMarkers1(image.cvPtr, dictionary.cvPtr, out cornersPtr, out idsPtr, parameters.cvPtr, out rejectedPtr, exception.cvPtr);
      corners = new VectorVectorPoint2f(cornersPtr);
      ids = new VectorInt(idsPtr);
      rejectedImgPoints = new VectorVectorPoint2f(rejectedPtr);

      exception.Check();
    }

    public static void DetectMarkers(Mat image, Dictionary dictionary, out VectorVectorPoint2f corners,
      out VectorInt ids, DetectorParameters parameters)
    {
      Exception exception = new Exception();
      System.IntPtr cornersPtr, idsPtr;

      au_detectMarkers2(image.cvPtr, dictionary.cvPtr, out cornersPtr, out idsPtr, parameters.cvPtr, exception.cvPtr);
      corners = new VectorVectorPoint2f(cornersPtr);
      ids = new VectorInt(idsPtr);

      exception.Check();
    }

    public static void DetectMarkers(Mat image, Dictionary dictionary, out VectorVectorPoint2f corners,
      out VectorInt ids)
    {
      Exception exception = new Exception();
      System.IntPtr cornersPtr, idsPtr;

      au_detectMarkers3(image.cvPtr, dictionary.cvPtr, out cornersPtr, out idsPtr, exception.cvPtr);
      corners = new VectorVectorPoint2f(cornersPtr);
      ids = new VectorInt(idsPtr);

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

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate, bool checkAllOrders, ref Mat recoveredIdxs, 
      DetectorParameters parameters) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers1(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr, 
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cvPtr, parameters.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate, bool checkAllOrders, ref Mat recoveredIdxs) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers2(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate, bool checkAllOrders) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers3(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, checkAllOrders, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix, Mat distCoeffs, float minRepDistance, float errorCorrectionRate) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers4(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, errorCorrectionRate, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix, Mat distCoeffs, float minRepDistance) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers5(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, minRepDistance, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix, Mat distCoeffs) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers6(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        distCoeffs.cvPtr, exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners,
      Mat cameraMatrix) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers7(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, cameraMatrix.cvPtr,
        exception.cvPtr);
      exception.Check();
    }

    public static void RefineDetectedMarkers(Mat image, Board board, ref Mat detectedCorners, ref Mat detectedIds, ref Mat rejectedCorners) 
    {
      Exception exception = new Exception();
      au_refineDetectedMarkers8(image.cvPtr, board.cvPtr, detectedCorners.cvPtr, detectedIds.cvPtr, rejectedCorners.cvPtr, exception.cvPtr);
      exception.Check();
    }
  }
}