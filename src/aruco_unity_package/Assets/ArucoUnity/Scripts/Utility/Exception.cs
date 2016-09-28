using System.Runtime.InteropServices;
using System.Text;

public partial class ArucoUnity
{
  public partial class Exception : HandleCvPtr
  {
    // Constructor & Destructor
    [DllImport("ArucoUnity")]
    static extern System.IntPtr auNewException();

    [DllImport("ArucoUnity")]
    static extern void auDeleteException(System.IntPtr exception);

    // Functions
    [DllImport("ArucoUnity")]
    static extern void auExceptionWhat(System.IntPtr exception, StringBuilder sb);

    // Variables
    [DllImport("ArucoUnity")]
    static extern int auGetExceptionCode(System.IntPtr exception);

    StringBuilder sb;

    public Exception() : base(auNewException())
    {
      sb = new StringBuilder(1024);
    }

    ~Exception()
    {
      //auDeleteException(cvPtr); // TODO: fix the crash that occur when calling this function
    }

    public string What()
    {
      auExceptionWhat(cvPtr, sb);
      return sb.ToString();
    }

    public int code
    {
      get { return auGetExceptionCode(cvPtr); }
    }
  }
}