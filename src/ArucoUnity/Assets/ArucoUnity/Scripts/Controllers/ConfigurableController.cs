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

      // Variables

      private List<IConfigurableController> dependencies = new List<IConfigurableController>();
      private int dependencyWaitStarts;

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

      public void AddDependency(IConfigurableController controller)
      {
        dependencies.Add(controller);
      }

      public void RemoveDependency(IConfigurableController controller)
      {
        dependencies.Remove(controller);
      }

      public List<IConfigurableController> GetDependencies()
      {
        return new List<IConfigurableController>(dependencies);
      }

      public virtual void Configure()
      {
        if (IsStarted)
        {
          throw new Exception("Stop the controller before configure it.");
        }

        IsConfigured = false;
        IsReady = false;
      }

      public virtual void StartController()
      {
        if (!IsConfigured || !IsReady || IsStarted)
        {
          throw new Exception("Configure and stop the controller before start it.");
        }
      }

      public virtual void StopController()
      {
        if (!IsConfigured || !IsStarted)
        {
          throw new Exception("Configure and start the controller before stop it.");
        }
      }

      // Properties

      /// <summary>
      /// Calls the <see cref="Configured"/> event, and configure.
      /// </summary>
      protected virtual void OnConfigured()
      {
        dependencyWaitStarts = dependencies.Count;

        // Add callbacks to dependencies
        foreach (var controller in dependencies)
        {
          if (!controller.IsStarted)
          {
            controller.Started += Controller_Started;
          }
          controller.Stopped += Controller_Stopped;
        }

        // Configured
        IsConfigured = true;
        Configured();

        // Ready if no dependencies or if they are all started
        if (dependencyWaitStarts == 0)
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
      /// Calls <see cref="OnReady"/> if all the <see cref="dependencies"/> have started.
      /// </summary>
      private void Controller_Started()
      {
        dependencyWaitStarts--;
        if (dependencyWaitStarts == 0)
        {
          OnReady();
        }
      }

      /// <summary>
      /// Calls <see cref="StopController"/> if it has been configured and started and the dependency has stopped.
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