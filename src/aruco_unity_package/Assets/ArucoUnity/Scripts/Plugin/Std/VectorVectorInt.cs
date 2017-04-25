using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorVectorInt : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorInt_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorInt_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorVectorInt_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorVectorInt_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorVectorInt_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorVectorInt_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorVectorInt() : base(au_std_vectorVectorInt_new())
        {
        }

        public VectorVectorInt(System.IntPtr vectorVectorIntPtr,
          Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorVectorIntPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorVectorInt_delete(CppPtr);
        }

        // Methods

        public VectorInt At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          VectorInt element = new VectorInt(au_std_vectorVectorInt_at(CppPtr, pos, exception.CppPtr), Utility.DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe VectorInt[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorVectorInt_data(CppPtr);
          uint size = Size();

          VectorInt[] data = new VectorInt[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new VectorInt(dataPtr[i], Utility.DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(VectorInt value)
        {
          au_std_vectorVectorInt_push_back(CppPtr, value.CppPtr);
        }

        public uint Size()
        {
          return au_std_vectorVectorInt_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}