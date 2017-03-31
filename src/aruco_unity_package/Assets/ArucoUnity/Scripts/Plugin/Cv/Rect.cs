using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public static partial class Core
      {
        public class Rect : Utility.HandleCppPtr
        {
          // Constructor & Destructor
          [DllImport("ArucoUnity")]
          static extern System.IntPtr au_cv_Rect_new1();

          [DllImport("ArucoUnity")]
          static extern System.IntPtr au_cv_Rect_new2(int x, int y, int width, int height);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Rect_delete(System.IntPtr Rect);

          // Variables
          [DllImport("ArucoUnity")]
          static extern int au_cv_Rect_getX(System.IntPtr Rect);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Rect_setX(System.IntPtr Rect, int x);

          [DllImport("ArucoUnity")]
          static extern int au_cv_Rect_getY(System.IntPtr Rect);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Rect_setY(System.IntPtr Rect, int y);

          [DllImport("ArucoUnity")]
          static extern int au_cv_Rect_getWidth(System.IntPtr Rect);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Rect_setWidth(System.IntPtr Rect, int width);

          [DllImport("ArucoUnity")]
          static extern int au_cv_Rect_getHeight(System.IntPtr Rect);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Rect_setHeight(System.IntPtr Rect, int height);

          public Rect() : base(au_cv_Rect_new1())
          {
          }

          public Rect(int x, int y, int width, int height) : base(au_cv_Rect_new2(x, y, width, height))
          {
          }

          public Rect(System.IntPtr RectPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
            : base(RectPtr, deleteResponsibility)
          {
          }

          protected override void DeleteCvPtr()
          {
            au_cv_Rect_delete(cppPtr);
          }

          public int x
          {
            get { return au_cv_Rect_getX(cppPtr); }
            set { au_cv_Rect_setX(cppPtr, value); }
          }

          public int y
          {
            get { return au_cv_Rect_getY(cppPtr); }
            set { au_cv_Rect_setY(cppPtr, value); }
          }

          public int width
          {
            get { return au_cv_Rect_getWidth(cppPtr); }
            set { au_cv_Rect_setWidth(cppPtr, value); }
          }

          public int height
          {
            get { return au_cv_Rect_getHeight(cppPtr); }
            set { au_cv_Rect_setHeight(cppPtr, value); }
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}