using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Dictionary : HandleDllObject
  {
    [DllImport("ArucoUnity")]
    static extern void auDestroyDictionary(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern int auGetDictionaryMarkerSize(System.IntPtr dictionary);

    public Dictionary(System.IntPtr dictionary) : base(dictionary)
    {
    }

    ~Dictionary()
    {
      auDestroyDictionary(ptr);
    }

    public int markerSize
    {
      get { return auGetDictionaryMarkerSize(ptr); }
    }
  }
}