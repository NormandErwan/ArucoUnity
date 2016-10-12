using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CreateMarker : MonoBehaviour
    {
      public Utility.Mat markerImage;

      [Header("Marker configuration")]
      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("Marker id in the dictionary")]
      private int markerId;

      [SerializeField]
      [Tooltip("Marker size in pixels (default: 200)")]
      private int markerSize;

      [SerializeField]
      [Tooltip("Number of bits in marker borders (default: 1)")]
      private int markerBorderBits;

      [Header("Draw the marker")]
      [SerializeField]
      [Tooltip("Show generated image")]
      private bool drawMarker;

      [SerializeField]
      [Tooltip("The plane where to display the generated image")]
      private GameObject markerPlane;

      [Header("Save the marker")]
      [SerializeField]
      [Tooltip("Save the generated image")]
      private bool saveMarker;

      [SerializeField]
      [Tooltip("Output image")]
      private string outputImage = "ArucoUnity/marker.png";

      void Start()
      {
        markerImage = Create(dictionaryName, markerId, markerSize, markerBorderBits);

        if (drawMarker)
        {
          Texture2D markerTexture = CreateTexture(markerImage);
          Draw(markerImage, markerPlane, markerTexture);

          if (saveMarker && outputImage.Length > 0)
          {
            Save(markerTexture, outputImage);
          }
        }
      }

      public Utility.Mat Create(PREDEFINED_DICTIONARY_NAME dictionaryName, int markerId, int markerSize, int markerBorderBits)
      {
        Dictionary dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        Utility.Mat markerImage = new Utility.Mat();

        dictionary.DrawMarker(markerId, markerSize, ref markerImage, markerBorderBits);

        return markerImage;
      }

      public Texture2D CreateTexture(Utility.Mat markerImage)
      {
        return new Texture2D(markerImage.cols, markerImage.rows, TextureFormat.RGB24, false);
      }

      public void Draw(Utility.Mat markerImage, GameObject markerPlane, Texture2D markerTexture)
      {
        int markerDataSize = (int)(markerImage.ElemSize() * markerImage.Total());
        markerTexture.LoadRawTextureData(markerImage.data, markerDataSize);
        markerTexture.Apply();

        markerPlane.GetComponent<Renderer>().material.mainTexture = markerTexture;
      }

      public void Save(Texture2D markerTexture, string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, markerTexture.EncodeToPNG());
      }
    }
  }
}