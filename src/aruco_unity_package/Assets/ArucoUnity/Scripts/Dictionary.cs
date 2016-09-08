using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Dictionary : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern void auDeleteDictionary(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern int auGetDictionaryMarkerSize(System.IntPtr dictionary);

    public Dictionary(System.IntPtr dictionary) : base(dictionary)
    {
    }

    ~Dictionary()
    {
      auDeleteDictionary(cvPtr);
    }

    public int markerSize
    {
      get { return auGetDictionaryMarkerSize(cvPtr); }
    }
  }
}