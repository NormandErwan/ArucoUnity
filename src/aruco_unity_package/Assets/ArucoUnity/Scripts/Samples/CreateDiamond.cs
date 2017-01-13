using System.IO;
using UnityEngine;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    public class CreateDiamond : MonoBehaviour
    {
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
      private bool drawDiamond;

      [SerializeField]
      private GameObject diamondPlane;

      [Header("Save the marker")]
      [SerializeField]
      [Tooltip("Save the generated image")]
      private bool saveDiamond;

      [SerializeField]
      [Tooltip("Output image")]
      private string outputImage = "ArucoUnity/diamond-marker.png";

      // Configuration properties
      public Dictionary Dictionary { get; set; }
      public int SquareSideLength { get; set; }
      public int MarkerSideLength { get; set; }
      public int[] Ids { get; set; }
      public int MarginsSize { get; set; }
      public int MarkerBorderBits { get; set; }

      // Diamond properties
      public Mat Image { get; private set; }
      public Texture2D ImageTexture { get; private set; }

      void Start()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
        SquareSideLength = squareSideLength;
        MarkerSideLength = markerSideLength;
        Ids = ids;
        MarginsSize = marginsSize;
        MarkerBorderBits = markerBorderBits;

        Create();

        if (drawDiamond)
        {
          Draw(diamondPlane);

          if (saveDiamond && outputImage.Length > 0)
          {
            Save(outputImage);
          }
        }
      }

      public void Create()
      {
        Vec4i ids_vec4i = new Vec4i();
        for (int i = 0; i < Ids.Length; ++i)
        {
          ids_vec4i.Set(i, ids[i]);
        }

        Mat image;
        Functions.DrawCharucoDiamond(Dictionary, ids_vec4i, SquareSideLength, MarkerSideLength, out image, MarginsSize, MarkerBorderBits);
        Image = image;

        ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
      }

      public void Draw(GameObject boardPlane)
      {
        int boardDataSize = (int)(Image.ElemSize() * Image.Total());
        ImageTexture.LoadRawTextureData(Image.data, boardDataSize);
        ImageTexture.Apply();

        boardPlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
      }

      public void Save(string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }

  /// \} aruco_unity_package
}