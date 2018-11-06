using UnityEngine;

namespace ArucoUnity.Cameras.Undistortions
{
    /// <summary>
    /// Manages the processes of undistortion and rectification of <see cref="ArucoCamera.Images"/>. Generic class to
    /// inherit, not the base class.
    /// </summary>
    public abstract class ArucoCameraUndistortionGeneric<T> : ArucoCameraUndistortion, IArucoCameraUndistortion
        where T : ArucoCamera
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The camera to use.")]
        private T arucoCamera;

        // MonoBehaviour properties

        /// <summary>
        /// Sets <see cref="ArucoCameraController.ArucoCamera"/> with editor field if not null.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (arucoCamera != null)
            {
                ArucoCamera = arucoCamera;
            }
        }
    }
}