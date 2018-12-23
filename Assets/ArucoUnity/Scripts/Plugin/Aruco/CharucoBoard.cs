using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
    public static partial class Aruco
    {
        public class CharucoBoard : Board
        {
            // Native functions

            [DllImport("ArucoUnityPlugin")]
            static extern void au_CharucoBoard_delete(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_CharucoBoard_draw(IntPtr charucoBoard, IntPtr outSize, out IntPtr img, int marginSize,
                int borderBits, IntPtr exception);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_CharucoBoard_getChessboardSize(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern float au_CharucoBoard_getMarkerLength(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern float au_CharucoBoard_getSquareLength(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_CharucoBoard_create(int squaresX, int squaresY, float squareLength, float markerLength,
                IntPtr dictionary, IntPtr exception);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_CharucoBoard_getChessboardCorners(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_CharucoBoard_setChessboardCorners(IntPtr charucoBoard, IntPtr chessboardCorners);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_CharucoBoard_getNearestMarkerCorners(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_CharucoBoard_setNearestMarkerCorners(IntPtr charucoBoard, IntPtr nearestMarkerCorners);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_CharucoBoard_getNearestMarkerIdx(IntPtr charucoBoard);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_CharucoBoard_setNearestMarkerIdx(IntPtr charucoBoard, IntPtr nearestMarkerIdx);

            // Constructors & destructor

            internal CharucoBoard(IntPtr charucoBoardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
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
                get { return new Std.VectorPoint3f(au_CharucoBoard_getChessboardCorners(CppPtr), DeleteResponsibility.False); }
                set { au_CharucoBoard_setChessboardCorners(CppPtr, value.CppPtr); }
            }

            public Std.VectorVectorInt MarkerCorners
            {
                get { return new Std.VectorVectorInt(au_CharucoBoard_getNearestMarkerCorners(CppPtr), DeleteResponsibility.False); }
                set { au_CharucoBoard_setNearestMarkerCorners(CppPtr, value.CppPtr); }
            }

            public Std.VectorVectorInt MarkerIdx
            {
                get { return new Std.VectorVectorInt(au_CharucoBoard_getNearestMarkerIdx(CppPtr), DeleteResponsibility.False); }
                set { au_CharucoBoard_setNearestMarkerIdx(CppPtr, value.CppPtr); }
            }

            // Static methods

            static public CharucoBoard Create(int squaresX, int squaresY, float squareLength, float markerLength, Dictionary dictionary)
            {
                Cv.Exception exception = new Cv.Exception();
                IntPtr charucoBoardPtr = au_CharucoBoard_create(squaresX, squaresY, squareLength, markerLength, dictionary.CppPtr,
                    exception.CppPtr);
                exception.Check();
                return new CharucoBoard(charucoBoardPtr);
            }

            // Methods

            public void Draw(Cv.Size outSize, out Cv.Mat img, int marginSize = 0, int borderBits = 1)
            {
                Cv.Exception exception = new Cv.Exception();
                IntPtr imgPtr;

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