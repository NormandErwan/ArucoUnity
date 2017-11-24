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
    /// Generic configurable controller using a <see cref="Cameras.ArucoCamera"/>.
    /// </summary>
    public abstract class ArucoCameraController<T> : MonoBehaviour, IArucoCameraController where T : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private T arucoCamera;

      [SerializeField]
      [Tooltip("Start automatically when the configuration is done. Call alternatively StartDetector().")]
      private bool autoStart = true;

      // IArucoCameraController events

      public event Action Configured = delegate { };
      public event Action Started = delegate { };
      public event Action Stopped = delegate { };

      // IArucoCameraController properties

      IArucoCamera IArucoCameraController.ArucoCamera { get { return ArucoCamera; } }
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }
      public bool IsConfigured { get; protected set; }
      public bool IsStarted { get; protected set; }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use. Setting calls <see cref="SetArucoCamera(ArucoCamera)"/>.
      /// </summary>
      public T ArucoCamera { get { return arucoCamera; } set { SetArucoCamera(value); } }

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

      // IArucoCameraController methods

      /// <summary>
      /// Configures the controller and calls <see cref="OnConfigured"/>. It must be stopped.
      /// </summary>
      public virtual void Configure()
      {
        if (IsStarted)
        {
          throw new Exception("Stop the controller before configure it.");
        }

        IsConfigured = false;
      }

      /// <summary>
      /// Starts the controller and calls <see cref="OnStarted"/>. The controller must be configured and stopped.
      /// </summary>
      public virtual void StartController()
      {
        if (!IsConfigured || IsStarted)
        {
          throw new Exception("Configure and stop the controller before start it.");
        }
      }

      /// <summary>
      /// Stops the controller and calls <see cref="OnStopped"/>. The controller must be configured and started.
      /// </summary>
      public virtual void StopController()
      {
        if (!IsConfigured || !IsStarted)
        {
          throw new Exception("Configure and start the controller before stop it.");
        }
      }

      /// <summary>
      /// Calls the <see cref="Configured"/> event, and calls <see cref="StartController"/> if <see cref="AutoStart"/> is true.
      /// </summary>
      protected void OnConfigured()
      {
        // Update state
        IsConfigured = true;
        Configured();

        // AutoStart
        if (AutoStart)
        {
          StartController();
        }
      }

      /// <summary>
      /// Calls the <see cref="Started"/> event.
      /// </summary>
      protected void OnStarted()
      {
        IsStarted = true;
        Started();
      }

      /// <summary>
      /// Calls the <see cref="Stopped"/> event.
      /// </summary>
      protected void OnStopped()
      {
        IsStarted = false;
        Stopped();
      }

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