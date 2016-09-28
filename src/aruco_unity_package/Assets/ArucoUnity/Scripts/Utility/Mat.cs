using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Mat : HandleCvPtr
  {
    // Constructor & Destructor
    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_Mat_New();

    [DllImport("ArucoUnity")]
    static extern void au_Mat_Delete(System.IntPtr mat);

    // Functions
    [DllImport("ArucoUnity")]
    static extern uint au_Mat_Total(System.IntPtr mat);
    
    [DllImport("ArucoUnity")]
    static extern uint au_Mat_ElemSize(System.IntPtr mat);

    // Variables
    [DllImport("ArucoUnity")]
    static extern int au_Mat_GetCols(System.IntPtr mat);

    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_Mat_GetData(System.IntPtr mat);

    [DllImport("ArucoUnity")]
    static extern int au_Mat_GetRows(System.IntPtr mat);


    public Mat() : base(au_Mat_New())
    {
    }

    ~Mat()
    {
      au_Mat_Delete(cvPtr);
    }

    public uint ElemSize()
    {
      return au_Mat_ElemSize(cvPtr);
    }

    public uint Total()
    {
      return au_Mat_Total(cvPtr);
    }

    public int cols 
    {
      get { return au_Mat_GetCols(cvPtr); }
    }

    public System.IntPtr data
    {
      get { return au_Mat_GetData(cvPtr); }
    }

    public int rows
    {
      get { return au_Mat_GetRows(cvPtr); }
    }
  }
}