using UnityEngine;
using System.IO;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects.Displayers
  {
    /// <summary>
    /// Create and display images of an ArUco object ready to be printed.
    /// 
    /// See the OpenCV documentation for more information about the marker creation (second section of the following tutorial):
    /// http://docs.opencv.org/3.2.0/d5/dae/tutorial_aruco_detection.html
    /// </summary>
    public class ArucoObjectCreator : ArucoObjectDisplayer
    {
      // Constants

      public const float pixelsToMetersFactor = 0.001f;

      // Editor fields

      [SerializeField]
      [Tooltip("Save the image in play mode.")]
      private bool saveImage = true;

      [SerializeField]
      [Tooltip("The output folder for the image, relative to the Application.persistentDataPath folder.")]
      private string outputFolder = "ArucoUnity/Images/";

      [SerializeField]
      [Tooltip("Set automatically the image filename based from the ArUco object's property values.")]
      private bool automaticFilename = true;

      [SerializeField]
      [Tooltip("The name of the image, without the extension (.png added automatically).")]
      private string imageFilename;

      // Properties

      /// <summary>
      /// Gets or sets if the <see cref="ArucoObjectDisplayer.ImageTexture"/> is saved in play mode.
      /// </summary>
      public bool SaveImage { get { return saveImage; } set { saveImage = value; } }

      /// <summary>
      /// Gets or sets the output folder for the image, relative to the Application.persistentDataPath folder
      /// (default: ArucoUnity/Images/).
      /// </summary>
      public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }

      /// <summary>
      /// Gets or sets the name of the image, without the extension (.png added automatically). If null, it will be
      /// generated automatically.
      /// </summary>
      public string ImageFilename { get { return imageFilename; } set { imageFilename = value; } }

      // ArucoObjectDisplayer methods

      /// <summary>
      /// Calls <see cref="ArucoObjectDisplayer.Create"/>, <see cref="ArucoObjectDisplayer.Display"/> and
      /// <see cref="ArucoObjectDisplayer.Save"/>.
      /// </summary>
      protected override void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
      {
        base.ArucoObject_PropertyUpdated(arucoObject);

#if UNITY_EDITOR
        if (automaticFilename)
        {
          ImageFilename = ArucoObject.GenerateName();
        }
#endif

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

      protected override void PlaceImagePlane()
      {
        base.PlaceImagePlane();
        if (ImagePlane != null && ArucoObject != null)
        {
          var scale = ArucoObject.GetGameObjectScale();
          ImagePlane.transform.localScale = pixelsToMetersFactor * new Vector3(scale.x, scale.z, scale.y);
        }
      }

      // Methods

      /// <summary>
      /// Save the <see cref="ImageTexture"/> on a image file in the <see cref="OutputFolder"/> with
      /// <see cref="ImageFilename"/> as filename.
      /// </summary>
      public virtual void Save()
      {
        if (ImageFilename == null || ImageFilename.Length == 0)
        {
          ImageFilename = ArucoObject.GenerateName() + ".png";
        }

        string outputFolderPath = Path.Combine((Application.isEditor) ? Application.dataPath
          : Application.persistentDataPath, OutputFolder);
        if (!Directory.Exists(outputFolderPath))
        {
          Directory.CreateDirectory(outputFolderPath);
        }

        string imageFilePath = outputFolderPath + ImageFilename;
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }

  /// \} aruco_unity_package
}