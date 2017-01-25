using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      public class Point2f : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Point2f_new();

        [DllImport("ArucoUnity")]
        static extern void au_cv_Point2f_delete(System.IntPtr point2f);

        // Variables
        [DllImport("ArucoUnity")]
        static extern float au_cv_Point2f_getX(System.IntPtr point2f);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Point2f_setX(System.IntPtr point2f, float x);

        [DllImport("ArucoUnity")]
        static extern float au_cv_Point2f_getY(System.IntPtr point2f);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Point2f_setY(System.IntPtr point2f, float y);

        public Point2f() : base(au_cv_Point2f_new())
        {
        }

        public Point2f(System.IntPtr point2fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(point2fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_cv_Point2f_delete(cppPtr);
        }

        public float x
        {
          get { return au_cv_Point2f_getX(cppPtr); }
          set { au_cv_Point2f_setX(cppPtr, value); }
        }

        public float y
        {
          get { return au_cv_Point2f_getY(cppPtr); }
          set { au_cv_Point2f_setY(cppPtr, value); }
        }

        public static implicit operator Vector2(Point2f point2f)
        {
          return new Vector2(point2f.x, point2f.y);
        }

        public static implicit operator Vector3(Point2f point2f)
        {
          return new Vector3(point2f.x, point2f.y, 0);
        }
      }
    }
  }

  /// \} aruco_unity_package
}