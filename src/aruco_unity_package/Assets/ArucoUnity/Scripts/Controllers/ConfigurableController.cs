using System;
using System.Collections.Generic;
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
    public abstract class ConfigurableController : MonoBehaviour, IConfigurableController
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Start automatically when the configuration is done. Call alternatively StartController().")]
      private bool autoStart = true;

      // IConfigurableController events

      public event Action Ready = delegate { };
      public event Action Configured = delegate { };
      public event Action Started = delegate { };
      public event Action Stopped = delegate { };

      // IConfigurableController properties

      public bool AutoStart { get { return autoStart; } set { SetAutoStart(value); } }
      public bool IsReady { get; private set; }
      public bool IsConfigured { get; private set; }
      public bool IsStarted { get; private set; }
      public List<IConfigurableController> ControllerDependencies { get; protected set; }

      // Variables

      private Dictionary<IConfigurableController, Action> dependencyStartedCallbacks;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the properties.
      /// </summary>
      protected virtual void Awake()
      {
        if (ControllerDependencies == null)
        {
          ControllerDependencies = new List<IConfigurableController>();
        }
        dependencyStartedCallbacks = new Dictionary<IConfigurableController, Action>();

        IsConfigured = false;
        IsStarted = false;
      }

      /// <summary>
      /// Calls <see cref="SetAutoStart(bool)"/>.
      /// </summary>
      protected virtual void Start()
      {
        SetAutoStart(AutoStart);
      }

      /// <summary>
      /// Calls <see cref="StopController"/> if it has been configured and started.
      /// </summary>
      protected virtual void OnDestroy()
      {
        if (IsStarted)
        {
          StopController();
        }
      }

      // IArucoCameraController methods

      /// <summary>
      /// Configures the controller and calls <see cref="OnConfigured"/>. Properties must be set and the controller must be stopped.
      /// </summary>
      public virtual void Configure()
      {
        if (IsStarted)
        {
          throw new Exception("Stop the controller before configure it.");
        }

        IsConfigured = false;
        IsReady = false;
      }

      /// <summary>
      /// Starts the controller and calls <see cref="OnStarted"/>. The controller must be configured and stopped.
      /// </summary>
      public virtual void StartController()
      {
        if (!IsConfigured || !IsReady || IsStarted)
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
      /// Calls the <see cref="Configured"/> event, and configure.
      /// </summary>
      protected virtual void OnConfigured()
      {
        // Remove the previous controller dependency callbacks
        foreach (var controller in dependencyStartedCallbacks)
        {
          if (controller.Key != null)
          {
            controller.Key.Started -= controller.Value;
            controller.Key.Stopped -= Controller_Stopped;
          }
        }
        dependencyStartedCallbacks.Clear();

        // Add a callback for each controller not started
        foreach (var controller in ControllerDependencies)
        {
          if (!controller.IsStarted)
          {
            // When the controller has started, remove the callback and start this current controller if not waiting for any other controller to start
            Action controller_Started = () =>
            {
              dependencyStartedCallbacks.Remove(controller);
              if (dependencyStartedCallbacks.Count == 0)
              {
                OnReady();
              }
            };

            dependencyStartedCallbacks.Add(controller, controller_Started);
            controller.Started += controller_Started;
            controller.Stopped += Controller_Stopped;
          }
        }

        // Configured
        IsConfigured = true;
        Configured();

        // Ready if no dependencies or if they are all started
        if (dependencyStartedCallbacks.Count == 0)
        {
          OnReady();
        }
      }

      /// <summary>
      /// Calls the <see cref="Ready"/> event, and calls <see cref="StartController"/> if <see cref="AutoStart"/> is true.
      /// </summary>
      protected virtual void OnReady()
      {
        IsReady = true;
        Ready();

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

      // Methods

      /// <summary>
      /// Calls <see cref="Configure"/> if <see cref="AutoStart"/> is true.
      /// </summary>
      private void SetAutoStart(bool value)
      {
        autoStart = value;
        if (AutoStart)
        {
          Configure();
        }
      }

      /// <summary>
      /// Calls <see cref="StopController"/> if it has been configured and started when a dependecy controller has stopped.
      /// </summary>
      private void Controller_Stopped()
      {
        if (IsStarted)
        {
          StopController();
        }
      }
    }
  }

  /// \} aruco_unity_package
}