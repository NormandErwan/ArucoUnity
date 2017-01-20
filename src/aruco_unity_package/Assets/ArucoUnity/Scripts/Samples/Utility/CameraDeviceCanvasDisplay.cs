using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Display the texture of the active camera on a canvas.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html.
    /// </summary>
    public class CameraDeviceCanvasDisplay : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The raw image that will display the camera device image.")]
      private RawImage image;

      [SerializeField]
      [Tooltip("The aspect ratio fitter associated with the raw image.")]
      private AspectRatioFitter imageFitter;

      [SerializeField]
      [Tooltip("The camera device controller from which its active camera device will be displayed.")]
      private CameraDeviceController cameraDeviceController;

      // MonoBehaviour methods

      /// <summary>
      /// Enable the image and subscribe to markers detector events.
      /// </summary>
      private void OnEnable()
      {
        cameraDeviceController.OnActiveCameraDeviceStarted += CameraDeviceController_OnActiveCameraStarted;
      }

      /// <summary>
      /// Disable the image and unsubscribe to markers detector events.
      /// </summary>
      private void OnDisable()
      {
        cameraDeviceController.OnActiveCameraDeviceStarted -= CameraDeviceController_OnActiveCameraStarted;
      }

      // Methods

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

  /// \} aruco_unity_package
}