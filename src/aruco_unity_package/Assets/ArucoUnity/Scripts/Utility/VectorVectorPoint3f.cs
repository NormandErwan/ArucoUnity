using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class VectorVectorPoint3f : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern void au_vectorVectorPoint3f_delete(System.IntPtr vector);

      // Functions
      [DllImport("ArucoUnity")]
      static extern unsafe System.IntPtr* au_vectorVectorPoint3f_data(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern unsafe void au_vectorVectorPoint3f_data_delete(System.IntPtr* vector);

      [DllImport("ArucoUnity")]
      static extern int au_vectorVectorPoint3f_size1(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern int au_vectorVectorPoint3f_size2(System.IntPtr vector);

      internal VectorVectorPoint3f(System.IntPtr vectorVectorPoint3fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(vectorVectorPoint3fPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_vectorVectorPoint3f_delete(cvPtr);
      }

      public unsafe Point3f[][] Data()
      {
        int dataSize1 = Size(),
            dataSize2 = Size2();
        System.IntPtr* dataPtr = au_vectorVectorPoint3f_data(cvPtr);

        Point3f[][] data = new Point3f[dataSize1][];
        for (var i = 0; i < dataSize1; i++)
        {
          Point3f[] data2 = new Point3f[dataSize2];
          for (var j = 0; j < dataSize2; j++)
          {
            data2[j] = new Point3f(dataPtr[i * dataSize2 + j]);
          }
          data[i] = data2;
        }

        au_vectorVectorPoint3f_data_delete(dataPtr);

        return data;
      }

      public int Size()
      {
        return au_vectorVectorPoint3f_size1(cvPtr);
      }

      public int Size2()
      {
        return au_vectorVectorPoint3f_size2(cvPtr);
      }
    }
  }
}