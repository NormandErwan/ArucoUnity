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
    /// Generic configurable controller that make use of one <see cref="Cameras.ArucoCamera"/>.
    /// </summary>
    public abstract class ArucoCameraController : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("Start automatically when the configuration is done. Call alternatively StartDetector().")]
      private bool autoStart = true;

      // Events

      /// <summary>
      /// Called when the controller is configured.
      /// </summary>
      public event Action Configured = delegate { };

      /// <summary>
      /// Called when the controller is started.
      /// </summary>
      public event Action Started = delegate { };

      /// <summary>
      /// Called when the controller is stopped.
      /// </summary>
      public event Action Stopped = delegate { };

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use. Set calls <see cref="SetArucoCamera(ArucoCamera)"/>.
      /// </summary>
      public ArucoCamera ArucoCamera { get { return arucoCamera; } set { SetArucoCamera(value); } }

      /// <summary>
      /// Gets or sets if starting automatically when the <see cref="Configure"/> is called. Start manually by calling <see cref="StartController"/>.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// Gets if the controller is configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      /// <summary>
      /// Gets if the controller is started.
      /// </summary>
      public bool IsStarted { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the properties.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;

        ArucoCamera = arucoCamera;
      }

      /// <summary>
      /// Calls <see cref="StopController"/> if it has been started and unsubscribes from <see cref="ArucoCamera"/> events.
      /// </summary>
      protected virtual void OnDestroy()
      {
        if (IsConfigured)
        {
          ArucoCamera.Stopped -= ArucoCamera_Stopped;
          arucoCamera.Started -= ArucoCamera_Started;

          if (IsStarted)
          {
            StopController();
          }
        }
      }

      // Methods

      /// <summary>
      /// Calls the <see cref="Started"/> event and subscribes to <see cref="ArucoCamera.ImagesUpdated"/>. The controller must be configured and
      /// stopped.
      /// </summary>
      public virtual void StartController()
      {
        if (!IsConfigured || IsStarted)
        {
          throw new Exception("Configure or stop the controller before start it.");
        }

        IsStarted = true;
        Started();

        ArucoCamera.ImagesUpdated += ArucoCamera_ImagesUpdated;
      }

      /// <summary>
      /// Calls the <see cref="Stopped"/> event and unsubscribes from <see cref="ArucoCamera.ImagesUpdated"/>. The controller must be configured and
      /// started.
      /// </summary>
      public virtual void StopController()
      {
        if (!IsConfigured || !IsStarted)
        {
          throw new Exception("Set ArucoCamera and start the controller before stop it.");
        }

        ArucoCamera.ImagesUpdated -= ArucoCamera_ImagesUpdated;

        IsStarted = false;
        Stopped();
      }

      /// <summary>
      /// Configures the controller when <see cref="ArucoCamera.IsStarted"/> is set to true.
      /// </summary>
      protected abstract void Configure();

      /// <summary>
      /// Subscribes to the <see cref="ArucoCamera.Started"/> and <see cref="ArucoCamera.Stopped"/> events, and unsubscribes from the previous
      /// ArucoCamera events. If <see cref="ArucoCamera.IsStarted"/> is true, also calls <see cref="ArucoCamera_Started"/>. The controller must be
      /// stopped.
      /// </summary>
      /// <param name="arucoCamera">The new ArucoCamera to subscribes on.</param>
      protected virtual void SetArucoCamera(ArucoCamera arucoCamera)
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
      /// Called when <see cref="ArucoCamera.ImagesUpdated"/> is invoked.
      /// </summary>
      protected virtual void ArucoCamera_ImagesUpdated()
      {
      }

      /// <summary>
      /// Calls the <see cref="StopController"/> action if it has been started.
      /// </summary>
      private void ArucoCamera_Stopped()
      {
        if (IsConfigured && IsStarted)
        {
          StopController();
        }
      }

      /// <summary>
      /// Calls <see cref="Configure"/>, the <see cref="Configured"/> action. If <see cref="AutoStart"/> is true, also calls <see cref="StartController"/>.
      /// </summary>
      private void ArucoCamera_Started()
      {
        Configure();
        IsConfigured = true;
        Configured();

        if (AutoStart)
        {
          StartController();
        }
      }
    }
  }

  /// \} aruco_unity_package
}