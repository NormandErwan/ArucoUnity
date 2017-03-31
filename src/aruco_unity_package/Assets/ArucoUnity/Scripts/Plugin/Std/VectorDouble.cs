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
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorDouble_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorDouble_delete(System.IntPtr vector);

        // Functions
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

        public VectorDouble() : base(au_std_vectorDouble_new())
        {
        }

        public VectorDouble(System.IntPtr vectorDoublePtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorDoublePtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorDouble_delete(cppPtr);
        }

        public double At(uint pos)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          double element = au_std_vectorDouble_at(cppPtr, pos, exception.cppPtr);
          exception.Check();
          return element;
        }

        public unsafe double[] Data()
        {
          double* dataPtr = au_std_vectorDouble_data(cppPtr);
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
          au_std_vectorDouble_push_back(cppPtr, value);
        }

        public void Reserve(uint newCap)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          au_std_vectorDouble_reserve(cppPtr, newCap, exception.cppPtr);
          exception.Check();
        }

        public uint Size()
        {
          return au_std_vectorDouble_size(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}