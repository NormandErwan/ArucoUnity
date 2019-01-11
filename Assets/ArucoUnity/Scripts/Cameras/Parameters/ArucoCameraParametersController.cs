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
        // Constants

        /// <summary>
        /// The folder where to load and save the <see cref="CameraParameters"/> files, relative to
        /// <see cref="Application.streamingAssetsPath"/>.
        /// </summary>
        protected const string CameraParametersFolderPath = "ArucoUnity";

        // Editor fields

        [SerializeField]
        [Tooltip("Automatically load the camera parameters file at start.")]
        private bool autoLoadFile = true;

        [SerializeField]
        [Tooltip("The xml file where to load and save the camera parameters, relative to Streaming Assets.")]
        private string cameraParametersFilename;

        // IHasArucoCameraParameters properties

        /// <summary>
        /// Gets or sets the <see cref="CameraParameters"/>.
        /// </summary>
        public ArucoCameraParameters CameraParameters { get; set; }

        // Properties

        /// <summary>
        /// Gets or sets if automatically <see cref="Load"/> at <see cref="Awake"/>.
        /// </summary>
        public bool AutoLoadFile { get { return autoLoadFile; } set { autoLoadFile = value; } }

        /// <summary>
        /// Gets or sets the xml file corresponding to the <see cref="CameraParameters"/>, relative to <see cref="CameraParametersFolderPath"/>.
        /// </summary>
        public string CameraParametersFilename { get { return cameraParametersFilename; } set { cameraParametersFilename = value; } }

        // Variables

        protected string cameraParametersPath;

        // MonoBehaviour methods

        /// <summary>
        /// Calls <see cref="Load"/> if <see cref="AutoLoadFile"/> is <c>true</c>.
        /// </summary>
        protected virtual void Awake()
        {
            cameraParametersPath = Path.Combine(Application.streamingAssetsPath, CameraParametersFolderPath, cameraParametersFilename);

            if (AutoLoadFile)
            {
                Load();
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
            CameraParameters = ArucoCameraParameters.LoadFromXmlFile(cameraParametersPath);
        }

        /// <summary>
        /// Calls <see cref="ArucoCameraParameters.SaveToXmlFile(string)"/>. Creates before the
        /// <see cref="CameraParametersFolderPath"/> folder before if necessary.
        /// </summary>
        public virtual void Save()
        {
            if (!Directory.Exists(CameraParametersFolderPath))
            {
                Directory.CreateDirectory(CameraParametersFolderPath);
            }
            CameraParameters.SaveToXmlFile(cameraParametersPath);
        }
    }
}