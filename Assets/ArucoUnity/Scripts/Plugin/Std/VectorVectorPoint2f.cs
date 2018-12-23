using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorVectorPoint2f : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVectorPoint2f_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVectorPoint2f_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVectorPoint2f_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe IntPtr* au_std_vectorVectorPoint2f_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVectorPoint2f_push_back(IntPtr vector, IntPtr value);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorVectorPoint2f_size(IntPtr vector);

      // Constructors & destructor

      public VectorVectorPoint2f() : base(au_std_vectorVectorPoint2f_new())
      {
      }

      public VectorVectorPoint2f(IntPtr vectorVectorPoint2fPtr,
        DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vectorVectorPoint2fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_std_vectorVectorPoint2f_delete(CppPtr);
      }

      // Methods

      public VectorPoint2f At(uint pos)
      {
        Cv.Exception exception = new Cv.Exception();
        VectorPoint2f element = new VectorPoint2f(au_std_vectorVectorPoint2f_at(CppPtr, pos, exception.CppPtr),
          DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe VectorPoint2f[] Data()
      {
        IntPtr* dataPtr = au_std_vectorVectorPoint2f_data(CppPtr);
        uint size = Size();

        VectorPoint2f[] data = new VectorPoint2f[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new VectorPoint2f(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(VectorPoint2f value)
      {
        au_std_vectorVectorPoint2f_push_back(CppPtr, value.CppPtr);
      }

      public uint Size()
      {
        return au_std_vectorVectorPoint2f_size(CppPtr);
      }
    }
  }
}