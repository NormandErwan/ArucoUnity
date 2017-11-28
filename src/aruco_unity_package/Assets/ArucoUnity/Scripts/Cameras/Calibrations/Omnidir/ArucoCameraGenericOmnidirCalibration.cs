using ArucoUnity.Cameras.Calibrations.Flags;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Calibrations.Omnidir
  {
    /// <summary>
    /// Calibrates a <see cref="Cameras.IArucoCamera"/> using the omnidirectional camera model with a <see cref="Objects.ArucoBoard"/> and saves the
    /// calibrated camera parameters in a file managed by <see cref="CameraParametersController"/>.
    /// </summary>
    public abstract class ArucoCameraGenericOmnidirCalibration : ArucoCameraCalibration
    {
      // Properties

      /// <summary>
      /// Gets or sets the flags for the camera calibration.
      /// </summary>
      public abstract OmnidirCameraCalibrationFlags CalibrationFlags { get; set; }
    }
  }

  /// \} aruco_unity_package
}