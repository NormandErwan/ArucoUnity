using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Manages a webcam device and updates its associated textures each frame.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </summary>
    public class CameraDevice : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of the camera device to use.")]
      private int deviceId = 0;

      [SerializeField]
      [Tooltip("The file path to load the camera parameters.")]
      private string cameraParametersFilePath = "Assets/ArucoUnity/aruco-calibration.xml";

      [SerializeField]
      [Tooltip("Start the camera automatically after configured it.")]
      private bool autoStart = true;

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
      /// The associated webcam device.
      /// </summary>
      public WebCamDevice WebCamDevice { get; protected set; }

      /// <summary>
      /// The texture of the associated webcam device.
      /// </summary>
      public WebCamTexture WebCamTexture { get; protected set; }

      /// <summary>
      /// The id of the camera device to use.
      /// </summary>
      public int DeviceId { get { return deviceId; } set { deviceId = value; } }

      /// <summary>
      /// The file path to load the camera parameters.
      /// </summary>
      public string CameraParametersFilePath { get { return cameraParametersFilePath; } set { cameraParametersFilePath = value; } }

      /// <summary>
      /// Start the camera automatically after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      // Variables

      GameObject cameraPlane;

      // MonoBehaviour methods

      /// <summary>
      /// Configure the selected camera device if <see cref="AutoStart"/> is true.
      /// </summary>
      protected void Start()
      {
        if (AutoStart)
        {
          Configure();
        }
      }

      /// <summary>
      /// Make adjustments to image every frame to be safe, since Unity isn't 
      /// guaranteed to report correct data as soon as device camera is started.
      /// </summary>
      protected void Update()
      {
        if (!Started)
        {
          // Skip making adjustment for incorrect camera data
          if (WebCamTexture.width < 100)
          {
            Debug.Log(gameObject.name + ": Still waiting another frame for correct info.");
            return;
          }
          else
          {
            // Initialize the Texture2D and the camera plane
            ImageTexture = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);
            ConfigureCameraPlane();

            // Notify that the camera has started
            Started = true;
            RaiseOnStarted();
          }
        }
        else
        {
          // Update the Texture2D content
          ImageTexture.SetPixels32(WebCamTexture.GetPixels32());
        }
      }

      // ArucoCamera methods

      /// <summary>
      /// Configure the camera device with the id <see cref="DeviceId"/> and its properties.
      /// </summary>
      /// <returns>If the operation has been successfull.</returns>
      public override bool Configure()
      {
        if (Started)
        {
          Debug.LogError(gameObject.name + ": Stop the camera to configure it. Aborting configuration.");
          return false;
        }

        // Try to check for the camera device
        WebCamDevice[] webcamDevices = WebCamTexture.devices;
        if (webcamDevices.Length < DeviceId)
        {
          Debug.LogError(gameObject.name + ": The camera device with the id '" + DeviceId + "' is not found. Aborting configuration.");
          return false;
        }

        // Try to load the camera parameters
        CameraParameters = CameraParameters.LoadFromXmlFile(cameraParametersFilePath);
        if (CameraParameters == null)
        {
          Debug.LogError(gameObject.name + ": Couldn't load the camera parameters file path '" + cameraParametersFilePath + ".");
        }

        // Switch the camera device
        WebCamDevice = webcamDevices[DeviceId];
        WebCamTexture = new WebCamTexture(WebCamDevice.name);

        // AutoStart
        if (AutoStart)
        {
          StartCamera();
        }

        return true;
      }

      /// <summary>
      /// Start the camera and the associated webcam device.
      /// </summary>
      public override bool StartCamera()
      {
        if (Started)
        {
          return false;
        }

        WebCamTexture.Play();
        Started = false; // Need some frames to be started, see Update()

        return true;
      }

      /// <summary>
      /// Stop the camera and the associated webcam device, and notify of the stopping.
      /// </summary>
      public override bool StopCamera()
      {
        if (!Started)
        {
          return false;
        }

        WebCamTexture.Stop();
        Started = false;
        RaiseOnStopped();

        return true;
      }

      /// <summary>
      /// 
      /// </summary>
      protected void ConfigureCameraPlane()
      {
        // Use the image texture's width as a fake value if there is no camera parameters
        float CameraPlaneDistance = (CameraParameters != null) ? CameraParameters.CameraFy : ImageTexture.width;

        // Configure the camera according to the camera parameters
        Camera = GetComponent<Camera>();

        float farClipPlaneNewValueFactor = 1.01f; // To be sure that the camera plane is visible by the camera
        float vFov = 2f * Mathf.Atan(0.5f * ImageTexture.height / CameraPlaneDistance) * Mathf.Rad2Deg;

        Camera.orthographic = false;
        Camera.fieldOfView = vFov;
        Camera.farClipPlane = CameraPlaneDistance * farClipPlaneNewValueFactor;
        Camera.aspect = ImageRatio;
        Camera.transform.position = Vector3.zero;
        Camera.transform.rotation = Quaternion.identity;

        // Configure the plane facing the camera that display the texture
        if (cameraPlane == null)
        {
          cameraPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
          cameraPlane.transform.SetParent(this.transform);
          cameraPlane.GetComponent<Renderer>().material = Resources.Load("CameraImageTexture") as Material;
        }

        cameraPlane.transform.position = new Vector3(0, 0, CameraPlaneDistance);
        cameraPlane.transform.rotation = ImageRotation;
        cameraPlane.transform.localScale = new Vector3(ImageTexture.width, ImageTexture.height, 1);
        cameraPlane.transform.localScale = Vector3.Scale(cameraPlane.transform.localScale, ImageScaleFrontFacing);
        cameraPlane.GetComponent<MeshFilter>().mesh = ImageMesh;
        cameraPlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
        cameraPlane.SetActive(true);
      }
    }
  }

  /// \} aruco_unity_package
}