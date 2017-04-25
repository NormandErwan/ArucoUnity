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
      public class Scalar : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Scalar_new(double v0, double v1, double v2);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Scalar_delete(System.IntPtr scalar);

        // Constructors & destructor

        public Scalar(double v0, double v1, double v2) : base(au_cv_Scalar_new(v0, v1, v2))
        {
        }

        public static implicit operator Scalar(Color color)
        {
          return new Scalar(color.r, color.g, color.b);
        }

        protected override void DeleteCppPtr()
        {
          au_cv_Scalar_delete(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}