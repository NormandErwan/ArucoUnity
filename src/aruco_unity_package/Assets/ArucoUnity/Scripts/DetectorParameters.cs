using UnityEngine;
using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class DetectorParameters
  {
    [DllImport("ArucoUnity")]
    static extern void auDestroyDetectorParameters(System.IntPtr detectorParameters);

    HandleRef _handle;

    public DetectorParameters(System.IntPtr detectorParameters)
    {
      _handle = new HandleRef(this, detectorParameters);
    }

    ~DetectorParameters()
    {
      auDestroyDetectorParameters(ptr);
    }

    public System.IntPtr ptr
    {
      get { return _handle.Handle; }
    }
  }
}