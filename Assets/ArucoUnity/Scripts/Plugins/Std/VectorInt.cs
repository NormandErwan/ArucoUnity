using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorInt : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorInt_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorInt_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_std_vectorInt_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe int* au_std_vectorInt_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorInt_push_back(IntPtr vector, int value);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorInt_reserve(IntPtr vector, uint new_cap, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorInt_size(IntPtr vector);

      // Constructors & destructor

      public VectorInt() : base(au_std_vectorInt_new())
      {
      }

      public VectorInt(IntPtr vectorIntPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
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