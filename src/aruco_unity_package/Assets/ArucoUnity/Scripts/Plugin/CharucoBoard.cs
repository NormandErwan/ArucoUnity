using System.Runtime.InteropServices;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public class CharucoBoard : Board
    {
      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_delete(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_draw1(System.IntPtr charucoBoard, System.IntPtr outSize, out System.IntPtr img, int marginSize,
        int borderBits, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_draw2(System.IntPtr charucoBoard, System.IntPtr outSize, out System.IntPtr img, int marginSize,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_draw3(System.IntPtr charucoBoard, System.IntPtr outSize, out System.IntPtr img, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_CharucoBoard_getChessboardSize(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern float au_CharucoBoard_getMarkerLength(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern float au_CharucoBoard_getSquareLength(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_CharucoBoard_create(int squaresX, int squaresY, float squareLength, float markerLength, System.IntPtr dictionary,
      System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_CharucoBoard_getChessboardCorners(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_setChessboardCorners(System.IntPtr charucoBoard, System.IntPtr chessboardCorners);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_CharucoBoard_getNearestMarkerCorners(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_setNearestMarkerCorners(System.IntPtr charucoBoard, System.IntPtr nearestMarkerCorners);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_CharucoBoard_getNearestMarkerIdx(System.IntPtr charucoBoard);

      [DllImport("ArucoUnity")]
      static extern void au_CharucoBoard_setNearestMarkerIdx(System.IntPtr charucoBoard, System.IntPtr nearestMarkerIdx);

      protected override void DeleteCvPtr()
      {
        au_CharucoBoard_delete(cppPtr);
      }

      internal CharucoBoard(System.IntPtr charucoBoardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(charucoBoardPtr, deleteResponsibility)
      {
      }

      public void Draw(Size outSize, out Mat img, int marginSize, int borderBits)
      {
        Exception exception = new Exception();
        System.IntPtr imgPtr;

        au_CharucoBoard_draw1(cppPtr, outSize.cppPtr, out imgPtr, marginSize, borderBits, exception.cppPtr);
        img = new Mat(imgPtr);

        exception.Check();
      }

      public void Draw(Size outSize, out Mat img, int marginSize)
      {
        Exception exception = new Exception();
        System.IntPtr imgPtr;

        au_CharucoBoard_draw2(cppPtr, outSize.cppPtr, out imgPtr, marginSize, exception.cppPtr);
        img = new Mat(imgPtr);

        exception.Check();
      }

      public void Draw(Size outSize, out Mat img)
      {
        Exception exception = new Exception();
        System.IntPtr imgPtr;

        au_CharucoBoard_draw3(cppPtr, outSize.cppPtr, out imgPtr, exception.cppPtr);
        img = new Mat(imgPtr);

        exception.Check();
      }

      static public CharucoBoard Create(int squaresX, int squaresY, float squareLength, float markerLength, Dictionary dictionary)
      {
        Exception exception = new Exception();
        System.IntPtr charucoBoardPtr = au_CharucoBoard_create(squaresX, squaresY, squareLength, markerLength, dictionary.cppPtr,
          exception.cppPtr);
        exception.Check();
        return new CharucoBoard(charucoBoardPtr);
      }

      public Size GetChessboardSize()
      {
        return new Size(au_CharucoBoard_getChessboardSize(cppPtr));
      }

      public float GetMarkerLength()
      {
        return au_CharucoBoard_getMarkerLength(cppPtr);
      }

      public float GetSquareLength()
      {
        return au_CharucoBoard_getSquareLength(cppPtr);
      }

      public VectorPoint3f chessboardCorners
      {
        get { return new VectorPoint3f(au_CharucoBoard_getChessboardCorners(cppPtr), DeleteResponsibility.False); }
        set { au_CharucoBoard_setChessboardCorners(cppPtr, value.cppPtr); }
      }

      public VectorVectorInt markerCorners
      {
        get { return new VectorVectorInt(au_CharucoBoard_getNearestMarkerCorners(cppPtr), DeleteResponsibility.False); }
        set { au_CharucoBoard_setNearestMarkerCorners(cppPtr, value.cppPtr); }
      }

      public VectorVectorInt markerIdx
      {
        get { return new VectorVectorInt(au_CharucoBoard_getNearestMarkerIdx(cppPtr), DeleteResponsibility.False); }
        set { au_CharucoBoard_setNearestMarkerIdx(cppPtr, value.cppPtr); }
      }
    }
  }
  
  /// \} aruco_unity_package
}