using System.Runtime.InteropServices;
using UnityEngine;

public partial class ArucoUnity
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

    ~VectorVectorPoint2f()
    {
      au_vectorVectorPoint2f_delete(cvPtr);
    }

    public unsafe ArucoUnity.Point2f[][] Data()
    {
      int dataSize1 = au_vectorVectorPoint2f_size1(cvPtr);
      int dataSize2 = au_vectorVectorPoint2f_size2(cvPtr);
      System.IntPtr* dataPtr = au_vectorVectorPoint2f_data(cvPtr);

      ArucoUnity.Point2f[][] data = new ArucoUnity.Point2f[dataSize1][];
      for (var i = 0; i < dataSize1; i++)
      {
        ArucoUnity.Point2f[] data2 = new ArucoUnity.Point2f[dataSize2];
        for (var j = 0; j < dataSize2; j++)
        {
          data2[j] = new ArucoUnity.Point2f(dataPtr[i * dataSize2 + j]);
        }
        data[i] = data2;
      }

      au_vectorVectorPoint2f_data_delete(dataPtr);

      return data;
    }

    public int Size1()
    {
      return au_vectorVectorPoint2f_size1(cvPtr);
    }

    public int Size2()
    {
      return au_vectorVectorPoint2f_size2(cvPtr);
    }
  }
}