using UnityEngine;
using System.IO;
using ArucoUnity.Objects;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Create and display images of an ArUco object ready to be printed.
    /// 
    /// See the OpenCV documentation for more information about the marker creation (second section of the following tutorial):
    /// http://docs.opencv.org/3.2.0/d5/dae/tutorial_aruco_detection.html
    /// </summary>
    [ExecuteInEditMode]
    public class ArucoCreator : ArucoObjectDisplayer
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Save the created image.")]
      private bool saveImage = false;

      [SerializeField]
      [Tooltip("The output folder for the saved image, relative to the Application.persistentDataPath folder.")]
      private string outputFolder = "ArucoUnity/Images/";

      [SerializeField]
      [Tooltip("The saved image name. The extension (.png) is added automatically. If empty, it will be generated automatically from the ArUco object.")]
      private string optionalImageFilename;

      // Properties

      /// <summary>
      /// Save the image.
      /// </summary>
      public bool SaveImage { get { return saveImage; } set { saveImage = value; } }

      /// <summary>
      /// The output folder for the saved image, relative to the Application.persistentDataPath folder (default: ArucoUnity/Images/).
      /// </summary>
      public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }

      /// <summary>
      /// The saved image name. The extension (.png) is added automatically. If null, it will be generated automatically.
      /// </summary>
      public string ImageFilename { get { return optionalImageFilename; } set { optionalImageFilename = value; } }

      // Methods

      /// <summary>
      /// Save the <see cref="ImageTexture"/> on a image file in the <see cref="OutputFolder"/> with <see cref="ImageFilename"/> as filename.
      /// </summary>
      public virtual void Save()
      {
        if (ImageFilename == null || ImageFilename.Length == 0)
        {
          ImageFilename = GenerateImageFilename();
        }

        string outputFolderPath = Path.Combine((Application.isEditor) ? Application.dataPath : Application.persistentDataPath, OutputFolder);
        if (!Directory.Exists(outputFolderPath))
        {
          Directory.CreateDirectory(outputFolderPath);
        }

        string imageFilePath = outputFolderPath + ImageFilename;
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }

      /// <summary>
      /// Returns a generated filemame with the <see cref="ArucoObject"/> properties.
      /// </summary>
      public virtual string GenerateImageFilename()
      {
        string imageFilename = "ArUcoUnity_";

        ArucoMarker marker = ArucoObject as ArucoMarker;
        if (marker != null)
        {
          imageFilename += "Marker_" + marker.Dictionary.Name + "_Id_" + marker.MarkerId;
        }

        ArucoGridBoard gridBoard = ArucoObject as ArucoGridBoard;
        if (gridBoard != null)
        {
          imageFilename += "GridBoard_" + gridBoard.Dictionary.Name + "_X_" + gridBoard.MarkersNumberX + "_Y_" + gridBoard.MarkersNumberY
            + "_MarkerSize_" + gridBoard.MarkerSideLength;
        }

        ArucoCharucoBoard charucoBoard = ArucoObject as ArucoCharucoBoard;
        if (charucoBoard != null)
        {
          imageFilename += "ChArUcoBoard_" + charucoBoard.Dictionary.Name + "_X_" + charucoBoard.SquaresNumberX
            + "_Y_" + charucoBoard.SquaresNumberY + "_SquareSize_" + charucoBoard.SquareSideLength
            + "_MarkerSize_" + charucoBoard.MarkerSideLength;
        }

        ArucoDiamond diamond = ArucoObject as ArucoDiamond;
        if (diamond != null && diamond.Ids.Length == 4)
        {
          imageFilename += "DiamondMarker_" + diamond.Dictionary.Name + "_Ids_" + diamond.Ids[0] + "_" + diamond.Ids[1] + "_" + diamond.Ids[2] + "_"
            + diamond.Ids[3] + "_SquareSize_" + diamond.SquareSideLength + "_MarkerSize_" + diamond.MarkerSideLength;
        }

        imageFilename += ".png";

        return imageFilename;
      }

      /// <summary>
      /// Calls <see cref="Create"/>, <see cref="Display"/> and <see cref="Save"/>.
      /// </summary>
      protected override void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
      {
        base.ArucoObject_PropertyUpdated(arucoObject);

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
#endif
          if (SaveImage)
          {
            Save();
          }
#if UNITY_EDITOR
        }
#endif
      }
    }
  }

  /// \} aruco_unity_package
}