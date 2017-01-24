using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  /// <summary>
  /// Create an ArUco marker image and a texture of the marker.
  /// </summary>
  public class CreateMarker : ArucoCreator
  {
    // Editor fields

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
    [Tooltip("The image file path, relative to the project file path")]
    private string outputImage = "ArucoUnity/marker.png";

    // Properties

    /// <summary>
    /// The marker id in the <see cref="Dictionary"/>.
    /// </summary>
    public int MarkerId { get { return markerId; } set { markerId = value; } }

    /// <summary>
    /// Marker size in pixels (default: 200).
    /// </summary>
    public int MarkerSize { get { return markerSize; } set { markerSize = value; } }

    /// <summary>
    /// Number of bits in marker borders (default: 1).
    /// </summary>
    public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

    // MonoBehaviour methods

    /// <summary>
    /// Set the dictionary, create the marker image and the texture and, if needed, draw the texture and save it to a image file.
    /// </summary>
    protected virtual void Start()
    {
      Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

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

    // Methods

    /// <summary>
    /// Create the marker image and the <see cref="ImageTexture"/> of the marker.
    /// </summary>
    public override void Create()
    {
      Mat image = new Mat();
      Dictionary.DrawMarker(MarkerId, MarkerSize, ref image, MarkerBorderBits);
      Image = image;

      ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
    }
  }

  /// \} aruco_unity_package
}