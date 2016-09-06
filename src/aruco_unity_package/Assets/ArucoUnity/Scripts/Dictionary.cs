using UnityEngine;
using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class Dictionary
  {
    [DllImport("ArucoUnity")]
    static extern void auDestroyDictionary(System.IntPtr dictionary);

    [DllImport("ArucoUnity")]
    static extern int auGetDictionaryMarkerSize(System.IntPtr dictionary);

    HandleRef _handle;

    public Dictionary(System.IntPtr dictionary)
    {
      _handle = new HandleRef(this, dictionary);
    }

    ~Dictionary()
    {
      auDestroyDictionary(ptr);
    }

    public System.IntPtr ptr
    {
      get { return _handle.Handle; }
    }

    public int markerSize
    {
      get { return auGetDictionaryMarkerSize(ptr); }
    }
  }
}