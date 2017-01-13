using System.Runtime.InteropServices;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace std
    {
      public class VectorVec4i : HandleCvPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_vectorVec4i_new();

        [DllImport("ArucoUnity")]
        static extern void au_vectorVec4i_delete(System.IntPtr vector);

        // Functions
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_vectorVec4i_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_vectorVec4i_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_vectorVec4i_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_vectorVec4i_size(System.IntPtr vector);

        public VectorVec4i() : base(au_vectorVec4i_new())
        {
        }

        public VectorVec4i(System.IntPtr vectorVec4iPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorVec4iPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_vectorVec4i_delete(cvPtr);
        }

        public Vec4i At(uint pos)
        {
          Exception exception = new Exception();
          Vec4i element = new Vec4i(au_vectorVec4i_at(cvPtr, pos, exception.cvPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Vec4i[] Data()
        {
          System.IntPtr* dataPtr = au_vectorVec4i_data(cvPtr);
          uint size = Size();

          Vec4i[] data = new Vec4i[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Vec4i(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Vec4i value)
        {
          au_vectorVec4i_push_back(cvPtr, value.cvPtr);
        }

        public uint Size()
        {
          return au_vectorVec4i_size(cvPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}