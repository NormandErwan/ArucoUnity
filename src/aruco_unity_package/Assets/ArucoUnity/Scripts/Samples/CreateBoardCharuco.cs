using UnityEngine;
using ArucoUnity.Utility.cv;
using ArucoUnity.Samples.Utility;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    /// <summary>
    /// Create an ChArUco grid board image and a texture of the board.
    /// </summary>
    public class CreateBoardCharuco : MarkerCreator
    {
      // Editor fields

      [Header("ChArUco board configuration")]
      [SerializeField]
      [Tooltip("Number of squares in the X direction")]
      public int squaresNumberX;

      [SerializeField]
      [Tooltip("Number of squares in the Y direction")]
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

      // Properties

      /// <summary>
      /// Number of markers in the X direction.
      /// </summary>
      public int SquaresNumberX { get { return squaresNumberX; } set { squaresNumberX = value; } }

      /// <summary>
      /// Number of markers in the Y direction.
      /// </summary>
      public int SquaresNumberY { get { return squaresNumberY; } set { squaresNumberY = value; } }

      /// <summary>
      /// Square side length (in pixels).
      /// </summary>
      public int SquareSideLength { get { return squareSideLength; } set { squareSideLength = value; } }

      /// <summary>
      /// Marker side length (in pixels).
      /// </summary>
      public int MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }

      /// <summary>
      /// Margins size (in pixels). Default is: <see cref="SquareSideLength"/> - <see cref="MarkerSideLength"/>.
      /// </summary>
      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }

      /// <summary>
      /// Number of bits in marker borders.
      /// </summary>
      public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

      /// <summary>
      /// The generated grid board.
      /// </summary>
      public CharucoBoard Board { get; private set; }

      /// <summary>
      /// The size of the <see cref="Board"/>.
      /// </summary>
      public Size Size { get; private set; }

      /// <summary>
      /// Set the dictionary, create the grid board image and the texture and, if needed, draw the texture and save it to a image file.
      /// </summary>
      void Start()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

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

      // Methods

      /// <summary>
      /// Create the <see cref="Board"/>, the grid board image and the <see cref="ImageTexture"/> of the grid board.
      /// </summary>
      public override void Create()
      {
        Size = new Size();
        Size.width = SquaresNumberX * SquareSideLength + 2 * MarginsSize;
        Size.height = SquaresNumberY * SquareSideLength + 2 * MarginsSize;

        Board = CharucoBoard.Create(SquaresNumberX, SquaresNumberY, SquareSideLength, MarkerSideLength, Dictionary);

        Mat image;
        Board.Draw(Size, out image, MarginsSize, MarkerBorderBits);
        Image = image;

        ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
      }
    }
  }

  /// \} aruco_unity_package
}