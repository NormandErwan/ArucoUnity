using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity.Cameras
{
    /// <summary>
    /// Captures images of a webcam.
    /// </summary>
    public class ArucoWebcam : ArucoCamera
    {
        // Constants

        protected const int cameraId = 0;

        // Editor fields

        [SerializeField]
        [Tooltip("The id of the webcam to use.")]
        private int webcamId;

        // IArucoCamera properties

        public override int CameraNumber { get { return 1; } }

        public override string Name { get; protected set; }

        // Properties

        /// <summary>
        /// Gets or set the id of the webcam to use.
        /// </summary>
        public int WebcamId { get { return webcamId; } set { webcamId = value; } }

        /// <summary>
        /// Gets the controller of the webcam to use.
        /// </summary>
        public WebcamController WebcamController { get; private set; }

        // MonoBehaviour methods

        /// <summary>
        /// Initializes <see cref="WebcamController"/> and subscribes to.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            WebcamController = gameObject.AddComponent<WebcamController>();
            WebcamController.Started += WebcamController_Started;
        }

        /// <summary>
        /// Unsubscribes to <see cref="WebcamController"/>.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            WebcamController.Started -= WebcamController_Started;
        }

        // ConfigurableController methods

        /// <summary>
        /// Calls <see cref="WebcamController.Configure"/> and sets <see cref="Name"/>.
        /// </summary>
        protected override void Configuring()
        {
            base.Configuring();

            WebcamController.Ids.Clear();
            WebcamController.Ids.Add(WebcamId);
            WebcamController.Configure();

            Name = WebcamController.Devices[cameraId].name;
        }

        /// <summary>
        /// Calls <see cref="WebcamController.StartWebcams"/>.
        /// </summary>
        protected override void Starting()
        {
            base.Starting();
            WebcamController.StartWebcams();
        }

        /// <summary>
        /// Calls <see cref="WebcamController.StopWebcams"/>.
        /// </summary>
        protected override void Stopping()
        {
            base.Stopping();
            WebcamController.StopWebcams();
        }

        /// <summary>
        /// Blocks <see cref="ArucoCamera.OnStarted"/> until <see cref="WebcamController.IsStarted"/>.
        /// </summary>
        protected override void OnStarted()
        {
        }

        // ArucoCamera methods

        /// <summary>
        /// Copy current webcam images to <see cref="ArucoCamera.NextImages"/>.
        /// </summary>
        protected override bool UpdatingImages()
        {
            Array.Copy(WebcamController.Textures2D[cameraId].GetRawTextureData(), NextImageDatas[cameraId], ImageDataSizes[cameraId]);
            return true;
        }

        // Methods

        /// <summary>
        /// Configures <see cref="ArucoCamera.Textures"/> and calls <see cref="ArucoCamera.OnStarted"/>.
        /// </summary>
        protected virtual void WebcamController_Started(WebcamController webcamController)
        {
            var webcamTexture = WebcamController.Textures2D[cameraId];
            Textures[cameraId] = new Texture2D(webcamTexture.width, webcamTexture.height, webcamTexture.format, false);
            base.OnStarted();
        }
    }
}