using ArucoUnity.Cameras.Undistortions;
using UnityEngine;

namespace ArucoUnity.Cameras.Displays
{
    /// <summary>
    /// Manages Unity virual cameras that shoot 3D content aligned with the <see cref="IArucoCamera.Images"/> displayed as background. It
    /// creates the augmented reality effect by aligning the images from the physical cameras and the <see cref="Objects.ArucoObject"/> tracked by
    /// <see cref="IArucoObjectsTracker"/>.
    /// </summary>
    public interface IArucoCameraDisplay : IArucoCameraController
    {
        // Properties

        /// <summary>
        /// Gets the optional undistortion process associated with the ArucoCamera.
        /// </summary>
        IArucoCameraUndistortion ArucoCameraUndistortion { get; set; }

        /// <summary>
        /// Gets the Unity virtual camera that will shoot the 3D content aligned with the <see cref="Background"/>.
        /// </summary>
        Camera[] Cameras { get; }

        /// <summary>
        /// Gets the Unity virtual camera that will shoot the <see cref="Backgrounds"/>.
        /// </summary>
        Camera[] BackgroundCameras { get; }

        /// <summary>
        /// Gets the backgrounds displaying the <see cref="IArucoCamera.Images"/> of the corresponding physical camera in ArucoCamera.
        /// </summary>
        Renderer[] Backgrounds { get; }

        // Methods

        /// <summary>
        /// Updates the transform of an ArUco object.
        /// </summary>
        /// <param name="arucoObject">The transfomr to the ArUco object to place.</param>
        /// <param name="cameraId">The id of the camera to use. The transform is placed and oriented relative to this camera.</param>
        /// <param name="localPosition">The estimated translation of the transform relative to the camera.</param>
        /// <param name="localRotation">The estimated rotation of the transform relative to the camera.</param>
        void PlaceArucoObject(Transform arucoObject, int cameraId, Vector3 localPosition, Quaternion localRotation);
    }
}