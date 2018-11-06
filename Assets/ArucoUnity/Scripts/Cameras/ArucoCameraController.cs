using ArucoUnity.Utilities;
using System;

namespace ArucoUnity.Cameras
{
    /// <summary>
    /// Generic configurable controller using a <see cref="ArucoCamera"/> as starting dependency.
    /// </summary>
    public abstract class ArucoCameraController : Controller, IArucoCameraController
    {
        // IArucoCameraController properties

        public IArucoCamera ArucoCamera { get; set; }

        // ConfigurableController methods

        /// <summary>
        /// Adds <see cref="ArucoCamera"/> as dependency.
        /// </summary>
        protected override void Configuring()
        {
            base.Configuring();

            if (ArucoCamera == null)
            {
                throw new ArgumentNullException("ArucoCamera", "This property needs to be set for the configuration.");
            }
            AddDependency(ArucoCamera);
        }
    }
}