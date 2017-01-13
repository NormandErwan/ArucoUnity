using System.Runtime.InteropServices;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace cv
    {
      public class Vec3d : HandleCppPtr
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

        public double Get(int i)
        {
          Exception exception = new Exception();
          double value = au_Vec3d_get(cvPtr, i, exception.cvPtr);
          exception.Check();
          return value;
        }

        public void Set(int i, double value)
        {
          Exception exception = new Exception();
          au_Vec3d_set(cvPtr, i, value, exception.cvPtr);
          exception.Check();
        }

        public Vector3 ToPosition()
        {
          // Convert from OpenCV's right-handed coordinates system to Unity's left-handed coordinates system
          Vector3 position = new Vector3();
          position.x = -(float)Get(0);
          position.y = (float)Get(1);
          position.z = (float)Get(2);
          return position;
        }

        public Quaternion ToRotation()
        {
          // Based on: http://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToQuaternion/
          Vector3 angleAxis = new Vector3((float)Get(0), -(float)Get(1), -(float)Get(2));
          Vector3 angleAxisNormalized = angleAxis.normalized;
          float angle = angleAxis.magnitude;
          float s = Mathf.Sin(angle / 2);

          Quaternion rotation;
          rotation.x = angleAxisNormalized.x * s;
          rotation.y = angleAxisNormalized.y * s;
          rotation.z = angleAxisNormalized.z * s;
          rotation.w = Mathf.Cos(angle / 2);

          rotation = rotation * Quaternion.Euler(0f, 90f, 90f); // Re-orient to put the y axis up, and the the x and z axis each side of the first corner of the marker

          return rotation;
        }
      }
    }
  }

  /// \} aruco_unity_package
}