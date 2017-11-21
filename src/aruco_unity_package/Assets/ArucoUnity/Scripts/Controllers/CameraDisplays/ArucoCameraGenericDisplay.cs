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
    /// Manages Unity virual cameras that shoot 3D content aligned with the <see cref="ArucoCamera.Images"/> displayed as background. It creates the
    /// augmented reality effect by the images from the physical cameras and the <see cref="Objects.ArucoObject"/> tracked by
    /// <see cref="ObjectTrackers.ArucoObjectsTracker"/>.
    /// </summary>
    public class ArucoCameraGenericDisplay<T> : ArucoCameraController<T> where T : ArucoCamera
    {
      // Constants

      public const float cameraBackgroundDistance = 1f;

      // Editor fields

      [SerializeField]
      [Tooltip("Optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      // Properties

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the <see cref="Backgrounds"/>.
      /// </summary>
      public Camera[] BackgroundCameras { get; set; }

      /// <summary>
      /// Gets or sets the backgrounds displaying the <see cref="ArucoCamera.Images"/> of the corresponding physical camera in ArucoCamera.
      /// </summary>
      public Renderer[] Backgrounds { get; set; }

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();
        BackgroundCameras = new Camera[ArucoCamera.CameraNumber];
        Backgrounds = new Renderer[ArucoCamera.CameraNumber];
      }

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

        if (ArucoCameraUndistortion != null)
        {
          ArucoCameraUndistortion.Started -= StartController;
        }
      }

      /// <summary>
      /// If <see cref="ArucoCameraUndistortion"/> is set, wait is started before starting this display.
      /// </summary>
      protected override void Configure()
      {
        if (ArucoCameraUndistortion != null)
        {
          AutoStart = false;
          ArucoCameraUndistortion.Started += StartController;
        }
      }

      // Methods

      /// <summary>
      /// Configures the <see cref="BackgroundCameras"/> and the <see cref="Backgrounds"/> according to the
      /// <see cref="ArucoCameraUndistortion"/> if set otherwise with default values.
      /// </summary>
      protected virtual void ConfigureCamerasBackground()
      {
        // Sets the background texture
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          Backgrounds[cameraId].material.mainTexture = ArucoCamera.ImageTextures[cameraId];
        }

        // Default background configuration is ArucoCameraUndistortion is null
        if (ArucoCameraUndistortion == null)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            DefaultConfigureBackground(cameraId);
          }
        }
      }

      /// <summary>
      /// Activates or deactivates <see cref="BackgroundCameras"/> and <see cref="Backgrounds"/>.
      /// </summary>
      /// <param name="value">True to activate, false to deactivate.</param>
      protected virtual void SetDisplayActive(bool value)
      {
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          BackgroundCameras[cameraId].gameObject.SetActive(value);
          Backgrounds[cameraId].gameObject.SetActive(value);
        }
      }

      /// <summary>
      /// Place the background centered in front of the background camera scaled to fit in the camera view.
      /// </summary>
      protected virtual void DefaultConfigureBackground(int cameraId)
      {
        Vector3 localScale = Vector3.one;
        if (BackgroundCameras[cameraId].aspect < ArucoCamera.ImageRatios[cameraId])
        {
          localScale.x = 2f * cameraBackgroundDistance * BackgroundCameras[cameraId].aspect * Mathf.Tan(0.5f * BackgroundCameras[cameraId].fieldOfView * Mathf.Deg2Rad);
          localScale.y = localScale.x / ArucoCamera.ImageRatios[cameraId];
        }
        else
        {
          localScale.y = 2f * cameraBackgroundDistance * Mathf.Tan(0.5f * BackgroundCameras[cameraId].fieldOfView * Mathf.Deg2Rad);
          localScale.x = localScale.y * ArucoCamera.ImageRatios[cameraId];
        }

        Backgrounds[cameraId].transform.localPosition = new Vector3(0, 0, cameraBackgroundDistance);
        Backgrounds[cameraId].transform.localScale = localScale;
      }
    }
  }

  /// \} aruco_unity_package
}
