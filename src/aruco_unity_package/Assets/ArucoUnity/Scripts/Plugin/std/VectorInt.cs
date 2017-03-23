using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace Std
    {
      public class VectorInt : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorInt_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorInt_delete(System.IntPtr vector);

        // Functions
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

        public VectorInt() : base(au_std_vectorInt_new())
        {
        }

        public VectorInt(System.IntPtr vectorIntPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorIntPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorInt_delete(cppPtr);
        }

        public int At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          int element = au_std_vectorInt_at(cppPtr, pos, exception.cppPtr);
          exception.Check();
          return element;
        }

        public unsafe int[] Data()
        {
          int* dataPtr = au_std_vectorInt_data(cppPtr);
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
          au_std_vectorInt_push_back(cppPtr, value);
        }

        public void Reserve(uint newCap)
        {
          Cv.Exception exception = new Cv.Exception();
          au_std_vectorInt_reserve(cppPtr, newCap, exception.cppPtr);
          exception.Check();
        }

        public uint Size()
        {
          return au_std_vectorInt_size(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}