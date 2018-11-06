using ArucoUnity.Cameras.Undistortions;
using System;
using UnityEngine;

namespace ArucoUnity.Cameras.Displays
{
    /// <summary>
    /// Displays a mono <see cref="ArucoCamera"/>.
    /// </summary>
    public class MonoArucoCameraDisplay : ArucoCameraDisplayGeneric<ArucoCamera, ArucoCameraUndistortion>
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The Unity virtual camera that will shoot the 3D content aligned with the background.")]
        private Camera[] cameras;

        [SerializeField]
        [Tooltip("The Unity virtual camera that will shoot the background.")]
        private Camera[] backgroundCameras;

        [SerializeField]
        [Tooltip("The background displaying the image of the corresponding physical camera in ArucoCamera.")]
        private Renderer[] backgrounds;

        // ArucoCameraGenericDisplay properties

        public override Camera[] Cameras { get { return cameras; } protected set { cameras = value; } }
        public override Camera[] BackgroundCameras { get { return backgroundCameras; } protected set { backgroundCameras = value; } }
        public override Renderer[] Backgrounds { get { return backgrounds; } protected set { backgrounds = value; } }

        /// <summary>
        /// Resizes the length of the <see cref="cameras"/>, <see cref="backgroundCameras"/> and <see cref="backgrounds"/>
        /// editor fields to <see cref="ArucoCamera.CameraNumber"/> if different.
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