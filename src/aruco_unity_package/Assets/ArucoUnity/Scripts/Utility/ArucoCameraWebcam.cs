using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
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
      private string cameraParametersFilePath = "Assets/ArucoUnity/aruco-calibration.xml";

      [SerializeField]
      [Tooltip("Preserve the aspect ratio of the webcam image.")]
      private bool preserveAspectRatio = true;

      // ArucoCamera properties implementation

      /// <summary>
      /// <see cref="ArucoCamera.ImageRotations"/>
      /// </summary>
      public override Quaternion[] ImageRotations
      {
        get
        {
          return new Quaternion[] { Quaternion.Euler(0f, 0f, -WebCamTexture.videoRotationAngle) };
        }
      }

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

          Vector2[] defaultUv = new Vector2[]
          {
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f)
          };
          Vector2[] verticallyMirroredUv = new Vector2[]
          {
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 0.0f)
          };
          mesh.uv = WebCamTexture.videoVerticallyMirrored ? verticallyMirroredUv : defaultUv;

          mesh.RecalculateNormals();

          return new Mesh[] { mesh };
        }
      }

      /// <summary>
      /// <see cref="ArucoCamera.ImageUvRectFlips"/>
      /// </summary>
      public override Rect[] ImageUvRectFlips
      {
        get
        {
          Rect defaultRect = new Rect(0f, 0f, 1f, 1f),
               verticallyMirroredRect = new Rect(0f, 1f, 1f, -1f);
          return new Rect[] { WebCamTexture.videoVerticallyMirrored ? verticallyMirroredRect : defaultRect };
        }
      }

      /// <summary>
      /// <see cref="ArucoCamera.ImageScalesFrontFacing"/>
      /// </summary>
      public override Vector3[] ImageScalesFrontFacing
      {
        get
        {
          Vector3 defaultScale = new Vector3(1f, 1f, 1f),
                  frontFacingScale = new Vector3(-1f, 1f, 1f);
          return new Vector3[] { WebCamDevice.isFrontFacing ? frontFacingScale : defaultScale };
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

      // MonoBehaviour methods

      /// <summary>
      /// <see cref="ArucoCamera.Awake"/>
      /// </summary>
      protected override void Awake()
      {
        startInitiated = false;

        ImageTextures = new Texture2D[1];
        ImageCameras = new Camera[1] { GetComponent<Camera>() };

        base.Awake();
      }

      // ArucoCamera methods

      /// <summary>
      /// Configure the webcam and its properties with the id <see cref="WebcamId"/>. The camera needs to be stopped before configured.
      /// </summary>
      public override void Configure()
      {
        if (Started || startInitiated)
        {
          return;
        }

        // Try to load the webcam
        WebCamDevice[] webcamDevices = WebCamTexture.devices;
        if (webcamDevices.Length <= WebcamId)
        {
          Configured = false;
          throw new System.ArgumentException("The webcam with the id '" + WebcamId + "' is not found.", "WebcamId");
        }
        WebCamDevice = webcamDevices[WebcamId];
        WebCamTexture = new WebCamTexture(WebCamDevice.name);

        // Try to load the camera parameters
        if (CameraParametersFilePath != null)
        {
          CameraParameters = new CameraParameters[] { Utility.CameraParameters.LoadFromXmlFile(CameraParametersFilePath) };
        }

        // Update state
        Configured = true;
        RaiseOnConfigured();

        // AutoStart
        if (AutoStart)
        {
          StartCameras();
        }
      }

      /// <summary>
      /// Start the camera and the associated webcam device.
      /// </summary>
      public override void StartCameras()
      {
        if (!Configured || Started || startInitiated)
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
        if (!Configured || (!Started && !startInitiated))
        {
          return;
        }

        WebCamTexture.Stop();

        startInitiated = false;
        Started = false;
        RaiseOnStopped();
      }

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, update every frame the <see cref="ArucoCamera.ImageTextures"/> with the 
      /// <see cref="WebCamTexture"/> content.
      /// </summary>
      protected override void UpdateCameraImages()
      {
        if (!Configured || (!Started && !startInitiated))
        {
          ImagesUpdatedThisFrame = false;
          return;
        }

       if (startInitiated)
        {
          if (WebCamTexture.width < 100) // Wait the WebCamTexture initialization
          {
            ImagesUpdatedThisFrame = false;
            return;
          }
          else
          {
            // Configure texture
            ImageTextures[0] = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);

            // Configure display
            if (DisplayImages)
            {
              ConfigureCameraPlane();
            }

            // Update state
            startInitiated = false;
            Started = true;
            RaiseOnStarted();
          }
        }

        // Update the ImageTexture content
        ImageTextures[0].SetPixels32(WebCamTexture.GetPixels32());
        ImageTextures[0].Apply(false);

        ImagesUpdatedThisFrame = true;
        RaiseOnImageUpdated();
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
        float CameraPlaneDistance = (CameraParameters != null) ? CameraParameters[0].CameraFy : ImageTextures[0].width;

        // Configure the CameraImage according to the camera parameters
        float farClipPlaneNewValueFactor = 1.01f; // To be sure that the camera plane is visible by the camera
        float vFov = 2f * Mathf.Atan(0.5f * ImageTextures[0].height / CameraPlaneDistance) * Mathf.Rad2Deg;
        ImageCameras[0].orthographic = false;
        ImageCameras[0].fieldOfView = vFov;
        ImageCameras[0].farClipPlane = CameraPlaneDistance * farClipPlaneNewValueFactor;
        ImageCameras[0].aspect = ImageRatios[0];
        ImageCameras[0].transform.position = Vector3.zero;
        ImageCameras[0].transform.rotation = Quaternion.identity;

        // Configure the plane facing the CameraImage that display the texture
        if (cameraPlane == null)
        {
          cameraPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
          cameraPlane.name = "CameraImagePlane";
          cameraPlane.transform.parent = this.transform;
          cameraPlane.GetComponent<Renderer>().material = Resources.Load("CameraImage") as Material;
        }

        cameraPlane.transform.position = new Vector3(0, 0, CameraPlaneDistance);
        cameraPlane.transform.rotation = ImageRotations[0];
        cameraPlane.transform.localScale = new Vector3(ImageTextures[0].width, ImageTextures[0].height, 1);
        cameraPlane.transform.localScale = Vector3.Scale(cameraPlane.transform.localScale, ImageScalesFrontFacing[0]);
        cameraPlane.GetComponent<MeshFilter>().mesh = ImageMeshes[0];
        cameraPlane.GetComponent<Renderer>().material.mainTexture = ImageTextures[0];
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
            CameraBackground.depth = ImageCameras[0].depth + 1; // Render after the CameraImage

            CameraBackground.orthographic = false;
            CameraBackground.fieldOfView = ImageCameras[0].fieldOfView;
            CameraBackground.nearClipPlane = ImageCameras[0].nearClipPlane;
            CameraBackground.farClipPlane = ImageCameras[0].farClipPlane;
            CameraBackground.transform.position = ImageCameras[0].transform.position;
            CameraBackground.transform.rotation = ImageCameras[0].transform.rotation;
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