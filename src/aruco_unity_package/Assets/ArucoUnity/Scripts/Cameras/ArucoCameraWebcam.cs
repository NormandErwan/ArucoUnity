using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Manages any connected webcam to the machine, retrieves and displays the camera's image every frame. Use one webcam at a time.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ArucoCameraWebcam : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of the webcam to use.")]
      private int webcamId = 0;

      [SerializeField]
      [Tooltip("The file path to load the camera parameters.")]
      private string cameraParametersFilePath;

      // ArucoCamera properties implementation

      /// <summary>
      /// <see cref="ArucoCamera.CameraNumber"/>
      /// </summary>
      public override int CameraNumber { get { return 1; } protected set { } }

      /// <summary>
      /// <see cref="ArucoCamera.Name"/>
      /// </summary>
      public override string Name { get; protected set; }

      // Properties

      /// <summary>
      /// The id of the webcam to use.
      /// </summary>
      public int WebcamId { get { return webcamId; } set { webcamId = value; } }

      /// <summary>
      /// The file path to load the camera parameters.
      /// </summary>
      public string CameraParametersFilePath { get { return cameraParametersFilePath; } set { cameraParametersFilePath = value; } }

      /// <summary>
      /// The webcam to use.
      /// </summary>
      public WebCamDevice WebCamDevice { get; protected set; }

      /// <summary>
      /// The texture of the associated webcam.
      /// </summary>
      public WebCamTexture WebCamTexture { get; protected set; }

      // Variables

      protected bool startInitiated = false;
      protected int cameraId = 0;

      // MonoBehaviour methods

      /// <summary>
      /// If the camera has been started, wait for Unity to start the webcam to initialize the textures, the Unity camera backgrounds and to trigger
      /// the <see cref="ArucoCamera.Started"/> event.
      /// </summary>
      protected override void Update()
      {
        if (startInitiated)
        {
          if (WebCamTexture.width < 100) // Wait the WebCamTexture initialization
          {
            return;
          }
          else
          {
            // Configure
            ImageTextures[cameraId] = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);
            if (ImageCameras[cameraId] == null)
            {
              ImageCameras[cameraId] = GetComponent<Camera>();
            }

            // Update state
            startInitiated = false;
            OnStarted();
          }
        }

        base.Update();
      }

      // ArucoCamera methods

      /// <summary>
      /// Configure the webcam and its properties with the id <see cref="WebcamId"/>. The camera needs to be stopped before configured.
      /// </summary>
      public override void Configure()
      {
        if (IsStarted || startInitiated)
        {
          return;
        }

        // Reset state
        startInitiated = false;
        IsConfigured = false;

        // Try to load the webcam
        WebCamDevice[] webcamDevices = WebCamTexture.devices;
        if (webcamDevices.Length <= WebcamId)
        {
          IsConfigured = false;
          throw new System.ArgumentException("The webcam with the id '" + WebcamId + "' is not found.", "WebcamId");
        }
        WebCamDevice = webcamDevices[WebcamId];
        WebCamTexture = new WebCamTexture(WebCamDevice.name);
        Name = webcamDevices[WebcamId].name;

        // Try to load the camera parameters
        if (CameraParametersFilePath != null && CameraParametersFilePath.Length > 0)
        {
          string fullCameraParametersFilePath = Path.Combine((Application.isEditor) ? Application.dataPath : Application.persistentDataPath, CameraParametersFilePath);
          CameraParameters = Parameters.CameraParameters.LoadFromXmlFile(fullCameraParametersFilePath);
        }

        base.Configure();
      }

      /// <summary>
      /// Initiate the camera start and the associated webcam device.
      /// </summary>
      public override void StartCameras()
      {
        if (IsConfigured && !IsStarted && !startInitiated)
        {
          WebCamTexture.Play();
          startInitiated = true;
        }
      }

      /// <summary>
      /// Stop the camera and the associated webcam device.
      /// </summary>
      public override void StopCameras()
      {
        if (IsConfigured && (IsStarted || startInitiated))
        {
          WebCamTexture.Stop();
          startInitiated = false;
          OnStopped();
        }
      }

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, update every frame the <see cref="ArucoCamera.ImageTextures"/> and the
      /// <see cref="ArucoCamera.ImageDatas"/> with the <see cref="WebCamTexture"/> content.
      /// </summary>
      protected override void UpdateCameraImages()
      {
        ImageTextures[cameraId].SetPixels32(WebCamTexture.GetPixels32());
        System.Array.Copy(ImageTextures[cameraId].GetRawTextureData(), ImageDatas[cameraId], ImageDataSizes[cameraId]);

        OnImagesUpdated();
      }
    }
  }

  /// \} aruco_unity_package
}