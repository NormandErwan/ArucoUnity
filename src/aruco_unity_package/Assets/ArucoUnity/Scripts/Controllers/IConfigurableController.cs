using System;

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
      /// Called when the controller has been configured.
      /// </summary>
      event Action Configured;

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
      /// Gets or sets if configuring and starting automatically when the controller is created. Configure manually by calling
      /// <see cref="Configure"/> and start manually by calling <see cref="StartController"/>.
      /// </summary>
      bool AutoStart { get; set; }

      /// <summary>
      /// Gets if the controller is configured.
      /// </summary>
      bool IsConfigured { get; }

      /// <summary>
      /// Gets if the controller is started.
      /// </summary>
      bool IsStarted { get; }

      // Methods

      /// <summary>
      /// Configures the controller and calls the <see cref="Configured"/> event. It must be stopped.
      /// </summary>
      void Configure();

      /// <summary>
      /// Starts the controller and calls the <see cref="Started"/> event. The controller must be configured and stopped.
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