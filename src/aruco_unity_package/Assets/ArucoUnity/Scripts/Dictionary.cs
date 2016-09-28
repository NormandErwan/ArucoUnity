using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Dictionary : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern void au_Dictionary_Delete(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern int au_Dictionary_GetMarkerSize(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern void au_Dictionary_DrawMarker(System.IntPtr dictionary, int id, int sidePixels, System.IntPtr img, int borderBits,
      System.IntPtr exception);

    public Dictionary(System.IntPtr dictionary) : base(dictionary)
    {
    }

    ~Dictionary()
    {
      au_Dictionary_Delete(cvPtr);
    }

    public int markerSize
    {
      get { return au_Dictionary_GetMarkerSize(cvPtr); }
    }
    
    public void DrawMarker(int id, int sidePixels, ref Mat img, int borderBits)
    {
      ArucoUnity.Exception exception = new ArucoUnity.Exception();
      au_Dictionary_DrawMarker(cvPtr, id, sidePixels, img.cvPtr, borderBits, exception.cvPtr);

      if (exception.code != 0)
      {
        throw new System.Exception(exception.What());
      }
    }
  }
}