using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorVectorPoint3f : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVectorPoint3f_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVectorPoint3f_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVectorPoint3f_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe IntPtr* au_std_vectorVectorPoint3f_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVectorPoint3f_push_back(IntPtr vector, IntPtr value);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorVectorPoint3f_size(IntPtr vector);

      // Constructors & destructor

      public VectorVectorPoint3f() : base(au_std_vectorVectorPoint3f_new())
      {
      }

      public VectorVectorPoint3f(IntPtr vectorVectorPoint3fPtr,
        DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vectorVectorPoint3fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_std_vectorVectorPoint3f_delete(CppPtr);
      }

      // Methods

      public VectorPoint3f At(uint pos)
      {
        Cv.Exception exception = new Cv.Exception();
        VectorPoint3f element = new VectorPoint3f(au_std_vectorVectorPoint3f_at(CppPtr, pos, exception.CppPtr),
          DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe VectorPoint3f[] Data()
      {
        IntPtr* dataPtr = au_std_vectorVectorPoint3f_data(CppPtr);
        uint size = Size();

        VectorPoint3f[] data = new VectorPoint3f[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new VectorPoint3f(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(VectorPoint3f value)
      {
        au_std_vectorVectorPoint3f_push_back(CppPtr, value.CppPtr);
      }

      public uint Size()
      {
        return au_std_vectorVectorPoint3f_size(CppPtr);
      }
    }
  }
}