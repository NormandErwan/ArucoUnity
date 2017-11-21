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

      public const float cameraBackgroundDistance = 1f;

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

      public static void DefaultConfigureBackground(int cameraId, ArucoCamera arucoCamera, Camera backgroundCamera, GameObject background)
      {
        Vector3 localScale = Vector3.one;
        if (backgroundCamera.aspect < arucoCamera.ImageRatios[cameraId])
        {
          localScale.x = 2f * cameraBackgroundDistance * backgroundCamera.aspect * Mathf.Tan(0.5f * backgroundCamera.fieldOfView * Mathf.Deg2Rad);
          localScale.y = localScale.x / arucoCamera.ImageRatios[cameraId];
        }
        else
        {
          localScale.y = 2f * cameraBackgroundDistance * Mathf.Tan(0.5f * backgroundCamera.fieldOfView * Mathf.Deg2Rad);
          localScale.x = localScale.y * arucoCamera.ImageRatios[cameraId];
        }

        background.transform.localPosition = new Vector3(0, 0, cameraBackgroundDistance);
        background.transform.localScale = localScale;
      }

      /// <summary>
      /// Configures <see cref="Camera"/>, <see cref="BackgroundCamera"/> and <see cref="Background"/> according to the
      /// <see cref="ArucoCameraUndistortion"/> if set otherwise with default values.
      /// </summary>
      protected virtual void ConfigureCamerasBackground()
      {
        // Configure background and camera
        if (ArucoCameraUndistortion != null)
        {
          // Initialize
          Vector3 localPosition = new Vector3(0, 0, cameraBackgroundDistance);
          Vector3 localScale = Vector3.one;

          var cameraParameters = ArucoCameraUndistortion.CameraParametersController.CameraParameters;
          float imageWidth = cameraParameters.ImageWidths[cameraId];
          float imageHeight = cameraParameters.ImageHeights[cameraId];
          Vector2 cameraF = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();
          Vector2 cameraC = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraPrincipalPoint();

          // Configure the cameras fov
          float fovY = 2f * Mathf.Atan(0.5f * imageHeight / cameraF.y) * Mathf.Rad2Deg;
          Camera.fieldOfView = fovY;
          BackgroundCamera.fieldOfView = fovY;

          // Considering https://docs.opencv.org/3.3.0/d4/d94/tutorial_camera_calibration.html, we are looking for X=posX and Y=posY
          // with x=0.5*ImageWidth, y=0.5*ImageHeight (center of the camera projection) and w=Z=cameraBackgroundDistance 
          localPosition.x = (0.5f * imageWidth - cameraC.x) / cameraF.x * cameraBackgroundDistance;
          localPosition.y = -(0.5f * imageHeight - cameraC.y) / cameraF.y * cameraBackgroundDistance; // a minus because OpenCV camera coordinates origin is top - left, but bottom-left in Unity

          // Considering https://stackoverflow.com/a/41137160
          // scale.x = 2 * cameraBackgroundDistance * tan(fovx / 2), cameraF.x = imageWidth / (2 * tan(fovx / 2))
          localScale.x = imageWidth / cameraF.x * cameraBackgroundDistance;
          localScale.y = imageHeight / cameraF.y * cameraBackgroundDistance;

          // Place and scale the background
          Background.transform.localPosition = localPosition;
          Background.transform.localScale = localScale;
        }
        else
        {
          DefaultConfigureBackground(cameraId, ArucoCamera, BackgroundCamera, Background);
        }

        // Sets the background texture
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
