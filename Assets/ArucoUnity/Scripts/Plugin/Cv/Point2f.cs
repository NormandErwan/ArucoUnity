using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class Point2f : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_Point2f_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point2f_delete(IntPtr point2f);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_cv_Point2f_getX(IntPtr point2f);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point2f_setX(IntPtr point2f, float x);

      [DllImport("ArucoUnityPlugin")]
      static extern float au_cv_Point2f_getY(IntPtr point2f);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_Point2f_setY(IntPtr point2f, float y);

      // Constructors & destructor

      public Point2f() : base(au_cv_Point2f_new())
      {
      }

      public Point2f(IntPtr point2fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
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