using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Display the texture of a <see cref="ArucoCamera"/> on a canvas.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html.
    /// </summary>
    public class ArucoCameraCanvasDisplay : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The raw image that will display the camera device image.")]
      private RawImage image;

      [SerializeField]
      [Tooltip("The aspect ratio fitter associated with the raw image.")]
      private AspectRatioFitter imageFitter;

      [SerializeField]
      [Tooltip("The Aruco camera to display.")]
      private ArucoCamera arucoCamera;

      // MonoBehaviour methods

      /// <summary>
      /// Enable the image and subscribe to markers detector events.
      /// </summary>
      private void OnEnable()
      {
        arucoCamera.OnStarted += CameraDeviceController_OnActiveCameraStarted;
        if (arucoCamera.Started)
        {
          CameraDeviceController_OnActiveCameraStarted();
        }
      }

      /// <summary>
      /// Disable the image and unsubscribe to markers detector events.
      /// </summary>
      private void OnDisable()
      {
        arucoCamera.OnStarted -= CameraDeviceController_OnActiveCameraStarted;
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
      private void CameraDeviceController_OnActiveCameraStarted()
      {
        SetActiveTexture(arucoCamera.ImageTexture);

        image.rectTransform.localScale = arucoCamera.ImageScaleFrontFacing;
        image.rectTransform.localRotation = arucoCamera.ImageRotation;
        imageFitter.aspectRatio = arucoCamera.ImageRatio;
        image.uvRect = arucoCamera.ImageUvRectFlip;
      }
    }
  }

  /// \} aruco_unity_package
}