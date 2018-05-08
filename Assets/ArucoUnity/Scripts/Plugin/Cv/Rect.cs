using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Rect : Utility.HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern System.IntPtr au_cv_Rect_new1();

      [DllImport("ArucoUnityPlugin")]
      static extern System.IntPtr au_cv_Rect_new2(int x, int y, int width, int height);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Rect_delete(System.IntPtr Rect);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Rect_getX(System.IntPtr Rect);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Rect_setX(System.IntPtr Rect, int x);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Rect_getY(System.IntPtr Rect);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Rect_setY(System.IntPtr Rect, int y);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Rect_getWidth(System.IntPtr Rect);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Rect_setWidth(System.IntPtr Rect, int width);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_Rect_getHeight(System.IntPtr Rect);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Rect_setHeight(System.IntPtr Rect, int height);

      // Constructors & destructor

      public Rect() : base(au_cv_Rect_new1())
      {
      }

      public Rect(int x, int y, int width, int height) : base(au_cv_Rect_new2(x, y, width, height))
      {
      }

      public Rect(System.IntPtr RectPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
        : base(RectPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_cv_Rect_delete(CppPtr);
      }

      // Properties

      public int X
      {
        get { return au_cv_Rect_getX(CppPtr); }
        set { au_cv_Rect_setX(CppPtr, value); }
      }

      public int Y
      {
        get { return au_cv_Rect_getY(CppPtr); }
        set { au_cv_Rect_setY(CppPtr, value); }
      }

      public int Width
      {
        get { return au_cv_Rect_getWidth(CppPtr); }
        set { au_cv_Rect_setWidth(CppPtr, value); }
      }

      public int Height
      {
        get { return au_cv_Rect_getHeight(CppPtr); }
        set { au_cv_Rect_setHeight(CppPtr, value); }
      }
    }
  }
}