using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraCalibrations.Flags;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraCalibrations.Pinhole
  {
    public class ArucoCameraPinholeCalibration : ArucoCameraGenericPinholeCalibration
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The flags for the cameras calibration.")]
      private PinholeCameraCalibrationFlags calibrationFlags;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // ArucoCameraGenericPinholeCalibration properties

      public override PinholeCameraCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public ArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

      // ArucoCameraCalibration methods

      protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          Std.VectorVec3d rvecs, tvecs;
          cameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(objectPoints[cameraId], imagePoints[cameraId],
            calibrationImageSizes[cameraId], cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs,
            CalibrationFlags.CalibrationFlags);

          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }
      }
    }
  }

  /// \} aruco_unity_package
}