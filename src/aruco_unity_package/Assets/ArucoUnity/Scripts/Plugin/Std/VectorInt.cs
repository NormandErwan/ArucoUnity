using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorInt : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorInt_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorInt_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern int au_std_vectorInt_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe int* au_std_vectorInt_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorInt_push_back(System.IntPtr vector, int value);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorInt_reserve(System.IntPtr vector, uint new_cap, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorInt_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorInt() : base(au_std_vectorInt_new())
        {
        }

        public VectorInt(System.IntPtr vectorIntPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorIntPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorInt_delete(CppPtr);
        }

        // Methods

        public int At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          int element = au_std_vectorInt_at(CppPtr, pos, exception.CppPtr);
          exception.Check();
          return element;
        }

        public unsafe int[] Data()
        {
          int* dataPtr = au_std_vectorInt_data(CppPtr);
          uint size = Size();

          int[] data = new int[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = dataPtr[i];
          }

          return data;
        }

        public void PushBack(int value)
        {
          au_std_vectorInt_push_back(CppPtr, value);
        }

        public void Reserve(uint newCap)
        {
          Cv.Exception exception = new Cv.Exception();
          au_std_vectorInt_reserve(CppPtr, newCap, exception.CppPtr);
          exception.Check();
        }

        public uint Size()
        {
          return au_std_vectorInt_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}