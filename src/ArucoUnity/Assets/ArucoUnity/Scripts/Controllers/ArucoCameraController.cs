using ArucoUnity.Cameras;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Generic configurable controller using a <see cref="ArucoCamera"/> as starting dependency.
    /// </summary>
    public abstract class ArucoCameraController : ConfigurableController, IArucoCameraController
    {
      // IArucoCameraController properties

      public IArucoCamera ArucoCamera { get; set; }

      // MonoBehaviour methods

      /// <summary>
      /// Adds <see cref="ArucoCamera"/> in <see cref="ControllerDependencies"/> and calls <see cref="OnConfigured"/>.
      /// </summary>
      public override void Configure()
      {
        base.Configure();
        if (ArucoCamera != null)
        {
          AddDependency(ArucoCamera);
        }
      }
    }
  }

  /// \} aruco_unity_package
}