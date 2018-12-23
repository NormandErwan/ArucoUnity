using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Aruco
  {
    public class Dictionary : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_Dictionary_new1(IntPtr bytesList, int markerSize, int maxCorrectionBits);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_Dictionary_new2(IntPtr dictionary);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_Dictionary_delete(IntPtr dictionary);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_Dictionary_drawMarker(IntPtr dictionary, int id, int sidePixels, out IntPtr img, int borderBits, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_Dictionary_getDistanceToId(IntPtr dictionary, IntPtr bits, int id, bool allRotations, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern bool au_Dictionary_identify(IntPtr dictionary, IntPtr onlyBits, out int idx, out int rotation, double maxCorrectionRate, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_Dictionary_getBitsFromByteList(IntPtr byteList, int markerSize, IntPtr exception);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_Dictionary_getByteListFromBits(IntPtr bits);

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_Dictionary_getBytesList(IntPtr dictionary);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_Dictionary_setBytesList(IntPtr dictionary, IntPtr bytesList);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_Dictionary_getMarkerSize(IntPtr dictionary);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_Dictionary_setMarkerSize(IntPtr dictionary, int markerSize);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_Dictionary_getMaxCorrectionBits(IntPtr dictionary);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_Dictionary_setMaxCorrectionBits(IntPtr dictionary, int maxCorrectionBits);

      // Constructors & destructor

      public Dictionary(Cv.Mat bytesList, int markerSize = 0, int maxCorrectionBits = 0)
        : base(au_Dictionary_new1(bytesList.CppPtr, markerSize, maxCorrectionBits))
      {
      }

      public Dictionary() : this(new Cv.Mat())
      {
      }

      public Dictionary(Dictionary dictionary) : base(au_Dictionary_new2(dictionary.CppPtr))
      {
      }

      internal Dictionary(IntPtr dictionaryPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
        : base(dictionaryPtr, deleteResponsibility)
      {
      }

      protected override void DeleteCppPtr()
      {
        au_Dictionary_delete(CppPtr);
      }

      // Properties

      public Cv.Mat BytesList
      {
        get { return new Cv.Mat(au_Dictionary_getBytesList(CppPtr), DeleteResponsibility.False); }
        set { au_Dictionary_setBytesList(CppPtr, value.CppPtr); }
      }

      public int MarkerSize
      {
        get { return au_Dictionary_getMarkerSize(CppPtr); }
        set { au_Dictionary_setMarkerSize(CppPtr, value); }
      }

      public int MaxCorrectionBits
      {
        get { return au_Dictionary_getMaxCorrectionBits(CppPtr); }
        set { au_Dictionary_setMaxCorrectionBits(CppPtr, value); }
      }

      public PredefinedDictionaryName Name { get; set; }

      // Static methods

      static public Cv.Mat GetBitsFromByteList(Cv.Mat byteList, int markerSiz)
      {
        Cv.Exception exception = new Cv.Exception();
        Cv.Mat bits = new Cv.Mat(au_Dictionary_getBitsFromByteList(byteList.CppPtr, markerSiz, exception.CppPtr));
        exception.Check();
        return bits;
      }

      static public Cv.Mat GetByteListFromBits(IntPtr bits)
      {
        return new Cv.Mat(au_Dictionary_getByteListFromBits(bits));
      }

      // Methods

      public void DrawMarker(int id, int sidePixels, out Cv.Mat img, int borderBits)
      {
        Cv.Exception exception = new Cv.Exception();
        IntPtr imgPtr;

        au_Dictionary_drawMarker(CppPtr, id, sidePixels, out imgPtr, borderBits, exception.CppPtr);
        img = new Cv.Mat(imgPtr);

        exception.Check();
      }

      public int GetDistanceToId(Dictionary dictionary, Cv.Mat bits, int id, bool allRotations = true)
      {
        Cv.Exception exception = new Cv.Exception();
        int distanceToId = au_Dictionary_getDistanceToId(CppPtr, bits.CppPtr, id, allRotations, exception.CppPtr);
        exception.Check();
        return distanceToId;
      }

      public bool Identify(Dictionary dictionary, Cv.Mat onlyBits, out int idx, out int rotation, double maxCorrectionRate)
      {
        Cv.Exception exception = new Cv.Exception();
        bool result = au_Dictionary_identify(CppPtr, onlyBits.CppPtr, out idx, out rotation, maxCorrectionRate, exception.CppPtr);
        exception.Check();
        return result;
      }
    }
  }
}