using System;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utilities
  {
    /// <summary>
    /// Configurable and startable controller. It can have other configurable controllers as dependencies. They must
    /// have started before starting this controller. They stop this controller when at least one of them stops.
    /// </summary>
    public interface IConfigurableController
    {
      // Events

      /// <summary>
      /// Called when the controller is configured.
      /// </summary>
      event Action Configured;

      /// <summary>
      /// Called when the controller is ready to be started, when all dependencies have been started.
      /// </summary>
      event Action Ready;

      /// <summary>
      /// Called when the controller is started.
      /// </summary>
      event Action Started;

      /// <summary>
      /// Called when the controller is stopped.
      /// </summary>
      event Action Stopped;

      // Properties

      /// <summary>
      /// Gets or sets if configuring and starting automatically when when all dependencies have started. Manually
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

      // Methods
      
      /// <summary>
      /// Add a new dependency.
      /// </summary>
      void AddDependency(IConfigurableController controller);

      /// <summary>
      /// Remove a dependency.
      /// </summary>
      void RemoveDependency(IConfigurableController controller);

      /// <summary>
      /// Gets the list of the dependencies.
      /// </summary>
      List<IConfigurableController> GetDependencies();

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

  /// \} aruco_unity_package
}