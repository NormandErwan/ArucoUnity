using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Parameters
  {
    /// <summary>
    /// Editor controller for <see cref="CameraParameters"/>.
    /// </summary>
    public class CameraParametersController : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The folder of the camera parameters file, relative to the Application.persistentDataPath folder.")]
      private string cameraParametersFolderPath = "ArucoUnity/CameraParameters/";

      [SerializeField]
      [Tooltip("The xml file corresponding to the camera parameters.")]
      private string cameraParametersFilename;

      // Properties

      /// <summary>
      /// Gets or sets the folder of the camera parameters files, relative to the <see cref="Application.persistentDataPath"/> folder.
      /// </summary>
      public string CameraParametersFolderPath { get { return cameraParametersFolderPath; } set { SetCameraParametersFilePath(value, CameraParametersFilename); } }

      /// <summary>
      /// Gets or sets the xml file corresponding to the camera parameters, relative to <see cref="CameraParametersFolderPath"/> folder.
      /// </summary>
      public string CameraParametersFilename { get { return cameraParametersFilename; } set { SetCameraParametersFilePath(CameraParametersFolderPath, value); } }

      /// <summary>
      /// Gets the file path to the camera parameters.
      /// </summary>
      public string CameraParametersFilePath { get; protected set; }

      /// <summary>
      /// Gets the camera parameters.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      // Variables

      string dataPath;
      string outputFolderPath;

      // MonoBehaviour methods

      /// <summary>
      /// Calls <see cref="SetCameraParametersFilePath"/> then <see cref="Load"/> if <see cref="CameraParametersFilePath"/> is set.
      /// </summary>
      protected virtual void Awake()
      {
        dataPath = (Application.isEditor) ? Application.dataPath : Application.persistentDataPath;

        SetCameraParametersFilePath(CameraParametersFolderPath, CameraParametersFilename);
        if (CameraParametersFilePath != null)
        {
          Load();
        }
      }

      // MonoBehaviour methods

      /// <summary>
      /// Sets <see cref="CameraParametersFilePath"/> and tries to load <see cref="CameraParameters"/> from this file if
      /// <see cref="CameraParametersFolderPath"/> and <see cref="CameraParametersFilename"/> are set, otherwise set
      /// <see cref="CameraParametersFilePath"/> and <see cref="<see cref="CameraParameters"/> to null.
      /// </summary>
      protected virtual void SetCameraParametersFilePath(string cameraParametersFolderPath, string cameraParametersFilename)
      {
        CameraParametersFilePath = null;
        if (cameraParametersFolderPath != null && cameraParametersFolderPath.Length > 0 && cameraParametersFilename != null
          && cameraParametersFilename.Length > 0)
        {
          outputFolderPath = Path.Combine(dataPath, cameraParametersFolderPath);
          CameraParametersFilePath = Path.Combine(outputFolderPath, cameraParametersFilename);
        }
      }

      // Methods

      /// <summary>
      /// Initializes <see cref="CameraParameters"/> with <see cref="CameraParameters.CameraParameters(int)"/>
      /// </summary>
      /// <param name="cameraNumber">The number of cameras in the calibrated camera system.</param>
      public virtual void Initialize(int cameraNumber)
      {
        CameraParameters = new CameraParameters(cameraNumber);
      }

      /// <summary>
      /// Initializes <see cref="CameraParameters"/> with <see cref="CameraParameters.LoadFromXmlFile(string)"/>.
      /// </summary>
      public virtual void Load()
      {
        CameraParameters = CameraParameters.LoadFromXmlFile(CameraParametersFilePath);
      }

      /// <summary>
      /// Calls <see cref="CameraParameters.SaveToXmlFile(string)"/> with <see cref="CameraParametersFolderPath"/>. Also creates the
      /// <see cref="CameraParametersFolderPath"/> folder before if necessary.
      /// </summary>
      public virtual void Save()
      {
        if (!Directory.Exists(outputFolderPath))
        {
          Directory.CreateDirectory(outputFolderPath);
        }
        CameraParameters.SaveToXmlFile(CameraParametersFilePath);
      }
    }
  }

  /// \} aruco_unity_package
}