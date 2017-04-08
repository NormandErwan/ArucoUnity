using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Utility
    {
      public enum DeleteResponsibility
      {
        True,
        False
      }

      public abstract class HandleCppPtr
      {
        // Constructors & destructor

        public HandleCppPtr(DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        {
          CppPtr = System.IntPtr.Zero;
          DeleteResponsibility = deleteResponsibility;
        }

        public HandleCppPtr(System.IntPtr cppPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        {
          CppPtr = cppPtr;
          DeleteResponsibility = deleteResponsibility;
        }

        ~HandleCppPtr()
        {
          if (DeleteResponsibility == DeleteResponsibility.True)
          {
            DeleteCppPtr();
          }
        }

        // Properties

        public DeleteResponsibility DeleteResponsibility { get; set; }

        public System.IntPtr CppPtr
        {
          get { return handle.Handle; }
          set { handle = new HandleRef(this, value); }
        }

        // Variables

        HandleRef handle;

        // Methods

        protected abstract void DeleteCppPtr();
      }
    }
  }

  /// \} aruco_unity_package
}