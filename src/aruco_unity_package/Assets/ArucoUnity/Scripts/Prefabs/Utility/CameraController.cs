//
// Based on: http://answers.unity3d.com/answers/1155328/view.html
//

using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CameraController : Singleton<CameraController>
    {
      // Properties
      public bool CameraStarted { get; private set; }
      public WebCamDevice ActiveCameraDevice { get; private set; }
      public WebCamTexture ActiveCameraTexture { get; private set; }
      public Texture2D ActiveCameraTexture2D { get; private set; }

      // Events
      public delegate void CameraAction();
      public event CameraAction OnCameraStarted;
      public event CameraAction OnActiveCameraChanged;

      // Device cameras
      private int activeCameraDeviceIndex;

      // The correct image orientation 
      public Quaternion ImageRotation
      {
        get
        {
          return Quaternion.Euler(0f, 0f, -ActiveCameraTexture.videoRotationAngle);
        }
        private set { }
      }
      
      // The image ratio
      public float ImageRatio
      {
        get
        {
          return ActiveCameraTexture.width / (float)ActiveCameraTexture.height;
        }
        private set { }
      }

      // Allow to unflip the image if vertically flipped (use for image plane)
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
          mesh.uv = ActiveCameraTexture.videoVerticallyMirrored ? verticallyMirroredUv : defaultUv;

          mesh.RecalculateNormals();

          return mesh;
        }
        private set { }
      }

      // Allow to unflip the image if vertically flipped (use for canvas)
      public Rect ImageUvRectFlip
      {
        get
        {
          Rect defaultRect = new Rect(0f, 0f, 1f, 1f),
               verticallyMirroredRect = new Rect(0f, 1f, 1f, -1f);
          return ActiveCameraTexture.videoVerticallyMirrored ? verticallyMirroredRect : defaultRect;
        }
        private set { }
      }

      // Mirror front-facing camera's image horizontally to look more natural
      public Vector3 ImageScaleFrontFacing
      {
        get
        {
          Vector3 defaultScale = new Vector3(1f, 1f, 1f),
                  frontFacingScale = new Vector3(-1f, 1f, 1f);
          return ActiveCameraDevice.isFrontFacing ? frontFacingScale : defaultScale;
        }
        private set { }
      }
      
      void Start()
      {
        activeCameraDeviceIndex = -1;
        SwitchCamera(); // Activate the first camera
      }

      public void SetActiveCamera(WebCamDevice cameraToUse)
      {
        // Switch the activeCameraTexture
        if (ActiveCameraTexture != null)
        {
          ActiveCameraTexture.Stop();
        }

        ActiveCameraDevice = cameraToUse;
        ActiveCameraTexture = new WebCamTexture(cameraToUse.name);
        ActiveCameraTexture.filterMode = FilterMode.Trilinear;

        ActiveCameraTexture.Play();

        // Reset the Texture2D
        ActiveCameraTexture2D = new Texture2D(ActiveCameraTexture.width, ActiveCameraTexture.height,
          TextureFormat.RGB24, false);

        // Call the event
        if (OnActiveCameraChanged != null)
        {
          OnActiveCameraChanged();
        }
        CameraStarted = false;
      }

      // Switch between cameras
      public void SwitchCamera()
      {
        WebCamDevice[] cameraDevices = WebCamTexture.devices;

        // Check for device cameras
        if (cameraDevices.Length == 0)
        {
          Debug.Log("No devices cameras found");
          return;
        }

        // Switch to the next camera
        activeCameraDeviceIndex++;
        activeCameraDeviceIndex %= cameraDevices.Length;
        print(activeCameraDeviceIndex);

        ActiveCameraDevice = cameraDevices[activeCameraDeviceIndex];
        SetActiveCamera(ActiveCameraDevice);
      }

      // Make adjustments to image every frame to be safe, since Unity isn't 
      // guaranteed to report correct data as soon as device camera is started
      void Update()
      {
        // Skip making adjustment for incorrect camera data
        if (ActiveCameraTexture.width < 100)
        {
          Debug.Log("Still waiting another frame for correct info...");
          return;
        }
        else
        {
          if (OnCameraStarted != null && !CameraStarted)
          {
            OnCameraStarted();
          }
          CameraStarted = true;
        }

        // Update the Texture2D content
        ActiveCameraTexture2D.SetPixels32(ActiveCameraTexture.GetPixels32());
      }
    }
  }
}