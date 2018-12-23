using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
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
        CppPtr = IntPtr.Zero;
        DeleteResponsibility = deleteResponsibility;
      }

      public HandleCppPtr(IntPtr cppPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
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

      public IntPtr CppPtr
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