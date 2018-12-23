using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Vec3d : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Vec3d_new(double v0, double v1, double v2);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Vec3d_delete(IntPtr vec3d);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_cv_Vec3d_get(IntPtr vec3d, int i, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Vec3d_set(IntPtr vec3d, int i, double value, IntPtr exception);

      // Constructors & destructor

      public Vec3d(double v0 = 0, double v1 = 0, double v2 = 0) : base(au_cv_Vec3d_new(v0, v1, v2))
      {
      }

      public Vec3d(IntPtr vec3dPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vec3dPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_cv_Vec3d_delete(CppPtr);
      }

      // Methods

      public double Get(int i)
      {
        Exception exception = new Exception();
        double value = au_cv_Vec3d_get(CppPtr, i, exception.CppPtr);
        exception.Check();
        return value;
      }

      public void Set(int i, double value)
      {
        Exception exception = new Exception();
        au_cv_Vec3d_set(CppPtr, i, value, exception.CppPtr);
        exception.Check();
      }

      /// <summary>
      /// Converts the Vec3d as an OpenCV's translation vector to a Vector3.
      /// </summary>
      /// <returns>The converted vector.</returns>
      public Vector3 ToPosition()
      {
        return new Vector3((float)Get(0), -(float)Get(1), (float)Get(2)); // Convert the vector from left-handed to right-handed
      }

      /// <summary>
      /// Converts the Vec3d as an OpenCV's rotation vector to a Quaternion.
      /// Based on: http://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToQuaternion/
      /// </summary>
      /// <returns>The converted quaternion.</returns>
      public Quaternion ToRotation()
      {
        // Convert the vector from left-handed to right-handed
        Vector3 angleAxis = new Vector3((float)Get(0), -(float)Get(1), (float)Get(2));
        Vector3 unitVector = angleAxis.normalized;
        float angle = -angleAxis.magnitude;

        // Convert from axis-angle to quaternion
        Quaternion rotation;
        float sinHalfAngle = Mathf.Sin(angle / 2);
        rotation.x = unitVector.x * sinHalfAngle;
        rotation.y = unitVector.y * sinHalfAngle;
        rotation.z = unitVector.z * sinHalfAngle;
        rotation.w = Mathf.Cos(angle / 2);

        // Re-orient to put the y axis up
        rotation *= Quaternion.Euler(90f, 0f, 0f);

        return rotation;
      }
    }
  }
}