using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraUndistortions;
using ArucoUnity.Utilities;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraDisplays
  {
    /// <summary>
    /// Manages a Unity virual augmented reality camera that shoot 3D content aligned with the <see cref="ArucoCamera.Images"/> from one physical
    /// camera displayed as background.
    /// </summary>
    public class StereoArucoCameraDisplay : ArucoCameraController<StereoArucoCamera>
    {
      // Constants

      protected const float cameraBackgroundDistance = 1f;

      // Editor fields

      [SerializeField]
      [Tooltip("Optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

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
      private GameObject leftBackground;

      [SerializeField]
      [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
      private GameObject rightBackground;

      // Properties

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the 3D content aligned with the <see cref="LeftBackground"/>.
      /// </summary>
      public Camera Camera { get { return camera; } set { camera = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the <see cref="LeftBackground"/>.
      /// </summary>
      public Camera LeftBackgroundCamera { get { return leftBackgroundCamera; } set { leftBackgroundCamera = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the <see cref="LeftBackground"/>.
      /// </summary>
      public Camera RightBackgroundCamera { get { return rightBackgroundCamera; } set { rightBackgroundCamera = value; } }

      /// <summary>
      /// Gets or sets the background displaying the <see cref="ArucoCamera.Images"/> of the corresponding physical camera in ArucoCamera.
      /// </summary>
      public GameObject LeftBackground { get { return leftBackground; } set { leftBackground = value; } }

      /// <summary>
      /// Gets or sets the background displaying the <see cref="ArucoCamera.Images"/> of the corresponding physical camera in ArucoCamera.
      /// </summary>
      public GameObject RightBackground { get { return rightBackground; } set { rightBackground = value; } }

      // Variables

      protected int leftCameraId = 0, rightCameraId = 1;

      // ArucoCameraController methods

      /// <summary>
      /// Calls <see cref="ConfigureCamerasBackground"/> the <see cref="SetDisplayActive(bool)"/> to activate the display.
      /// </summary>
      public override void StartController()
      {
        base.StartController();
        ConfigureCamerasBackground();
        SetDisplayActive(true);
      }

      /// <summary>
      /// Deactivates the display with <see cref="SetDisplayActive(bool)"/>.
      /// </summary>
      public override void StopController()
      {
        base.StopController();
        SetDisplayActive(false);
      }

      protected override void Configure()
      {
      }

      // Methods

      /// <summary>
      /// Configures <see cref="Camera"/>, <see cref="LeftBackgroundCamera"/> and <see cref="LeftBackground"/> according to the
      /// <see cref="ArucoCameraUndistortion"/> if set otherwise with default values.
      /// </summary>
      protected virtual void ConfigureCamerasBackground()
      {
        // Place and scale the backgrounds
        ArucoCameraDisplay.DefaultConfigureBackground(leftCameraId, ArucoCamera, LeftBackgroundCamera, LeftBackground);
        ArucoCameraDisplay.DefaultConfigureBackground(rightCameraId, ArucoCamera, RightBackgroundCamera, RightBackground);

        // Sets the background textures
        LeftBackground.GetComponent<Renderer>().material.mainTexture = ArucoCamera.ImageTextures[leftCameraId];
        RightBackground.GetComponent<Renderer>().material.mainTexture = ArucoCamera.ImageTextures[rightCameraId];
      }

      /// <summary>
      /// Activates or deactivates <see cref="Camera"/>, <see cref="LeftBackgroundCamera"/> and <see cref="LeftBackground"/>.
      /// </summary>
      /// <param name="value">True to activate, false to deactivate.</param>
      protected virtual void SetDisplayActive(bool value)
      {
        Camera.gameObject.SetActive(value);
        LeftBackgroundCamera.gameObject.SetActive(value);
        RightBackgroundCamera.gameObject.SetActive(value);
        LeftBackground.gameObject.SetActive(value);
        RightBackground.gameObject.SetActive(value);
      }
    }
  }

  /// \} aruco_unity_package
}
