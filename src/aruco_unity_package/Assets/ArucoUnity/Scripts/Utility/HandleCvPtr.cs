using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public abstract partial class HandleCvPtr
  {
    HandleRef handle;

    public HandleCvPtr(System.IntPtr cvPtr)
    {
      this.cvPtr = cvPtr;
    }

    public System.IntPtr cvPtr
    {
      get { return handle.Handle; }
      set { handle = new HandleRef(this, value); }
    }
  }
}