using ArucoUnity.Utilities;

namespace ArucoUnity.Cameras
{
  /// <summary>
  /// Configurable controller using a <see cref="IArucoCamera"/>.
  /// </summary>
  public interface IArucoCameraController : IConfigurableController
  {
    // Properties

    /// <summary>
    /// Gets or sets the camera to use.
    /// </summary>
    IArucoCamera ArucoCamera { get; set; }
  }
}