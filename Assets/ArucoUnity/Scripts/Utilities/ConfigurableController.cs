using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity.Utilities
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

    public event Action<IConfigurableController> Ready = delegate { };
    public event Action<IConfigurableController> Configured = delegate { };
    public event Action<IConfigurableController> Started = delegate { };
    public event Action<IConfigurableController> Stopped = delegate { };

    // IConfigurableController properties

    public bool AutoStart { get { return autoStart; } set { SetAutoStart(value); } }
    public bool IsReady { get; private set; }
    public bool IsConfigured { get; private set; }
    public bool IsStarted { get; private set; }

    // Variables

    private HashSet<IConfigurableController> dependencies = new HashSet<IConfigurableController>();
    private HashSet<IConfigurableController> stoppedDependencies = new HashSet<IConfigurableController>();

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

    public void AddDependency(IConfigurableController dependency)
    {
      if (IsStarted)
      {
        throw new Exception("Stop the controller before updating the dependencies.");
      }

      dependencies.Add(dependency);
      if (!dependency.IsStarted)
      {
        stoppedDependencies.Add(dependency);
      }

      dependency.Started += Dependency_Started;
      dependency.Stopped += Dependency_Stopped;
    }

    public void RemoveDependency(IConfigurableController dependency)
    {
      if (IsStarted)
      {
        throw new Exception("Stop the controller before updating the dependencies.");
      }

      dependencies.Remove(dependency);
      stoppedDependencies.Remove(dependency);

      dependency.Started -= Dependency_Started;
      dependency.Stopped -= Dependency_Stopped;
    }

    public List<IConfigurableController> GetDependencies()
    {
      return new List<IConfigurableController>(dependencies);
    }

    public void ConfigureController()
    {
      if (IsStarted)
      {
        throw new Exception("Stop the controller before configure it.");
      }

      IsConfigured = false;
      IsReady = false;

      Configuring();
      OnConfigured();
    }

    public void StartController()
    {
      if (!IsConfigured || !IsReady || IsStarted)
      {
        throw new Exception("Configure and stop the controller before start it.");
      }

      Starting();
      OnStarted();
    }

    public void StopController()
    {
      if (!IsConfigured || !IsStarted)
      {
        throw new Exception("Configure and start the controller before stop it.");
      }

      Stopping();
      OnStopped();
    }

    // Methods

    protected virtual void Configuring()
    {
    }

    /// <summary>
    /// Sets <see cref="IsConfigured"/> to true, calls <see cref="Configured"/> and if all dependencies started calls
    /// <see cref="OnReady"/>.
    /// </summary>
    protected virtual void OnConfigured()
    {
      IsConfigured = true;
      Configured(this);

      if (stoppedDependencies.Count == 0)
      {
        OnReady();
      }
    }

    protected virtual void Starting()
    {
    }

    /// <summary>
    /// Sets <see cref="IsStarted"/> to true and calls <see cref="Started"/>.
    /// </summary>
    protected virtual void OnStarted()
    {
      IsStarted = true;
      Started(this);
    }

    protected virtual void Stopping()
    {
    }

    /// <summary>
    /// Sets <see cref="IsStarted"/> to false and calls <see cref="Stopped"/>.
    /// </summary>
    protected virtual void OnStopped()
    {
      IsStarted = false;
      Stopped(this);
    }

    /// <summary>
    /// Calls <see cref="ConfigureController"/> if <see cref="AutoStart"/> is true.
    /// </summary>
    private void SetAutoStart(bool value)
    {
      autoStart = value;
      if (AutoStart)
      {
        ConfigureController();
      }
    }

    /// <summary>
    /// Calls <see cref="OnReady"/> if the controller is configured and all the dependencies are started.
    /// </summary>
    private void Dependency_Started(IConfigurableController dependency)
    {
      stoppedDependencies.Remove(dependency);
      if (IsConfigured && stoppedDependencies.Count == 0)
      {
        OnReady();
      }
    }

    /// <summary>
    /// Calls <see cref="StopController"/> if the controller is started.
    /// </summary>
    private void Dependency_Stopped(IConfigurableController dependency)
    {
      stoppedDependencies.Add(dependency);
      if (IsStarted)
      {
        StopController();
      }
    }

    /// <summary>
    /// Calls the <see cref="Ready"/> event, and calls <see cref="StartController"/> if <see cref="AutoStart"/> is true.
    /// </summary>
    private void OnReady()
    {
      IsReady = true;
      Ready(this);

      if (AutoStart)
      {
        StartController();
      }
    }
  }
}