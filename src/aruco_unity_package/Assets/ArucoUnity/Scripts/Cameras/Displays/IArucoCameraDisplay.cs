using ArucoUnity.Cameras.Undistortions;
using ArucoUnity.Controllers;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Displays
  {
    /// <summary>
    /// Manages Unity virual cameras that shoot 3D content aligned with the <see cref="Cameras.IArucoCamera.Images"/> displayed as background. It
    /// creates the augmented reality effect by aligning the images from the physical cameras and the <see cref="Objects.ArucoObject"/> tracked by
    /// <see cref="IArucoObjectsTracker"/>.
    /// </summary>
    public interface IArucoCameraDisplay : IArucoCameraController
    {
      // Properties

      /// <summary>
      /// Gets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      IArucoCameraUndistortion ArucoCameraUndistortion { get; }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the 3D content aligned with the <see cref="Background"/>.
      /// </summary>
      Camera[] Cameras { get; }

      /// <summary>
      /// Gets or sets the Unity virtual camera that will shoot the <see cref="Backgrounds"/>.
      /// </summary>
      Camera[] BackgroundCameras { get; }

      /// <summary>
      /// Gets or sets the backgrounds displaying the <see cref="Cameras.IArucoCamera.Images"/> of the corresponding physical camera in ArucoCamera.
      /// </summary>
      Renderer[] Backgrounds { get; }
    }
  }

  /// \} aruco_unity_package
}
