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
      // Constants

      protected const float cameraBackgroundDistance = 1f;

      // Editor fields

      [SerializeField]
      [Tooltip("The id of the webcam to use.")]
      private int webcamId = 0;

      [SerializeField]
      [Tooltip("The file path to load the camera parameters.")]
      private string cameraParametersFilePath = "ArucoUnity/Calibrations/<calibration_file>.xml";

      // ArucoCamera properties implementation

      /// <summary>
      /// <see cref="ArucoCamera.CameraNumber"/>
      /// </summary>
      public override int CameraNumber { get { return 1; } protected set { } }

      /// <summary>
      /// <see cref="ArucoCamera.Name"/>
      /// </summary>
      public override string Name { get; protected set; }

      /// <summary>
      /// <see cref="ArucoCamera.ImageRatios"/>
      /// </summary>
      public override float[] ImageRatios { get { return new float[] { WebCamTexture.width / (float)WebCamTexture.height }; } }

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

      protected GameObject cameraBackground;
      protected Renderer cameraBackgroundRenderer;
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
            ConfigureCameraAndBackground();
            cameraBackground.SetActive(DisplayImages);

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
      /// Stop the camera and the associated webcam device, and trigger the <see cref="ArucoCamera.Stopped"/> event.
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

      // Methods

      /// <summary>
      /// Configure the <see cref="ArucoCamera.ImageCameras"/>, the background that will display live webcam video through the
      /// <see cref="ArucoCamera.ImageTextures"/>.
      /// </summary>
      /// <remarks>See https://docs.opencv.org/3.3.0/d4/d94/tutorial_camera_calibration.html and 
      /// https://docs.opencv.org/3.3.0/d9/d0c/group__calib3d.html#details for reference.</remarks>
      // TODO: handle case of CameraParameters.ImageHeight != ImageTexture.height or CameraParameters.ImageWidth != ImageTexture.width
      // TODO: handle case of CameraParameters.FixAspectRatio != 0
      protected void ConfigureCameraAndBackground()
      {
        float imageWidth = CameraParameters.ImageWidths[cameraId];
        float imageHeight = CameraParameters.ImageHeights[cameraId];
        Vector2 f = CameraParameters.GetCameraFocalLengths(cameraId);
        Vector2 c = CameraParameters.GetCameraPrincipalPoint(cameraId);

        // Configure the camera according to the camera parameters
        if (ImageCameras[cameraId] == null)
        {
          ImageCameras[cameraId] = GetComponent<Camera>();
        }

        // Estimate fov using these equations : https://stackoverflow.com/questions/39992968/how-to-calculate-field-of-view-of-the-camera-from-camera-intrinsic-matrix
        float fovX = 2f * Mathf.Atan(0.5f * imageWidth / f.x) * Mathf.Rad2Deg;
        float fovY = 2f * Mathf.Atan(0.5f * imageHeight / f.y) * Mathf.Rad2Deg;
        ImageCameras[cameraId].fieldOfView = fovY;
        ImageCameras[cameraId].aspect = ImageRatios[cameraId];

        // Configure the background plane facing the camera
        if (cameraBackground == null)
        {
          cameraBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
          cameraBackground.name = "CameraBackground";
          cameraBackground.transform.parent = this.transform;
          cameraBackground.transform.rotation = Quaternion.identity;

          cameraBackgroundRenderer = cameraBackground.GetComponent<Renderer>();
          cameraBackgroundRenderer.material = Resources.Load("UnlitImage") as Material;
        }
        cameraBackgroundRenderer.material.mainTexture = ImageTextures[cameraId];

        // Estimate background plane position relative to the camera, considering the first link in remarks: here x=0.5*ImageWidth and y=0.5*ImageHeight
        // (center of the camera projection), w=Z=cameraBackgroundDistance and we are looking for X=posX and Y=posY
        float posX = (0.5f * imageWidth - c.x) / f.x * cameraBackgroundDistance;
        float posY = (0.5f * imageHeight - c.y) / f.y * cameraBackgroundDistance;
        cameraBackground.transform.position = new Vector3(posX, posY * -1, cameraBackgroundDistance); // c=-1 for posY is because OpenCV (u=0, v=0) camera coordinates origin is top-left, but bottom-left in Unity (see https://docs.unity3d.com/ScriptReference/Camera.html)

        // Estimate background plane scale, using the same equations used for fov but here we are looking for w=scaleX and h=scaleY
        float scaleX = 2f * cameraBackgroundDistance * Mathf.Tan(fovX * Mathf.Deg2Rad / 2f);
        float scaleY = 2f * cameraBackgroundDistance * Mathf.Tan(fovY * Mathf.Deg2Rad / 2f);
        cameraBackground.transform.localScale = new Vector3(scaleX, scaleY, 1);
      }
    }
  }

  /// \} aruco_unity_package
}