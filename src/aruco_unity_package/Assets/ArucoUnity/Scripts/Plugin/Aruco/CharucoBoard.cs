using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      public class CharucoBoard : Board
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern void au_CharucoBoard_delete(System.IntPtr charucoBoard);

        [DllImport("ArucoUnity")]
        static extern void au_CharucoBoard_draw(System.IntPtr charucoBoard, System.IntPtr outSize, out System.IntPtr img, int marginSize,
          int borderBits, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_CharucoBoard_getChessboardSize(System.IntPtr charucoBoard);

        [DllImport("ArucoUnity")]
        static extern float au_CharucoBoard_getMarkerLength(System.IntPtr charucoBoard);

        [DllImport("ArucoUnity")]
        static extern float au_CharucoBoard_getSquareLength(System.IntPtr charucoBoard);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_CharucoBoard_create(int squaresX, int squaresY, float squareLength, float markerLength,
          System.IntPtr dictionary, System.IntPtr exception);

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

        // Constructors & destructor

        internal CharucoBoard(System.IntPtr charucoBoardPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
            : base(charucoBoardPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_CharucoBoard_delete(CppPtr);
        }

        // Properties

        public Std.VectorPoint3f ChessboardCorners
        {
          get { return new Std.VectorPoint3f(au_CharucoBoard_getChessboardCorners(CppPtr), Utility.DeleteResponsibility.False); }
          set { au_CharucoBoard_setChessboardCorners(CppPtr, value.CppPtr); }
        }

        public Std.VectorVectorInt MarkerCorners
        {
          get { return new Std.VectorVectorInt(au_CharucoBoard_getNearestMarkerCorners(CppPtr), Utility.DeleteResponsibility.False); }
          set { au_CharucoBoard_setNearestMarkerCorners(CppPtr, value.CppPtr); }
        }

        public Std.VectorVectorInt MarkerIdx
        {
          get { return new Std.VectorVectorInt(au_CharucoBoard_getNearestMarkerIdx(CppPtr), Utility.DeleteResponsibility.False); }
          set { au_CharucoBoard_setNearestMarkerIdx(CppPtr, value.CppPtr); }
        }

        // Static methods

        static public CharucoBoard Create(int squaresX, int squaresY, float squareLength, float markerLength, Dictionary dictionary)
        {
          Cv.Exception exception = new Cv.Exception();
          System.IntPtr charucoBoardPtr = au_CharucoBoard_create(squaresX, squaresY, squareLength, markerLength, dictionary.CppPtr,
            exception.CppPtr);
          exception.Check();
          return new CharucoBoard(charucoBoardPtr);
        }

        // Methods

        public void Draw(Cv.Size outSize, out Cv.Mat img, int marginSize = 0, int borderBits = 1)
        {
          Cv.Exception exception = new Cv.Exception();
          System.IntPtr imgPtr;

          au_CharucoBoard_draw(CppPtr, outSize.CppPtr, out imgPtr, marginSize, borderBits, exception.CppPtr);
          img = new Cv.Mat(imgPtr);

          exception.Check();
        }

        public Cv.Size GetChessboardSize()
        {
          return new Cv.Size(au_CharucoBoard_getChessboardSize(CppPtr));
        }

        public float GetMarkerLength()
        {
          return au_CharucoBoard_getMarkerLength(CppPtr);
        }

        public float GetSquareLength()
        {
          return au_CharucoBoard_getSquareLength(CppPtr);
        }
      }
    }
  }
  
  /// \} aruco_unity_package
}