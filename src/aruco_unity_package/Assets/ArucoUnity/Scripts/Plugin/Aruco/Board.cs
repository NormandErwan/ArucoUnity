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

        internal Board(System.IntPtr boardPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
            : base(boardPtr, deleteResponsibility)
        {
        }

        public Dictionary dictionary
        {
          get { return new Dictionary(au_Board_getDictionary(cppPtr), DeleteResponsibility.False); }
          set { au_Board_setDictionary(cppPtr, value.cppPtr); }
        }

        public Std.VectorInt ids
        {
          get { return new Std.VectorInt(au_Board_getIds(cppPtr), DeleteResponsibility.False); }
          set { au_Board_setIds(cppPtr, value.cppPtr); }
        }

        public Std.VectorVectorPoint3f objPoints
        {
          get { return new Std.VectorVectorPoint3f(au_Board_getObjPoints(cppPtr), DeleteResponsibility.False); }
          set { au_Board_setObjPoints(cppPtr, value.cppPtr); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}