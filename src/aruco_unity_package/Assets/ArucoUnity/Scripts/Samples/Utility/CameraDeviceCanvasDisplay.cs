using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Samples
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
      private CameraDeviceMarkersDetector markersDetector;

      /// <summary>
      /// Enable the image and subscribe to markers detector events.
      /// </summary>
      private void Awake()
      {
        markersDetector.OnConfigurated += CameraDeviceMarkersDetector_OnConfigurated;
      }

      /// <summary>
      /// Disable the image and unsubscribe to markers detector events.
      /// </summary>
      private void OnDestroy()
      {
        markersDetector.OnConfigurated -= CameraDeviceMarkersDetector_OnConfigurated;
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
      private void CameraDeviceMarkersDetector_OnConfigurated()
      {
        CameraDevice activeCameraDevice = markersDetector.CameraDeviceController.ActiveCameraDevice;

        SetActiveTexture(activeCameraDevice.Texture2D);

        image.rectTransform.localScale = activeCameraDevice.ImageScaleFrontFacing;
        image.rectTransform.localRotation = activeCameraDevice.ImageRotation;
        imageFitter.aspectRatio = activeCameraDevice.ImageRatio;
        image.uvRect = activeCameraDevice.ImageUvRectFlip;
      }
    }
  }
}