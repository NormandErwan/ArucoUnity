using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity.Cameras
{
    /// <summary>
    /// Captures image of a stereoscopic webcam.
    /// </summary>
    public class StereoArucoWebcam : StereoArucoCamera
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The id of the first webcam to use.")]
        private int webcamId1;

        [SerializeField]
        [Tooltip("The id of the second webcam to use.")]
        private int webcamId2;

        // IArucoCamera properties

        public override string Name { get; protected set; }

        // Properties

        /// <summary>
        /// Gets or sets the id of the first webcam to use.
        /// </summary>
        public int WebcamId1 { get { return webcamId1; } set { webcamId1 = value; } }

        /// <summary>
        /// Gets or sets the id of the second webcam to use.
        /// </summary>
        public int WebcamId2 { get { return webcamId2; } set { webcamId2 = value; } }

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
            WebcamController.Ids.AddRange(new int[] { WebcamId1, WebcamId2 });
            WebcamController.Configure();

            Name = "'" + WebcamController.Devices[CameraId1].name + "'+'" + WebcamController.Devices[CameraId2].name + "'";
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
            for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
            {
                Array.Copy(WebcamController.Textures2D[cameraId].GetRawTextureData(), NextImageDatas[cameraId], ImageDataSizes[cameraId]);
            }
            return true;
        }

        // Methods

        /// <summary>
        /// Configures <see cref="ArucoCamera.Textures"/> and calls <see cref="ArucoCamera.OnStarted"/>.
        /// </summary>
        protected virtual void WebcamController_Started(WebcamController webcamController)
        {
            for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
            {
                var webcamTexture = WebcamController.Textures2D[cameraId];
                Textures[cameraId] = new Texture2D(webcamTexture.width, webcamTexture.height, webcamTexture.format, false);
            }
            base.OnStarted();
        }
    }
}