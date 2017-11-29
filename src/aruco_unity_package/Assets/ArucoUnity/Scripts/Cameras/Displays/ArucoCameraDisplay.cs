using ArucoUnity.Cameras.Undistortions;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Displays
  {
    /// <summary>
    /// Displays a mono <see cref="ArucoCamera"/>.
    /// </summary>
    public class ArucoCameraDisplay : ArucoCameraGenericDisplay
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the 3D content aligned with the background.")]
      private Camera[] cameras;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the background.")]
      private Camera[] backgroundCameras;

      [SerializeField]
      [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
      private Renderer[] backgrounds;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // ArucoCameraGenericDisplay properties

      public override IArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } }
      public override Camera[] Cameras { get { return cameras; } protected set { cameras = value; } }
      public override Camera[] BackgroundCameras { get { return backgroundCameras; } protected set { backgroundCameras = value; } }
      public override Renderer[] Backgrounds { get { return backgrounds; } protected set { backgrounds = value; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public ArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ConcreteArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      // MonoBehaviour methods

      /// <summary>
      /// Resizes the length of the <see cref="cameras"/>, <see cref="backgroundCameras"/> and <see cref="backgrounds"/> editor fields to
      /// <see cref="ArucoCamera.CameraNumber"/> if differents.
      /// </summary>
      protected virtual void OnValidate()
      {
        if (ArucoCamera != null)
        {
          if (cameras != null && cameras.Length != ArucoCamera.CameraNumber)
          {
            Array.Resize(ref cameras, ArucoCamera.CameraNumber);
          }
          if (backgroundCameras != null && backgroundCameras.Length != ArucoCamera.CameraNumber)
          {
            Array.Resize(ref backgroundCameras, ArucoCamera.CameraNumber);
          }
          if (backgrounds != null && backgrounds.Length != ArucoCamera.CameraNumber)
          {
            Array.Resize(ref backgrounds, ArucoCamera.CameraNumber);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}
