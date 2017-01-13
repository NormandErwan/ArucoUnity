using System.IO;
using UnityEngine;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    public class CreateBoard : MonoBehaviour
    {
      [Header("Board configuration")]
      [SerializeField]
      [Tooltip("Number of markers in X direction")]
      public int markersNumberX;

      [SerializeField]
      [Tooltip("Number of markers in Y direction")]
      public int markersNumberY;

      [SerializeField]
      [Tooltip("Marker side length (in pixels)")]
      public int markerSideLength;

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

      // Configuration properties
      public int MarkersNumberX { get; set; }
      public int MarkersNumberY { get; set; }
      public int MarkerSideLength { get; set; }
      public int MarkerSeparation { get; set; }
      public Dictionary Dictionary { get; set; }
      public int MarginsSize { get; set; }
      public int MarkerBorderBits { get; set; }

      // Board properties
      public GridBoard Board { get; private set; }
      public Mat Image { get; private set; }
      public Size Size { get; private set; }
      public Texture2D ImageTexture { get; private set; }

      void Start()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
        MarkersNumberX = markersNumberX;
        MarkersNumberY = markersNumberY;
        MarkerSideLength = markerSideLength;
        MarkerSeparation = markerSeparation;
        MarginsSize = marginsSize;
        MarkerBorderBits = markerBorderBits;

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

      public void Create()
      {
        Size = new Size();
        Size.width = markersNumberX * (markerSideLength + markerSeparation) - markerSeparation + 2 * marginsSize;
        Size.height = markersNumberY * (markerSideLength + markerSeparation) - markerSeparation + 2 * marginsSize;

        Board = GridBoard.Create(markersNumberX, markersNumberY, markerSideLength, markerSeparation, Dictionary);

        Mat image;
        Board.Draw(Size, out image, marginsSize, markerBorderBits);
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