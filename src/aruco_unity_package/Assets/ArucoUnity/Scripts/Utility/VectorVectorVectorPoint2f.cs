using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class VectorVectorVectorPoint2f : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_vectorVectorVectorPoint2f_new();

      [DllImport("ArucoUnity")]
      static extern void au_vectorVectorVectorPoint2f_delete(System.IntPtr vector);

      // Functions
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_vectorVectorVectorPoint2f_at(System.IntPtr vector, int pos, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern unsafe System.IntPtr* au_vectorVectorVectorPoint2f_data(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern void au_vectorVectorVectorPoint2f_push_back(System.IntPtr vector, System.IntPtr value);

      [DllImport("ArucoUnity")]
      static extern int au_vectorVectorVectorPoint2f_size(System.IntPtr vector);

      public VectorVectorVectorPoint2f() : base(au_vectorVectorVectorPoint2f_new())
      {
      }

      public VectorVectorVectorPoint2f(System.IntPtr vectorVectorVectorPoint2fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(vectorVectorVectorPoint2fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_vectorVectorVectorPoint2f_delete(cvPtr);
      }

      public VectorVectorPoint2f At(int pos) 
      {
        Exception exception = new Exception();
        VectorVectorPoint2f element = new VectorVectorPoint2f(au_vectorVectorVectorPoint2f_at(cvPtr, pos, exception.cvPtr), DeleteResponsibility.False);
        exception.Check();
        return element;
      }

      public unsafe VectorVectorPoint2f[] Data()
      {
        System.IntPtr* dataPtr = au_vectorVectorVectorPoint2f_data(cvPtr);
        int size = Size();

        VectorVectorPoint2f[] data = new VectorVectorPoint2f[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new VectorVectorPoint2f(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public void PushBack(VectorVectorPoint2f value)
      {
        au_vectorVectorVectorPoint2f_push_back(cvPtr, value.cvPtr);
      }

      public int Size()
      {
        return au_vectorVectorVectorPoint2f_size(cvPtr);
      }
    }
  }
}