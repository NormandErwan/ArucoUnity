//
// Based on: http://answers.unity3d.com/answers/1155328/view.html
//

using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DeviceCameraCanvasDisplay : MonoBehaviour
    {
      [SerializeField]
      private RawImage image;

      [SerializeField]
      private RectTransform imageParent;

      [SerializeField]
      private AspectRatioFitter imageFitter;

      private DeviceCameraController deviceCameraController;

      // Image rotation
      Vector3 rotationVector = new Vector3(0f, 0f, 0f);

      // Image uvRect
      Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
      Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

      // Image parent's scale
      Vector3 defaultScale = new Vector3(1f, 1f, 1f);
      Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

      void Awake()
      {
        deviceCameraController = DeviceCameraController.Instance;
      }

      void OnEnable()
      {
        deviceCameraController.OnActiveCameraChanged += DeviceCameraController_OnActiveCameraChanged;
        deviceCameraController.OnCameraStarted += DeviceCameraController_OnCameraStarted;
      }

      void OnDisable()
      {
        deviceCameraController.OnActiveCameraChanged -= DeviceCameraController_OnActiveCameraChanged;
        deviceCameraController.OnCameraStarted -= DeviceCameraController_OnCameraStarted;
      }

      // Set the image texture of the active camera
      private void DeviceCameraController_OnActiveCameraChanged()
      {
        SetActiveTexture(deviceCameraController.activeCameraTexture2D);
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
        // Rotate image to show correct orientation 
        rotationVector.z = -deviceCameraController.activeCameraTexture.videoRotationAngle;
        image.rectTransform.localEulerAngles = rotationVector;

        // Set AspectRatioFitter's ratio
        float videoRatio = deviceCameraController.activeCameraTexture.width / (float)deviceCameraController.activeCameraTexture.height;
        imageFitter.aspectRatio = videoRatio;

        // Unflip if vertically flipped
        image.uvRect = deviceCameraController.activeCameraTexture.videoVerticallyMirrored ? fixedRect : defaultRect;

        // Mirror front-facing camera's image horizontally to look more natural
        imageParent.localScale = deviceCameraController.activeCameraDevice.isFrontFacing ? fixedScale : defaultScale;
      }
    }
  }
}