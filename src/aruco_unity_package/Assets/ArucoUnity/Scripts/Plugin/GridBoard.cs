using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{


  namespace Plugin
  {
    public class GridBoard : Board
    {
      [DllImport("ArucoUnity")]
      static extern void au_GridBoard_delete(System.IntPtr gridBoard);

      [DllImport("ArucoUnity")]
      static extern void au_GridBoard_draw1(System.IntPtr gridBoard, System.IntPtr outSize, out System.IntPtr img, int marginSize, int borderBits,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_GridBoard_draw2(System.IntPtr gridBoard, System.IntPtr outSize, out System.IntPtr img, int marginSize,
        System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_GridBoard_draw3(System.IntPtr gridBoard, System.IntPtr outSize, out System.IntPtr img, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_GridBoard_getGridSize(System.IntPtr gridBoard);

      [DllImport("ArucoUnity")]
      static extern float au_GridBoard_getMarkerLength(System.IntPtr gridBoard);

      [DllImport("ArucoUnity")]
      static extern float au_GridBoard_getMarkerSeparation(System.IntPtr gridBoard);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_GridBoard_create1(int markersX, int markersY, float markerLength, float markerSeparation, System.IntPtr dictionary,
        int firstMarker, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_GridBoard_create2(int markersX, int markersY, float markerLength, float markerSeparation, System.IntPtr dictionary,
        System.IntPtr exception);

      protected override void DeleteCvPtr()
      {
        au_GridBoard_delete(cppPtr);
      }

      internal GridBoard(System.IntPtr gridBoardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(gridBoardPtr, deleteResponsibility)
      {
      }

      public void Draw(Cv.Size outSize, out Cv.Mat img, int marginSize, int borderBits)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr imgPtr;

        au_GridBoard_draw1(cppPtr, outSize.cppPtr, out imgPtr, marginSize, borderBits, exception.cppPtr);
        img = new Cv.Mat(imgPtr);

        exception.Check();
      }

      public void Draw(Cv.Size outSize, out Cv.Mat img, int marginSize)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr imgPtr;

        au_GridBoard_draw2(cppPtr, outSize.cppPtr, out imgPtr, marginSize, exception.cppPtr);
        img = new Cv.Mat(imgPtr);

        exception.Check();
      }

      public void Draw(Cv.Size outSize, out Cv.Mat img)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr imgPtr;

        au_GridBoard_draw3(cppPtr, outSize.cppPtr, out imgPtr, exception.cppPtr);
        img = new Cv.Mat(imgPtr);

        exception.Check();
      }

      static public GridBoard Create(int markersX, int markersY, float markerLength, float markerSeparation, Dictionary dictionary, int firstMarker)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr gridBoardPtr = au_GridBoard_create1(markersX, markersY, markerLength, markerSeparation, dictionary.cppPtr, firstMarker,
          exception.cppPtr);
        exception.Check();
        return new GridBoard(gridBoardPtr);
      }

      static public GridBoard Create(int markersX, int markersY, float markerLength, float markerSeparation, Dictionary dictionary)
      {
        Cv.Exception exception = new Cv.Exception();
        System.IntPtr gridBoardPtr = au_GridBoard_create2(markersX, markersY, markerLength, markerSeparation, dictionary.cppPtr, exception.cppPtr);
        exception.Check();
        return new GridBoard(gridBoardPtr);
      }

      public Cv.Size GetGridSize()
      {
        return new Cv.Size(au_GridBoard_getGridSize(cppPtr));
      }

      public float GetMarkerLength()
      {
        return au_GridBoard_getMarkerLength(cppPtr);
      }

      public float GetMarkerSeparation()
      {
        return au_GridBoard_getMarkerSeparation(cppPtr);
      }
    }
  }

  /// \} aruco_unity_package
}