using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Exception : HandleCppPtr
    {
      // Constants

      private const int WhatLength = 1024;

      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Exception_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Exception_delete(IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Exception_what(IntPtr exception, StringBuilder sb, int sbLength);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Exception_getCode(IntPtr exception);

      // Variables

      private StringBuilder sb;

      // Constructor & Destructor

      public Exception() : base(au_cv_Exception_new())
      {
        sb = new StringBuilder(WhatLength);
      }

      protected override void DeleteCppPtr()
      {
        //au_cv_Exception_delete(cvPtr); // TODO: fix
      }

      // Properties

      public int Code
      {
        get { return au_cv_Exception_getCode(CppPtr); }
      }

      // Methods

      public string What()
      {
        au_cv_Exception_what(CppPtr, sb, WhatLength);
        return sb.ToString();
      }

      public void Check()
      {
        if (Code != 0)
        {
          throw new System.Exception(What());
        }
      }
    }
  }
}