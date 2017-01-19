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
    /// Create a ChArUco marker image and a texture of the marker.
    /// </summary>
    public class CreateDiamond : ArucoCreator
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
      [Tooltip("The image file path, relative to the project file path")]
      private string outputImage = "ArucoUnity/diamond-marker.png";

      // Properties

      // Configuration properties
      public int SquareSideLength { get { return squareSideLength; } set { squareSideLength = value; } }
      public int MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }
      public int[] Ids { get { return ids; } set { ids = value; } }
      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }
      public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

      // MonoBehaviour methods

      /// <summary>
      /// Set the dictionary, create the marker image and the texture and, if needed, draw the texture and save it to a image file.
      /// </summary>
      void Start()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

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

      // Methods

      /// <summary>
      /// Create the marker and the <see cref="ImageTexture"/> of the marker.
      /// </summary>
      public override void Create()
      {
        Vec4i ids_vec4i = new Vec4i();
        for (int i = 0; i < ids.Length; ++i)
        {
          ids_vec4i.Set(i, ids[i]);
        }

        Mat image;
        Functions.DrawCharucoDiamond(Dictionary, ids_vec4i, SquareSideLength, MarkerSideLength, out image, MarginsSize, MarkerBorderBits);
        Image = image;

        ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
      }
    }
  }

  /// \} aruco_unity_package
}