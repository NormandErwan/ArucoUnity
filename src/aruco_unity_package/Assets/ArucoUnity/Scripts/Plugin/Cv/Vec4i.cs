using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public class Vec4i : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Vec4i_new();

        [DllImport("ArucoUnity")]
        static extern void au_cv_Vec4i_delete(System.IntPtr vec4i);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Vec4i_get(System.IntPtr vec4i, int i, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Vec4i_set(System.IntPtr vec4i, int i, int value, System.IntPtr exception);

        // Constructors & destructor

        public Vec4i() : base(au_cv_Vec4i_new())
        {
        }

        public Vec4i(System.IntPtr vec4iPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vec4iPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_cv_Vec4i_delete(CppPtr);
        }

        // Methods

        public int Get(int i)
        {
          Exception exception = new Exception();
          int value = au_cv_Vec4i_get(CppPtr, i, exception.CppPtr);
          exception.Check();
          return value;
        }

        public void Set(int i, int value)
        {
          Exception exception = new Exception();
          au_cv_Vec4i_set(CppPtr, i, value, exception.CppPtr);
          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}