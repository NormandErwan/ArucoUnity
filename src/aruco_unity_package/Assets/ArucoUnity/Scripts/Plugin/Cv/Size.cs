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
        public class Size : Utility.HandleCppPtr
        {
          // Native functions

          [DllImport("ArucoUnity")]
          static extern System.IntPtr au_cv_Size_new();

          [DllImport("ArucoUnity")]
          static extern void au_cv_Size_delete(System.IntPtr size);

          [DllImport("ArucoUnity")]
          static extern int au_cv_Size_area(System.IntPtr size);

          [DllImport("ArucoUnity")]
          static extern int au_cv_Size_getHeight(System.IntPtr size);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Size_setHeight(System.IntPtr size, int height);

          [DllImport("ArucoUnity")]
          static extern int au_cv_Size_getWidth(System.IntPtr size);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Size_setWidth(System.IntPtr size, int width);

          // Constructors & destructor

          public Size() : base(au_cv_Size_new())
          {
          }

          public Size(System.IntPtr sizePtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
            : base(sizePtr, deleteResponsibility)
          {
          }

          protected override void DeleteCvPtr()
          {
            au_cv_Size_delete(cppPtr);
          }

          // Properties

          public int Height
          {
            get { return au_cv_Size_getHeight(cppPtr); }
            set { au_cv_Size_setHeight(cppPtr, value); }
          }

          public int Width
          {
            get { return au_cv_Size_getWidth(cppPtr); }
            set { au_cv_Size_setWidth(cppPtr, value); }
          }

          // Methods

          public int Area()
          {
            return au_cv_Size_area(cppPtr);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}