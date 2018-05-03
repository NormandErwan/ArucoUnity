using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Calibrations.Omnidir
  {
    public class ArucoCameraOmnidirCalibration : ArucoCameraGenericOmnidirCalibration
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The flags for the camera calibration.")]
      private OmnidirCalibrationFlags calibrationFlags;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // ArucoCameraGenericOmnidirCalibration properties

      public override OmnidirCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

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
          cameraParameters.ReprojectionErrors[cameraId] = Cv.Omnidir.Calibrate(objectPoints[cameraId], imagePoints[cameraId],
            calibrationImageSizes[cameraId], cameraParameters.CameraMatrices[cameraId], cameraParameters.OmnidirXis[cameraId],
            cameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs, CalibrationFlags.Flags);

          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }
      }
    }
  }

  /// \} aruco_unity_package
}