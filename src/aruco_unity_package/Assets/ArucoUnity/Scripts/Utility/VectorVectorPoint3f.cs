using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class VectorVectorPoint3f : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_vectorVectorPoint3f_new();

      [DllImport("ArucoUnity")]
      static extern void au_vectorVectorPoint3f_delete(System.IntPtr vector);

      // Functions
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_vectorVectorPoint3f_at(System.IntPtr vector, int pos, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern unsafe System.IntPtr* au_vectorVectorPoint3f_data(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern void au_vectorVectorPoint3f_push_back(System.IntPtr vector, System.IntPtr value);

      [DllImport("ArucoUnity")]
      static extern int au_vectorVectorPoint3f_size(System.IntPtr vector);

      public VectorVectorPoint3f() : base(au_vectorVectorPoint3f_new())
      {
      }

      public VectorVectorPoint3f(System.IntPtr vectorVectorPoint3fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(vectorVectorPoint3fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_vectorVectorPoint3f_delete(cvPtr);
      }

      public VectorPoint3f At(int pos) 
      {
        Exception exception = new Exception();
        VectorPoint3f element = new VectorPoint3f(au_vectorVectorPoint3f_at(cvPtr, pos, exception.cvPtr), DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe VectorPoint3f[] Data()
      {
        System.IntPtr* dataPtr = au_vectorVectorPoint3f_data(cvPtr);
        int size = Size();

        VectorPoint3f[] data = new VectorPoint3f[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new VectorPoint3f(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(VectorPoint3f value)
      {
        au_vectorVectorPoint3f_push_back(cvPtr, value.cvPtr);
      }

      public int Size()
      {
        return au_vectorVectorPoint3f_size(cvPtr);
      }
    }
  }
}