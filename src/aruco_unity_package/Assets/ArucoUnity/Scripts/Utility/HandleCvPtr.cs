using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public enum DeleteResponsibility
    {
      True,
      False
    }

    public abstract class HandleCvPtr // TODO: rename to HanddleCppPtr, and put the enum DeleteResponsibility into the class
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

  /// \} aruco_unity_package
}