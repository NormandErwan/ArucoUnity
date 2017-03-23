using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace Std
    {
      public class VectorVectorPoint2f : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorPoint2f_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorPoint2f_delete(System.IntPtr vector);

        // Functions
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorPoint2f_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVectorPoint2f_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorPoint2f_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVectorPoint2f_size(System.IntPtr vector);

        public VectorVectorPoint2f() : base(au_std_vectorVectorPoint2f_new())
        {
        }

        public VectorVectorPoint2f(System.IntPtr vectorVectorPoint2fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorVectorPoint2fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorVectorPoint2f_delete(cppPtr);
        }

        public VectorPoint2f At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          VectorPoint2f element = new VectorPoint2f(au_std_vectorVectorPoint2f_at(cppPtr, pos, exception.cppPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe VectorPoint2f[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVectorPoint2f_data(cppPtr);
          uint size = Size();

          VectorPoint2f[] data = new VectorPoint2f[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new VectorPoint2f(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(VectorPoint2f value)
        {
          au_std_vectorVectorPoint2f_push_back(cppPtr, value.cppPtr);
        }

        public uint Size()
        {
          return au_std_vectorVectorPoint2f_size(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}