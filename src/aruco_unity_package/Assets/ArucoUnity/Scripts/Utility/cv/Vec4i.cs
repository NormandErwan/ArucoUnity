using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace cv
    {
      public class Vec4i : HandleCvPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Vec4i_new();

        [DllImport("ArucoUnity")]
        static extern void au_Vec4i_delete(System.IntPtr vec4i);

        // Variables
        [DllImport("ArucoUnity")]
        static extern int au_Vec4i_get(System.IntPtr vec4i, int i, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_Vec4i_set(System.IntPtr vec4i, int i, int value, System.IntPtr exception);

        public Vec4i() : base(au_Vec4i_new())
        {
        }

        public Vec4i(System.IntPtr vec4iPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vec4iPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_Vec4i_delete(cvPtr);
        }

        public int Get(int i)
        {
          Exception exception = new Exception();
          int value = au_Vec4i_get(cvPtr, i, exception.cvPtr);
          exception.Check();
          return value;
        }

        public void Set(int i, int value)
        {
          Exception exception = new Exception();
          au_Vec4i_set(cvPtr, i, value, exception.cvPtr);
          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}