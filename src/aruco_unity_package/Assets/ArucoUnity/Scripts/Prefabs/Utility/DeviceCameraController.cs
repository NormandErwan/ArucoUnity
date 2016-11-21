//
// Based on: http://answers.unity3d.com/answers/1155328/view.html
//

using UnityEngine;
using System.Linq;
using System;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DeviceCameraController : Singleton<DeviceCameraController>
    {
      // Active camera info
      [NonSerialized]
      public bool cameraStarted;

      [NonSerialized]
      public WebCamTexture activeCameraTexture;

      [NonSerialized]
      public Texture2D activeCameraTexture2D;

      // Events
      public delegate void CameraAction();
      public event CameraAction OnCameraStarted;
      public event CameraAction OnActiveCameraChanged;
      public event CameraAction OnActiveTextureChanged;

      // Device cameras
      public WebCamDevice activeCameraDevice;
      private WebCamDevice frontCameraDevice;
      private WebCamDevice backCameraDevice;

      // The correct image orientation 
      public Quaternion ImageRotation
      {
        get
        {
          return Quaternion.Euler(0f, 0f, -activeCameraTexture.videoRotationAngle);
        }
        private set { }
      }
      
      // The image ratio
      public float ImageRatio
      {
        get
        {
          return activeCameraTexture.width / (float)activeCameraTexture.height;
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
          return activeCameraTexture.videoVerticallyMirrored ? verticallyMirroredRect : defaultRect;
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
          return activeCameraDevice.isFrontFacing ? frontFacingScale : defaultScale;
        }
        private set { }
      }
      
      void Start()
      {
        cameraStarted = false;

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
        if (activeCameraTexture != null)
        {
          activeCameraTexture.Stop();
        }

        activeCameraDevice = cameraToUse;
        activeCameraTexture = new WebCamTexture(cameraToUse.name);
        activeCameraTexture.filterMode = FilterMode.Trilinear;

        activeCameraTexture.Play();

        // Reset the Texture2D
        activeCameraTexture2D = new Texture2D(activeCameraTexture.width, activeCameraTexture.height,
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
        SetActiveCamera(activeCameraDevice.Equals(frontCameraDevice) ? backCameraDevice : frontCameraDevice);
      }

      // Make adjustments to image every frame to be safe, since Unity isn't 
      // guaranteed to report correct data as soon as device camera is started
      void Update()
      {
        // Skip making adjustment for incorrect camera data
        if (activeCameraTexture.width < 100)
        {
          Debug.Log("Still waiting another frame for correct info...");
          return;
        }
        else
        {
          if (OnCameraStarted != null && !cameraStarted)
          {
            OnCameraStarted();
          }
          cameraStarted = true;
        }

        // Update the Texture2D content
        activeCameraTexture2D.SetPixels32(activeCameraTexture.GetPixels32());
      }
    }
  }
}