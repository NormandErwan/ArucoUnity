using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
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

        public Size(System.IntPtr sizePtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
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

  /// \} aruco_unity_package
}