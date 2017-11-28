using ArucoUnity.Cameras;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Configurable controller using a <see cref="IArucoCamera"/>.
    /// </summary>
    public interface IArucoCameraController : IConfigurableController
    {
      // Properties

      /// <summary>
      /// Gets the camera system to use.
      /// </summary>
      IArucoCamera ArucoCamera { get; }
    }
  }

  /// \} aruco_unity_package
}