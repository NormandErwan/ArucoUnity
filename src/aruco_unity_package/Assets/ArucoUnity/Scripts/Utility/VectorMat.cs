using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class VectorMat : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern void au_vectorMat_delete(System.IntPtr vector);

      // Functions
      [DllImport("ArucoUnity")]
      static extern unsafe System.IntPtr* au_vectorMat_data(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern int au_vectorMat_size(System.IntPtr vector);

      public VectorMat(System.IntPtr vectorMatPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(vectorMatPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_vectorMat_delete(cvPtr);
      }

      public unsafe Mat[] Data()
      {
        System.IntPtr* dataPtr = au_vectorMat_data(cvPtr);
        int size = Size();

        Mat[] data = new Mat[size];
        for (int i = 0; i < size; i++)
        {
          data[i] = new Mat(dataPtr[i], DeleteResponsibility.False);
        }

        return data;
      }

      public int Size()
      {
        return au_vectorMat_size(cvPtr);
      }
    }
  }
}