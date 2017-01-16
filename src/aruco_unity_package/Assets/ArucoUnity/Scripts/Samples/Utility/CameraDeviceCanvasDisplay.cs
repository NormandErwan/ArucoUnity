using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    namespace Utility
    {
      /// <summary>
      /// Display the texture of the active camera on a canvas.
      /// Based on: http://answers.unity3d.com/answers/1155328/view.html.
      /// </summary>
      public class CameraDeviceCanvasDisplay : MonoBehaviour
      {
        [SerializeField]
        private RawImage image;

        [SerializeField]
        private AspectRatioFitter imageFitter;

        [SerializeField]
        private CameraDeviceController cameraDeviceController;

        /// <summary>
        /// Enable the image and subscribe to markers detector events.
        /// </summary>
        private void Awake()
        {
          cameraDeviceController.OnActiveCameraStarted += CameraDeviceController_OnActiveCameraStarted;
        }

        /// <summary>
        /// Disable the image and unsubscribe to markers detector events.
        /// </summary>
        private void OnDestroy()
        {
          cameraDeviceController.OnActiveCameraStarted -= CameraDeviceController_OnActiveCameraStarted;
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
        /// Configure the display of the active camera device when it's started.
        /// </summary>
        private void CameraDeviceController_OnActiveCameraStarted(CameraDevice activeCameraDevice)
        {
          SetActiveTexture(activeCameraDevice.Texture2D);

          image.rectTransform.localScale = activeCameraDevice.ImageScaleFrontFacing;
          image.rectTransform.localRotation = activeCameraDevice.ImageRotation;
          imageFitter.aspectRatio = activeCameraDevice.ImageRatio;
          image.uvRect = activeCameraDevice.ImageUvRectFlip;
        }
      }
    }
  }

  /// \} aruco_unity_package
}