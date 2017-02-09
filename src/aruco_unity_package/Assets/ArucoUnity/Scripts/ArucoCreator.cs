using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
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
      [Tooltip("The output folder for the saved image, relative to the Assets/ folder.")]
      private string outputFolder = "ArucoUnity/Images/";

      [SerializeField]
      [Tooltip("The saved image name. If empty, it will be generated automatically.")]
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
      /// The output folder for the saved image, relative to the Assets/ folder (default: ArucoUnity/Images/).
      /// </summary>
      public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }

      /// <summary>
      /// The saved image name. If null, it will be generated automatically.
      /// </summary>
      public string ImageFilename { get { return imageFilename; } set { imageFilename = value; } }

      /// <summary>
      /// The created image of the <see cref="ArucoObject"/>.
      /// </summary>
      public Mat Image { get; protected set; }

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
        Mat image = null;
        ImageTexture = null;

        // In case of a marker
        ArucoMarker marker = ArucoObject as ArucoMarker;
        if (marker != null)
        {
          marker.Dictionary.DrawMarker(marker.Id, (int)marker.MarkerSideLength, out image, marker.MarkerBorderBits);
        }

        // In case of a grid board
        ArucoGridBoard gridBoard = ArucoObject as ArucoGridBoard;
        if (gridBoard != null)
        {
          gridBoard.Board.Draw(gridBoard.ImageSize, out image, gridBoard.MarginsSize, gridBoard.MarkerBorderBits);
        }

        // In case of a charuco board
        ArucoCharucoBoard charucoBoard = ArucoObject as ArucoCharucoBoard;
        if (charucoBoard != null)
        {
          charucoBoard.Board.Draw(charucoBoard.ImageSize, out image, charucoBoard.MarginsSize, charucoBoard.MarkerBorderBits);
        }

        // In case of a diamond
        ArucoDiamond diamond = ArucoObject as ArucoDiamond;
        if (diamond != null && diamond.Ids.Length == 4)
        {
          Vec4i ids = new Vec4i();
          for (int i = 0; i < diamond.Ids.Length; ++i)
          {
            ids.Set(i, diamond.Ids[i]);
          }
          Functions.DrawCharucoDiamond(diamond.Dictionary, ids, diamond.SquareSideLength, (int)diamond.MarkerSideLength, out image);
        }

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
        ImagePlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
      }

      /// <summary>
      /// Save the <see cref="ImageTexture"/> on a image file in the <see cref="OutputFolder"/>. Use <see cref="ImageFilename"/> as filename is
      /// specified or generate one automatically.
      /// </summary>
      public virtual void Save()
      {
        string outputImage = OutputFolder + ImageFilename;
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }

  /// \} aruco_unity_package
}