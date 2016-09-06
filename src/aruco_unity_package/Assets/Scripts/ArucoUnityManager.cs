using UnityEngine;

public class ArucoUnityManager : MonoBehaviour {

  ArucoUnity.Dictionary dictionary;
  ArucoUnity.DetectorParameters detectorParameters;

  void Start()
  {
    dictionary = ArucoUnity.GetPredefinedDictionary(ArucoUnity.PREDEFINED_DICTIONARY_NAME.DICT_4X4_100);
    print(dictionary.markerSize);

    detectorParameters = ArucoUnity.CreateDetectorParameters();
  }
	
	void Update()
  {
	}
}
