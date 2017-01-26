using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Manages any connected webcam to the machine, and retrieves and displays the camera's image every frame.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </summary>
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
      /// The correct image orientation.
      /// </summary>
      public override Quaternion ImageRotation
      {
        get
        {
          return Quaternion.Euler(0f, 0f, -WebCamTexture.videoRotationAngle);
        }
      }

      /// <summary>
      /// The image ratio.
      /// </summary>
      public override float ImageRatio
      {
        get
        {
          return WebCamTexture.width / (float)WebCamTexture.height;
        }
      }

      /// <summary>
      /// Allow to unflip the image if vertically flipped (use for image plane).
      /// </summary>
      public override Mesh ImageMesh
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

          return mesh;
        }
      }

      /// <summary>
      /// Allow to unflip the image if vertically flipped (use for canvas).
      /// </summary>
      public override Rect ImageUvRectFlip
      {
        get
        {
          Rect defaultRect = new Rect(0f, 0f, 1f, 1f),
               verticallyMirroredRect = new Rect(0f, 1f, 1f, -1f);
          return WebCamTexture.videoVerticallyMirrored ? verticallyMirroredRect : defaultRect;
        }
      }

      /// <summary>
      /// Mirror front-facing camera's image horizontally to look more natural.
      /// </summary>
      public override Vector3 ImageScaleFrontFacing
      {
        get
        {
          Vector3 defaultScale = new Vector3(1f, 1f, 1f),
                  frontFacingScale = new Vector3(-1f, 1f, 1f);
          return WebCamDevice.isFrontFacing ? frontFacingScale : defaultScale;
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
      /// Camera that shot the <see cref="ArucoCamera.ImageCamera"/> in order to maintain the aspect ratio of 
      /// <see cref="ArucoCamera.ImageTexture"/> on screen.
      /// </summary>
      public Camera CameraBackground { get; protected set; }

      // Variables

      GameObject cameraPlane;

      // MonoBehaviour methods

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, update every frame the <see cref="ArucoCamera.ImageTexture"/> with the 
      /// <see cref="WebCamTexture"/> content.
      /// </summary>
      protected void Update()
      {
        if (!Configured)
        {
          ImageUpdatedThisFrame = false;
          return;
        }

        // Wait the WebCamTexture to be initialized, to configure the ImageTexture, the CameraPlane and notify the camera has started
        if (!Started)
        {
          if (WebCamTexture.width < 100)
          {
            Debug.Log(gameObject.name + ": Still waiting another frame for correct info.");
          }
          else
          {
            ImageTexture = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);

            if (DisplayImage)
            {
              ConfigureCameraPlane();
            }

            Started = true;
            RaiseOnStarted();
          }
        }

        // Update the ImageTexture content
        if (Started)
        {
          ImageTexture.SetPixels32(WebCamTexture.GetPixels32());
          ImageTexture.Apply(false);

          ImageUpdatedThisFrame = true;
          RaiseOnImageUpdated();
        }
      }

      // ArucoCamera methods

      /// <summary>
      /// Configure the webcam, with the id <see cref="WebcamId"/> and its the ArucoCamera properties.
      /// </summary>
      public override void Configure()
      {
        if (Started)
        {
          Debug.LogError(gameObject.name + ": Stop the camera to configure it. Aborting configuration.");
          Configured = false;
          return;
        }

        // Try to check for the webcam
        WebCamDevice[] webcamDevices = WebCamTexture.devices;
        if (webcamDevices.Length < WebcamId)
        {
          Debug.LogError(gameObject.name + ": The webcam with the id '" + WebcamId + "' is not found. Aborting configuration.");
          Configured = false;
          return;
        }

        // Try to load the camera parameters
        CameraParameters = CameraParameters.LoadFromXmlFile(CameraParametersFilePath);

        // Switch the camera device
        WebCamDevice = webcamDevices[WebcamId];
        WebCamTexture = new WebCamTexture(WebCamDevice.name);

        // Update state
        Configured = true;
        RaiseOnConfigured();

        // AutoStart
        if (AutoStart)
        {
          StartCamera();
        }
      }

      /// <summary>
      /// Start the camera and the associated webcam device.
      /// </summary>
      public override void StartCamera()
      {
        if (Started)
        {
          return;
        }

        WebCamTexture.Play();
        Started = false; // Need some frames to be started, see Update()
      }

      /// <summary>
      /// Stop the camera and the associated webcam device, and notify of the stopping.
      /// </summary>
      public override void StopCamera()
      {
        if (!Started)
        {
          return;
        }

        WebCamTexture.Stop();
        Started = false;
        RaiseOnStopped();
      }

      /// <summary>
      /// Configure the <see cref="ArucoCamera.ImageCamera"/>, the <see cref="CameraBackground"/> and a facing plane of the CameraImage that will 
      /// display the <see cref="ArucoCamera.ImageTexture"/>.
      /// </summary>
      // TODO: handle case of CameraParameters.ImageHeight != ImageTexture.height or CameraParameters.ImageWidth != ImageTexture.width
      // TODO: handle case of CameraParameters.FixAspectRatio != 0
      protected void ConfigureCameraPlane()
      {
        // Use the image texture's width as a default value if there is no camera parameters
        float CameraPlaneDistance = (CameraParameters != null) ? CameraParameters.CameraFy : ImageTexture.width;

        // Configure the CameraImage according to the camera parameters
        ImageCamera = GetComponent<Camera>();

        float farClipPlaneNewValueFactor = 1.01f; // To be sure that the camera plane is visible by the camera
        float vFov = 2f * Mathf.Atan(0.5f * ImageTexture.height / CameraPlaneDistance) * Mathf.Rad2Deg;

        ImageCamera.orthographic = false;
        ImageCamera.fieldOfView = vFov;
        ImageCamera.farClipPlane = CameraPlaneDistance * farClipPlaneNewValueFactor;
        ImageCamera.aspect = ImageRatio;
        ImageCamera.transform.position = Vector3.zero;
        ImageCamera.transform.rotation = Quaternion.identity;

        // Configure the plane facing the CameraImage that display the texture
        if (cameraPlane == null)
        {
          cameraPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
          cameraPlane.name = "CameraImagePlane";
          cameraPlane.transform.parent = this.transform;
          cameraPlane.GetComponent<Renderer>().material = Resources.Load("CameraImage") as Material;
        }

        cameraPlane.transform.position = new Vector3(0, 0, CameraPlaneDistance);
        cameraPlane.transform.rotation = ImageRotation;
        cameraPlane.transform.localScale = new Vector3(ImageTexture.width, ImageTexture.height, 1);
        cameraPlane.transform.localScale = Vector3.Scale(cameraPlane.transform.localScale, ImageScaleFrontFacing);
        cameraPlane.GetComponent<MeshFilter>().mesh = ImageMesh;
        cameraPlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
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
            CameraBackground.depth = ImageCamera.depth + 1; // Render after the CameraImage

            CameraBackground.orthographic = false;
            CameraBackground.fieldOfView = ImageCamera.fieldOfView;
            CameraBackground.nearClipPlane = ImageCamera.nearClipPlane;
            CameraBackground.farClipPlane = ImageCamera.farClipPlane;
            CameraBackground.transform.position = ImageCamera.transform.position;
            CameraBackground.transform.rotation = ImageCamera.transform.rotation;
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