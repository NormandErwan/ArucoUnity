using UnityEngine;

public class ArucoUnityManager : MonoBehaviour {

  ArucoUnity.Dictionary dictionary, dictionary2;
	
	void Start()
  {
    dictionary = ArucoUnity.GetPredefinedDictionary(ArucoUnity.PREDEFINED_DICTIONARY_NAME.DICT_4X4_100);
    dictionary2 = ArucoUnity.GetPredefinedDictionary(ArucoUnity.PREDEFINED_DICTIONARY_NAME.DICT_5X5_100);
    print(dictionary.markerSize);
    print(dictionary2.markerSize);
  }
	
	void Update()
  {
	
	}
}
