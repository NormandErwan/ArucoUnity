using ArucoUnity.Cameras.Parameters;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Contains a <see cref="Cameras.Parameters.CameraParameters"/>.
    /// </summary>
    public interface IHasCameraParameters
    {
      // Properties

      /// <summary>
      /// Gets or sets the <see cref="Cameras.IArucoCamera"/> camera parameters.
      /// </summary>
      CameraParameters CameraParameters { get; set; }
    }
  }

  /// \} aruco_unity_package
}