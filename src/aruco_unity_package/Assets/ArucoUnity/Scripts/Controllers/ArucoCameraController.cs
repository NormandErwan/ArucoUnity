using ArucoUnity.Cameras;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Generic configurable controller using a <see cref="Cameras.ArucoCamera"/> that auto-start when the ArucoCamera is started.
    /// </summary>
    public abstract class ArucoCameraController<T> : ConfigurableController, IArucoCameraController where T : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private T arucoCamera;

      // IArucoCameraController properties

      IArucoCamera IArucoCameraController.ArucoCamera { get { return ArucoCamera; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use. Setting calls <see cref="SetArucoCamera(ArucoCamera)"/>.
      /// </summary>
      public T ArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

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