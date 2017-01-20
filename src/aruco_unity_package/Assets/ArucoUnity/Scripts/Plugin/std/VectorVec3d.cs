using System.Runtime.InteropServices;
using ArucoUnity.Plugin.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace std
    {
      public class VectorVec3d : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVec3d_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVec3d_delete(System.IntPtr vector);

        // Functions
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVec3d_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVec3d_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVec3d_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVec3d_size(System.IntPtr vector);

        public VectorVec3d() : base(au_std_vectorVec3d_new())
        {
        }

        public VectorVec3d(System.IntPtr vectorVec3dPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorVec3dPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorVec3d_delete(cvPtr);
        }

        public Vec3d At(uint pos)
        {
          Exception exception = new Exception();
          Vec3d element = new Vec3d(au_std_vectorVec3d_at(cvPtr, pos, exception.cvPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Vec3d[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVec3d_data(cvPtr);
          uint size = Size();

          Vec3d[] data = new Vec3d[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Vec3d(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Vec3d value)
        {
          au_std_vectorVec3d_push_back(cvPtr, value.cvPtr);
        }

        public uint Size()
        {
          return au_std_vectorVec3d_size(cvPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}