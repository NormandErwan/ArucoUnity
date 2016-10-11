using System.Runtime.InteropServices;
using ArucoUnity.Utility;

namespace ArucoUnity
{
  public class GridBoard : Board
  {
    [DllImport("ArucoUnity")]
    static extern void au_GridBoard_delete(System.IntPtr gridBoard);

    [DllImport("ArucoUnity")]
    static extern void au_GridBoard_draw1(System.IntPtr gridBoard, System.IntPtr outSize, System.IntPtr img, int marginSize, int borderBits,
      System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_GridBoard_draw2(System.IntPtr gridBoard, System.IntPtr outSize, System.IntPtr img, int marginSize,
      System.IntPtr exception);

    [DllImport("ArucoUnity")]
    static extern void au_GridBoard_draw3(System.IntPtr gridBoard, System.IntPtr outSize, System.IntPtr img, System.IntPtr exception);

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
      au_GridBoard_delete(cvPtr);
    }

    internal GridBoard(System.IntPtr gridBoardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(gridBoardPtr, deleteResponsibility)
    {
    }

    public void Draw(Size outSize, ref Mat img, int marginSize, int borderBits)
    {
      Exception exception = new Exception();
      au_GridBoard_draw1(cvPtr, outSize.cvPtr, img.cvPtr, marginSize, borderBits, exception.cvPtr);
      exception.Check();
    }

    public void Draw(Size outSize, ref Mat img, int marginSize)
    {
      Exception exception = new Exception();
      au_GridBoard_draw2(cvPtr, outSize.cvPtr, img.cvPtr, marginSize, exception.cvPtr);
      exception.Check();
    }

    public void Draw(Size outSize, ref Mat img)
    {
      Exception exception = new Exception();
      au_GridBoard_draw3(cvPtr, outSize.cvPtr, img.cvPtr, exception.cvPtr);
      exception.Check();
    }

    static public GridBoard Create(int markersX, int markersY, float markerLength, float markerSeparation, Dictionary dictionary, int firstMarker)
    {
      Exception exception = new Exception();
      GridBoard gridBoard = new GridBoard(au_GridBoard_create1(markersX, markersY, markerLength, markerSeparation, dictionary.cvPtr, firstMarker, 
        exception.cvPtr));
      exception.Check();
      return gridBoard;
    }

    static public GridBoard Create(int markersX, int markersY, float markerLength, float markerSeparation, Dictionary dictionary)
    {
      Exception exception = new Exception();
      GridBoard gridBoard = new GridBoard(au_GridBoard_create2(markersX, markersY, markerLength, markerSeparation, dictionary.cvPtr, exception.cvPtr));
      exception.Check();
      return gridBoard;
    }

    public Size GetGridSize()
    {
      return new Size(au_GridBoard_getGridSize(cvPtr));
    }

    public float GetMarkerLength()
    {
      return au_GridBoard_getMarkerLength(cvPtr);
    }

    public float GetMarkerSeparation()
    {
      return au_GridBoard_getMarkerSeparation(cvPtr);
    }
  }
}