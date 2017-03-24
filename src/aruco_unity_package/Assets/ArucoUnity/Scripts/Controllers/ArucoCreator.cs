using ArucoUnity.Plugin;
using UnityEngine;
using System.IO;
using ArucoUnity.Objects;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    public class ArucoCreator : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The ArUco object to create.")]
      protected ArucoObject arucoObject;

      [SerializeField]
      [Tooltip("Create the image and the image texture automatically at start.")]
      private bool createAtStart;

      [SerializeField]
      [Tooltip("Display the created image.")]
      private bool drawImage;

      [SerializeField]
      [Tooltip("The plane where to display the created image.")]
      private GameObject imagePlane;

      [SerializeField]
      [Tooltip("Save the created image.")]
      private bool saveImage;

      [SerializeField]
      [Tooltip("The output folder for the saved image, relative to the Application.persistentDataPath folder.")]
      private string outputFolder = "ArucoUnity/Images/";

      [SerializeField]
      [Tooltip("The saved image name. The extension (.png) is added automatically. If empty, it will be generated automatically from the ArUco object.")]
      private string imageFilename;

      // Properties

      /// <summary>
      /// The ArUco object to create.
      /// </summary>
      protected ArucoObject ArucoObject { get { return arucoObject; } set { arucoObject = value; } }

      /// <summary>
      /// Create the image and the image texture automatically at start.
      /// </summary>
      public bool CreateAtStart { get { return createAtStart; } set { createAtStart = value; } }

      /// <summary>
      /// Display the created image.
      /// </summary>
      public bool DrawImage { get { return drawImage; } set { drawImage = value; } }

      /// <summary>
      /// The plane where to display the created image.
      /// </summary>
      public GameObject ImagePlane { get { return imagePlane; } set { imagePlane = value; } }

      /// <summary>
      /// Save the created image.
      /// </summary>
      public bool SaveImage { get { return saveImage; } set { saveImage = value; } }

      /// <summary>
      /// The output folder for the saved image, relative to the Application.persistentDataPath folder (default: ArucoUnity/Images/).
      /// </summary>
      public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }

      /// <summary>
      /// The saved image name. The extension (.png) is added automatically. If null, it will be generated automatically.
      /// </summary>
      public string ImageFilename { get { return imageFilename; } set { imageFilename = value; } }

      /// <summary>
      /// The created image of the <see cref="ArucoObject"/>.
      /// </summary>
      public Cv.Mat Image { get; protected set; }

      /// <summary>
      /// The created texture of the <see cref="ArucoObject"/>.
      /// </summary>
      public Texture2D ImageTexture { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Create, draw and save the image of the <see cref="ArucoObject"/> at start if specified by the user.
      /// </summary>
      protected virtual void Start()
      {
        if (CreateAtStart && ArucoObject)
        {
          Create();

          if (DrawImage)
          {
            Draw();
          }

          if (SaveImage)
          {
            Save();
          }
        }
      }

      // Methods

      /// <summary>
      /// Create the image and the image texture of the <see cref="ArucoObject"/>.
      /// </summary>
      public virtual void Create()
      {
        Cv.Mat image = null;
        ImageTexture = null;

        // In case of a marker
        ArucoMarker marker = ArucoObject as ArucoMarker;
        if (marker != null)
        {
          marker.Dictionary.DrawMarker(marker.MarkerId, (int)marker.MarkerSideLength, out image, marker.MarkerBorderBits);
        }

        // In case of a grid board
        ArucoGridBoard arucoGridBoard = ArucoObject as ArucoGridBoard;
        if (arucoGridBoard != null)
        {
          Aruco.GridBoard gridBoard = arucoGridBoard.Board as Aruco.GridBoard;
          gridBoard.Draw(arucoGridBoard.ImageSize, out image, arucoGridBoard.MarginsSize, arucoGridBoard.MarkerBorderBits);
        }

        // In case of a charuco board
        ArucoCharucoBoard arucoCharucoBoard = ArucoObject as ArucoCharucoBoard;
        if (arucoCharucoBoard != null)
        {
          Aruco.CharucoBoard charucoBoard = arucoCharucoBoard.Board as Aruco.CharucoBoard;
          charucoBoard.Draw(arucoCharucoBoard.ImageSize, out image, arucoCharucoBoard.MarginsSize, arucoCharucoBoard.MarkerBorderBits);
        }

        // In case of a diamond
        ArucoDiamond diamond = ArucoObject as ArucoDiamond;
        if (diamond != null && diamond.Ids.Length == 4)
        {
          Cv.Vec4i ids = new Cv.Vec4i();
          for (int i = 0; i < diamond.Ids.Length; ++i)
          {
            ids.Set(i, diamond.Ids[i]);
          }
          Aruco.DrawCharucoDiamond(diamond.Dictionary, ids, (int)diamond.SquareSideLength, (int)diamond.MarkerSideLength, out image);
        }

        // Vertical flip to convert the image from Unity's left-handed coordinate system to OpenCV's right-handed coordinate system
        int verticalFlipCode = 0;
        Cv.Flip(image, image, verticalFlipCode);

        // Set the properties
        Image = image;
        if (Image != null)
        {
          int markerDataSize = (int)(Image.ElemSize() * Image.Total());
          ImageTexture = new Texture2D(Image.cols, Image.rows, TextureFormat.RGB24, false);
          ImageTexture.LoadRawTextureData(Image.dataIntPtr, markerDataSize);
          ImageTexture.Apply();
        }
      }

      /// <summary>
      /// Draw the <see cref="ImageTexture"/> on the <see cref="ImagePlane"/>.
      /// </summary>
      public virtual void Draw()
      {
        ImagePlane.SetActive(true);
        ImagePlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
      }

      /// <summary>
      /// Save the <see cref="ImageTexture"/> on a image file in the <see cref="OutputFolder"/>. Use <see cref="ImageFilename"/> as filename is
      /// specified or generate one automatically.
      /// </summary>
      public virtual void Save()
      {
        string imageFilePath = ImageFilename;
        if (imageFilePath == null || imageFilePath.Length == 0)
        {
          imageFilePath = "ArUcoUnity_";

          // In case of a marker
          ArucoMarker marker = ArucoObject as ArucoMarker;
          if (marker != null)
          {
            imageFilePath += "Marker_" + marker.Dictionary.name + "_Id_" + marker.MarkerId;
          }

          // In case of a grid board
          ArucoGridBoard gridBoard = ArucoObject as ArucoGridBoard;
          if (gridBoard != null)
          {
            imageFilePath += "GridBoard_" + gridBoard.Dictionary.name + "_X_" + gridBoard.MarkersNumberX + "_Y_" + gridBoard.MarkersNumberY
              + "_MarkerSize_" + gridBoard.MarkerSideLength;
          }

          // In case of a charuco board
          ArucoCharucoBoard charucoBoard = ArucoObject as ArucoCharucoBoard;
          if (charucoBoard != null)
          {
            imageFilePath += "ChArUcoBoard_" + charucoBoard.Dictionary.name + "_X_" + charucoBoard.SquaresNumberX
              + "_Y_" + charucoBoard.SquaresNumberY + "_SquareSize_" + charucoBoard.SquareSideLength
              + "_MarkerSize_" + charucoBoard.MarkerSideLength;
          }

          // In case of a diamond
          ArucoDiamond diamond = ArucoObject as ArucoDiamond;
          if (diamond != null && diamond.Ids.Length == 4)
          {
            imageFilePath += "DiamondMarker_" + diamond.Dictionary.name + "_Ids_" + diamond.Ids[0] + "_" + diamond.Ids[1] + "_" + diamond.Ids[2] + "_"
              + diamond.Ids[3] + "_SquareSize_" + diamond.SquareSideLength + "_MarkerSize_" + diamond.MarkerSideLength;
          }
        }
        imageFilePath += ".png";

        string outputFolderPath = Path.Combine((Application.isEditor) ? Application.dataPath : Application.persistentDataPath, OutputFolder);
        if (!Directory.Exists(outputFolderPath))
        {
          Directory.CreateDirectory(outputFolderPath);
        }

        imageFilePath = outputFolderPath + imageFilePath;
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }

  /// \} aruco_unity_package
}