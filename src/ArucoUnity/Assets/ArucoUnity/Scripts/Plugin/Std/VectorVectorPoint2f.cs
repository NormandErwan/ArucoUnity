using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorVectorPoint2f : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorPoint2f_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorPoint2f_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorPoint2f_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVectorPoint2f_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorPoint2f_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVectorPoint2f_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorVectorPoint2f() : base(au_std_vectorVectorPoint2f_new())
        {
        }

        public VectorVectorPoint2f(System.IntPtr vectorVectorPoint2fPtr,
          Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorVectorPoint2fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorVectorPoint2f_delete(CppPtr);
        }

        // Methods

        public VectorPoint2f At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          VectorPoint2f element = new VectorPoint2f(au_std_vectorVectorPoint2f_at(CppPtr, pos, exception.CppPtr),
            Utility.DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe VectorPoint2f[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVectorPoint2f_data(CppPtr);
          uint size = Size();

          VectorPoint2f[] data = new VectorPoint2f[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new VectorPoint2f(dataPtr[i], Utility.DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(VectorPoint2f value)
        {
          au_std_vectorVectorPoint2f_push_back(CppPtr, value.CppPtr);
        }

        public uint Size()
        {
          return au_std_vectorVectorPoint2f_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}