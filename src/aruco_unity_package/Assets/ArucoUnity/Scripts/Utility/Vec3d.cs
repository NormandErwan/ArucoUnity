using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity
{
  namespace Utility
  {
    public class Vec3d : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_Vec3d_new();

      [DllImport("ArucoUnity")]
      static extern void au_Vec3d_delete(System.IntPtr vec3d);

      // Variables
      [DllImport("ArucoUnity")]
      static extern double au_Vec3d_get(System.IntPtr vec3d, int i, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern void au_Vec3d_set(System.IntPtr vec3d, int i, double value, System.IntPtr exception);

      public Vec3d() : base(au_Vec3d_new())
      {
      }

      public Vec3d(System.IntPtr vec3dPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(vec3dPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_Vec3d_delete(cvPtr);
      }

      public double Get(int i) {
        Exception exception = new Exception();
        double value = au_Vec3d_get(cvPtr, i, exception.cvPtr);
        exception.Check();
        return value;
      }

      public void Set(int i, double value) {
        Exception exception = new Exception();
        au_Vec3d_set(cvPtr, i, value, exception.cvPtr);
        exception.Check();
      }

      public static implicit operator Vector3(Vec3d vec3d)
      {
        return new Vector3((float)vec3d.Get(0), (float)vec3d.Get(1), (float)vec3d.Get(2));
      }
    }
  }
}