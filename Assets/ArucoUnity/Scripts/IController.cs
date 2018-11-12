using System;
using System.Collections.Generic;

namespace ArucoUnity
{
    /// <summary>
    /// Configurable and startable controller.
    /// </summary>
    /// <remarks>
    /// It can have other configurable controllers as dependencies. They must have started before starting this
    /// controller. They stop this controller when one of them stops.
    /// </remarks>
    public interface IController
    {
        /// <summary>
        /// Called when the controller is configured.
        /// </summary>
        event EventHandler Configured;

        /// <summary>
        /// Called when the controller is configured and ready to be started, when all its dependencies started.
        /// </summary>
        event EventHandler Ready;

        /// <summary>
        /// Called when the controller is started.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Called when the controller is stopped.
        /// </summary>
        event EventHandler Stopped;

        /// <summary>
        /// Gets or sets if configuring and starting automatically when when all dependencies started. Manually
        /// configure and start by calling <see cref="Configure"/> and <see cref="StartController"/>.
        /// </summary>
        bool AutoStart { get; set; }

        /// <summary>
        /// Gets if the controller is configured.
        /// </summary>
        bool IsConfigured { get; }

        /// <summary>
        /// Gets if the controller is ready to be started when all dependencies have started.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// Gets if the controller is started.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Add a new dependency. The controller must be stopped.
        /// </summary>
        /// <param name="dependency">The dependency to add.</param>
        void AddDependency(IController dependency);

        /// <summary>
        /// Remove a dependency. The controller must be stopped.
        /// </summary>
        /// <param name="dependency">The dependency to remove.</param>
        void RemoveDependency(IController dependency);

        /// <summary>
        /// Gets the list of the dependencies.
        /// </summary>
        /// <returns>The list of the dependencies of this instance.</returns>
        List<IController> GetDependencies();

        /// <summary>
        /// Configures the controller and calls the <see cref="Configured"/> event. Properties must be set and the
        /// controller must be stopped.
        /// </summary>
        void Configure();

        /// <summary>
        /// Starts the controller and calls the <see cref="Started"/> event. The controller must be configured, ready and
        /// stopped.
        /// </summary>
        void StartController();

        /// <summary>
        /// Stops the controller and calls the <see cref="Stopped"/> event. The controller must be configured and started.
        /// </summary>
        void StopController();
    }
}