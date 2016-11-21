//
// Based on: http://answers.unity3d.com/answers/1155328/view.html
//

using UnityEngine;
using System.Linq;

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
      private WebCamDevice frontCameraDevice;
      private WebCamDevice backCameraDevice;

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

      // Allow to unflip the image if vertically flipped
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
        CameraStarted = false;

        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
          Debug.Log("No devices cameras found");
          return;
        }

        // Get the device's cameras
        frontCameraDevice = WebCamTexture.devices.Last();
        backCameraDevice = WebCamTexture.devices.First();

        // Set the camera to use by default
        SetActiveCamera(frontCameraDevice);
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
      }

      // Switch between the device's front and back camera
      public void SwitchCamera()
      {
        SetActiveCamera(ActiveCameraDevice.Equals(frontCameraDevice) ? backCameraDevice : frontCameraDevice);
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