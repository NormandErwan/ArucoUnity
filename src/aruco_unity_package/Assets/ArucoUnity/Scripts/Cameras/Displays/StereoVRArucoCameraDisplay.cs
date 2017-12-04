using ArucoUnity.Cameras.Undistortions;
using ArucoUnity.Utilities;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Displays
  {
    /// <summary>
    /// Displays a <see cref="StereoArucoCamera"/> in a VR HMD.
    /// </summary>
    public class StereoVRArucoCameraDisplay : ArucoCameraGenericDisplay
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private StereoArucoCamera stereoArucoCamera;

      [SerializeField]
      [Tooltip("The optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      [SerializeField]
      [Tooltip("The Unity stereo VR virtual camera that will shoot the 3D content aligned with the backgrounds.")]
      private Camera stereoVRCamera;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the left eye background.")]
      private Camera leftBackgroundCamera;

      [SerializeField]
      [Tooltip("The Unity virtual camera that will shoot the right eye background.")]
      private Camera rightBackgroundCamera;

      [SerializeField]
      [Tooltip("The background displaying the image of the left physical camera in ArucoCamera.")]
      private Renderer leftBackground;

      [SerializeField]
      [Tooltip("The background displaying the image of the right physical camera in ArucoCamera.")]
      private Renderer rightBackground;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return stereoArucoCamera; } }

      // ArucoCameraGenericDisplay properties

      public override IArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public StereoArucoCamera StereoArucoCamera { get { return stereoArucoCamera; } set { stereoArucoCamera = value; } }

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ConcreteArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      // Variables

      protected Vector3 backgroundsPositionOffset;

      // MonoBehaviour methods

      /// <summary>
      /// Populates <see cref="Eyes"/>, <see cref="ArucoCameraGenericDisplay.Cameras"/>, <see cref="ArucoCameraGenericDisplay.BackgroundCameras"/>
      /// and <see cref="ArucoCameraGenericDisplay.Backgrounds"/> from editor fields.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();

        Cameras = new Camera[ArucoCamera.CameraNumber];
        Cameras[StereoArucoCamera.CameraId1] = Cameras[StereoArucoCamera.CameraId2] = stereoVRCamera;

        BackgroundCameras = new Camera[ArucoCamera.CameraNumber];
        BackgroundCameras[StereoArucoCamera.CameraId1] = leftBackgroundCamera;
        BackgroundCameras[StereoArucoCamera.CameraId2] = rightBackgroundCamera;

        Backgrounds = new Renderer[ArucoCamera.CameraNumber];
        Backgrounds[StereoArucoCamera.CameraId1] = leftBackground;
        Backgrounds[StereoArucoCamera.CameraId2] = rightBackground;
      }

      // IArucoCameraDisplay methods

      public override void PlaceArucoObject(Transform arucoObject, int cameraId, Vector3 localPosition, Quaternion localRotation)
      {
        base.PlaceArucoObject(arucoObject, cameraId, localPosition, localRotation);

        float direction = (cameraId == StereoArucoCamera.CameraId1) ? 1 : -1;
        arucoObject.transform.position += direction * backgroundsPositionOffset / 2;
      }

      // Methods

      /// <summary>
      /// Configure the backgrounds for VR mode.
      /// </summary>
      protected override void ConfigureDisplay()
      {
        backgroundsPositionOffset = ArucoCameraUndistortion.CameraParameters.StereoCameraParameters.TranslationVector.ToPosition();

        base.ConfigureDisplay();

        if (ArucoCameraUndistortion != null)
        {
          // Place the backgrounds to allow the eyes to fuse them in a VR HMD
          Backgrounds[StereoArucoCamera.CameraId1].transform.localPosition += backgroundsPositionOffset / 2;
          Backgrounds[StereoArucoCamera.CameraId2].transform.localPosition -= backgroundsPositionOffset / 2;

          // Adjust the stereo convergence of the background camera to the focal length
          BackgroundCameras[StereoArucoCamera.CameraId1].stereoConvergence =
            ArucoCameraUndistortion.RectifiedCameraMatrices[StereoArucoCamera.CameraId1].GetCameraFocalLengths().x;
          BackgroundCameras[StereoArucoCamera.CameraId2].stereoConvergence =
            ArucoCameraUndistortion.RectifiedCameraMatrices[StereoArucoCamera.CameraId2].GetCameraFocalLengths().x;
        }
      }

      /// <summary>
      /// Cancel the base class configuration of the virtual cameras as Unity already handles them.
      /// </summary>
      protected override void ConfigureRectifiedCamera(int cameraId)
      {
      }
    }
  }

  /// \} aruco_unity_package
}
