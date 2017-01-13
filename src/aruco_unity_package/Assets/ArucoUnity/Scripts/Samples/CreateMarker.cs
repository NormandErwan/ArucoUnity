using System.IO;
using UnityEngine;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  namespace Samples
  {
    public class CreateMarker : MonoBehaviour
    {
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

      // Configuration properties
      public Dictionary Dictionary { get; set; }
      public int MarkerId { get; set; }
      public int MarkerSize { get; set; }
      public int MarkerBorderBits { get; set; }

      // Marker properties
      public Mat Image { get; private set; }
      public Texture2D ImageTexture { get; private set; }

      void Start()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
        MarkerId = markerId;
        MarkerSize = markerSize;
        MarkerBorderBits = markerBorderBits;

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

      public void Create()
      {
        Mat image = new Mat();
        Dictionary.DrawMarker(MarkerId, MarkerSize, ref image, MarkerBorderBits);
        Image = image;

        ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
      }

      public void Draw(GameObject markerPlane)
      {
        int markerDataSize = (int)(Image.ElemSize() * Image.Total());
        ImageTexture.LoadRawTextureData(Image.data, markerDataSize);
        ImageTexture.Apply();

        markerPlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
      }

      public void Save(string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }
}