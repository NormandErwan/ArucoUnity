using System.Runtime.InteropServices;

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
      // Enums

      public enum PredefinedDictionaryName
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

      // Native functions

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraAruco(System.IntPtr corners, System.IntPtr ids, System.IntPtr counter, System.IntPtr board,
      System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
      System.IntPtr criteria, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern double au_calibrateCameraCharuco(System.IntPtr charucoCorners, System.IntPtr charucoIds, System.IntPtr board,
        System.IntPtr imageSize, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, out System.IntPtr rvecs, out System.IntPtr tvecs, int flags,
        System.IntPtr criteria, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectCharucoDiamond(System.IntPtr image, System.IntPtr markerCorners, System.IntPtr markerIds,
        float squareMarkerLengthRate, out System.IntPtr diamondCorners, out System.IntPtr diamondIds, System.IntPtr cameraMatrix,
        System.IntPtr distCoeffs, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_detectMarkers(System.IntPtr image, System.IntPtr dictionary, out System.IntPtr corners, out System.IntPtr ids,
        System.IntPtr parameters, out System.IntPtr rejectedImgPoints, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawAxis(System.IntPtr image, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, System.IntPtr rvec,
        System.IntPtr tvec, float length, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawCharucoDiamond(System.IntPtr dictionary, System.IntPtr ids, int squareLength, int markerLength,
        out System.IntPtr img, int marginSize, int borderBits, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedCornersCharuco(System.IntPtr image, System.IntPtr charucoCorners, System.IntPtr charucoIds,
        System.IntPtr cornerColor, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedDiamonds(System.IntPtr image, System.IntPtr diamondCorners, System.IntPtr diamondIds,
        System.IntPtr borderColor, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_drawDetectedMarkers(System.IntPtr image, System.IntPtr corners, System.IntPtr ids, System.IntPtr borderColor,
        System.IntPtr exception);

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
      static extern System.IntPtr au_generateCustomDictionary(int nMarkers, int markerSize, System.IntPtr baseDictionary, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_getBoardObjectAndImagePoints(System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        out System.IntPtr objPoints, out System.IntPtr imgPoints, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_getPredefinedDictionary(PredefinedDictionaryName name);

      [DllImport("ArucoUnity")]
      static extern int au_interpolateCornersCharuco(System.IntPtr markerCorners, System.IntPtr markerIds, System.IntPtr image, System.IntPtr board,
        out System.IntPtr charucoCorners, out System.IntPtr charucoIds, System.IntPtr cameraMatrix, System.IntPtr distCoeffs,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_refineDetectedMarkers(System.IntPtr image, System.IntPtr board, System.IntPtr detectedCorners, System.IntPtr detectedIds,
        System.IntPtr rejectedCorners, System.IntPtr cameraMatrix, System.IntPtr distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, System.IntPtr recoveredIdxs, System.IntPtr parameters, System.IntPtr exception);

      // Static methods

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs,
        Cv.Calib3d.Calib flags, Cv.Core.TermCriteria criteria)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraAruco(corners.cppPtr, ids.cppPtr, counter.cppPtr, board.cppPtr, imageSize.cppPtr,
          cameraMatrix.cppPtr, distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
        rvecs = new Std.VectorMat(rvecsPtr);
        tvecs = new Std.VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs,
        Cv.Calib3d.Calib flags = 0)
      {
        Cv.Core.TermCriteria criteria = new Cv.Core.TermCriteria(Cv.Core.TermCriteria.Type.Count | Cv.Core.TermCriteria.Type.Eps, 30, Cv.Core.EPSILON);
        return CalibrateCameraAruco(corners, ids, counter, board, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, flags, criteria);
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Std.VectorMat rvecs)
      {
        Std.VectorMat tvecs;
        return CalibrateCameraAruco(corners, ids, counter, board, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs);
      }

      public static double CalibrateCameraAruco(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Std.VectorInt counter, Board board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs)
      {
        Std.VectorMat rvecs;
        return CalibrateCameraAruco(corners, ids, counter, board, imageSize, cameraMatrix, distCoeffs, out rvecs);
      }

      public static double CalibrateCameraCharuco(Std.VectorVectorPoint2f charucoCorners, Std.VectorVectorInt charucoIds, CharucoBoard board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs,
        Cv.Calib3d.Calib flags, Cv.Core.TermCriteria criteria)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        double reProjectionError = au_calibrateCameraCharuco(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, imageSize.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr, (int)flags, criteria.cppPtr, exception.cppPtr);
        rvecs = new Std.VectorMat(rvecsPtr);
        tvecs = new Std.VectorMat(tvecsPtr);

        exception.Check();
        return reProjectionError;
      }

      public static double CalibrateCameraCharuco(Std.VectorVectorPoint2f charucoCorners, Std.VectorVectorInt charucoIds, CharucoBoard board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Std.VectorMat rvecs, out Std.VectorMat tvecs,
        Cv.Calib3d.Calib flags = 0)
      {
        Cv.Core.TermCriteria criteria = new Cv.Core.TermCriteria(Cv.Core.TermCriteria.Type.Count | Cv.Core.TermCriteria.Type.Eps, 30, Cv.Core.EPSILON);
        return CalibrateCameraCharuco(charucoCorners, charucoIds, board, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, flags, criteria);
      }

      public static double CalibrateCameraCharuco(Std.VectorVectorPoint2f charucoCorners, Std.VectorVectorInt charucoIds, CharucoBoard board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Std.VectorMat rvecs)
      {
        Std.VectorMat tvecs;
        return CalibrateCameraCharuco(charucoCorners, charucoIds, board, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs);
      }

      public static double CalibrateCameraCharuco(Std.VectorVectorPoint2f charucoCorners, Std.VectorVectorInt charucoIds, CharucoBoard board,
        Cv.Core.Size imageSize, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs)
      {
        Std.VectorMat rvecs;
        return CalibrateCameraCharuco(charucoCorners, charucoIds, board, imageSize, cameraMatrix, distCoeffs, out rvecs);
      }

      public static void DetectCharucoDiamond(Cv.Core.Mat image, Std.VectorVectorPoint2f markerCorners, Std.VectorInt markerIds,
        float squareMarkerLengthRate, out Std.VectorVectorPoint2f diamondCorners, out Std.VectorVec4i diamondIds, Cv.Core.Mat cameraMatrix,
        Cv.Core.Mat distCoeffs)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr diamondCornersPtr, diamondIdsPtr;

        au_detectCharucoDiamond(image.cppPtr, markerCorners.cppPtr, markerIds.cppPtr, squareMarkerLengthRate, out diamondCornersPtr,
          out diamondIdsPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, exception.cppPtr);
        diamondCorners = new Std.VectorVectorPoint2f(diamondCornersPtr);
        diamondIds = new Std.VectorVec4i(diamondIdsPtr);

        exception.Check();
      }

      public static void DetectCharucoDiamond(Cv.Core.Mat image, Std.VectorVectorPoint2f markerCorners, Std.VectorInt markerIds,
        float squareMarkerLengthRate, out Std.VectorVectorPoint2f diamondCorners, out Std.VectorVec4i diamondIds, Cv.Core.Mat cameraMatrix)
      {
        Cv.Core.Mat distCoeffs = new Cv.Core.Mat();
        DetectCharucoDiamond(image, markerCorners, markerIds, squareMarkerLengthRate, out diamondCorners, out diamondIds, cameraMatrix, distCoeffs);
      }

      public static void DetectCharucoDiamond(Cv.Core.Mat image, Std.VectorVectorPoint2f markerCorners, Std.VectorInt markerIds,
        float squareMarkerLengthRate, out Std.VectorVectorPoint2f diamondCorners, out Std.VectorVec4i diamondIds)
      {
        Cv.Core.Mat cameraMatrix = new Cv.Core.Mat();
        DetectCharucoDiamond(image, markerCorners, markerIds, squareMarkerLengthRate, out diamondCorners, out diamondIds, cameraMatrix);
      }

      public static void DetectMarkers(Cv.Core.Mat image, Dictionary dictionary, out Std.VectorVectorPoint2f corners, out Std.VectorInt ids,
        DetectorParameters parameters, out Std.VectorVectorPoint2f rejectedImgPoints)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr cornersPtr, idsPtr, rejectedPtr;

        au_detectMarkers(image.cppPtr, dictionary.cppPtr, out cornersPtr, out idsPtr, parameters.cppPtr, out rejectedPtr, exception.cppPtr);
        corners = new Std.VectorVectorPoint2f(cornersPtr);
        ids = new Std.VectorInt(idsPtr);
        rejectedImgPoints = new Std.VectorVectorPoint2f(rejectedPtr);

        exception.Check();
      }

      public static void DetectMarkers(Cv.Core.Mat image, Dictionary dictionary, out Std.VectorVectorPoint2f corners, out Std.VectorInt ids,
        DetectorParameters parameters)
      {
        Std.VectorVectorPoint2f rejectedImgPoints;
        DetectMarkers(image, dictionary, out corners, out ids, parameters, out rejectedImgPoints);
      }

      public static void DetectMarkers(Cv.Core.Mat image, Dictionary dictionary, out Std.VectorVectorPoint2f corners, out Std.VectorInt ids)
      {
        DetectorParameters parameters = new DetectorParameters();
        DetectMarkers(image, dictionary, out corners, out ids, parameters);
      }

      public static void DrawAxis(Cv.Core.Mat image, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, Cv.Core.Vec3d rvec, Cv.Core.Vec3d tvec,
        float length)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        au_drawAxis(image.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, rvec.cppPtr, tvec.cppPtr, length, exception.cppPtr);
        exception.Check();
      }
      public static void DrawCharucoDiamond(Dictionary dictionary, Cv.Core.Vec4i ids, int squareLength, int markerLength, out Cv.Core.Mat image,
        int marginSize = 0, int borderBits = 1)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr imagePtr;

        au_drawCharucoDiamond(dictionary.cppPtr, ids.cppPtr, squareLength, markerLength, out imagePtr, marginSize, borderBits, exception.cppPtr);
        image = new Cv.Core.Mat(imagePtr);

        exception.Check();
      }

      public static void DrawDetectedCornersCharuco(Cv.Core.Mat image, Std.VectorPoint2f charucoCorners, Std.VectorInt charucoIds,
        Cv.Core.Scalar cornerColor)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        au_drawDetectedCornersCharuco(image.cppPtr, charucoCorners.cppPtr, charucoIds.cppPtr, cornerColor.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedCornersCharuco(Cv.Core.Mat image, Std.VectorPoint2f charucoCorners, Std.VectorInt charucoIds)
      {
        Cv.Core.Scalar cornerColor = new Cv.Core.Scalar(255, 0, 0);
        DrawDetectedCornersCharuco(image, charucoCorners, charucoIds, cornerColor);
      }

      public static void DrawDetectedCornersCharuco(Cv.Core.Mat image, Std.VectorPoint2f charucoCorners)
      {
        Std.VectorInt charucoIds = new Std.VectorInt();
        DrawDetectedCornersCharuco(image, charucoCorners, charucoIds);
      }

      public static void DrawDetectedDiamonds(Cv.Core.Mat image, Std.VectorVectorPoint2f diamondCorners, Std.VectorVec4i diamondIds,
        Cv.Core.Scalar borderColor)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        au_drawDetectedDiamonds(image.cppPtr, diamondCorners.cppPtr, diamondIds.cppPtr, borderColor.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedDiamonds(Cv.Core.Mat image, Std.VectorVectorPoint2f diamondCorners, Std.VectorVec4i diamondIds)
      {
        Cv.Core.Scalar borderColor = new Cv.Core.Scalar(0, 0, 255);
        DrawDetectedDiamonds(image, diamondCorners, diamondIds, borderColor);
      }

      public static void DrawDetectedDiamonds(Cv.Core.Mat image, Std.VectorVectorPoint2f diamondCorners)
      {
        Std.VectorVec4i diamondIds = new Std.VectorVec4i();
        DrawDetectedDiamonds(image, diamondCorners, diamondIds);
      }

      public static void DrawDetectedMarkers(Cv.Core.Mat image, Std.VectorVectorPoint2f corners, Std.VectorInt ids, Cv.Core.Scalar borderColor)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        au_drawDetectedMarkers(image.cppPtr, corners.cppPtr, ids.cppPtr, borderColor.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void DrawDetectedMarkers(Cv.Core.Mat image, Std.VectorVectorPoint2f diamondCorners, Std.VectorInt ids)
      {
        Cv.Core.Scalar borderColor = new Cv.Core.Scalar(0, 255, 0);
        DrawDetectedMarkers(image, diamondCorners, ids, borderColor);
      }

      public static void DrawDetectedMarkers(Cv.Core.Mat image, Std.VectorVectorPoint2f diamondCorners)
      {
        Std.VectorInt ids = new Std.VectorInt();
        DrawDetectedMarkers(image, diamondCorners, ids);
      }

      public static int EstimatePoseBoard(Std.VectorVectorPoint2f corners, Std.VectorInt ids, Board board, Cv.Core.Mat cameraMatrix,
        Cv.Core.Mat distCoeffs, out Cv.Core.Vec3d rvec, out Cv.Core.Vec3d tvec)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr rvecPtr, tvecPtr;

        int valid = au_estimatePoseBoard(corners.cppPtr, ids.cppPtr, board.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, out rvecPtr, out tvecPtr,
          exception.cppPtr);
        rvec = new Cv.Core.Vec3d(rvecPtr);
        tvec = new Cv.Core.Vec3d(tvecPtr);

        exception.Check();
        return valid;
      }

      public static bool EstimatePoseCharucoBoard(Std.VectorPoint2f charucoCorners, Std.VectorInt charucoIds, CharucoBoard board,
        Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, out Cv.Core.Vec3d rvec, out Cv.Core.Vec3d tvec)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr rvecPtr, tvecPtr;

        bool valid = au_estimatePoseCharucoBoard(charucoCorners.cppPtr, charucoIds.cppPtr, board.cppPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr,
          out rvecPtr, out tvecPtr, exception.cppPtr);
        rvec = new Cv.Core.Vec3d(rvecPtr);
        tvec = new Cv.Core.Vec3d(tvecPtr);

        exception.Check();
        return valid;
      }

      public static void EstimatePoseSingleMarkers(Std.VectorVectorPoint2f corners, float markerLength, Cv.Core.Mat cameraMatrix,
         Cv.Core.Mat distCoeffs, out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr rvecsPtr, tvecsPtr;

        au_estimatePoseSingleMarkers(corners.cppPtr, markerLength, cameraMatrix.cppPtr, distCoeffs.cppPtr, out rvecsPtr, out tvecsPtr,
          exception.cppPtr);
        rvecs = new Std.VectorVec3d(rvecsPtr);
        tvecs = new Std.VectorVec3d(tvecsPtr);

        exception.Check();
      }

      public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize, Dictionary baseDictionary)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr dictionaryPtr = au_generateCustomDictionary(nMarkers, markerSize, baseDictionary.cppPtr, exception.cppPtr);
        exception.Check();
        return new Dictionary(dictionaryPtr);
      }

      public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize)
      {
        Dictionary baseDictionary = new Dictionary();
        return GenerateCustomDictionary(nMarkers, markerSize, baseDictionary);
      }

      public static void GetBoardObjectAndImagePoints(Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        out Std.VectorPoint3f objPoints, out Std.VectorPoint2f imgPoints)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr objPointsPtr, imgPointsPtr;

        au_getBoardObjectAndImagePoints(board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, out objPointsPtr, out imgPointsPtr,
          exception.cppPtr);
        objPoints = new Std.VectorPoint3f(objPointsPtr);
        imgPoints = new Std.VectorPoint2f(imgPointsPtr);

        exception.Check();
      }

      public static Dictionary GetPredefinedDictionary(PredefinedDictionaryName name)
      {
        Dictionary dictionary = new Dictionary(au_getPredefinedDictionary(name));
        dictionary.Name = name;
        return dictionary;
      }

      public static int InterpolateCornersCharuco(Std.VectorVectorPoint2f markerCorners, Std.VectorInt markerIds, Cv.Core.Mat image,
        CharucoBoard board, out Std.VectorPoint2f charucoCorners, out Std.VectorInt charucoIds, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        System.IntPtr charucoCornersPtr, charucoIdsPtr;

        int interpolateCorners = au_interpolateCornersCharuco(markerCorners.cppPtr, markerIds.cppPtr, image.cppPtr, board.cppPtr,
          out charucoCornersPtr, out charucoIdsPtr, cameraMatrix.cppPtr, distCoeffs.cppPtr, exception.cppPtr);
        charucoCorners = new Std.VectorPoint2f(charucoCornersPtr);
        charucoIds = new Std.VectorInt(charucoIdsPtr);
        exception.Check();

        return interpolateCorners;
      }

      public static int InterpolateCornersCharuco(Std.VectorVectorPoint2f markerCorners, Std.VectorInt markerIds, Cv.Core.Mat image,
        CharucoBoard board, out Std.VectorPoint2f charucoCorners, out Std.VectorInt charucoIds, Cv.Core.Mat cameraMatrix)
      {
        Cv.Core.Mat distCoeffs = new Cv.Core.Mat();
        return InterpolateCornersCharuco(markerCorners, markerIds, image, board, out charucoCorners, out charucoIds, cameraMatrix, distCoeffs);
      }

      public static int InterpolateCornersCharuco(Std.VectorVectorPoint2f markerCorners, Std.VectorInt markerIds, Cv.Core.Mat image,
        CharucoBoard board, out Std.VectorPoint2f charucoCorners, out Std.VectorInt charucoIds)
      {
        Cv.Core.Mat cameraMatrix = new Cv.Core.Mat();
        return InterpolateCornersCharuco(markerCorners, markerIds, image, board, out charucoCorners, out charucoIds, cameraMatrix);
      }

      public static void RefineDetectedMarkers(Cv.Core.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, Std.VectorInt recoveredIdxs, DetectorParameters parameters)
      {
        Cv.Core.Exception exception = new Cv.Core.Exception();
        au_refineDetectedMarkers(image.cppPtr, board.cppPtr, detectedCorners.cppPtr, detectedIds.cppPtr, rejectedCorners.cppPtr, cameraMatrix.cppPtr,
          distCoeffs.cppPtr, minRepDistance, errorCorrectionRate, checkAllOrders, recoveredIdxs.cppPtr, parameters.cppPtr, exception.cppPtr);
        exception.Check();
      }

      public static void RefineDetectedMarkers(Cv.Core.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, float minRepDistance, float errorCorrectionRate,
        bool checkAllOrders, Std.VectorInt recoveredIdxs)
      {
        DetectorParameters parameters = new DetectorParameters();
        RefineDetectedMarkers(image, board, detectedCorners, detectedIds, rejectedCorners, cameraMatrix, distCoeffs, minRepDistance,
          errorCorrectionRate, checkAllOrders, recoveredIdxs, parameters);
      }

      public static void RefineDetectedMarkers(Cv.Core.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Core.Mat cameraMatrix, Cv.Core.Mat distCoeffs, float minRepDistance = 10f,
        float errorCorrectionRate = 3f, bool checkAllOrders = true)
      {
        Std.VectorInt recoveredIdxs = new Std.VectorInt();
        RefineDetectedMarkers(image, board, detectedCorners, detectedIds, rejectedCorners, cameraMatrix, distCoeffs, minRepDistance,
          errorCorrectionRate, checkAllOrders, recoveredIdxs);
      }

      public static void RefineDetectedMarkers(Cv.Core.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners, Cv.Core.Mat cameraMatrix)
      {
        Cv.Core.Mat distCoeffs = new Cv.Core.Mat();
        RefineDetectedMarkers(image, board, detectedCorners, detectedIds, rejectedCorners, cameraMatrix, distCoeffs);
      }

      public static void RefineDetectedMarkers(Cv.Core.Mat image, Board board, Std.VectorVectorPoint2f detectedCorners, Std.VectorInt detectedIds,
        Std.VectorVectorPoint2f rejectedCorners)
      {
        Cv.Core.Mat cameraMatrix = new Cv.Core.Mat();
        RefineDetectedMarkers(image, board, detectedCorners, detectedIds, rejectedCorners, cameraMatrix);
      }
    }
  }

  /// \} aruco_unity_package
}