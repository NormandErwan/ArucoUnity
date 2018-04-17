using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Captures images of a webcam. Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </summary>
    public class WebcamArucoCamera : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of the webcam to use.")]
      private int webcamId;

      // IArucoCamera properties

      public override int CameraNumber { get { return 1; } }

      public override string Name { get; protected set; }

      // Properties

      /// <summary>
      /// Gets or sets the id of the webcam to use.
      /// </summary>
      public int WebcamId { get { return webcamId; } set { webcamId = value; } }

      /// <summary>
      /// Gets the used webcam.
      /// </summary>
      public WebCamDevice WebCamDevice { get; private set; }

      /// <summary>
      /// Gets the texture of the used webcam.
      /// </summary>
      public WebCamTexture WebCamTexture { get; private set; }

      // Variables

      protected Texture2D webcamTexture2D;
      protected bool startInitiated = false;
      protected int cameraId = 0;

      // MonoBehaviour methods

      /// <summary>
      /// If the camera has been started, waits for Unity to start the webcam to initialize the <see cref="ImageTextures"/> and to
      /// call the <see cref="ArucoCamera.Started"/> event.
      /// </summary>
      protected override void Update()
      {
        if (startInitiated)
        {
          if (WebCamTexture.width < 100) // Wait the WebCamTexture initialization
          {
            return;
          }
          else
          {
            // Configure
            ImageTextures[cameraId] = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);
            webcamTexture2D = new Texture2D(WebCamTexture.width, WebCamTexture.height, TextureFormat.RGB24, false);

            // Update state
            startInitiated = false;
            OnStarted();
          }
        }

        base.Update();
      }

      // ArucoCamera methods

      /// <summary>
      /// Configures the webcam and the properties with the id <see cref="WebcamId"/>.
      /// </summary>
      public override void Configure()
      {
        base.Configure();

        // Reset state
        startInitiated = false;

        // Try to load the webcam
        WebCamDevice = WebCamTexture.devices[WebcamId];
        WebCamTexture = new WebCamTexture(WebCamDevice.name);
        Name = WebCamDevice.name;
        
        OnConfigured();
      }

      /// <summary>
      /// Initiates the camera start and the associated webcam device.
      /// </summary>
      public override void StartController()
      {
        base.StartController();
        if (startInitiated)
        {
          throw new Exception("Camera has already been started.");
        }

        WebCamTexture.Play();
        startInitiated = true;
      }

      /// <summary>
      /// Stops the camera and the associated webcam device.
      /// </summary>
      public override void StopController()
      {
        base.StopController();
        WebCamTexture.Stop();
        startInitiated = false;
        OnStopped();
      }

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, updates every frame the <see cref="ArucoCamera.ImageTextures"/> and the
      /// <see cref="ArucoCamera.ImageDatas"/> with the <see cref="WebCamTexture"/> content.
      /// </summary>
      protected override void UpdateCameraImages()
      {
        webcamTexture2D.SetPixels32(WebCamTexture.GetPixels32());
        Array.Copy(webcamTexture2D.GetRawTextureData(), NextImageDatas[cameraId], ImageDataSizes[cameraId]);
        OnImagesUpdated();
      }
    }
  }

  /// \} aruco_unity_package
}