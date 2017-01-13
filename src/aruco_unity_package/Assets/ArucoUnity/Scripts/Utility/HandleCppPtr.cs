using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public abstract class HandleCppPtr
    {
      public enum DeleteResponsibility
      {
        True,
        False
      }

      public DeleteResponsibility deleteResponsibility;
      
      HandleRef handle;

      public HandleCppPtr(DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
      {
        this.cvPtr = System.IntPtr.Zero;
        this.deleteResponsibility = deleteResponsibility;
      }

      public HandleCppPtr(System.IntPtr cvPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
      {
        this.cvPtr = cvPtr;
        this.deleteResponsibility = deleteResponsibility;
      }

      ~HandleCppPtr()
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