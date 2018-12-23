using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorPoint3f : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorPoint3f_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorPoint3f_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorPoint3f_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe IntPtr* au_std_vectorPoint3f_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorPoint3f_push_back(IntPtr vector, IntPtr value);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorPoint3f_size(IntPtr vector);

      // Constructors & destructor

      public VectorPoint3f() : base(au_std_vectorPoint3f_new())
      {
      }

      public VectorPoint3f(IntPtr vectorPoint3fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vectorPoint3fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_std_vectorPoint3f_delete(CppPtr);
      }

      // Methods

      public Cv.Point3f At(uint pos)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Point3f element = new Cv.Point3f(au_std_vectorPoint3f_at(CppPtr, pos, exception.CppPtr), DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe Cv.Point3f[] Data()
      {
        IntPtr* dataPtr = au_std_vectorPoint3f_data(CppPtr);
        uint size = Size();

        Cv.Point3f[] data = new Cv.Point3f[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new Cv.Point3f(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(Cv.Point3f value)
      {
        au_std_vectorPoint3f_push_back(CppPtr, value.CppPtr);
      }

      public uint Size()
      {
        return au_std_vectorPoint3f_size(CppPtr);
      }
    }
  }
}