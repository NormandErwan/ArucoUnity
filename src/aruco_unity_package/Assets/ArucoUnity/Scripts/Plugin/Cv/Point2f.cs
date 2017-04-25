using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public class Point2f : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Point2f_new();

        [DllImport("ArucoUnity")]
        static extern void au_cv_Point2f_delete(System.IntPtr point2f);

        [DllImport("ArucoUnity")]
        static extern float au_cv_Point2f_getX(System.IntPtr point2f);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Point2f_setX(System.IntPtr point2f, float x);

        [DllImport("ArucoUnity")]
        static extern float au_cv_Point2f_getY(System.IntPtr point2f);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Point2f_setY(System.IntPtr point2f, float y);

        // Constructors & destructor

        public Point2f() : base(au_cv_Point2f_new())
        {
        }

        public Point2f(System.IntPtr point2fPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(point2fPtr, deleteResponsibility)
        {
        }

        public static implicit operator Vector2(Point2f point2f)
        {
          return new Vector2(point2f.X, point2f.Y);
        }

        public static implicit operator Vector3(Point2f point2f)
        {
          return new Vector3(point2f.X, point2f.Y, 0);
        }

        protected override void DeleteCppPtr()
        {
          au_cv_Point2f_delete(CppPtr);
        }

        // Properties

        public float X
        {
          get { return au_cv_Point2f_getX(CppPtr); }
          set { au_cv_Point2f_setX(CppPtr, value); }
        }

        public float Y
        {
          get { return au_cv_Point2f_getY(CppPtr); }
          set { au_cv_Point2f_setY(CppPtr, value); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}