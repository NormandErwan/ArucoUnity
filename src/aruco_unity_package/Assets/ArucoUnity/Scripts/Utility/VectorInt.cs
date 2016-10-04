using System.Runtime.InteropServices;
using UnityEngine;

public partial class ArucoUnity
{
  public partial class VectorInt : HandleCvPtr
  {
    // Constructor & Destructor
    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_vectorInt_new();

    [DllImport("ArucoUnity")]
    static extern void au_vectorInt_delete(System.IntPtr vector);

    // Functions
    [DllImport("ArucoUnity")]
    static extern unsafe int* au_vectorInt_data(System.IntPtr vector);

    [DllImport("ArucoUnity")]
    static extern int au_vectorInt_size(System.IntPtr vector);

    public VectorInt() : base(au_vectorInt_new())
    {
    }

    ~VectorInt()
    {
      au_vectorInt_delete(cvPtr);
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

    public int Size()
    {
      return au_vectorInt_size(cvPtr);
    }
  }
}