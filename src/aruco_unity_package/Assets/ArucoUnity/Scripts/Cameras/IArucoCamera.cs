using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Captures the image frames of a camera system.
    /// </summary>
    public interface IArucoCamera
    {
      // Events

      /// <summary>
      /// Called when the camera system has been configured.
      /// </summary>
      event Action Configured;

      /// <summary>
      /// Called when the camera system is started.
      /// </summary>
      event Action Started;

      /// <summary>
      /// Called when the camera system is stopped.
      /// </summary>
      event Action Stopped;

      /// <summary>
      /// Called when the images has been updated.
      /// </summary>
      event Action ImagesUpdated;

      /// <summary>
      /// Callback to undistort the <see cref="Images"/>.
      /// </summary>
      event Action UndistortRectifyImages;

      // Properties

      /// <summary>
      /// Gets or sets if automatically start the camera system when <see cref="Configure"/> is call. Manually start by calling
      /// <see cref="StartCameras"/>.
      /// </summary>
      bool AutoStart { get; set; }

      /// <summary>
      /// Gets the number of cameras in the system.
      /// </summary>
      int CameraNumber { get; }

      /// <summary>
      /// Gets the name of the camera system used.
      /// </summary>
      string Name { get; }

      /// <summary>
      /// Gets the the current image frames manipulated by Unity. There are <see cref="CameraNumber"/> images: one for each camera.
      /// </summary>
      Texture2D[] ImageTextures { get; }

      /// <summary>
      /// Gets or sets the current image frames manipulated by OpenCV. There are <see cref="CameraNumber"/> images: one for each camera.
      /// </summary>
      Cv.Mat[] Images { get; }

      /// <summary>
      /// Gets the <see cref="Images"/> content.
      /// </summary>
      byte[][] ImageDatas { get; }

      /// <summary>
      /// Gets the size of each image in <see cref="ImageDatas"/>.
      /// </summary>
      int[] ImageDataSizes { get; }

      /// <summary>
      /// Gets the <see cref="Images"/> ratios.
      /// </summary>
      float[] ImageRatios { get; }

      /// <summary>
      /// Gets if the camera system is configured.
      /// </summary>
      bool IsConfigured { get; }

      /// <summary>
      /// Gets if the camera system is started.
      /// </summary>
      bool IsStarted { get; }

      // Methods

      /// <summary>
      /// Configures the camera system and calls the <see cref="Configured"/> event. It must be stopped.
      /// </summary>
      void Configure();

      /// <summary>
      /// Starts the camera system, initializes the <see cref="Images"/> and calls the <see cref="Started"/> event. It must be configured and stopped.
      /// </summary>
      void StartCameras();

      /// <summary>
      /// Stops the camera system and calls the <see cref="Stopped"/> event. It must be configured and started.
      /// </summary>
      void StopCameras();
    }
  }

  /// \} aruco_unity_package
}