using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Vec4i : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Vec4i_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Vec4i_delete(IntPtr vec4i);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Vec4i_get(IntPtr vec4i, int i, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Vec4i_set(IntPtr vec4i, int i, int value, IntPtr exception);

      // Constructors & destructor

      public Vec4i() : base(au_cv_Vec4i_new())
      {
      }

      public Vec4i(IntPtr vec4iPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vec4iPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_cv_Vec4i_delete(CppPtr);
      }

      // Methods

      public int Get(int i)
      {
        Exception exception = new Exception();
        int value = au_cv_Vec4i_get(CppPtr, i, exception.CppPtr);
        exception.Check();
        return value;
      }

      public void Set(int i, int value)
      {
        Exception exception = new Exception();
        au_cv_Vec4i_set(CppPtr, i, value, exception.CppPtr);
        exception.Check();
      }
    }
  }
}