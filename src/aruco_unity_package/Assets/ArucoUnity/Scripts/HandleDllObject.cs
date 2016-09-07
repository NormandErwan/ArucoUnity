using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public abstract partial class HandleDllObject
  {
    HandleRef _handle;

    public HandleDllObject(System.IntPtr dllPtr)
    {
      _handle = new HandleRef(this, dllPtr);
    }

    public System.IntPtr dllPtr
    {
      get { return _handle.Handle; }
    }
  }
}