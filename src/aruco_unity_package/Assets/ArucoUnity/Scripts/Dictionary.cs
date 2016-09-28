using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Dictionary : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern void au_Dictionary_delete(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern int au_Dictionary_getMarkerSize(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern void au_Dictionary_drawMarker(System.IntPtr dictionary, int id, int sidePixels, System.IntPtr img, int borderBits,
      System.IntPtr exception);

    public Dictionary(System.IntPtr dictionary) : base(dictionary)
    {
    }

    ~Dictionary()
    {
      au_Dictionary_delete(cvPtr);
    }

    public int markerSize
    {
      get { return au_Dictionary_getMarkerSize(cvPtr); }
    }
    
    public void DrawMarker(int id, int sidePixels, ref Mat img, int borderBits)
    {
      ArucoUnity.Exception exception = new ArucoUnity.Exception();
      au_Dictionary_drawMarker(cvPtr, id, sidePixels, img.cvPtr, borderBits, exception.cvPtr);
      exception.Try();
    }
  }
}