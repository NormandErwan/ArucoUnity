using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorPoint2f : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorPoint2f_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorPoint2f_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorPoint2f_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe IntPtr* au_std_vectorPoint2f_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorPoint2f_push_back(IntPtr vector, IntPtr value);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorPoint2f_size(IntPtr vector);

      // Constructors & destructor

      public VectorPoint2f() : base(au_std_vectorPoint2f_new())
      {
      }

      public VectorPoint2f(IntPtr vectorPoint2fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vectorPoint2fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_std_vectorPoint2f_delete(CppPtr);
      }

      // Methods

      public Cv.Point2f At(uint pos)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Point2f element = new Cv.Point2f(au_std_vectorPoint2f_at(CppPtr, pos, exception.CppPtr), DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe Cv.Point2f[] Data()
      {
        IntPtr* dataPtr = au_std_vectorPoint2f_data(CppPtr);
        uint size = Size();

        Cv.Point2f[] data = new Cv.Point2f[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new Cv.Point2f(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(Cv.Point2f value)
      {
        au_std_vectorPoint2f_push_back(CppPtr, value.CppPtr);
      }

      public uint Size()
      {
        return au_std_vectorPoint2f_size(CppPtr);
      }
    }
  }
}