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
    /// Create an ArUco grid board image and a texture of the board.
    /// </summary>
    public class CreateBoard : ArucoCreator
    {
      // Editor fields

      [Header("Board configuration")]
      [SerializeField]
      [Tooltip("Number of markers in the X direction")]
      public int markersNumberX;

      [SerializeField]
      [Tooltip("Number of markers in the Y direction")]
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

      // Properties

      /// <summary>
      /// Number of markers in the X direction.
      /// </summary>
      public int MarkersNumberX { get { return markersNumberX; } set { markersNumberX = value; } }

      /// <summary>
      /// Number of markers in the Y direction.
      /// </summary>
      public int MarkersNumberY { get { return markersNumberY; } set { markersNumberY = value; } }

      /// <summary>
      /// Marker side length (in pixels).
      /// </summary>
      public int MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }

      /// <summary>
      /// Separation between two consecutive markers in the grid (in pixels).
      /// </summary>
      public int MarkerSeparation { get { return markerSeparation; } set { markerSeparation = value; } }

      /// <summary>
      /// Margins size (in pixels). Default is equal to <see cref="MarkerSeparation"/>".
      /// </summary>
      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }

      /// <summary>
      /// Number of bits in marker borders.
      /// </summary>
      public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

      /// <summary>
      /// The generated grid board.
      /// </summary>
      public GridBoard Board { get; private set; }

      /// <summary>
      /// The size of the <see cref="Board"/>.
      /// </summary>
      public Size Size { get; private set; }

      // MonoBehaviour methods

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
        Size.width = MarkersNumberX * (MarkerSideLength + MarkerSeparation) - MarkerSeparation + 2 * MarginsSize;
        Size.height = MarkersNumberY * (MarkerSideLength + MarkerSeparation) - MarkerSeparation + 2 * MarginsSize;

        Board = GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);

        Mat image;
        Board.Draw(Size, out image, MarginsSize, MarkerBorderBits);
        Image = image;

        ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
      }
    }
  }

  /// \} aruco_unity_package
}