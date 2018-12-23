using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Size : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Size_new1();

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Size_new2(int width, int height);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Size_delete(IntPtr size);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Size_area(IntPtr size);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Size_getHeight(IntPtr size);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Size_setHeight(IntPtr size, int height);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Size_getWidth(IntPtr size);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Size_setWidth(IntPtr size, int width);

      // Constructors & destructor

      public Size() : base(au_cv_Size_new1())
      {
      }

      public Size(int width, int height) : base(au_cv_Size_new2(width, height))
      {
      }

      public Size(IntPtr sizePtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(sizePtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_cv_Size_delete(CppPtr);
      }

      // Properties

      public int Height
      {
        get { return au_cv_Size_getHeight(CppPtr); }
        set { au_cv_Size_setHeight(CppPtr, value); }
      }

      public int Width
      {
        get { return au_cv_Size_getWidth(CppPtr); }
        set { au_cv_Size_setWidth(CppPtr, value); }
      }

      // Methods

      public int Area()
      {
        return au_cv_Size_area(CppPtr);
      }
    }
  }
}