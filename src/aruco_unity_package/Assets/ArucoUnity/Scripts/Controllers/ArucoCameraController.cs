using ArucoUnity.Cameras;
using System;
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
      public T ArucoCamera { get { return arucoCamera; } set { SetArucoCamera(value); } }

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();
        ArucoCamera = arucoCamera;
      }

      /// <summary>
      /// Unsubscribes from the <see cref="ArucoCamera"/> events.
      /// </summary>
      protected override void OnDestroy()
      {
        base.OnDestroy();

        if (IsConfigured)
        {
          ArucoCamera.Stopped -= ArucoCamera_Stopped;
          arucoCamera.Started -= ArucoCamera_Started;
        }
      }

      // Methods

      /// <summary>
      /// Subscribes to the <see cref="ArucoCamera.Started"/> and <see cref="ArucoCamera.Stopped"/> events, and unsubscribes from the previous
      /// ArucoCamera events. If <see cref="ArucoCamera.IsStarted"/> is true, also calls <see cref="ArucoCamera_Started"/>. The controller must be
      /// stopped.
      /// </summary>
      /// <param name="arucoCamera">The new ArucoCamera to subscribes on.</param>
      protected virtual void SetArucoCamera(T arucoCamera)
      {
        if (IsStarted)
        {
          throw new Exception("Stop the controller before setting the ArucoCamera.");
        }

        // Reset configuration
        IsConfigured = false;

        // Unsubscribe from the previous ArucoCamera
        if (ArucoCamera != null)
        {
          ArucoCamera.Started -= ArucoCamera_Started;
          ArucoCamera.Stopped -= ArucoCamera_Stopped;
        }

        // Subscribe to the new ArucoCamera
        this.arucoCamera = arucoCamera;
        if (arucoCamera != null)
        {
          if (ArucoCamera.IsStarted)
          {
            ArucoCamera_Started();
          }
          ArucoCamera.Stopped += ArucoCamera_Stopped;
          arucoCamera.Started += ArucoCamera_Started;
        }
      }

      /// <summary>
      /// Calls <see cref="Configure"/> and <see cref="StartController"/> if <see cref="AutoStart"/> is true.
      /// </summary>
      private void ArucoCamera_Started()
      {
        if (AutoStart)
        {
          Configure();
        }
      }

      /// <summary>
      /// Calls the <see cref="StopController"/> action if the controller has been cofnigured and started.
      /// </summary>
      private void ArucoCamera_Stopped()
      {
        if (IsConfigured && IsStarted)
        {
          StopController();
        }
      }
    }
  }

  /// \} aruco_unity_package
}