using ArucoUnity.Cameras.Undistortions;
using ArucoUnity.Utilities;
using UnityEngine;

namespace ArucoUnity.Cameras.Displays
{
    /// <summary>
    /// Manages Unity virual cameras that shoot 3D content aligned with the <see cref="IArucoCamera.Images"/> displayed
    /// as background. It creates the augmented reality effect by the images from the physical cameras and the
    /// <see cref="Objects.ArucoObject"/> tracked by <see cref="Objects.Trackers.ArucoObjectsTracker"/>.
    /// </summary>
    public abstract class ArucoCameraDisplay : ArucoCameraController, IArucoCameraDisplay
    {
        // Constants

        public const float cameraBackgroundDistance = 1f;

        // IArucoCameraDisplay properties

        public virtual Camera[] Cameras { get; protected set; }
        public virtual Camera[] BackgroundCameras { get; protected set; }
        public virtual Renderer[] Backgrounds { get; protected set; }

        // Properties

        /// <summary>
        /// Gets or sets the optional undistortion process associated with the <see cref="ArucoCameraController.ArucoCamera"/>.
        /// </summary>
        public IArucoCameraUndistortion ArucoCameraUndistortion { get; set; }

        // ConfigurableController methods

        /// <summary>
        /// Adds <see cref="ArucoCameraUndistortion"/> as dependency if set.
        /// </summary>
        protected override void Configuring()
        {
            base.Configuring();
            if (ArucoCameraUndistortion != null)
            {
                AddDependency(ArucoCameraUndistortion);
            }
        }

        /// <summary>
        /// Calls <see cref="ConfigureDisplay"/> the <see cref="SetDisplayActive(bool)"/> to activate the display.
        /// </summary>
        protected override void Starting()
        {
            base.Starting();
            ConfigureDisplay();
            SetDisplayActive(true);
        }

        /// <summary>
        /// Deactivates the display with <see cref="SetDisplayActive(bool)"/>.
        /// </summary>
        protected override void Stopping()
        {
            base.Stopping();
            SetDisplayActive(false);
        }

        // IArucoCameraDisplay methods

        public virtual void PlaceArucoObject(Transform arucoObject, int cameraId, Vector3 localPosition, Quaternion localRotation)
        {
            var parent = arucoObject.transform.parent;
            arucoObject.transform.SetParent(Cameras[cameraId].transform);

            arucoObject.transform.localPosition = localPosition;
            arucoObject.transform.localRotation = localRotation;

            arucoObject.transform.SetParent(parent);
            arucoObject.gameObject.SetActive(true);
        }

        // Methods

        /// <summary>
        /// Configures the <see cref="BackgroundCameras"/> and the <see cref="Backgrounds"/> according to the
        /// <see cref="ArucoCameraUndistortion"/> if set otherwise with default values.
        /// </summary>
        protected virtual void ConfigureDisplay()
        {
            // Sets the background texture
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                Backgrounds[cameraId].material.mainTexture = ArucoCamera.Textures[cameraId];
            }

            // Cameras and background configurations
            if (ArucoCameraUndistortion == null)
            {
                ConfigureDefaultBackgrounds();
            }
            else
            {
                for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
                {
                    ConfigureRectifiedCamera(cameraId);
                    ConfigureRectifiedBackground(cameraId);
                }
            }
        }

