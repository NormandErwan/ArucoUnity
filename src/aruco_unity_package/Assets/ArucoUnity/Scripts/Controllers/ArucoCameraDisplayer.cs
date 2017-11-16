using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraUndistortions;
using ArucoUnity.Utilities;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Creates planes displaying each <see cref="ArucoCamera.Images"/> as background of it corresponding <see cref="ArucoCamera.ImageCameras"/>. If
    /// <see cref="ArucoCamera.CalibrationController"/> is set, the background is configured to makes the tracked
    /// <see cref="Objects.ArucoObject"/> aligned with their corresponding 3D content.
    /// </summary>
    public class ArucoCameraDisplayer : ArucoCameraController
    {
      // Constants

      protected const float cameraBackgroundDistance = 1f;

      // Editor fields

      [SerializeField]
      [Tooltip("Optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      // Properties

      /// <summary>
      /// Gets or sets the Optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      /// <summary>
      /// Gets the planes displaying the <see cref="ArucoCamera.Images"/> as background of the <see cref="ArucoCamera.ImageCameras"/>.
      /// </summary>
      public GameObject[] ImageCameraBackgrounds { get; protected set; }

      // ArucoCameraController methods

      /// <summary>
      /// Configures the <see cref="ImageCameraBackgrounds"/> according to the <see cref="ArucoCamera.CalibrationController"/> if set otherwise
      /// with default values.
      /// </summary>
      // TODO: handle case of CameraParameters.ImageHeight != ImageTexture.height or CameraParameters.ImageWidth != ImageTexture.width
      // TODO: handle case of CameraParameters.FixAspectRatio != 0
      public override void StartController()
      {
        base.StartController();

        ImageCameraBackgrounds = new GameObject[ArucoCamera.CameraNumber];

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          // Configure the background
          if (ImageCameraBackgrounds[cameraId] == null)
          {
            ImageCameraBackgrounds[cameraId] = GameObject.CreatePrimitive(PrimitiveType.Quad);
            ImageCameraBackgrounds[cameraId].name = "CameraBackground";
            ImageCameraBackgrounds[cameraId].transform.SetParent(ArucoCamera.ImageCameras[cameraId].transform);
            ImageCameraBackgrounds[cameraId].transform.localRotation = Quaternion.identity;

            var cameraBackgroundRenderer = ImageCameraBackgrounds[cameraId].GetComponent<Renderer>();
            cameraBackgroundRenderer.material = Resources.Load("UnlitImage") as Material;
            cameraBackgroundRenderer.material.mainTexture = ArucoCamera.ImageTextures[cameraId];
          }
          ImageCameraBackgrounds[cameraId].SetActive(true);

          // Place background
          Vector2 position = Vector2.zero;
          Vector2 scale = Vector2.one;
          if (ArucoCameraUndistortion != null && ArucoCameraUndistortion.IsStarted)
          {
            float imageWidth = ArucoCameraUndistortion.CameraParametersController.CameraParameters.ImageWidths[cameraId];
            float imageHeight = ArucoCameraUndistortion.CameraParametersController.CameraParameters.ImageHeights[cameraId];
            Vector2 cameraF = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();
            Vector2 cameraC = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraPrincipalPoint();

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
            // Default : place background centered on the camera and full size of the camera image
            float aspectRatioFactor = Mathf.Min(ArucoCamera.ImageRatios[cameraId], 1f);
            scale.y = 2 * cameraBackgroundDistance * aspectRatioFactor * Mathf.Tan(0.5f * ArucoCamera.ImageCameras[cameraId].fieldOfView * Mathf.Deg2Rad);
            scale.x = scale.y * ArucoCamera.ImageRatios[cameraId];
          }

          ImageCameraBackgrounds[cameraId].transform.localPosition = new Vector3(position.x, position.y, cameraBackgroundDistance);
          ImageCameraBackgrounds[cameraId].transform.localScale = new Vector3(scale.x, scale.y, 1);
        }
      }

      /// <summary>
      /// Deactivates the <see cref="ImageCameraBackgrounds"/>.
      /// </summary>
      public override void StopController()
      {
        base.StopController();

        foreach (var cameraBackground in ImageCameraBackgrounds)
        {
          if (cameraBackground != null)
          {
            cameraBackground.SetActive(false);
          }
        }
      }

      protected override void Configure()
      {
      }
    }
  }

  /// \} aruco_unity_package
}
