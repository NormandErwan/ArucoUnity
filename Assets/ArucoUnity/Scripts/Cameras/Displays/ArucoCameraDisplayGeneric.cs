using ArucoUnity.Cameras.Undistortions;
using UnityEngine;

namespace ArucoUnity.Cameras.Displays
{
    public abstract class ArucoCameraDisplayGeneric<T, U> : ArucoCameraDisplay
        where T : ArucoCamera
        where U : ArucoCameraUndistortion
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The camera to use.")]
        private T arucoCamera;

        [SerializeField]
        [Tooltip("The optional undistortion process associated with the ArucoCamera.")]
        private U arucoCameraUndistortion;

        // MonoBehaviour methods

        /// <summary>
        /// Sets <see cref="ArucoCameraDisplay.ArucoCamera"/> and <see cref="ArucoCameraDisplay.ArucoCameraUndistortion"/>
        /// from editor fields if not nulls.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (arucoCamera != null)
            {
                ArucoCamera = arucoCamera;
            }
            if (arucoCameraUndistortion != null)
            {
                ArucoCameraUndistortion = arucoCameraUndistortion;
            }
        }
    }
}