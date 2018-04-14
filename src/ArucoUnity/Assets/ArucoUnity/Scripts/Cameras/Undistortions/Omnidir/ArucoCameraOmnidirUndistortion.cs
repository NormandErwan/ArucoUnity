using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Undistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of fisheye and omnidir <see cref="Cameras.ArucoCamera"/>.
    /// </summary>
    public class ArucoCameraOmnidirUndistortion : ArucoCameraGenericOmnidirUndistortion
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public ArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }
    }
  }

  /// \} aruco_unity_package
}