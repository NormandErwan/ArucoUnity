using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public enum DeleteResponsibility
    {
      True,
      False
    }

    public abstract partial class HandleCvPtr
    {
      public DeleteResponsibility deleteResponsibility;
      
      HandleRef handle;

      public HandleCvPtr(DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
      {
        this.cvPtr = System.IntPtr.Zero;
        this.deleteResponsibility = deleteResponsibility;
      }

      public HandleCvPtr(System.IntPtr cvPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
      {
        this.cvPtr = cvPtr;
        this.deleteResponsibility = deleteResponsibility;
      }

      ~HandleCvPtr()
      {
        if (deleteResponsibility == DeleteResponsibility.True)
        {
          DeleteCvPtr();
        }
      }

      public System.IntPtr cvPtr
      {
        get { return handle.Handle; }
        set { handle = new HandleRef(this, value); }
      }

      protected abstract void DeleteCvPtr();
    }
  }
}