using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorDouble : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorDouble_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorDouble_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern int au_std_vectorDouble_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe double* au_std_vectorDouble_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorDouble_push_back(System.IntPtr vector, double value);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorDouble_reserve(System.IntPtr vector, uint new_cap, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorDouble_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorDouble() : base(au_std_vectorDouble_new())
        {
        }

        public VectorDouble(System.IntPtr vectorDoublePtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(vectorDoublePtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_std_vectorDouble_delete(CppPtr);
        }

        // Methods

        public double At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          double element = au_std_vectorDouble_at(CppPtr, pos, exception.CppPtr);
          exception.Check();
          return element;
        }

        public unsafe double[] Data()
        {
          double* dataPtr = au_std_vectorDouble_data(CppPtr);
          uint size = Size();

          double[] data = new double[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = dataPtr[i];
          }

          return data;
        }

        public void PushBack(double value)
        {
          au_std_vectorDouble_push_back(CppPtr, value);
        }

        public void Reserve(uint newCap)
        {
          Cv.Exception exception = new Cv.Exception();
          au_std_vectorDouble_reserve(CppPtr, newCap, exception.CppPtr);
          exception.Check();
        }

        public uint Size()
        {
          return au_std_vectorDouble_size(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}