using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity.Utilities
{
    /// <summary>
    /// Get images from multiple webcams.
    /// </summary>
    /// <remarks>
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </remarks>
    public class WebcamController : MonoBehaviour
    {
        /// <summary>
        /// Called when the webcams started.
        /// </summary>
        public event Action<WebcamController> Started = delegate { };

        /// <summary>
        /// Gets the ids of the webcams to use.
        /// </summary>
        public List<int> Ids { get; private set; }

        /// <summary>
        /// Gets the used webcams.
        /// </summary>
        public List<WebCamDevice> Devices { get; private set; }

        /// <summary>
        /// Gets the textures of the used webcams.
        /// </summary>
        public List<WebCamTexture> Textures { get; private set; }

        /// <summary>
        /// Gets <see cref="Textures"/> converted in Texture2D.
        /// </summary>
        public List<Texture2D> Textures2D
        {
            get
            {
                for (int cameraId = 0; cameraId < Textures.Count; cameraId++)
                {
                    textures2D[cameraId].SetPixels32(Textures[cameraId].GetPixels32());
                }
                return textures2D;
            }
        }

        /// <summary>
        /// Gets or sets the format of <see cref="Textures2D"/>, by default <see cref="TextureFormat.RGB24"/>.
        /// </summary>
        public TextureFormat Textures2DFormat { get { return textures2DFormat; } set { textures2DFormat = value; } }

        /// <summary>
        /// Gets if the controller is configured.
        /// </summary>
        public bool IsConfigured { get; private set; }

        /// <summary>
        /// Gets if the webcams started.
        /// </summary>
        public bool IsStarted { get; private set; }

        protected bool starting = false;
        private List<Texture2D> textures2D = new List<Texture2D>();
        private TextureFormat textures2DFormat = TextureFormat.RGB24;

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        protected void Awake()
        {
            IsStarted = false;
            IsConfigured = false;

            Ids = new List<int>();
            Devices = new List<WebCamDevice>();
            Textures = new List<WebCamTexture>();
        }

        /// <summary>
        /// Configures <see cref="Devices"/> and <see cref="Textures"/> from <see cref="Ids"/>.
        /// </summary>
        public void Configure()
        {
            IsStarted = false;
            IsConfigured = true;

            Devices.Clear();
            Textures.Clear();
            Textures2D.Clear();

            foreach (int webcamId in Ids)
            {
                var webcamDevice = WebCamTexture.devices[webcamId];
                Devices.Add(webcamDevice);
                Textures.Add(new WebCamTexture(webcamDevice.name));
            }
        }

        /// <summary>
        /// Starts the webcams.
        /// </summary>
        public void StartWebcams()
        {
            if (!IsConfigured || starting || IsStarted)
            {
                throw new Exception("Configure the controller, wait the webcams to start or stop the controller.");
            }
            StartCoroutine(StartingAsync());
        }

        /// <summary>
        /// Stops the webcams.
        /// </summary>
        public void StopWebcams()
        {
            if (!IsConfigured || !IsStarted)
            {
                throw new Exception("Configure the controller and start the controller.");
            }

            IsStarted = false;
            if (starting)
            {
                StopCoroutine(StartingAsync());
            }

            foreach (var webcam in Textures)
            {
                webcam.Stop();
            }
        }

        /// <summary>
        /// Waits for Unity to start the webcams to set <see cref="Textures2D"/>, <see cref="Textures"/> and call
        /// <see cref="ConfigurableController.OnStarted"/>.
        /// </summary>
        protected IEnumerator StartingAsync()
        {
            starting = true;
            foreach (var webcam in Textures)
            {
                webcam.Play();
            }

            bool webcamsStarted;
            do
            {
                webcamsStarted = true;
                foreach (var texture in Textures)
                {
                    webcamsStarted &= texture.width > 100;
                }

                if (webcamsStarted)
                {
                    foreach (var webcam in Textures)
                    {
                        textures2D.Add(new Texture2D(webcam.width, webcam.height, Textures2DFormat, false));
                    }

                    starting = false;
                    IsStarted = true;
                    Started(this);
                }
                else
                {
                    yield return null;
                }
            }
            while (!webcamsStarted);

            starting = false;
        }
    }
}