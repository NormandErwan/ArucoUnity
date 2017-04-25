using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      public class GridBoard : Board
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern void au_GridBoard_delete(System.IntPtr gridBoard);

        [DllImport("ArucoUnity")]
        static extern void au_GridBoard_draw(System.IntPtr gridBoard, System.IntPtr outSize, out System.IntPtr img, int marginSize, int borderBits,
          System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_GridBoard_getGridSize(System.IntPtr gridBoard);

        [DllImport("ArucoUnity")]
        static extern float au_GridBoard_getMarkerLength(System.IntPtr gridBoard);

        [DllImport("ArucoUnity")]
        static extern float au_GridBoard_getMarkerSeparation(System.IntPtr gridBoard);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_GridBoard_create(int markersX, int markersY, float markerLength, float markerSeparation,
          System.IntPtr dictionary, int firstMarker, System.IntPtr exception);

        // Constructors & destructor

        internal GridBoard(System.IntPtr gridBoardPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
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
          System.IntPtr gridBoardPtr = au_GridBoard_create(markersX, markersY, markerLength, markerSeparation, dictionary.CppPtr, firstMarker,
            exception.CppPtr);
          exception.Check();
          return new GridBoard(gridBoardPtr);
        }

        // Methods

        public void Draw(Cv.Size outSize, out Cv.Mat img, int marginSize = 0, int borderBits = 1)
        {
          Cv.Exception exception = new Cv.Exception();
          System.IntPtr imgPtr;

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

  /// \} aruco_unity_package
}