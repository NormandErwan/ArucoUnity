using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Aruco
  {
    public class GridBoard : Board
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern void au_GridBoard_delete(IntPtr gridBoard);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_GridBoard_draw(IntPtr gridBoard, IntPtr outSize, out IntPtr img, int marginSize, int borderBits,
        IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_GridBoard_getGridSize(IntPtr gridBoard);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_GridBoard_getMarkerLength(IntPtr gridBoard);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_GridBoard_getMarkerSeparation(IntPtr gridBoard);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_GridBoard_create(int markersX, int markersY, float markerLength, float markerSeparation,
        IntPtr dictionary, int firstMarker, IntPtr exception);

      // Constructors & destructor

      internal GridBoard(IntPtr gridBoardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(gridBoardPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_GridBoard_delete(CppPtr);
      }

      // Static methods

      static public GridBoard Create(int markersX, int markersY, float markerLength, float markerSeparation, Dictionary dictionary,
        int firstMarker = 0)
      {
        Cv.Exception exception = new Cv.Exception();
        IntPtr gridBoardPtr = au_GridBoard_create(markersX, markersY, markerLength, markerSeparation, dictionary.CppPtr, firstMarker,
          exception.CppPtr);
        exception.Check();
        return new GridBoard(gridBoardPtr);
      }

      // Methods

      public void Draw(Cv.Size outSize, out Cv.Mat img, int marginSize = 0, int borderBits = 1)
      {
        Cv.Exception exception = new Cv.Exception();
        IntPtr imgPtr;

        au_GridBoard_draw(CppPtr, outSize.CppPtr, out imgPtr, marginSize, borderBits, exception.CppPtr);
        img = new Cv.Mat(imgPtr);

        exception.Check();
      }

      public Cv.Size GetGridSize()
      {
        return new Cv.Size(au_GridBoard_getGridSize(CppPtr));
      }

      public float GetMarkerLength()
      {
        return au_GridBoard_getMarkerLength(CppPtr);
      }

      public float GetMarkerSeparation()
      {
        return au_GridBoard_getMarkerSeparation(CppPtr);
      }
    }
  }
}