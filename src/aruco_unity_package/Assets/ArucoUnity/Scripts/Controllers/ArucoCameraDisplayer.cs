using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraUndistortions;
using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Creates planes displaying each <see cref="ArucoCamera.Images"/> as background of it corresponding <see cref="Cameras"/>. If
    /// <see cref="ArucoCameraUndistortion"/> is set, the background is configured to makes the tracked <see cref="Objects.ArucoObject"/> aligned
    /// with their corresponding 3D content.
    /// </summary>
    public class ArucoCameraDisplayer : ArucoCameraController
    {
      // Constants

      protected const float cameraBackgroundDistance = 1f;

      // Editor fields

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the images of the physical cameras. Must be equal to ArucoCamera.CameraNumber.")]
      private Camera[] cameras;

      [SerializeField]
      [Tooltip("Optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      // Properties

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      /// <summary>
      /// Gets or sets the Unity virtual cameras that will shoot the images of the physical cameras. There is one for each physical camera 
      /// (<see cref="ArucoCamera.CameraNumber"/> cameras).
      /// </summary>
      public Camera[] Cameras { get { return cameras; } set { cameras = value; } }

      /// <summary>
      /// Gets or sets the prefab of the <see cref="CameraBackgrounds"/>. If null, default will be loaded:
      /// `Prefabs/Resources/ArucoCameraImagePlane.prefab`.
      /// </summary>
      public GameObject CameraBackgroundsPrefab { get; set; }

      /// <summary>
      /// Gets the planes displaying the <see cref="ArucoCamera.Images"/> as background of the <see cref="Cameras"/>.
      /// </summary>
      public GameObject[] CameraBackgrounds { get; protected set; }

      // ArucoCameraController methods

      /// <summary>
      /// Configures the <see cref="CameraBackgrounds"/> according to the <see cref="ArucoCamera.CalibrationController"/> if set otherwise
      /// with default values.
      /// </summary>
      // TODO: handle case of CameraParameters.ImageHeight != ImageTexture.height or CameraParameters.ImageWidth != ImageTexture.width
      // TODO: handle case of CameraParameters.FixAspectRatio != 0
      public override void StartController()
      {
        base.StartController();

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          Vector2 position = Vector2.zero;
          Vector2 scale = Vector2.one;
          if (ArucoCameraUndistortion != null && ArucoCameraUndistortion.IsStarted)
          {
            float imageWidth = ArucoCameraUndistortion.CameraParametersController.CameraParameters.ImageWidths[cameraId];
            float imageHeight = ArucoCameraUndistortion.CameraParametersController.CameraParameters.ImageHeights[cameraId];
            Vector2 cameraF = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();
            Vector2 cameraC = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraPrincipalPoint();

            // Configure the camera
            float fovY = 2f * Mathf.Atan(0.5f * imageHeight / cameraF.y) * Mathf.Rad2Deg;
            Cameras[cameraId].fieldOfView = fovY;

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
            // Default placement of the background: centered on the camera and full size of the camera image
            float aspectRatioFactor = Mathf.Min(ArucoCamera.ImageRatios[cameraId], 1f);
            scale.y = 2 * cameraBackgroundDistance * aspectRatioFactor * Mathf.Tan(0.5f * Cameras[cameraId].fieldOfView * Mathf.Deg2Rad);
            scale.x = scale.y * ArucoCamera.ImageRatios[cameraId];
          }

          // Place and scale the background
          CameraBackgrounds[cameraId].transform.localPosition = new Vector3(position.x, position.y, cameraBackgroundDistance);
          CameraBackgrounds[cameraId].transform.localScale = new Vector3(scale.x, scale.y, 1);
          CameraBackgrounds[cameraId].SetActive(true);
        }
      }

      /// <summary>
      /// Deactivates the <see cref="CameraBackgrounds"/>.
      /// </summary>
      public override void StopController()
      {
        base.StopController();

        foreach (var cameraBackground in CameraBackgrounds)
        {
          if (cameraBackground != null)
          {
            cameraBackground.SetActive(false);
          }
        }
      }

      protected override void Configure()
      {
        if (Cameras.Length != ArucoCamera.CameraNumber)
        {
          throw new Exception("The number of cameras in the displayer must be equal to the number of cameras in ArucoCamera");
        }

        // Configure the background
        if (CameraBackgroundsPrefab == null)
        {
          CameraBackgroundsPrefab = Resources.Load("ArucoCameraDisplayerBackground") as GameObject;
        }

        CameraBackgrounds = new GameObject[ArucoCamera.CameraNumber];
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          if (CameraBackgrounds[cameraId] == null)
          {
            CameraBackgrounds[cameraId] = Instantiate(CameraBackgroundsPrefab, Cameras[cameraId].transform);
            CameraBackgrounds[cameraId].transform.localRotation = Quaternion.identity;
            CameraBackgrounds[cameraId].GetComponent<Renderer>().material.mainTexture = ArucoCamera.ImageTextures[cameraId];
            CameraBackgrounds[cameraId].SetActive(false);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}
