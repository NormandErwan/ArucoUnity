using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
    public static partial class Aruco
    {
        public abstract class Board : HandleCppPtr
        {
            // Native functions

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_Board_getDictionary(IntPtr board);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_Board_setDictionary(IntPtr board, IntPtr dictionary);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_Board_getIds(IntPtr board);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_Board_setIds(IntPtr board, IntPtr ids);

            [DllImport("ArucoUnityPlugin")]
            static extern IntPtr au_Board_getObjPoints(IntPtr board);

            [DllImport("ArucoUnityPlugin")]
            static extern void au_Board_setObjPoints(IntPtr board, IntPtr objPoints);

            // Constructors

            internal Board(IntPtr boardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
                    : base(boardPtr, deleteResponsibility)
            {
            }

            // Properties

            public Dictionary Dictionary
            {
                get { return new Dictionary(au_Board_getDictionary(CppPtr), DeleteResponsibility.False); }
                set { au_Board_setDictionary(CppPtr, value.CppPtr); }
            }

            public Std.VectorInt Ids
            {
                get { return new Std.VectorInt(au_Board_getIds(CppPtr), DeleteResponsibility.False); }
                set { au_Board_setIds(CppPtr, value.CppPtr); }
            }

            public Std.VectorVectorPoint3f ObjPoints
            {
                get { return new Std.VectorVectorPoint3f(au_Board_getObjPoints(CppPtr), DeleteResponsibility.False); }
                set { au_Board_setObjPoints(CppPtr, value.CppPtr); }
            }
        }
    }
}