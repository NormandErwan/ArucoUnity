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

      // Member Functions
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

      public Vector3 ToPosition()
      {
        Vector3 position = new Vector3();
        position.x = -(float)Get(0);
        position.y = -(float)Get(1);
        position.z =  (float)Get(2);
        return position;
      }

      public Quaternion ToRotation()
      {
        Mat rotationMatrix;
        Calib3d.Rodrigues(this, out rotationMatrix);

        Quaternion rotation = new Quaternion();
        rotation.x = -(float)rotationMatrix.AtDouble(0, 0);
        rotation.y = -(float)rotationMatrix.AtDouble(1, 0);
        rotation.z =  (float)rotationMatrix.AtDouble(2, 0);
        rotation.w =  (float)rotationMatrix.AtDouble(3, 0);

        return rotation;
      }
    }
  }
}