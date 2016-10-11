using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public partial class VectorVectorPoint2f : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern void au_vectorVectorPoint2f_delete(System.IntPtr vector);

      // Functions
      [DllImport("ArucoUnity")]
      static extern unsafe System.IntPtr* au_vectorVectorPoint2f_data(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern unsafe void au_vectorVectorPoint2f_data_delete(System.IntPtr* vector);

      [DllImport("ArucoUnity")]
      static extern int au_vectorVectorPoint2f_size1(System.IntPtr vector);

      [DllImport("ArucoUnity")]
      static extern int au_vectorVectorPoint2f_size2(System.IntPtr vector);

      internal VectorVectorPoint2f(System.IntPtr vectorVectorPoint2fPtr) : base(vectorVectorPoint2fPtr)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_vectorVectorPoint2f_delete(cvPtr);
      }

      public unsafe Point2f[][] Data()
      {
        int dataSize1 = Size(),
            dataSize2 = Size2();
        System.IntPtr* dataPtr = au_vectorVectorPoint2f_data(cvPtr);

        Point2f[][] data = new Point2f[dataSize1][];
        for (var i = 0; i < dataSize1; i++)
        {
          Point2f[] data2 = new Point2f[dataSize2];
          for (var j = 0; j < dataSize2; j++)
          {
            data2[j] = new Point2f(dataPtr[i * dataSize2 + j]);
          }
          data[i] = data2;
        }

        au_vectorVectorPoint2f_data_delete(dataPtr);

        return data;
      }

      public int Size()
      {
        return au_vectorVectorPoint2f_size1(cvPtr);
      }

      public int Size2()
      {
        return au_vectorVectorPoint2f_size2(cvPtr);
      }
    }
  }
}