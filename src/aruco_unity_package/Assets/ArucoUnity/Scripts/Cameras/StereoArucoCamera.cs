namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Captures every frame the images of stereo camera system.
    /// </summary>
    public abstract class StereoArucoCamera : ArucoCamera
    {
      // Properties

      /// <summary>
      /// Gets the number of cameras in the system.
      /// </summary>
      public override int CameraNumber { get { return 2; } protected set { } }
    }
  }

  /// \} aruco_unity_package
}