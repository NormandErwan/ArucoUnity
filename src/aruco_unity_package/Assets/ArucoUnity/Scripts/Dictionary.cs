using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Dictionary : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern void auDeleteDictionary(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern int auGetDictionaryMarkerSize(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern void auDictionaryDrawMarker(System.IntPtr dictionary, int id, int sidePixels, System.IntPtr img, int borderBits);

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
    
    public void DrawMarker(int id, int sidePixels, ref Mat img, int borderBits)
    {
      auDictionaryDrawMarker(cvPtr, id, sidePixels, img.cvPtr, borderBits);
    }
  }
}