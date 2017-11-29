using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Undistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of pinhole <see cref="ArucoCamera"/>.
    /// </summary>
    public class ArucoCameraPinholeUndistortion : ArucoCameraGenericPinholeUndistortion
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public ArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

      // ArucoCameraUndistortion methods

      protected override void InitializeRectification()
      {
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          var c = CameraParameters;
          var cm = CameraParameters.CameraMatrices[cameraId];
          var d = CameraParameters.DistCoeffs[cameraId];
          var i = ArucoCamera.Images[cameraId];
          var si = ArucoCamera.Images[cameraId].Size;

          RectifiedCameraMatrices[cameraId] = Cv.GetOptimalNewCameraMatrix(CameraParameters.CameraMatrices[cameraId],
            CameraParameters.DistCoeffs[cameraId], ArucoCamera.Images[cameraId].Size, RectificationScalingFactor);
          RectificationMatrices[cameraId] = noRectificationMatrix;
        }
      }
    }
  }

  /// \} aruco_unity_package
}