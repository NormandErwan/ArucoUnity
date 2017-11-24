using ArucoUnity.Cameras;
using ArucoUnity.Utilities;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraDisplays
  {
    /// <summary>
    /// Displays a mono <see cref="ArucoCamera"/>.
    /// </summary>
    public class ArucoCameraDisplay : ArucoCameraGenericDisplay<ArucoCamera>
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the 3D content aligned with the background.")]
      private new Camera camera;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the background.")]
      private Camera backgroundCamera;

      [SerializeField]
      [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
      private Renderer background;

      // Variables

      protected int cameraId = 0;

      // MonoBehaviour methods

      /// <summary>
      /// Populates <see cref="BackgroundCameras"/> and <see cref="Backgrounds"/> from editor fields if they are set.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();
        if (camera != null)
        {
          Cameras[cameraId] = camera;
        }
        if (backgroundCamera != null)
        {
          BackgroundCameras[cameraId] = backgroundCamera;
        }
        if (background != null)
        {
          Backgrounds[cameraId] = background;
        }
      }

      // Methods

      protected override void ConfigureCamerasBackground()
      {
        base.ConfigureCamerasBackground();

        if (ArucoCameraUndistortion != null)
        {
          // Initialize
          var cameraParameters = ArucoCameraUndistortion.CameraParameters;
          float imageWidth = cameraParameters.ImageWidths[cameraId];
          float imageHeight = cameraParameters.ImageHeights[cameraId];
          Vector2 cameraF = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();
          Vector2 cameraC = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraPrincipalPoint();

          // Configure the cameras fov
          float fovY = 2f * Mathf.Atan(0.5f * imageHeight / cameraF.y) * Mathf.Rad2Deg;
          Cameras[cameraId].fieldOfView = fovY;
          BackgroundCameras[cameraId].fieldOfView = fovY;

          // Considering https://docs.opencv.org/3.3.0/d4/d94/tutorial_camera_calibration.html, we are looking for X=posX and Y=posY
          // with x=0.5*ImageWidth, y=0.5*ImageHeight (center of the camera projection) and w=Z=cameraBackgroundDistance 
          float localPositionX = (0.5f * imageWidth - cameraC.x) / cameraF.x * cameraBackgroundDistance;
          float localPositionY = -(0.5f * imageHeight - cameraC.y) / cameraF.y * cameraBackgroundDistance; // a minus because OpenCV camera coordinates origin is top - left, but bottom-left in Unity

          // Considering https://stackoverflow.com/a/41137160
          // scale.x = 2 * cameraBackgroundDistance * tan(fovx / 2), cameraF.x = imageWidth / (2 * tan(fovx / 2))
          float localScaleX = imageWidth / cameraF.x * cameraBackgroundDistance;
          float localScaleY = imageHeight / cameraF.y * cameraBackgroundDistance;

          // Place and scale the background
          Backgrounds[cameraId].transform.localPosition = new Vector3(localPositionX, localPositionY, cameraBackgroundDistance);
          Backgrounds[cameraId].transform.localScale = new Vector3(localScaleX, localScaleY, 1);
        }
      }
    }
  }

  /// \} aruco_unity_package
}