        /// <summary>
        /// Activates or deactivates the <see cref="Cameras"/>, the <see cref="BackgroundCameras"/> and the <see cref="Backgrounds"/>.
        /// </summary>
        /// <param name="value">True to activate, false to deactivate.</param>
        protected virtual void SetDisplayActive(bool value)
        {
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                Cameras[cameraId].gameObject.SetActive(value);
                BackgroundCameras[cameraId].gameObject.SetActive(value);
                Backgrounds[cameraId].gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Places the <see cref="Backgrounds"/> in front of the corresponding <see cref="BackgroundCameras"/> centered and scaled to fit in the
        /// camera view.
        /// </summary>
        protected virtual void ConfigureDefaultBackgrounds()
        {
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                Vector3 localScale = Vector3.one;
                if (BackgroundCameras[cameraId].aspect < ArucoCamera.ImageRatios[cameraId])
                {
                    localScale.x = 2f * cameraBackgroundDistance * BackgroundCameras[cameraId].aspect * Mathf.Tan(0.5f * BackgroundCameras[cameraId].fieldOfView * Mathf.Deg2Rad);
                    localScale.y = localScale.x / ArucoCamera.ImageRatios[cameraId];
                }
                else
                {
                    localScale.y = 2f * cameraBackgroundDistance * Mathf.Tan(0.5f * BackgroundCameras[cameraId].fieldOfView * Mathf.Deg2Rad);
                    localScale.x = localScale.y * ArucoCamera.ImageRatios[cameraId];
                }

                Backgrounds[cameraId].transform.localPosition = new Vector3(0, 0, cameraBackgroundDistance);
                Backgrounds[cameraId].transform.localScale = localScale;
            }
        }

        /// <summary>
        /// Configures the field of view of a <see cref="Cameras"/> according to the vertical focal length of the corresponding rectified camera matrix
        /// in <see cref="ArucoCameraUndistortion.RectifiedCameraMatrices"/>. If the camera targets an eye in VR mode, Unity has already configured it.
        /// </summary>
        /// <param name="cameraId">The id of the camera to configure.</param>
        protected virtual void ConfigureRectifiedCamera(int cameraId)
        {
            float imageHeight = ArucoCameraUndistortion.CameraParameters.ImageHeights[cameraId];
            Vector2 cameraF = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();

            float fovY = 2f * Mathf.Atan(0.5f * imageHeight / cameraF.y) * Mathf.Rad2Deg;
            Cameras[cameraId].fieldOfView = fovY;
            BackgroundCameras[cameraId].fieldOfView = fovY;
        }

        /// <summary>
        /// Places a <see cref="Backgrounds"/> in front of the corresponding <see cref="BackgroundCameras"/> centered with the principal point of the
        /// corresponding rectified camera matrix in <see cref="ArucoCameraUndistortion.RectifiedCameraMatrices"/> and scaled to fit in the field of
        /// view calculated from the focal lengths of the rectified camera matrix.
        /// </summary>
        /// <param name="cameraId">The id of the background and the background camera to configure.</param>
        protected virtual void ConfigureRectifiedBackground(int cameraId)
        {
            float imageWidth = ArucoCameraUndistortion.CameraParameters.ImageWidths[cameraId];
            float imageHeight = ArucoCameraUndistortion.CameraParameters.ImageHeights[cameraId];
            Vector2 focalLength = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraFocalLengths();
            Vector2 principalPoint = ArucoCameraUndistortion.RectifiedCameraMatrices[cameraId].GetCameraPrincipalPoint();

            // Considering https://docs.opencv.org/3.4/d4/d94/tutorial_camera_calibration.html, we are looking for X=posX and Y=posY
            // with x=0.5*ImageWidth, y=0.5*ImageHeight (center of the camera projection) and w=Z=cameraBackgroundDistance 
            float localPositionX = (0.5f * imageWidth - principalPoint.x) / focalLength.x * cameraBackgroundDistance;
            float localPositionY = -(0.5f * imageHeight - principalPoint.y) / focalLength.y * cameraBackgroundDistance; // a minus because OpenCV camera coordinates origin is top - left, but bottom-left in Unity

            // Considering https://stackoverflow.com/a/41137160
            // scale.x = 2 * cameraBackgroundDistance * tan(fovx / 2), cameraF.x = imageWidth / (2 * tan(fovx / 2))
            float localScaleX = imageWidth / focalLength.x * cameraBackgroundDistance;
            float localScaleY = imageHeight / focalLength.y * cameraBackgroundDistance;

            // Place and scale the background
            Backgrounds[cameraId].transform.localPosition = new Vector3(localPositionX, localPositionY, cameraBackgroundDistance);
            Backgrounds[cameraId].transform.localScale = new Vector3(localScaleX, localScaleY, 1);
        }
    }
}