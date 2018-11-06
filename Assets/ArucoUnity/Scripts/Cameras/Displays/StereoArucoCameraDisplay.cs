using ArucoUnity.Cameras.Undistortions;
using UnityEngine;

namespace ArucoUnity.Cameras.Displays
{
    /// <summary>
    /// Displays a <see cref="StereoArucoCamera"/>.
    /// </summary>
    public class StereoArucoCameraDisplay : ArucoCameraDisplayGeneric<StereoArucoCamera, ArucoCameraUndistortion>
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The container of the leftCamera and the leftBackgroundCamera.")]
        private Transform leftEye;

        [SerializeField]
        [Tooltip("The container of the rightCamera and the rightBackgroundCamera.")]
        private Transform rightEye;

        [SerializeField]
        [Tooltip("The Unity virtual camera that will shoot the 3D content aligned with the left background.")]
        private Camera leftCamera;

        [SerializeField]
        [Tooltip("The Unity virtual camera that will shoot the 3D content aligned with the right background.")]
        private Camera rightCamera;

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

        // Properties

        /// <summary>
        /// Gets or sets the containers of the <see cref="ArucoCameraDisplayGeneric{T}.Cameras"/> and the
        /// <see cref="ArucoCameraDisplayGeneric{T}.BackgroundCameras"/>.
        /// </summary>
        public Transform[] Eyes { get; protected set; }

        // Variables

        protected Vector3 backgroundsPositionOffset = Vector3.zero;

        // MonoBehaviour methods

        /// <summary>
        /// Sets <see cref="Eyes"/>, <see cref="ArucoCameraDisplayGeneric.Cameras"/>,
        /// <see cref="ArucoCameraDisplayGeneric.BackgroundCameras"/> and
        /// <see cref="ArucoCameraDisplayGeneric.Backgrounds"/> from editor fields if not nulls.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Eyes = new Transform[StereoArucoCamera.StereoCameraNumber];
            Eyes[StereoArucoCamera.CameraId1] = leftEye;
            Eyes[StereoArucoCamera.CameraId2] = rightEye;

            Cameras = new Camera[StereoArucoCamera.StereoCameraNumber];
            Cameras[StereoArucoCamera.CameraId1] = leftCamera;
            Cameras[StereoArucoCamera.CameraId2] = rightCamera;

            BackgroundCameras = new Camera[StereoArucoCamera.StereoCameraNumber];
            BackgroundCameras[StereoArucoCamera.CameraId1] = leftBackgroundCamera;
            BackgroundCameras[StereoArucoCamera.CameraId2] = rightBackgroundCamera;

            Backgrounds = new Renderer[StereoArucoCamera.StereoCameraNumber];
            Backgrounds[StereoArucoCamera.CameraId1] = leftBackground;
            Backgrounds[StereoArucoCamera.CameraId2] = rightBackground;
        }

        // IArucoCameraDisplay methods

        public override void PlaceArucoObject(Transform arucoObject, int cameraId, Vector3 localPosition, Quaternion localRotation)
        {
            base.PlaceArucoObject(arucoObject, cameraId, localPosition, localRotation);

            float direction = (cameraId == StereoArucoCamera.CameraId1) ? 1 : -1;
            arucoObject.transform.position += direction * backgroundsPositionOffset / 2 * localPosition.z;
        }

        // ArucoCameraDisplay methods

        protected override void ConfigureDisplay()
        {
            if (ArucoCameraUndistortion != null)
            {
                backgroundsPositionOffset = ArucoCameraUndistortion.CameraParameters.StereoCameraParameters.TranslationVector.ToPosition();
            }
            base.ConfigureDisplay();
        }

        // ArucoCameraDisplay methods

        /// <summary>
        /// Place the virtual cameras in the same placement than the physical cameras.
        /// </summary>
        /// <param name="cameraId">The id of the camera to configure.</param>
        protected override void ConfigureRectifiedCamera(int cameraId)
        {
            base.ConfigureRectifiedCamera(cameraId);

            float direction = (cameraId == StereoArucoCamera.CameraId1) ? 1 : -1;
            Eyes[cameraId].transform.localPosition += backgroundsPositionOffset / 2;
        }
    }
}