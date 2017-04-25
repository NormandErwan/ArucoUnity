using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorVec4i : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVec4i_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVec4i_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVec4i_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVec4i_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVec4i_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVec4i_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorVec4i() : base(au_std_vectorVec4i_new())
        {
        }

        public VectorVec4i(System.IntPtr vectorVec4iPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorVec4iPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorVec4i_delete(CppPtr);
        }

        // Methods

        public Cv.Vec4i At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          Cv.Vec4i element = new Cv.Vec4i(au_std_vectorVec4i_at(CppPtr, pos, exception.CppPtr), Utility.DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Cv.Vec4i[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVec4i_data(CppPtr);
          uint size = Size();

          Cv.Vec4i[] data = new Cv.Vec4i[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Cv.Vec4i(dataPtr[i], Utility.DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Cv.Vec4i value)
        {
          au_std_vectorVec4i_push_back(CppPtr, value.CppPtr);
        }

        public uint Size()
        {
          return au_std_vectorVec4i_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}