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

        public Dictionary(Cv.Core.Mat bytesList, int markerSize = 0, int maxCorrectionBits = 0)
          : base(au_Dictionary_new1(bytesList.cppPtr, markerSize, maxCorrectionBits))
        {
        }

        public Dictionary() : this(new Cv.Core.Mat())
        {
        }

        public Dictionary(Dictionary dictionary) : base(au_Dictionary_new2(dictionary.cppPtr))
        {
        }

        internal Dictionary(System.IntPtr dictionaryPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(dictionaryPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_Dictionary_delete(cppPtr);
        }

        // Properties

        public Cv.Core.Mat BytesList
        {
          get { return new Cv.Core.Mat(au_Dictionary_getBytesList(cppPtr), DeleteResponsibility.False); }
          set { au_Dictionary_setBytesList(cppPtr, value.cppPtr); }
        }

        public int MarkerSize
        {
          get { return au_Dictionary_getMarkerSize(cppPtr); }
          set { au_Dictionary_setMarkerSize(cppPtr, value); }
        }

        public int MaxCorrectionBits
        {
          get { return au_Dictionary_getMaxCorrectionBits(cppPtr); }
          set { au_Dictionary_setMaxCorrectionBits(cppPtr, value); }
        }

        public PredefinedDictionaryName Name { get; set; }

        // Static methods

        static public Cv.Core.Mat GetBitsFromByteList(Cv.Core.Mat byteList, int markerSiz)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          Cv.Core.Mat bits = new Cv.Core.Mat(au_Dictionary_getBitsFromByteList(byteList.cppPtr, markerSiz, exception.cppPtr));
          exception.Check();
          return bits;
        }

        static public Cv.Core.Mat GetByteListFromBits(System.IntPtr bits)
        {
          return new Cv.Core.Mat(au_Dictionary_getByteListFromBits(bits));
        }

        // Methods

        public void DrawMarker(int id, int sidePixels, out Cv.Core.Mat img, int borderBits)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          System.IntPtr imgPtr;

          au_Dictionary_drawMarker(cppPtr, id, sidePixels, out imgPtr, borderBits, exception.cppPtr);
          img = new Cv.Core.Mat(imgPtr);

          exception.Check();
        }

        public int GetDistanceToId(Dictionary dictionary, Cv.Core.Mat bits, int id, bool allRotations = true)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          int distanceToId = au_Dictionary_getDistanceToId(cppPtr, bits.cppPtr, id, allRotations, exception.cppPtr);
          exception.Check();
          return distanceToId;
        }

        public bool Identify(Dictionary dictionary, Cv.Core.Mat onlyBits, out int idx, out int rotation, double maxCorrectionRate)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          bool result = au_Dictionary_identify(cppPtr, onlyBits.cppPtr, out idx, out rotation, maxCorrectionRate, exception.cppPtr);
          exception.Check();
          return result;
        }
      }
    }
  }

  /// \} aruco_unity_package
}