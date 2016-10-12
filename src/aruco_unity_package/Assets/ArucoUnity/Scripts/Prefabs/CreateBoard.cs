using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CreateBoard : MonoBehaviour
    {
      public GridBoard board;

      public Utility.Mat boardImage;

      public Utility.Size boardSize;

      [Header("Board configuration")]
      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("Number of markers in X direction")]
      private int markersX;

      [SerializeField]
      [Tooltip("Number of markers in Y direction")]
      private int markersY;

      [SerializeField]
      [Tooltip("Marker side length (in pixels)")]
      private int markerLength;

      [SerializeField]
      [Tooltip("Separation between two consecutive markers in the grid (in pixels)")]
      private int markerSeparation;

      [SerializeField]
      [Tooltip("Margins size (in pixels). Default is marker separation")]
      private int margins;

      [SerializeField]
      [Tooltip("Number of bits in marker borders")]
      private int markerBorderBits;

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
        board = Create(dictionaryName, markersX, markersY, markerLength, markerSeparation, margins, markerBorderBits, out boardImage, out boardSize);

        if (drawBoard)
        {
          Texture2D boardTexture = CreateTexture(boardImage);
          Draw(boardImage, boardPlane, boardTexture);

          if (saveBoard && outputImage.Length > 0)
          {
            Save(boardTexture, outputImage);
          }
        }
      }

      public GridBoard Create(PREDEFINED_DICTIONARY_NAME dictionaryName, int markersX, int markersY, int markerLength, int markerSeparation, 
        int margins, int markerBorderBits, out Utility.Mat boardImage, out Utility.Size boardSize)
      {
        boardSize = new Utility.Size();
        boardSize.width = markersX * (markerLength + markerSeparation) - markerSeparation + 2 * margins;
        boardSize.height = markersY * (markerLength + markerSeparation) - markerSeparation + 2 * margins;

        Dictionary dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        GridBoard board = GridBoard.Create(markersX, markersY, markerLength, markerSeparation, dictionary);

        boardImage = new Utility.Mat();
        board.Draw(boardSize, ref boardImage, margins, markerBorderBits);

        return board;
      }

      public Texture2D CreateTexture(Utility.Mat boardImage)
      {
        return new Texture2D(boardImage.cols, boardImage.rows, TextureFormat.RGB24, false);
      }

      public void Draw(Utility.Mat boardImage, GameObject boardPlane, Texture2D boardTexture)
      {
        int boardDataSize = (int)(boardImage.ElemSize() * boardImage.Total());
        boardTexture.LoadRawTextureData(boardImage.data, boardDataSize);
        boardTexture.Apply();

        boardPlane.GetComponent<Renderer>().material.mainTexture = boardTexture;
      }

      public void Save(Texture2D boardTexture, string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, boardTexture.EncodeToPNG());
      }
    }
  }
}