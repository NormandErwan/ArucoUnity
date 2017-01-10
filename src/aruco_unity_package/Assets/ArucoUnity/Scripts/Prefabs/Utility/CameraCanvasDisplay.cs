using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    /// <summary>
    /// Display the texture of the active camera on a canvas.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html.
    /// </summary>
    public class CameraCanvasDisplay : MonoBehaviour
    {
      [SerializeField]
      private RawImage image;

      [SerializeField]
      private AspectRatioFitter imageFitter;

      private CameraDeviceController deviceCameraController;

      void Awake()
      {
        deviceCameraController = CameraDeviceController.Instance;
      }

      /// <summary>
      /// Enable the image and subscribe to active camera events.
      /// </summary>
      void OnEnable()
      {
        image.enabled = true;

        deviceCameraController.OnActiveCameraChanged += DeviceCameraController_OnActiveCameraChanged;
        deviceCameraController.OnCameraStarted += DeviceCameraController_OnCameraStarted;
      }

      /// <summary>
      /// Disable the image and unsubscribe to active camera events.
      /// </summary>
      void OnDisable()
      {
        image.enabled = false;

        deviceCameraController.OnActiveCameraChanged -= DeviceCameraController_OnActiveCameraChanged;
        deviceCameraController.OnCameraStarted -= DeviceCameraController_OnCameraStarted;
      }

      /// <summary>
      /// When the active camera changes, set its texture to display.
      /// </summary>
      private void DeviceCameraController_OnActiveCameraChanged()
      {
        SetActiveTexture(deviceCameraController.ActiveCameraTexture2D);
      }

      /// <summary>
      /// Set the new texture to display.
      /// </summary>
      public void SetActiveTexture(Texture textureToUse)
      {
        image.texture = textureToUse;
        image.material.mainTexture = textureToUse;
      }

      /// <summary>
      /// Configure the image display when the camera is started.
      /// </summary>
      private void DeviceCameraController_OnCameraStarted()
      {
        image.rectTransform.localScale = deviceCameraController.ImageScaleFrontFacing;
        image.rectTransform.localRotation = deviceCameraController.ImageRotation;
        imageFitter.aspectRatio = deviceCameraController.ImageRatio;
        image.uvRect = deviceCameraController.ImageUvRectFlip;
      }
    }
  }
}