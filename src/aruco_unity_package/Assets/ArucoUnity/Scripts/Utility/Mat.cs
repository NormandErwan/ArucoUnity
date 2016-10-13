using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class Mat : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_Mat_new1();

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_Mat_new2(int rows, int cols, byte[] data);

      [DllImport("ArucoUnity")]
      static extern void au_Mat_delete(System.IntPtr mat);

      // Functions
      [DllImport("ArucoUnity")]
      static extern uint au_Mat_total(System.IntPtr mat);

      [DllImport("ArucoUnity")]
      static extern uint au_Mat_elemSize(System.IntPtr mat);

      // Variables
      [DllImport("ArucoUnity")]
      static extern int au_Mat_getCols(System.IntPtr mat);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_Mat_getData(System.IntPtr mat);

      [DllImport("ArucoUnity")]
      static extern int au_Mat_getRows(System.IntPtr mat);

      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_Mat_getSize(System.IntPtr mat);

      public Mat() : base(au_Mat_new1())
      {
      }

      public Mat(int rows, int cols, byte[] data) : base(au_Mat_new2(rows, cols, data))
      {
      }

      internal Mat(System.IntPtr matPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(matPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_Mat_delete(cvPtr);
      }

      public uint ElemSize()
      {
        return au_Mat_elemSize(cvPtr);
      }

      public uint Total()
      {
        return au_Mat_total(cvPtr);
      }

      public int cols
      {
        get { return au_Mat_getCols(cvPtr); }
      }

      public System.IntPtr data
      {
        get { return au_Mat_getData(cvPtr); }
      }

      public int rows
      {
        get { return au_Mat_getRows(cvPtr); }
      }
      
      public Size size
      {
        get { return new Size(au_Mat_getSize(cvPtr)); }
      }
    }
  }
}