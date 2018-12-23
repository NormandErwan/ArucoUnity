using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorDouble : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorDouble_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorDouble_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_std_vectorDouble_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe double* au_std_vectorDouble_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorDouble_push_back(IntPtr vector, double value);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorDouble_reserve(IntPtr vector, uint new_cap, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorDouble_size(IntPtr vector);

      // Constructors & destructor

      public VectorDouble() : base(au_std_vectorDouble_new())
      {
      }

      public VectorDouble(IntPtr vectorDoublePtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
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