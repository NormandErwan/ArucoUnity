using System.Runtime.InteropServices;
using System.Text;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      public class Exception : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Exception_new();

        [DllImport("ArucoUnity")]
        static extern void au_cv_Exception_delete(System.IntPtr exception);

        // Functions
        [DllImport("ArucoUnity")]
        static extern void au_cv_Exception_what(System.IntPtr exception, StringBuilder sb);

        // Variables
        [DllImport("ArucoUnity")]
        static extern int au_cv_Exception_getCode(System.IntPtr exception);

        StringBuilder sb;

        public Exception() : base(au_cv_Exception_new())
        {
          sb = new StringBuilder(1024);
        }

        protected override void DeleteCvPtr()
        {
          //au_cv_Exception_delete(cvPtr); // TODO: fix the crash that occur when calling this function
        }

        public string What()
        {
          au_cv_Exception_what(cppPtr, sb);
          return sb.ToString();
        }

        public void Check()
        {
          if (code != 0)
          {
            throw new System.Exception(What());
          }
        }

        public int code
        {
          get { return au_cv_Exception_getCode(cppPtr); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}