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

      public abstract IArucoCamera ArucoCamera { get; }

      // MonoBehaviour methods

      /// <summary>
      /// Adds <see cref="ArucoCamera"/> in <see cref="ControllerDependencies"/> and calls <see cref="OnConfigured"/>.
      /// </summary>
      public override void Configure()
      {
        base.Configure();
        ControllerDependencies.Add(ArucoCamera);
        OnConfigured();
      }
    }
  }

  /// \} aruco_unity_package
}