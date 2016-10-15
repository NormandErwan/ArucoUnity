using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CreateBoard : MonoBehaviour
    {
      public Dictionary dictionary;

      public GridBoard board;
      public Utility.Mat image;
      public Utility.Size size;

      [HideInInspector]
      public Texture2D imageTexture;

      [Header("Board configuration")]
      [SerializeField]
      [Tooltip("Number of markers in X direction")]
      public int markersNumberX;

      [SerializeField]
      [Tooltip("Number of markers in Y direction")]
      public int markersNumberY;

      [SerializeField]
      [Tooltip("Marker side length (in pixels)")]
      public int markerLength;

      [SerializeField]
      [Tooltip("Separation between two consecutive markers in the grid (in pixels)")]
      public int markerSeparation;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("Margins size (in pixels). Default is marker separation")]
      public int marginsSize;

      [SerializeField]
      [Tooltip("Number of bits in marker borders")]
      public int markerBorderBits;

      [Header("Draw the board")]
      [SerializeField]
      private bool drawBoard;

      [SerializeField]
      private GameObject boardPlane;

      [Header("Save the board")]
      [SerializeField]
      [Tooltip("Save the generated image")]
      private bool saveBoard;

      [SerializeField]
      [Tooltip("Output image")]
      private string outputImage = "ArucoUnity/board.png";

      void Start()
      {
        dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        Create();

        if (drawBoard)
        {
          Draw(boardPlane);

          if (saveBoard && outputImage.Length > 0)
          {
            Save(outputImage);
          }
        }
      }

      // Call it first if you're using the Script alone, not with the Prefab.
      public void Configurate(Dictionary dictionary, int markersNumberX, int markersNumberY, int markerLength, int markerSeparation, int marginsSize, int markerBorderBits)
      {
        this.dictionary = dictionary;
        this.markersNumberX = markersNumberX;
        this.markersNumberY = markersNumberY;
        this.markerLength = markerLength;
        this.markerSeparation = markerSeparation;
        this.marginsSize = marginsSize;
        this.markerBorderBits = markerBorderBits;
      }

      public void Create()
      {
        size = new Utility.Size();
        size.width = markersNumberX * (markerLength + markerSeparation) - markerSeparation + 2 * marginsSize;
        size.height = markersNumberY * (markerLength + markerSeparation) - markerSeparation + 2 * marginsSize;

        GridBoard board = GridBoard.Create(markersNumberX, markersNumberY, markerLength, markerSeparation, dictionary);

        board.Draw(size, out image, marginsSize, markerBorderBits);

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