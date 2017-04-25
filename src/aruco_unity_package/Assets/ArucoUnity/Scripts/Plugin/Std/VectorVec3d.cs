using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorVec3d : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVec3d_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVec3d_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVec3d_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVec3d_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVec3d_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVec3d_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorVec3d() : base(au_std_vectorVec3d_new())
        {
        }

        public VectorVec3d(System.IntPtr vectorVec3dPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorVec3dPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorVec3d_delete(CppPtr);
        }

        // Methods

        public Cv.Vec3d At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          Cv.Vec3d element = new Cv.Vec3d(au_std_vectorVec3d_at(CppPtr, pos, exception.CppPtr), Utility.DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Cv.Vec3d[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVec3d_data(CppPtr);
          uint size = Size();

          Cv.Vec3d[] data = new Cv.Vec3d[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Cv.Vec3d(dataPtr[i], Utility.DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Cv.Vec3d value)
        {
          au_std_vectorVec3d_push_back(CppPtr, value.CppPtr);
        }

        public uint Size()
        {
          return au_std_vectorVec3d_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}