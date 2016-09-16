using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Mat : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern System.IntPtr auNewMat();

    [DllImport("ArucoUnity")]
    static extern void auDeleteMat(System.IntPtr mat);

    [DllImport("ArucoUnity")]
    static extern uint auMatElemSize(System.IntPtr mat);

    [DllImport("ArucoUnity")]
    static extern uint auMatTotal(System.IntPtr mat);

    [DllImport("ArucoUnity")]
    static extern System.IntPtr auGetMatData(System.IntPtr mat);

    public Mat() : base(auNewMat())
    {
    }

    ~Mat()
    {
      auDeleteMat(cvPtr);
    }

    public uint ElemSize()
    {
      return auMatElemSize(cvPtr);
    }

    public uint Total()
    {
      return auMatTotal(cvPtr);
    }

    public System.IntPtr data 
    {
      get { return auGetMatData(cvPtr); }
    }
  }
}