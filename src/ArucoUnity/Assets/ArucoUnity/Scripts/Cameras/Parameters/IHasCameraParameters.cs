namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Parameters
  {
    /// <summary>
    /// Contains a <see cref="Parameters.CameraParameters"/>.
    /// </summary>
    public interface IHasCameraParameters
    {
      // Properties

      /// <summary>
      /// Gets or sets the <see cref="IArucoCamera"/> camera parameters.
      /// </summary>
      CameraParameters CameraParameters { get; set; }
    }
  }

  /// \} aruco_unity_package
}