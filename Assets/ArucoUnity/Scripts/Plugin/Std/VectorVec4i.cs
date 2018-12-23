using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorVec4i : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVec4i_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVec4i_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVec4i_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe IntPtr* au_std_vectorVec4i_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVec4i_push_back(IntPtr vector, IntPtr value);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorVec4i_size(IntPtr vector);

      // Constructors & destructor

      public VectorVec4i() : base(au_std_vectorVec4i_new())
      {
      }

      public VectorVec4i(IntPtr vectorVec4iPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
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
        Cv.Vec4i element = new Cv.Vec4i(au_std_vectorVec4i_at(CppPtr, pos, exception.CppPtr), DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe Cv.Vec4i[] Data()
      {
        IntPtr* dataPtr = au_std_vectorVec4i_data(CppPtr);
        uint size = Size();

        Cv.Vec4i[] data = new Cv.Vec4i[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new Cv.Vec4i(dataPtr[i], DeleteResponsibility.False);
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