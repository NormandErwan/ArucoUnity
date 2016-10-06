using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DetectMarkers : MonoBehaviour
    {
      public Dictionary dictionary;

      public DetectorParameters detectorParameters;

      public Utility.VectorVectorPoint2f corners;
      public Utility.VectorInt ids;
      public Utility.VectorVectorPoint2f rejectedImgPoints;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      private bool showRejectedCandidates;

      [SerializeField]
      private RawImage image;

      // TODO : detector parameters manager

      void Start()
      {
        Configurate(dictionaryName);
      }

      void LateUpdate()
      {
        Detect(ref image, showRejectedCandidates);
      }

      public void Configurate(PREDEFINED_DICTIONARY_NAME dictionaryName)
      {
        dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        detectorParameters = new DetectorParameters();
      }

      public void Detect(ref RawImage image, bool showRejectedCandidates)
      {
        Texture2D imageTexture = (Texture2D)image.texture;
        Detect(ref imageTexture, showRejectedCandidates);
      }

      public void Detect(ref Texture2D imageTexture, bool showRejectedCandidates)
      {
        byte[] imageData = imageTexture.GetRawTextureData();
        print(imageData[0]);
        //DetectMarkers(ArucoUnity.Mat image, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints)
      }
    }
  }
}