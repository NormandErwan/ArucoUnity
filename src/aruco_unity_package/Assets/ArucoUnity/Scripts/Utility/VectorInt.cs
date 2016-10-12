using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class VectorInt : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_vectorInt_new();

      [DllImport("ArucoUnity")]
      static extern void au_vectorInt_delete(System.IntPtr vector);

      // Functions
      [DllImport("ArucoUnity")]
      static extern int au_vectorInt_at(System.IntPtr vector, int pos, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern unsafe int* au_vectorInt_data(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern void au_vectorInt_push_back(System.IntPtr vector, int value);

      [DllImport("ArucoUnity")]
      static extern void au_vectorInt_reserve(System.IntPtr vector, int new_cap, System.IntPtr exception);

      [DllImport("ArucoUnity")]
      static extern int au_vectorInt_size(System.IntPtr vector);

      public VectorInt() : base(au_vectorInt_new())
      {
      }

      public VectorInt(System.IntPtr vectorIntPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(vectorIntPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_vectorInt_delete(cvPtr);
      }

      public int At(int pos) 
      {
        Exception exception = new Exception();
        int element = au_vectorInt_at(cvPtr, pos, exception.cvPtr);
        exception.Check();
        return element;
      }

      public unsafe int[] Data()
      {
        int* dataPtr = au_vectorInt_data(cvPtr);
        int size = Size();

        int[] data = new int[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = dataPtr[i];
        }

        return data;
      }

      public void PushBack(int value)
      {
        au_vectorInt_push_back(cvPtr, value);
      }

      public void Reserve(int newCap)
      {
        Exception exception = new Exception();
        au_vectorInt_reserve(cvPtr, newCap, exception.cvPtr);
        exception.Check();
      }

      public int Size()
      {
        return au_vectorInt_size(cvPtr);
      }
    }
  }
}