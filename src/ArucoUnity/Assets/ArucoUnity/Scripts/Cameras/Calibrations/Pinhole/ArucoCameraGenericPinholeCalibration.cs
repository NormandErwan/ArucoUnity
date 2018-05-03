using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Calibrations.Pinhole
  {
    /// <summary>
    /// Calibrates a <see cref="Cameras.IArucoCamera"/> using perspective camera model with a <see cref="Objects.ArucoBoard"/> and saves the
    /// calibrated camera parameters in a file managed by <see cref="CameraParametersController"/>.
    /// </summary>
    public abstract class ArucoCameraGenericPinholeCalibration : ArucoCameraCalibration
    {
      // Properties

      /// <summary>
      /// Gets or sets the flags for the cameras calibration.
      /// </summary>
      public abstract PinholeCalibrationFlags CalibrationFlags { get; set; }

      // ArucoCameraCalibration methods

      protected override void InitializeCameraParameters(CalibrationFlags calibrationFlags)
      {
        base.InitializeCameraParameters(CalibrationFlags);

        if (CalibrationFlags.FixAspectRatio)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            CameraParametersController.CameraParameters.CameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] {
              CalibrationFlags.FixAspectRatioValue, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
          }
          CameraParametersController.CameraParameters.FixAspectRatioValue = CalibrationFlags.FixAspectRatioValue;
        }
      }
    }
  }

  /// \} aruco_unity_package
}