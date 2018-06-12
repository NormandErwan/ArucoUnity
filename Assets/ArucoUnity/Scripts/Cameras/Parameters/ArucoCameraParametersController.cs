using System;
using System.IO;
using UnityEngine;

namespace ArucoUnity.Cameras.Parameters
{
  /// <summary>
  /// Editor controller for <see cref="CameraParameters"/>.
  /// </summary>
  public class ArucoCameraParametersController : MonoBehaviour, IHasArucoCameraParameters
  {
    // Editor fields

    [SerializeField]
    [Tooltip("Automatically load the camera parameters file at start.")]
    private bool autoLoadFile = true;

    [SerializeField]
    [Tooltip("The folder of the camera parameters file, relative to the Application.persistentDataPath folder.")]
    private string cameraParametersFolderPath = "ArucoUnity/CameraParameters/";

    [SerializeField]
    [Tooltip("The xml file where to load and save the camera parameters.")]
    private string cameraParametersFilename;

    // IHasCameraParameters properties

    /// <summary>
    /// Gets or sets the camera parameters.
    /// </summary>
    public ArucoCameraParameters CameraParameters { get; set; }

    // Properties

    /// <summary>
    /// Gets or sets if automatically load the camera parameters file at start.
    /// </summary>
    public bool AutoLoadFile { get { return autoLoadFile; } set { autoLoadFile = value; } }

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
      if (AutoLoadFile)
      {
        if (CameraParametersFilePath.Length == 0)
        {
          throw new Exception("Cant't load the camera parameters file: CameraParametersFolderPath and CameraParametersFilename must be set.");
        }
        Load();
      }
    }

    // MonoBehaviour methods

    /// <summary>
    /// Sets <see cref="CameraParametersFilePath"/> and tries to load <see cref="CameraParameters"/> from this file if
    /// <see cref="CameraParametersFolderPath"/> and <see cref="CameraParametersFilename"/> are set, otherwise set
    /// <see cref="CameraParametersFilePath"/> and <see cref="CameraParameters"/> to null.
    /// </summary>
    protected virtual void SetCameraParametersFilePath(string cameraParametersFolderPath, string cameraParametersFilename)
    {
      CameraParametersFilePath = "";
      if (cameraParametersFolderPath != null && cameraParametersFolderPath.Length > 0
        && cameraParametersFilename != null && cameraParametersFilename.Length > 0)
      {
        outputFolderPath = Path.Combine(dataPath, cameraParametersFolderPath);
        CameraParametersFilePath = Path.Combine(outputFolderPath, cameraParametersFilename);
      }
    }

    // Methods

    /// <summary>
    /// Initializes <see cref="CameraParameters"/> with <see cref="ArucoCameraParameters.ArucoCameraParameters(int)"/>
    /// </summary>
    /// <param name="cameraNumber">The number of cameras in the calibrated camera system.</param>
    public virtual void Initialize(int cameraNumber)
    {
      CameraParameters = new ArucoCameraParameters(cameraNumber);
    }

    /// <summary>
    /// Initializes <see cref="CameraParameters"/> with <see cref="ArucoCameraParameters.LoadFromXmlFile(string)"/>.
    /// </summary>
    public virtual void Load()
    {
      CameraParameters = ArucoCameraParameters.LoadFromXmlFile(CameraParametersFilePath);
    }

    /// <summary>
    /// Calls <see cref="ArucoCameraParameters.SaveToXmlFile(string)"/> with <see cref="CameraParametersFolderPath"/>. Also creates the
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