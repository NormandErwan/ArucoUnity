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
      public class Scalar : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Scalar_new(double v0, double v1, double v2);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Scalar_delete(System.IntPtr scalar);

        public Scalar(double v0, double v1, double v2) : base(au_cv_Scalar_new(v0, v1, v2))
        {
        }

        public static implicit operator Scalar(Color color)
        {
          return new Scalar(color.r, color.g, color.b);
        }

        protected override void DeleteCvPtr()
        {
          au_cv_Scalar_delete(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}