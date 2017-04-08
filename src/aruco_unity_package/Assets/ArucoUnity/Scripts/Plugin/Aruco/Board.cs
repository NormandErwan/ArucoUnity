using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      public abstract class Board : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Board_getDictionary(System.IntPtr board);

        [DllImport("ArucoUnity")]
        static extern void au_Board_setDictionary(System.IntPtr board, System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Board_getIds(System.IntPtr board);

        [DllImport("ArucoUnity")]
        static extern void au_Board_setIds(System.IntPtr board, System.IntPtr ids);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Board_getObjPoints(System.IntPtr board);

        [DllImport("ArucoUnity")]
        static extern void au_Board_setObjPoints(System.IntPtr board, System.IntPtr objPoints);

        // Constructors

        internal Board(System.IntPtr boardPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
            : base(boardPtr, deleteResponsibility)
        {
        }

        // Properties

        public Dictionary Dictionary
        {
          get { return new Dictionary(au_Board_getDictionary(CppPtr), Utility.DeleteResponsibility.False); }
          set { au_Board_setDictionary(CppPtr, value.CppPtr); }
        }

        public Std.VectorInt Ids
        {
          get { return new Std.VectorInt(au_Board_getIds(CppPtr), Utility.DeleteResponsibility.False); }
          set { au_Board_setIds(CppPtr, value.CppPtr); }
        }

        public Std.VectorVectorPoint3f ObjPoints
        {
          get { return new Std.VectorVectorPoint3f(au_Board_getObjPoints(CppPtr), Utility.DeleteResponsibility.False); }
          set { au_Board_setObjPoints(CppPtr, value.CppPtr); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}