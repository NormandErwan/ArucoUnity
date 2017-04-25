using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      public class Dictionary : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new1(System.IntPtr bytesList, int markerSize, int maxCorrectionBits);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new2(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_delete(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_drawMarker(System.IntPtr dictionary, int id, int sidePixels, out System.IntPtr img, int borderBits, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern int au_Dictionary_getDistanceToId(System.IntPtr dictionary, System.IntPtr bits, int id, bool allRotations, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern bool au_Dictionary_identify(System.IntPtr dictionary, System.IntPtr onlyBits, out int idx, out int rotation, double maxCorrectionRate, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_getBitsFromByteList(System.IntPtr byteList, int markerSize, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_getByteListFromBits(System.IntPtr bits);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_getBytesList(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_setBytesList(System.IntPtr dictionary, System.IntPtr bytesList);

        [DllImport("ArucoUnity")]
        static extern int au_Dictionary_getMarkerSize(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_setMarkerSize(System.IntPtr dictionary, int markerSize);

        [DllImport("ArucoUnity")]
        static extern int au_Dictionary_getMaxCorrectionBits(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_setMaxCorrectionBits(System.IntPtr dictionary, int maxCorrectionBits);

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

        internal Dictionary(System.IntPtr dictionaryPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
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
          get { return new Cv.Mat(au_Dictionary_getBytesList(CppPtr), Utility.DeleteResponsibility.False); }
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

        static public Cv.Mat GetByteListFromBits(System.IntPtr bits)
        {
          return new Cv.Mat(au_Dictionary_getByteListFromBits(bits));
        }

        // Methods

        public void DrawMarker(int id, int sidePixels, out Cv.Mat img, int borderBits)
        {
          Cv.Exception exception = new Cv.Exception();
          System.IntPtr imgPtr;

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

  /// \} aruco_unity_package
}