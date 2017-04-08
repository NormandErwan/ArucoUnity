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
      public static partial class Core
      {
        public class Vec3d : Utility.HandleCppPtr
        {
          // Native functions

          [DllImport("ArucoUnity")]
          static extern System.IntPtr au_cv_Vec3d_new();

          [DllImport("ArucoUnity")]
          static extern void au_cv_Vec3d_delete(System.IntPtr vec3d);

          [DllImport("ArucoUnity")]
          static extern double au_cv_Vec3d_get(System.IntPtr vec3d, int i, System.IntPtr exception);

          [DllImport("ArucoUnity")]
          static extern void au_cv_Vec3d_set(System.IntPtr vec3d, int i, double value, System.IntPtr exception);

          // Constructors & destructor

          public Vec3d() : base(au_cv_Vec3d_new())
          {
          }

          public Vec3d(System.IntPtr vec3dPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
            : base(vec3dPtr, deleteResponsibility)
          {
          }

          protected override void DeleteCvPtr()
          {
            au_cv_Vec3d_delete(cppPtr);
          }

          // Methods

          public double Get(int i)
          {
            Exception exception = new Exception();
            double value = au_cv_Vec3d_get(cppPtr, i, exception.cppPtr);
            exception.Check();
            return value;
          }

          public void Set(int i, double value)
          {
            Exception exception = new Exception();
            au_cv_Vec3d_set(cppPtr, i, value, exception.cppPtr);
            exception.Check();
          }

          public Vector3 ToPosition()
          {
            return new Vector3((float)Get(0), -(float)Get(1), (float)Get(2));
          }

          public Quaternion ToRotation()
          {
            // Based on: http://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToQuaternion/
            Vector3 angleAxis = new Vector3(-(float)Get(0), (float)Get(1), -(float)Get(2));
            Vector3 angleAxisNormalized = angleAxis.normalized;
            float angle = angleAxis.magnitude;
            float s = Mathf.Sin(angle / 2);

            Quaternion rotation;
            rotation.x = angleAxisNormalized.x * s;
            rotation.y = angleAxisNormalized.y * s;
            rotation.z = angleAxisNormalized.z * s;
            rotation.w = Mathf.Cos(angle / 2);

            rotation *= Quaternion.Euler(0f, 90f, 90f); // Re-orient to put the y axis up, and the the x and z axis each side of the first corner of the marker

            return rotation;
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}