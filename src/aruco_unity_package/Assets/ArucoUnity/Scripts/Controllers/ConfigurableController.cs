using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Configurable and startable controller.
    /// </summary>
    public abstract class ConfigurableController : IConfigurableController
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Start automatically when the configuration is done. Call alternatively StartController().")]
      private bool autoStart = true;

      // IConfigurableController events

      public event Action Configured = delegate { };
      public event Action Started = delegate { };
      public event Action Stopped = delegate { };

      // IConfigurableController properties

      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }
      public bool IsConfigured { get; protected set; }
      public bool IsStarted { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the properties.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;
      }

      /// <summary>
      /// Calls <see cref="StopController"/> if it has been configured and started.
      /// </summary>
      protected virtual void OnDestroy()
      {
        if (IsConfigured && IsStarted)
        {
          StopController();
        }
      }

      // IArucoCameraController methods

      /// <summary>
      /// Configures the controller and calls <see cref="OnConfigured"/>. The controller must be stopped.
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
      protected virtual void OnConfigured()
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
      protected virtual void OnStarted()
      {
        IsStarted = true;
        Started();
      }

      /// <summary>
      /// Calls the <see cref="Stopped"/> event.
      /// </summary>
      protected virtual void OnStopped()
      {
        IsStarted = false;
        Stopped();
      }
    }
  }

  /// \} aruco_unity_package
}