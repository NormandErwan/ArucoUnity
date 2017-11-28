using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of fisheye and omnidir <see cref="ArucoCamera"/>.
    /// </summary>
    public class ArucoCameraOmnidirUndistortion : ArucoCameraGenericOmnidirUndistortion
    {
      // Constants

      protected const float minPerspectiveFov = 1f;
      protected const float maxPerspectiveFov = 179f;

      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The desired field of view for the Unity cameras shooting the undistorted and rectified images.")]
      [Range(1f, 179f)]
      private float[] perspectiveDesiredFieldOfViews;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public ArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

      /// <summary>
      /// Gets or sets the desired field of view for the Unity cameras shooting the undistorted and rectified images.
      /// </summary>
      public float[] PerspectiveDesiredFieldOfViews { get { return perspectiveDesiredFieldOfViews; } set { perspectiveDesiredFieldOfViews = value; } }

      // ArucoCameraController methods

      public override void Configure()
      {
        if (PerspectiveDesiredFieldOfViews.Length != ArucoCamera.CameraNumber)
        {
          throw new Exception("The number of cameras for the perspective desired field of view must be equal to the number of cameras in" +
            "ArucoCamera");
        }

        base.Configure();
      }

      // ArucoCameraUndistortion methods

      protected override void InitializeRectification()
      {
        if (RectificationType == RectificationTypes.Perspective)
        {
          for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
          {
            float imageWidth = CameraParameters.ImageWidths[cameraId];
            float imageHeight = CameraParameters.ImageHeights[cameraId];

            float cameraF = imageHeight / (2f * Mathf.Tan(0.5f * PerspectiveDesiredFieldOfViews[cameraId] * Mathf.Deg2Rad));
            RectifiedCameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { cameraF, 0, imageWidth / 2, 0, cameraF, imageHeight / 2, 0, 0, 1 }).Clone();
            RectificationMatrices[cameraId] = noRectificationMatrix;
          }
        }
        else
        {
          base.InitializeRectification(); // Initalizes RectifiedCameraMatrices with default values for other types
        }
      }
    }
  }

  /// \} aruco_unity_package
}