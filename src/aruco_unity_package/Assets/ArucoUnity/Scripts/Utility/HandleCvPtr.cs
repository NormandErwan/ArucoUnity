using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public abstract partial class HandleCvPtr
    {
      HandleRef handle;

      public HandleCvPtr()
      {
        cvPtr = System.IntPtr.Zero;
      }

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
}