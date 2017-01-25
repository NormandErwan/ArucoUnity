using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
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
        this.cppPtr = System.IntPtr.Zero;
        this.deleteResponsibility = deleteResponsibility;
      }

      public HandleCppPtr(System.IntPtr cppPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
      {
        this.cppPtr = cppPtr;
        this.deleteResponsibility = deleteResponsibility;
      }

      ~HandleCppPtr()
      {
        if (deleteResponsibility == DeleteResponsibility.True)
        {
          DeleteCvPtr();
        }
      }

      public System.IntPtr cppPtr
      {
        get { return handle.Handle; }
        set { handle = new HandleRef(this, value); }
      }

      protected abstract void DeleteCvPtr();
    }
  }

  /// \} aruco_unity_package
}