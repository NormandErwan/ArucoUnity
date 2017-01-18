using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    namespace Utility
    {
      /// <summary>
      /// Manage a webcam device and its associated textures updated each frame.
      /// Based on: http://answers.unity3d.com/answers/1155328/view.html
      /// </summary>
      public class CameraDevice : MonoBehaviour
      {
        // Events

        public delegate void CameraDeviceAction();

        /// <summary>
        /// Executed when the camera has started.
        /// </summary>
        public event CameraDeviceAction OnStarted;

        /// <summary>
        /// Executed when the camera has stopped.
        /// </summary>
        public event CameraDeviceAction OnStopped;

        // Properties

        /// <summary>
        /// True when the camera has started.
        /// </summary>
        public bool Started { get; protected set; }

        /// <summary>
        /// The associated webcam device.
        /// </summary>
        public WebCamDevice WebCamDevice { get; protected set; }

        /// <summary>
        /// The texture of the associated webcam device.
        /// </summary>
        public WebCamTexture WebCamTexture { get; protected set; }

        /// <summary>
        /// Equivalent of the <see cref="WebCamTexture"/>, updated each frame.
        /// </summary>
        public Texture2D Texture2D { get; protected set; }

        /// <summary>
        /// The parameters of the camera.
        /// </summary>
        public CameraParameters CameraParameters { get; protected set; }

        /// <summary>
        /// The correct image orientation.
        /// </summary>
        public Quaternion ImageRotation
        {
          get
          {
            return Quaternion.Euler(0f, 0f, -WebCamTexture.videoRotationAngle);
          }
          private set { }
        }

        /// <summary>
        /// The image ratio.
        /// </summary>
        public float ImageRatio
        {
          get
          {
            return WebCamTexture.width / (float)WebCamTexture.height;
          }
          private set { }
        }

        /// <summary>
        /// Allow to unflip the image if vertically flipped (use for image plane).
        /// </summary>
        public Mesh ImageMesh
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
          private set { }
        }

        /// <summary>
        /// Allow to unflip the image if vertically flipped (use for canvas).
        /// </summary>
        public Rect ImageUvRectFlip
        {
          get
          {
            Rect defaultRect = new Rect(0f, 0f, 1f, 1f),
                 verticallyMirroredRect = new Rect(0f, 1f, 1f, -1f);
            return WebCamTexture.videoVerticallyMirrored ? verticallyMirroredRect : defaultRect;
          }
          private set { }
        }

        /// <summary>
        /// Mirror front-facing camera's image horizontally to look more natural.
        /// </summary>
        public Vector3 ImageScaleFrontFacing
        {
          get
          {
            Vector3 defaultScale = new Vector3(1f, 1f, 1f),
                    frontFacingScale = new Vector3(-1f, 1f, 1f);
            return WebCamDevice.isFrontFacing ? frontFacingScale : defaultScale;
          }
          private set { }
        }

        // Methods

        /// <summary>
        /// Initialize the camera device and its textures.
        /// </summary>
        /// <param name="webcamDeviceToUse">The webcam to use.</param>
        public void ResetCamera(WebCamDevice webcamDeviceToUse)
        {
          WebCamDevice = webcamDeviceToUse;

          WebCamTexture = new WebCamTexture(webcamDeviceToUse.name);
          WebCamTexture.filterMode = FilterMode.Trilinear;

          Started = false;
        }

        /// <summary>
        /// Start the camera and the associated webcam device.
        /// </summary>
        public void StartCamera()
        {
          if (!Started)
          {
            WebCamTexture.Play();
            Started = false; // Need some frames to be started, see Update()
          }
        }

        /// <summary>
        /// Stop the camera and the associated webcam device, and notify of the stopping.
        /// </summary>
        public void StopCamera()
        {
          if (Started)
          {
            WebCamTexture.Stop();
            Started = false;
            if (OnStopped != null)
            {
              OnStopped();
            }
          }
        }

        /// <summary>
        /// Populate <see cref="CameraParameters"/> from a previously saved camera parameters XML file.
        /// </summary>
        /// <param name="cameraParametersFilePath">The file path to load.</param>
        /// <returns>If the camera parameters has been successfully loaded.</returns>
        public bool LoadCameraParameters(string cameraParametersFilePath)
        {
          CameraParameters = CameraParameters.LoadFromXmlFile(cameraParametersFilePath);
          return CameraParameters != null;
        }

        /// <summary>
        /// Make adjustments to image every frame to be safe, since Unity isn't 
        /// guaranteed to report correct data as soon as device camera is started.
        /// </summary>
        private void Update()
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
              // Initialize the Texture2D
              Texture2D = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);

              // Notify that the camera has started
              Started = true;
              if (OnStarted != null)
              {
                OnStarted();
              }
            }
          }
          else
          {
            // Update the Texture2D content
            Texture2D.SetPixels32(WebCamTexture.GetPixels32());
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}