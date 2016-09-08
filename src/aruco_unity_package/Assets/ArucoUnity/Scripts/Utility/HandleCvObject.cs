using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public abstract partial class HandleCvPtr
  {
    HandleRef _handle;

    public HandleCvPtr(System.IntPtr cvPtr)
    {
      _handle = new HandleRef(this, cvPtr);
    }

    public System.IntPtr cvPtr
    {
      get { return _handle.Handle; }
    }
  }
}