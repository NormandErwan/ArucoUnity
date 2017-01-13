using System.Runtime.InteropServices;
using System.Text;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace cv
    {
      public class Exception : HandleCvPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Exception_new();

        [DllImport("ArucoUnity")]
        static extern void au_Exception_delete(System.IntPtr exception);

        // Functions
        [DllImport("ArucoUnity")]
        static extern void au_Exception_what(System.IntPtr exception, StringBuilder sb);

        // Variables
        [DllImport("ArucoUnity")]
        static extern int au_Exception_getCode(System.IntPtr exception);

        StringBuilder sb;

        public Exception() : base(au_Exception_new())
        {
          sb = new StringBuilder(1024);
        }

        protected override void DeleteCvPtr()
        {
          //au_Exception_delete(cvPtr); // TODO: fix the crash that occur when calling this function
        }

        public string What()
        {
          au_Exception_what(cvPtr, sb);
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
          get { return au_Exception_getCode(cvPtr); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}