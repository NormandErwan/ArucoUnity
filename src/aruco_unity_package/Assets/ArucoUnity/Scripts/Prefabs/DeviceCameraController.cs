//
// Based on: http://answers.unity3d.com/answers/1155328/view.html
//

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DeviceCameraController : MonoBehaviour
    {
      [SerializeField]
      private RawImage image;

      [SerializeField]
      private RectTransform imageParent;

      [SerializeField]
      private AspectRatioFitter imageFitter;

      [NonSerialized]
      public bool cameraStarted;
      
      [NonSerialized]
      public WebCamTexture activeCameraTexture;

      public delegate void CameraStartedAction();
      public static event CameraStartedAction OnCameraStarted; 

      // Device cameras
      WebCamDevice frontCameraDevice;
      WebCamDevice backCameraDevice;
      WebCamDevice activeCameraDevice;

      WebCamTexture frontCameraTexture;
      WebCamTexture backCameraTexture;

      // Image rotation
      Vector3 rotationVector = new Vector3(0f, 0f, 0f);

      // Image uvRect
      Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
      Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

      // Image Parent's scale
      Vector3 defaultScale = new Vector3(1f, 1f, 1f);
      Vector3 fixedScale = new Vector3(-1f, 1f, 1f);


      void Start()
      {
        cameraStarted = false;

        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
          Debug.Log("No devices cameras found");
          return;
        }

        // Get the device's cameras and create WebCamTextures with them
        frontCameraDevice = WebCamTexture.devices.Last();
        backCameraDevice = WebCamTexture.devices.First();

        frontCameraTexture = new WebCamTexture(frontCameraDevice.name);
        backCameraTexture = new WebCamTexture(backCameraDevice.name);

        // Set camera filter modes for a smoother looking image
        frontCameraTexture.filterMode = FilterMode.Trilinear;
        backCameraTexture.filterMode = FilterMode.Trilinear;

        // Set the camera to use by default
        SetActiveCamera(frontCameraTexture);
      }

      // Set the device camera to use and start it
      public void SetActiveTexture(Texture textureToUse)
      {
        image.texture = textureToUse;
        image.material.mainTexture = textureToUse;
      }

      public void SetActiveCamera(WebCamTexture cameraToUse)
      {
        if (activeCameraTexture != null)
        {
          activeCameraTexture.Stop();
        }

        activeCameraTexture = cameraToUse;
        activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device =>
            device.name == cameraToUse.deviceName);

        SetActiveTexture(activeCameraTexture);

        activeCameraTexture.Play();
      }

      // Switch between the device's front and back camera
      public void SwitchCamera()
      {
        SetActiveCamera(activeCameraTexture.Equals(frontCameraTexture) ?
            backCameraTexture : frontCameraTexture);
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

        // Rotate image to show correct orientation 
        rotationVector.z = -activeCameraTexture.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        // Set AspectRatioFitter's ratio
        float videoRatio =
            (float)activeCameraTexture.width / (float)activeCameraTexture.height;
        imageFitter.aspectRatio = videoRatio;

        // Unflip if vertically flipped
        image.uvRect =
            activeCameraTexture.videoVerticallyMirrored ? fixedRect : defaultRect;

        // Mirror front-facing camera's image horizontally to look more natural
        imageParent.localScale =
            activeCameraDevice.isFrontFacing ? fixedScale : defaultScale;
      }
    }
  }
}