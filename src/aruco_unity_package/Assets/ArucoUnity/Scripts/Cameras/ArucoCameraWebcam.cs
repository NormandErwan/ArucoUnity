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
      private string cameraParametersFilePath = "ArucoUnity/Calibrations/calibration.xml";

      [SerializeField]
      [Tooltip("Preserve the aspect ratio of the webcam image.")]
      private bool preserveAspectRatio = true;

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
      public override float[] ImageRatios
      {
        get
        {
          return new float[] { WebCamTexture.width / (float)WebCamTexture.height };
        }
      }

      /// <summary>
      /// <see cref="ArucoCamera.ImageMeshes"/>
      /// </summary>
      public override Mesh[] ImageMeshes
      {
        get
        {
          Mesh mesh = new Mesh();

          mesh.vertices = new Vector3[]
          {
            new Vector3(-0.5f, -0.5f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f),
            new Vector3(0.5f, -0.5f, 0.0f),
            new Vector3(-0.5f, 0.5f, 0.0f),
          };
          mesh.triangles = new int[] { 0, 1, 2, 1, 0, 3 };

          mesh.uv = new Vector2[]
          {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f)
          };

          mesh.RecalculateNormals();

          return new Mesh[] { mesh };
        }
      }

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
      /// Preserve the aspect ratio of the webcam's image.
      /// </summary>
      public bool PreserveAspectRatio { get { return preserveAspectRatio; } set { preserveAspectRatio = value; } }

      /// <summary>
      /// The associated webcam device.
      /// </summary>
      public WebCamDevice WebCamDevice { get; protected set; }

      /// <summary>
      /// The texture of the associated webcam device.
      /// </summary>
      public WebCamTexture WebCamTexture { get; protected set; }

      /// <summary>
      /// Camera that shot the <see cref="ArucoCamera.ImageCameras"/> in order to maintain the aspect ratio of 
      /// <see cref="ArucoCamera.ImageTextures"/> on screen.
      /// </summary>
      public Camera CameraBackground { get; protected set; }

      // Variables

      protected GameObject cameraPlane;
      protected bool startInitiated;
      protected int cameraId = 0;
      protected int imageWidth, imageHeight;

      // MonoBehaviour methods

      /// <summary>
      /// <see cref="ArucoCamera.Awake"/>
      /// </summary>
      protected override void Awake()
      {
        startInitiated = false;

        ImageTextures = new Texture2D[CameraNumber];
        ImageCameras = new Camera[CameraNumber];
        ImageCameras[cameraId] = GetComponent<Camera>();

        base.Awake();
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
          CameraParameters = CameraParameters.LoadFromXmlFile(fullCameraParametersFilePath);
        }

        base.Configure();
      }

      /// <summary>
      /// Start the camera and the associated webcam device.
      /// </summary>
      public override void StartCameras()
      {
        if (!IsConfigured || IsStarted || startInitiated)
        {
          return;
        }

        WebCamTexture.Play();
        startInitiated = true;
      }

      /// <summary>
      /// Stop the camera and the associated webcam device, and notify of the stopping.
      /// </summary>
      public override void StopCameras()
      {
        if (!IsConfigured || (!IsStarted && !startInitiated))
        {
          return;
        }

        WebCamTexture.Stop();

        startInitiated = false;
        IsStarted = false;
        OnStopped();
      }

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, update every frame the <see cref="ArucoCamera.ImageTextures"/> with the 
      /// <see cref="WebCamTexture"/> content.
      /// </summary>
      protected override void UpdateCameraImages()
      {
        if (startInitiated)
        {
          if (WebCamTexture.width < 100) // Wait the WebCamTexture initialization
          {
            return;
          }
          else
          {
            // Configure texture
            imageWidth = WebCamTexture.width;
            imageHeight = WebCamTexture.height;
            ImageTextures[cameraId] = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);

            // Configure display
            if (DisplayImages)
            {
              ConfigureCameraPlane();
            }

            // Update state
            startInitiated = false;
            IsStarted = true;
            OnStarted();
          }
        }

        // Update the ImageTexture content
        ImageTextures[cameraId].SetPixels32(WebCamTexture.GetPixels32());

        OnImagesUpdated();
      }

      // Methods

      /// <summary>
      /// Configure the <see cref="ArucoCamera.ImageCameras"/>, the <see cref="CameraBackground"/> and a facing plane of the CameraImage that will 
      /// display the <see cref="ArucoCamera.ImageTextures"/>.
      /// </summary>
      // TODO: handle case of CameraParameters.ImageHeight != ImageTexture.height or CameraParameters.ImageWidth != ImageTexture.width
      // TODO: handle case of CameraParameters.FixAspectRatio != 0
      protected void ConfigureCameraPlane()
      {
        // Use the image texture's width as a default value if there is no camera parameters
        float CameraPlaneDistance = (CameraParameters != null) ? CameraParameters.CameraFocalLengths[cameraId].y : ImageTextures[cameraId].width;

        // Configure the CameraImage according to the camera parameters
        float farClipPlaneNewValueFactor = 1.01f; // To be sure that the camera plane is visible by the camera
        float vFov = 2f * Mathf.Atan(0.5f * ImageTextures[cameraId].height / CameraPlaneDistance) * Mathf.Rad2Deg;
        ImageCameras[cameraId].orthographic = false;
        ImageCameras[cameraId].fieldOfView = vFov;
        ImageCameras[cameraId].farClipPlane = CameraPlaneDistance * farClipPlaneNewValueFactor;
        ImageCameras[cameraId].aspect = ImageRatios[cameraId];
        ImageCameras[cameraId].transform.position = Vector3.zero;
        ImageCameras[cameraId].transform.rotation = Quaternion.identity;

        // Configure the plane facing the CameraImage that display the texture
        if (cameraPlane == null)
        {
          cameraPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
          cameraPlane.name = "CameraImagePlane";
          cameraPlane.transform.parent = this.transform;
          cameraPlane.GetComponent<Renderer>().material = Resources.Load("CameraImage") as Material;
        }

        cameraPlane.transform.position = new Vector3(0, 0, CameraPlaneDistance);
        cameraPlane.transform.rotation = Quaternion.identity;
        cameraPlane.transform.localScale = new Vector3(ImageTextures[cameraId].width, ImageTextures[cameraId].height, 1);
        cameraPlane.GetComponent<MeshFilter>().mesh = ImageMeshes[cameraId];
        cameraPlane.GetComponent<Renderer>().material.mainTexture = ImageTextures[cameraId];
        cameraPlane.SetActive(true);

        // If preserving the aspect ratio of the CameraImage, create a second camera that shot it as background
        if (PreserveAspectRatio)
        {
          if (CameraBackground == null)
          {
            GameObject CameraBackgroundGameObject = new GameObject("BlackBackgroundCamera");
            CameraBackgroundGameObject.transform.parent = this.transform;

            CameraBackground = CameraBackgroundGameObject.AddComponent<Camera>();
            CameraBackground.clearFlags = CameraClearFlags.SolidColor;
            CameraBackground.backgroundColor = Color.black;
            CameraBackground.depth = ImageCameras[cameraId].depth + 1; // Render after the CameraImage

            CameraBackground.orthographic = false;
            CameraBackground.fieldOfView = ImageCameras[cameraId].fieldOfView;
            CameraBackground.nearClipPlane = ImageCameras[cameraId].nearClipPlane;
            CameraBackground.farClipPlane = ImageCameras[cameraId].farClipPlane;
            CameraBackground.transform.position = ImageCameras[cameraId].transform.position;
            CameraBackground.transform.rotation = ImageCameras[cameraId].transform.rotation;
          }
          CameraBackground.gameObject.SetActive(true);
        }
        else if (CameraBackground != null)
        {
          CameraBackground.gameObject.SetActive(false);
        }
      }
    }
  }

  /// \} aruco_unity_package
}