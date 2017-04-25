using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorVectorPoint3f : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorPoint3f_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorPoint3f_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorPoint3f_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVectorPoint3f_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorPoint3f_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVectorPoint3f_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorVectorPoint3f() : base(au_std_vectorVectorPoint3f_new())
        {
        }

        public VectorVectorPoint3f(System.IntPtr vectorVectorPoint3fPtr,
          Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorVectorPoint3fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorVectorPoint3f_delete(CppPtr);
        }

        // Methods

        public VectorPoint3f At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          VectorPoint3f element = new VectorPoint3f(au_std_vectorVectorPoint3f_at(CppPtr, pos, exception.CppPtr),
            Utility.DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe VectorPoint3f[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVectorPoint3f_data(CppPtr);
          uint size = Size();

          VectorPoint3f[] data = new VectorPoint3f[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new VectorPoint3f(dataPtr[i], Utility.DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(VectorPoint3f value)
        {
          au_std_vectorVectorPoint3f_push_back(CppPtr, value.CppPtr);
        }

        public uint Size()
        {
          return au_std_vectorVectorPoint3f_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}