using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
    /// <summary>
    /// Configurable and startable controller.
    /// </summary>
    /// <remarks>
    /// It can have other configurable controllers as dependencies. They must have started before starting this
    /// controller. They stop this controller when one of them stops.
    /// </remarks>
    public abstract class Controller : MonoBehaviour, IController
    {
        [SerializeField]
        [Tooltip("Start automatically when the configuration is done. Call alternatively StartController().")]
        private bool autoStart = true;

        /// <summary>
        /// Called when the controller is configured.
        /// </summary>
        public event EventHandler Configured = delegate { };

        /// <summary>
        /// Called when the controller is configured and ready to be started, when all its dependencies started.
        /// </summary>
        public event EventHandler Ready = delegate { };

        /// <summary>
        /// Called when the controller is started.
        /// </summary>
        public event EventHandler Started = delegate { };

        /// <summary>
        /// Called when the controller is stopped.
        /// </summary>
        public event EventHandler Stopped = delegate { };

        /// <summary>
        /// Gets or sets if configuring and starting automatically when when all dependencies started. Manually
        /// configure and start by calling <see cref="Configure"/> and <see cref="StartController"/>.
        /// </summary>
        public bool AutoStart { get { return autoStart; } set { SetAutoStart(value); } }

        /// <summary>
        /// Gets if the controller is configured.
        /// </summary>
        public bool IsConfigured { get; private set; }

        /// <summary>
        /// Gets if the controller is ready to be started when all dependencies have started.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Gets if the controller is started.
        /// </summary>
        public bool IsStarted { get; private set; }

        private HashSet<IController> dependencies = new HashSet<IController>();
        private HashSet<IController> stoppedDependencies = new HashSet<IController>();

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

        /// <summary>
        /// Add a new dependency. The controller must be stopped.
        /// </summary>
        /// <param name="dependency">The dependency to add.</param>
        public void AddDependency(IController dependency)
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

            dependency.Started += DependencyStarted;
            dependency.Stopped += DependencyStopped;
        }

        /// <summary>
        /// Remove a dependency. The controller must be stopped.
        /// </summary>
        /// <param name="dependency">The dependency to remove.</param>
        public void RemoveDependency(IController dependency)
        {
            if (IsStarted)
            {
                throw new Exception("Stop the controller before updating the dependencies.");
            }

            dependencies.Remove(dependency);
            stoppedDependencies.Remove(dependency);

            dependency.Started -= DependencyStarted;
            dependency.Stopped -= DependencyStopped;
        }

        /// <summary>
        /// Gets the list of the dependencies.
        /// </summary>
        /// <returns>The list of the dependencies of this instance.</returns>
        public List<IController> GetDependencies()
        {
            return new List<IController>(dependencies);
        }

        /// <summary>
        /// Configures the controller and calls the <see cref="Configured"/> event. Properties must be set and the
        /// controller must be stopped.
        /// </summary>
        public void Configure()
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

        /// <summary>
        /// Starts the controller and calls the <see cref="Started"/> event. The controller must be configured, ready and
        /// stopped.
        /// </summary>
        public void StartController()
        {
            if (!IsConfigured || !IsReady || IsStarted)
            {
                throw new Exception("Configure and stop the controller before start it.");
            }

            Starting();
            OnStarted();
        }

        /// <summary>
        /// Stops the controller and calls the <see cref="Stopped"/> event. The controller must be configured and started.
        /// </summary>
        public void StopController()
        {
            if (!IsConfigured || !IsStarted)
            {
                throw new Exception("Configure and start the controller before stop it.");
            }

            Stopping();
            OnStopped();
        }

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
            Configured(this, EventArgs.Empty);

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
            Started(this, EventArgs.Empty);
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
            Stopped(this, EventArgs.Empty);
        }

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
        /// Calls <see cref="OnReady"/> if the controller is configured and all the dependencies are started.
        /// </summary>
        private void DependencyStarted(object sender, EventArgs e)
        {
            var dependency = (IController)sender;
            stoppedDependencies.Remove(dependency);

            if (IsConfigured && stoppedDependencies.Count == 0)
            {
                OnReady();
            }
        }

        /// <summary>
        /// Calls <see cref="StopController"/> if the controller is started.
        /// </summary>
        private void DependencyStopped(object sender, EventArgs e)
        {
            var dependency = (IController)sender;
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
            Ready(this, EventArgs.Empty);

            if (AutoStart)
            {
                StartController();
            }
        }
    }
}