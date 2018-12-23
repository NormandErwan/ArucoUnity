using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Std
  {
    public class VectorVec3d : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVec3d_new();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVec3d_delete(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_std_vectorVec3d_at(IntPtr vector, uint pos, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern unsafe IntPtr* au_std_vectorVec3d_data(IntPtr vector);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_std_vectorVec3d_push_back(IntPtr vector, IntPtr value);

      [DllImport("ArucoUnityPlugin")]
      static extern uint au_std_vectorVec3d_size(IntPtr vector);

      // Constructors & destructor

      public VectorVec3d() : base(au_std_vectorVec3d_new())
      {
      }

      public VectorVec3d(IntPtr vectorVec3dPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(vectorVec3dPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_std_vectorVec3d_delete(CppPtr);
      }

      // Methods

      public Cv.Vec3d At(uint pos)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Vec3d element = new Cv.Vec3d(au_std_vectorVec3d_at(CppPtr, pos, exception.CppPtr), DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe Cv.Vec3d[] Data()
      {
        IntPtr* dataPtr = au_std_vectorVec3d_data(CppPtr);
        uint size = Size();

        Cv.Vec3d[] data = new Cv.Vec3d[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new Cv.Vec3d(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(Cv.Vec3d value)
      {
        au_std_vectorVec3d_push_back(CppPtr, value.CppPtr);
      }

      public uint Size()
      {
        return au_std_vectorVec3d_size(CppPtr);
      }
    }
  }
}