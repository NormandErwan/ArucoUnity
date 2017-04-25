using System.Runtime.InteropServices;
using System.Text;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public class Exception : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Exception_new();

        [DllImport("ArucoUnity")]
        static extern void au_cv_Exception_delete(System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Exception_what(System.IntPtr exception, StringBuilder sb);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Exception_getCode(System.IntPtr exception);

        // Variables

        private StringBuilder sb;

        // Constructor & Destructor

        public Exception() : base(au_cv_Exception_new())
        {
          sb = new StringBuilder(1024);
        }

        protected override void DeleteCppPtr()
        {
          //au_cv_Exception_delete(cvPtr); // TODO: fix the crash that occur when calling this function
        }

        // Properties

        public int Code
        {
          get { return au_cv_Exception_getCode(CppPtr); }
        }

        // Methods

        public string What()
        {
          au_cv_Exception_what(CppPtr, sb);
          return sb.ToString();
        }

        public void Check()
        {
          if (Code != 0)
          {
            throw new System.Exception(What());
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}