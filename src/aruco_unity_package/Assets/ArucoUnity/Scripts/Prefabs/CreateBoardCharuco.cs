using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CreateBoardCharuco : MonoBehaviour
    {
      public Dictionary dictionary;

      public CharucoBoard board;
      public Utility.Mat image;
      public Utility.Size size;

      [HideInInspector]
      public Texture2D imageTexture;

      [Header("ChArUco board configuration")]
      [SerializeField]
      [Tooltip("Number of markers in X direction")]
      public int squaresNumberX;

      [SerializeField]
      [Tooltip("Number of markers in Y direction")]
      public int squaresNumberY;

      [SerializeField]
      [Tooltip("Square side length (in pixels)")]
      public int squareSideLength;

      [SerializeField]
      [Tooltip("Marker side length (in pixels)")]
      public int markerSideLength;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("Margins size (in pixels). Default is: Square Side Length - Marker Side Length")]
      public int marginsSize;

      [SerializeField]
      [Tooltip("Number of bits in marker border. Default is: 1")]
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
      private string outputImage = "ArucoUnity/charuco-board.png";

      void Start()
      {
        dictionary = Functions.GetPredefinedDictionary(dictionaryName);
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
      public void Configurate(int squaresNumberX, int squaresNumberY, int squareSideLength, int markerSideLength, Dictionary dictionary, 
        int marginsSize, int markerBorderBits)
      {
        this.squaresNumberX = squaresNumberX;
        this.squaresNumberY = squaresNumberY;
        this.squareSideLength = squareSideLength;
        this.markerSideLength = markerSideLength;
        this.dictionary = dictionary;
        this.marginsSize = marginsSize;
        this.markerBorderBits = markerBorderBits;
      }

      public void Create()
      {
        size = new Utility.Size();
        size.width = squaresNumberX * squareSideLength + 2 * marginsSize;
        size.height = squaresNumberY * squareSideLength + 2 * marginsSize;

        board = CharucoBoard.Create(squaresNumberX, squaresNumberY, squareSideLength, markerSideLength, dictionary);

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