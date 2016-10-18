using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CreateDiamond : MonoBehaviour
    {
      public Dictionary dictionary;

      public Utility.Mat image;

      [HideInInspector]
      public Texture2D imageTexture;

      [Header("ChArUco marker configuration")]
      [SerializeField]
      [Tooltip("Marker side length (in pixels)")]
      public int squareSideLength;

      [SerializeField]
      [Tooltip("Marker side length (in pixels)")]
      public int markerSideLength;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("Four ids for the ChArUco marker: id1, id2, id3, id4")]
      public int[] ids;

      [SerializeField]
      [Tooltip("Margins size (in pixels). Default is: 0")]
      public int marginsSize;

      [SerializeField]
      [Tooltip("Number of bits in marker border. Default is: 1")]
      public int markerBorderBits;

      [Header("Draw the marker")]
      [SerializeField]
      private bool drawMarker;

      [SerializeField]
      private GameObject markerPlane;

      [Header("Save the marker")]
      [SerializeField]
      [Tooltip("Save the generated image")]
      private bool saveMarker;

      [SerializeField]
      [Tooltip("Output image")]
      private string outputImage = "ArucoUnity/diamond-marker.png";

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
      public void Configurate(int squareSideLength, int markerSideLength, Dictionary dictionary, int[] ids, int marginsSize, int markerBorderBits)
      {
        this.squareSideLength = squareSideLength;
        this.markerSideLength = markerSideLength;
        this.dictionary = dictionary;
        this.ids = ids;
        this.marginsSize = marginsSize;
        this.markerBorderBits = markerBorderBits;
      }

      public void Create()
      {
        Utility.Vec4i ids_vec4i = new Utility.Vec4i();
        for (int i = 0; i < ids.Length; ++i)
        {
          ids_vec4i.Set(i, ids[i]);
        }

        Methods.DrawCharucoDiamond(dictionary, ids_vec4i, squareSideLength, markerSideLength, out image, marginsSize, markerBorderBits);

        imageTexture = new Texture2D(image.cols, image.rows, TextureFormat.RGB24, false);
      }

      public void Draw(GameObject boardPlane)
      {
        int boardDataSize = (int)(image.ElemSize() * image.Total());
        imageTexture.LoadRawTextureData(image.data, boardDataSize);
        imageTexture.Apply();

        boardPlane.GetComponent<Renderer>().material.mainTexture = imageTexture;
      }

      public void Save(string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, imageTexture.EncodeToPNG());
      }
    }
  }
}