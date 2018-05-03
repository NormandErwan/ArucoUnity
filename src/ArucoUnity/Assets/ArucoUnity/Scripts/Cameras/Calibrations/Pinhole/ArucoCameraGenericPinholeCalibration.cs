using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Calibrations.Pinhole
  {
    /// <summary>
    /// Calibrates a <see cref="ArucoCamera"/> using the pinhole camera model with a <see cref="Objects.ArucoBoard"/> and
    /// saves the calibrated camera parameters in a file managed by <see cref="CameraParametersController"/>.
    /// </summary>
    public abstract class ArucoCameraGenericPinholeCalibration<T> : ArucoCameraCalibrationGeneric<T, PinholeCalibrationFlags>
      where T : ArucoCamera
    {
      protected override void InitializeCameraParameters()
      {
        base.InitializeCameraParameters();

        if (calibrationFlags.FixAspectRatio)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            CameraParametersController.CameraParameters.CameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F,
              new double[9] { calibrationFlags.FixAspectRatioValue, 0.0, 0.0,
                0.0, 1.0, 0.0,
                0.0, 0.0, 1.0
              });
          }
          CameraParametersController.CameraParameters.FixAspectRatioValue = calibrationFlags.FixAspectRatioValue;
        }
      }
    }
  }

  /// \} aruco_unity_package
}