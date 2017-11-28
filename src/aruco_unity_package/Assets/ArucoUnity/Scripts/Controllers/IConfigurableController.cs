using System;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Configurable and startable controller.
    /// </summary>
    public interface IConfigurableController
    {
      // Events

      /// <summary>
      /// Called when the controller is configured.
      /// </summary>
      event Action Configured;

      /// <summary>
      /// Called when the controller is ready to be started, when all <see cref="ControllerDependencies"/> have been started. 
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
      /// Gets or sets if configuring and starting automatically when set to true and when each controller in <see cref="ControllerDependencies"/>
      /// has started. Configure manually by calling <see cref="Configure"/> and start manually by calling <see cref="StartController"/>.
      /// </summary>
      bool AutoStart { get; set; }

      /// <summary>
      /// Gets if the controller is configured.
      /// </summary>
      bool IsConfigured { get; }

      /// <summary>
      /// Gets if the controller is ready to be started when all <see cref="ControllerDependencies"/> have been started.
      /// </summary>
      bool IsReady { get; }

      /// <summary>
      /// Gets if the controller is started.
      /// </summary>
      bool IsStarted { get; }

      /// <summary>
      /// List of dependency controllers that must have started before starting this controller, and that trigger stopping this controller when
      /// at least one of them stops.
      /// </summary>
      List<IConfigurableController> ControllerDependencies { get; }

      // Methods

      /// <summary>
      /// Configures the controller and calls the <see cref="Configured"/> event. Properties must be set and the controller must be stopped.
      /// </summary>
      void Configure();

      /// <summary>
      /// Starts the controller and calls the <see cref="Started"/> event. The controller must be configured, ready and stopped.
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