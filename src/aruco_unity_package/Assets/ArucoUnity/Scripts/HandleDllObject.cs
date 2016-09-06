using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public abstract partial class HandleDllObject
  {
    HandleRef _handle;

    public HandleDllObject(System.IntPtr ptr)
    {
      _handle = new HandleRef(this, ptr);
    }

    public System.IntPtr ptr
    {
      get { return _handle.Handle; }
    }
  }
}