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
    public class ArucoCameraDisplay : ArucoCameraController<ArucoCamera>
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
      private Camera backgroundCamera;

      [SerializeField]
      [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
      private GameObject background;

      // Properties

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the 3D content aligned with the <see cref="Background"/>.
      /// </summary>
      public Camera Camera { get { return camera; } set { camera = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the <see cref="Background"/>.
      /// </summary>
      public Camera BackgroundCamera { get { return backgroundCamera; } set { backgroundCamera = value; } }

      /// <summary>
      /// Gets or sets the background displaying the <see cref="ArucoCamera.Images"/> of the corresponding physical camera in ArucoCamera.
      /// </summary>
      public GameObject Background { get { return background; } set { background = value; } }

      // Variables

      protected int cameraId = 0;

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
      /// Configures <see cref="Camera"/>, <see cref="BackgroundCamera"/> and <see cref="Background"/> according to the
      /// <see cref="ArucoCameraUndistortion"/> if set otherwise with default values.
      /// </summary>
      protected virtual void ConfigureCamerasBackground()
      {
        Vector2 position = Vector2.zero;
        Vector2 scale = Vector2.one;
        if (ArucoCameraUndistortion != null)
        {
          var cameraParameters = ArucoCameraUndistortion.CameraParametersController.CameraParameters;
          float imageWidth = cameraParameters.ImageWidths[cameraId];
          float imageHeight = cameraParameters.ImageHeights[cameraId];
          Vector2 cameraF = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();
          Vector2 cameraC = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraPrincipalPoint();

          // Configure the cameras
          float fovY = 2f * Mathf.Atan(0.5f * imageHeight / cameraF.y) * Mathf.Rad2Deg;
          Camera.fieldOfView = fovY;
          BackgroundCamera.fieldOfView = fovY;

          // Considering https://docs.opencv.org/3.3.0/d4/d94/tutorial_camera_calibration.html, we are looking for X=posX and Y=posY
          // with x=0.5*ImageWidth, y=0.5*ImageHeight (center of the camera projection) and w=Z=cameraBackgroundDistance 
          position.x = (0.5f * imageWidth - cameraC.x) / cameraF.x * cameraBackgroundDistance;
          position.y = -(0.5f * imageHeight - cameraC.y) / cameraF.y * cameraBackgroundDistance; // a minus because OpenCV camera coordinates origin is top - left, but bottom-left in Unity

          // Considering https://stackoverflow.com/a/41137160
          // scale.x = 2 * cameraBackgroundDistance * tan(fovx / 2), cameraF.x = imageWidth / (2 * tan(fovx / 2))
          scale.x = imageWidth / cameraF.x * cameraBackgroundDistance;
          scale.y = imageHeight / cameraF.y * cameraBackgroundDistance;
        }
        else
        {
          // Default placement of the background: centered and scaled on the unity camera
          if (BackgroundCamera.aspect < ArucoCamera.ImageRatios[cameraId])
          {
            scale.x = 2f * cameraBackgroundDistance * BackgroundCamera.aspect * Mathf.Tan(0.5f * BackgroundCamera.fieldOfView * Mathf.Deg2Rad);
            scale.y = scale.x / ArucoCamera.ImageRatios[cameraId];
          }
          else
          {
            scale.y = 2f * cameraBackgroundDistance * Mathf.Tan(0.5f * BackgroundCamera.fieldOfView * Mathf.Deg2Rad);
            scale.x = scale.y * ArucoCamera.ImageRatios[cameraId];
          }
        }

        // Configures the background
        Background.transform.localPosition = new Vector3(position.x, position.y, cameraBackgroundDistance);
        Background.transform.localScale = new Vector3(scale.x, scale.y, 1);
        Background.GetComponent<Renderer>().material.mainTexture = ArucoCamera.ImageTextures[cameraId];
      }

      /// <summary>
      /// Activates or deactivates <see cref="Camera"/>, <see cref="BackgroundCamera"/> and <see cref="Background"/>.
      /// </summary>
      /// <param name="value">True to activate, false to deactivate.</param>
      protected virtual void SetDisplayActive(bool value)
      {
        Camera.gameObject.SetActive(value);
        BackgroundCamera.gameObject.SetActive(value);
        Background.gameObject.SetActive(value);
      }
    }
  }

  /// \} aruco_unity_package
}
