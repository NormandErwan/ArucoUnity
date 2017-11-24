using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the processes of undistortion and rectification of <see cref="Cameras.IArucoCamera.Images"/>.
    /// </summary>
    public interface IArucoCameraUndistortion : IArucoCameraController
    {
      // Properties

      /// <summary>
      /// Gets or sets the camera parameters to use.
      /// </summary>
      CameraParametersController CameraParametersController { get; set; }

      /// <summary>
      /// Gets the new camera matrices of the undistorted and rectified images of each camera.
      /// </summary>
      Cv.Mat[] RectifiedCameraMatrices { get; }

      /// <summary>
      /// Gets the rectification rotation matrices of each camera to make both camera image planes the same plane, in case of a stereo camera.
      /// </summary>
      Cv.Mat[] RectificationMatrices { get; }

      /// <summary>
      /// Gets the distorsion coefficients of the undistorted and rectified images of each camera.
      /// </summary>
      Cv.Mat UndistortedDistCoeffs { get; }

      /// <summary>
      /// Gets the undistortion and rectification images transformation map of each camera (two maps per camera).
      /// </summary>
      Cv.Mat[][] UndistortionRectificationMaps { get; }
    }
  }

  /// \} aruco_unity_package
}