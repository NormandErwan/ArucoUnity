using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity.Utilities
{
    /// <summary>
    /// Configurable and startable controller.
    /// </summary>
    public abstract class Controller : MonoBehaviour, IController
    {
        [SerializeField]
        [Tooltip("Start automatically when the configuration is done. Call alternatively StartController().")]
        private bool autoStart = true;

        public event EventHandler Ready = delegate { };
        public event EventHandler Configured = delegate { };
        public event EventHandler Started = delegate { };
        public event EventHandler Stopped = delegate { };

        public bool AutoStart { get { return autoStart; } set { SetAutoStart(value); } }
        public bool IsReady { get; private set; }
        public bool IsConfigured { get; private set; }
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

        public List<IController> GetDependencies()
        {
            return new List<IController>(dependencies);
        }

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