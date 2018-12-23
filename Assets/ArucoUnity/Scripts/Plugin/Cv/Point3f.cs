using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Point3f : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Point3f_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point3f_delete(IntPtr point3f);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_cv_Point3f_getX(IntPtr point3f);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point3f_setX(IntPtr point3f, float x);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_cv_Point3f_getY(IntPtr point3f);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point3f_setY(IntPtr point3f, float y);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_cv_Point3f_getZ(IntPtr point3f);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point3f_setZ(IntPtr point3f, float z);

      // Constructors & destructor

      public Point3f() : base(au_cv_Point3f_new())
      {
      }

      public Point3f(IntPtr point3fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(point3fPtr, deleteResponsibility)
      {
      }

      public static implicit operator Vector3(Point3f point3f)
      {
        return new Vector3(point3f.X, point3f.Y, point3f.Z);
      }

      protected override void DeleteCppPtr()
      {
        au_cv_Point3f_delete(CppPtr);
      }

      // Properties

      public float X
      {
        get { return au_cv_Point3f_getX(CppPtr); }
        set { au_cv_Point3f_setX(CppPtr, value); }
      }

      public float Y
      {
        get { return au_cv_Point3f_getY(CppPtr); }
        set { au_cv_Point3f_setY(CppPtr, value); }
      }

      public float Z
      {
        get { return au_cv_Point3f_getZ(CppPtr); }
        set { au_cv_Point3f_setZ(CppPtr, value); }
      }
    }
  }
}