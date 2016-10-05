using UnityEngine;
using System.Runtime.InteropServices;

public partial class ArucoUnity : MonoBehaviour
{
  public enum PREDEFINED_DICTIONARY_NAME
  {
    DICT_4X4_50 = 0,
    DICT_4X4_100,
    DICT_4X4_250,
    DICT_4X4_1000,
    DICT_5X5_50,
    DICT_5X5_100,
    DICT_5X5_250,
    DICT_5X5_1000,
    DICT_6X6_50,
    DICT_6X6_100,
    DICT_6X6_250,
    DICT_6X6_1000,
    DICT_7X7_50,
    DICT_7X7_100,
    DICT_7X7_250,
    DICT_7X7_1000,
    DICT_ARUCO_ORIGINAL
  }

  [DllImport("ArucoUnity")]
  static extern System.IntPtr au_generateCustomDictionary1(int nMarkers, int markerSize, System.IntPtr exception);

  [DllImport("ArucoUnity")]
  static extern System.IntPtr au_generateCustomDictionary2(int nMarkers, int markerSize, System.IntPtr baseDictionary, System.IntPtr exception);

  [DllImport("ArucoUnity")]
  static extern System.IntPtr au_getPredefinedDictionary(PREDEFINED_DICTIONARY_NAME name);

  public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize)
  {
    ArucoUnity.Exception exception = new ArucoUnity.Exception();
    System.IntPtr dictionaryPtr = au_generateCustomDictionary1(nMarkers, markerSize, exception.cvPtr);
    exception.Check();
    return new Dictionary(dictionaryPtr);
  }

  public static Dictionary GenerateCustomDictionary(int nMarkers, int markerSize, Dictionary baseDictionary)
  {
    ArucoUnity.Exception exception = new ArucoUnity.Exception();
    System.IntPtr dictionaryPtr = au_generateCustomDictionary2(nMarkers, markerSize, baseDictionary.cvPtr, exception.cvPtr);
    exception.Check();
    return new Dictionary(dictionaryPtr);
  }

  public static Dictionary GetPredefinedDictionary(PREDEFINED_DICTIONARY_NAME name)
  {
    return new Dictionary(au_getPredefinedDictionary(name));
  }
}