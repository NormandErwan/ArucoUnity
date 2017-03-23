using ArucoUnity.Plugin.Utility;
using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      public class Dictionary : HandleCppPtr
      {
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new1(System.IntPtr bytesList, int markerSize, int maxCorrectionBits);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new2(System.IntPtr bytesList, int markerSize);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new3(System.IntPtr bytesList);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new4();

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_Dictionary_new5(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_delete(System.IntPtr dictionary);

        [DllImport("ArucoUnity")]
        static extern void au_Dictionary_drawMarker(System.IntPtr dictionary, int id, int sidePixels, out System.IntPtr img, int borderBits, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern int au_Dictionary_getDistanceToId1(System.IntPtr dictionary, System.IntPtr bits, int id, bool allRotations, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern int au_Dictionary_getDistanceToId2(System.IntPtr dictionary, System.IntPtr bits, int id, System.IntPtr exception);

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

        public Dictionary(Cv.Mat bytesList, int markerSize, int maxCorrectionBits) : base(au_Dictionary_new1(bytesList.cppPtr, markerSize, maxCorrectionBits))
        {
        }

        public Dictionary(Cv.Mat bytesList, int markerSize) : base(au_Dictionary_new2(bytesList.cppPtr, markerSize))
        {
        }

        public Dictionary(Cv.Mat bytesList) : base(au_Dictionary_new3(bytesList.cppPtr))
        {
        }

        public Dictionary() : base(au_Dictionary_new4())
        {
        }

        public Dictionary(Dictionary dictionary) : base(au_Dictionary_new5(dictionary.cppPtr))
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

        public void DrawMarker(int id, int sidePixels, out Cv.Mat img, int borderBits)
        {
          Cv.Exception exception = new Cv.Exception();
          System.IntPtr imgPtr;

          au_Dictionary_drawMarker(cppPtr, id, sidePixels, out imgPtr, borderBits, exception.cppPtr);
          img = new Cv.Mat(imgPtr);

          exception.Check();
        }

        public int GetDistanceToId(Dictionary dictionary, Cv.Mat bits, int id, bool allRotations)
        {
          Cv.Exception exception = new Cv.Exception();
          int distanceToId = au_Dictionary_getDistanceToId1(cppPtr, bits.cppPtr, id, allRotations, exception.cppPtr);
          exception.Check();
          return distanceToId;
        }

        public int GetDistanceToId(Dictionary dictionary, Cv.Mat bits, int id)
        {
          Cv.Exception exception = new Cv.Exception();
          int distanceToId = au_Dictionary_getDistanceToId2(cppPtr, bits.cppPtr, id, exception.cppPtr);
          exception.Check();
          return distanceToId;
        }

        public bool Identify(Dictionary dictionary, Cv.Mat onlyBits, out int idx, out int rotation, double maxCorrectionRate)
        {
          Cv.Exception exception = new Cv.Exception();
          bool result = au_Dictionary_identify(cppPtr, onlyBits.cppPtr, out idx, out rotation, maxCorrectionRate, exception.cppPtr);
          exception.Check();
          return result;
        }

        static public Cv.Mat GetBitsFromByteList(Cv.Mat byteList, int markerSiz)
        {
          Cv.Exception exception = new Cv.Exception();
          Cv.Mat bits = new Cv.Mat(au_Dictionary_getBitsFromByteList(byteList.cppPtr, markerSiz, exception.cppPtr));
          exception.Check();
          return bits;
        }

        static public Cv.Mat GetByteListFromBits(System.IntPtr bits)
        {
          return new Cv.Mat(au_Dictionary_getByteListFromBits(bits));
        }

        public Cv.Mat bytesList
        {
          get { return new Cv.Mat(au_Dictionary_getBytesList(cppPtr), DeleteResponsibility.False); }
          set { au_Dictionary_setBytesList(cppPtr, value.cppPtr); }
        }

        public int markerSize
        {
          get { return au_Dictionary_getMarkerSize(cppPtr); }
          set { au_Dictionary_setMarkerSize(cppPtr, value); }
        }

        public int maxCorrectionBits
        {
          get { return au_Dictionary_getMaxCorrectionBits(cppPtr); }
          set { au_Dictionary_setMaxCorrectionBits(cppPtr, value); }
        }

        public PREDEFINED_DICTIONARY_NAME name { get; set; }
      }
    }
  }

  /// \} aruco_unity_package
}