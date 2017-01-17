using System.IO;
using UnityEngine;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    /// <summary>
    /// Create an ArUco marker image and a texture of the marker.
    /// </summary>
    public class CreateMarker : MonoBehaviour
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

      // Configuration properties
      /// <summary>
      /// The dictionnary to use.
      /// </summary>
      public Dictionary Dictionary { get; set; }

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

      // Marker properties
      /// <summary>
      /// The generated image of the marker.
      /// </summary>
      public Mat Image { get; private set; }

      /// <summary>
      /// The generated texture of the marker.
      /// </summary>
      public Texture2D ImageTexture { get; private set; }

      // MonoBehaviour methods

      /// <summary>
      /// Set the dictionary, create the marker image and the texture and, if needed, draw the texture and save it to a image file.
      /// </summary>
      void Start()
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
      /// Create the marker and the <see cref="ImageTexture"/> of the marker.
      /// </summary>
      public void Create()
      {
        Mat image = new Mat();
        Dictionary.DrawMarker(markerId, markerSize, ref image, markerBorderBits);
        Image = image;

        ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
      }

      /// <summary>
      /// Draw the <see cref="ImageTexture"/> of the marker on the markerPlane.
      /// </summary>
      /// <param name="markerPlane">The object where the <see cref="ImageTexture"/> is drawn.</param>
      public void Draw(GameObject markerPlane)
      {
        int markerDataSize = (int)(Image.ElemSize() * Image.Total());
        ImageTexture.LoadRawTextureData(Image.data, markerDataSize);
        ImageTexture.Apply();

        markerPlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
      }

      /// <summary>
      /// Save the <see cref="ImageTexture"/> on a image file.
      /// </summary>
      /// <param name="outputImage">The image file path, relative to the project file path.</param>
      public void Save(string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }

  /// \} aruco_unity_package
}