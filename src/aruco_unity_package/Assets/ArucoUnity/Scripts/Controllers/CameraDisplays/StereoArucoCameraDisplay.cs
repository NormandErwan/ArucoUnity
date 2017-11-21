using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraUndistortions;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraDisplays
  {
    /// <summary>
    /// Displays a <see cref="StereoArucoCamera"/>.
    /// </summary>
    public class StereoArucoCameraDisplay : ArucoCameraGenericDisplay<StereoArucoCamera>
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the 3D content aligned with the background.")]
      private new Camera camera;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the background.")]
      private Camera leftBackgroundCamera;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the background.")]
      private Camera rightBackgroundCamera;

      [SerializeField]
      [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
      private Renderer leftBackground;

      [SerializeField]
      [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
      private Renderer rightBackground;

      // Properties

      /// <summary>
      /// Gets or sets the Unity virtual stereo camera that will shoot the 3D content without the backgrounds.
      /// </summary>
      public Camera Camera { get { return camera; } set { camera = value; } }

      // Variables

      protected int leftCameraId = 0, rightCameraId = 1;

      // MonoBehaviour methods

      /// <summary>
      /// Populates <see cref="BackgroundCameras"/> and <see cref="Backgrounds"/> from editor fields if they are set.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();
        if (leftBackgroundCamera != null)
        {
          BackgroundCameras[leftCameraId] = leftBackgroundCamera;
        }
        if (rightBackgroundCamera != null)
        {
          BackgroundCameras[rightCameraId] = rightBackgroundCamera;
        }
        if (leftBackground != null)
        {
          Backgrounds[leftCameraId] = leftBackground;
        }
        if (rightBackground != null)
        {
          Backgrounds[rightCameraId] = rightBackground;
        }
      }

      // Methods

      protected override void ConfigureCamerasBackground()
      {
        base.ConfigureCamerasBackground();

        // TODO
      }

      protected override void SetDisplayActive(bool value)
      {
        base.SetDisplayActive(value);
        Camera.gameObject.SetActive(value);
      }
    }
  }

  /// \} aruco_unity_package
}
