using System.Runtime.InteropServices;
using System.Text;

public partial class ArucoUnity
{
  public partial class Exception : HandleCvPtr
  {
    // Constructor & Destructor
    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_Exception_New();

    [DllImport("ArucoUnity")]
    static extern void au_Exception_Delete(System.IntPtr exception);

    // Functions
    [DllImport("ArucoUnity")]
    static extern void au_Exception_What(System.IntPtr exception, StringBuilder sb);

    // Variables
    [DllImport("ArucoUnity")]
    static extern int au_Exception_GetCode(System.IntPtr exception);

    StringBuilder sb;

    public Exception() : base(au_Exception_New())
    {
      sb = new StringBuilder(1024);
    }

    ~Exception()
    {
      //au_Exception_Delete(cvPtr); // TODO: fix the crash that occur when calling this function
    }

    public string What()
    {
      au_Exception_What(cvPtr, sb);
      return sb.ToString();
    }

    public int code
    {
      get { return au_Exception_GetCode(cvPtr); }
    }
  }
}