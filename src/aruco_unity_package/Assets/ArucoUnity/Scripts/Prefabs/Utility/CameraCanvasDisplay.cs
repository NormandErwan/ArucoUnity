//
// Based on: http://answers.unity3d.com/answers/1155328/view.html
//

using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CameraCanvasDisplay : MonoBehaviour
    {
      [SerializeField]
      private RawImage image;

      [SerializeField]
      private RectTransform imageParent;

      [SerializeField]
      private AspectRatioFitter imageFitter;

      private CameraController deviceCameraController;

      void Awake()
      {
        deviceCameraController = CameraController.Instance;
      }

      void OnEnable()
      {
        image.gameObject.SetActive(true);

        deviceCameraController.OnActiveCameraChanged += DeviceCameraController_OnActiveCameraChanged;
        deviceCameraController.OnCameraStarted += DeviceCameraController_OnCameraStarted;
      }

      void OnDisable()
      {
        image.gameObject.SetActive(false);

        deviceCameraController.OnActiveCameraChanged -= DeviceCameraController_OnActiveCameraChanged;
        deviceCameraController.OnCameraStarted -= DeviceCameraController_OnCameraStarted;
      }

      // Set the image texture of the active camera
      private void DeviceCameraController_OnActiveCameraChanged()
      {
        SetActiveTexture(deviceCameraController.ActiveCameraTexture2D);
      }

      // If the texture is modified outside the class, use this new texture to display
      public void SetActiveTexture(Texture textureToUse)
      {
        image.texture = textureToUse;
        image.material.mainTexture = textureToUse;
      }

      // Configure the image display when the camera is started
      private void DeviceCameraController_OnCameraStarted()
      {
        image.rectTransform.localRotation = deviceCameraController.ImageRotation;
        imageFitter.aspectRatio = deviceCameraController.ImageRatio;
        image.uvRect = deviceCameraController.ImageUvRectFlip;
        imageParent.localScale = deviceCameraController.ImageScaleFrontFacing;
      }
    }
  }
}