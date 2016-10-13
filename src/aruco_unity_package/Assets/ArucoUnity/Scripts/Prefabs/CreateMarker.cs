using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CreateMarker : MonoBehaviour
    {
      public Dictionary dictionary;

      public Utility.Mat image;

      [HideInInspector]
      public Texture2D imageTexture;

      [Header("Marker configuration")]
      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("Marker id in the dictionary")]
      public int markerId;

      [SerializeField]
      [Tooltip("Marker size in pixels (default: 200)")]
      public int markerSize;

      [SerializeField]
      [Tooltip("Number of bits in marker borders (default: 1)")]
      public int markerBorderBits;

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
        dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        Create();

        if (drawMarker)
        {
          Draw(markerPlane);

          if (saveMarker && outputImage.Length > 0)
          {
            Save(outputImage);
          }
        }
      }

      // Call it first if you're using the Script alone, not with the Prefab.
      public void Configurate(Dictionary dictionary, int markerId, int markerSize, int markerBorderBits)
      {
        this.dictionary = dictionary;
        this.markerId = markerId;
        this.markerSize = markerSize;
        this.markerBorderBits = markerBorderBits;
      }

      public void Create()
      {
        image = new Utility.Mat();
        dictionary.DrawMarker(markerId, markerSize, ref image, markerBorderBits);

        imageTexture = new Texture2D(image.cols, image.rows, TextureFormat.RGB24, false);
      }

      public void Draw(GameObject markerPlane)
      {
        int markerDataSize = (int)(image.ElemSize() * image.Total());
        imageTexture.LoadRawTextureData(image.data, markerDataSize);
        imageTexture.Apply();

        markerPlane.GetComponent<Renderer>().material.mainTexture = imageTexture;
      }

      public void Save(string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, imageTexture.EncodeToPNG());
      }
    }
  }
}