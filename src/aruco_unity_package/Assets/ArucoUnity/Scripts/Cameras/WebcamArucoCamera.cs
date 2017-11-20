using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Manages any connected webcam to the machine, retrieves and displays the camera's image every frame. Use one webcam at a time.
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </summary>
    public class WebcamArucoCamera : ArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of the webcam to use.")]
      private int webcamId;

      // ArucoCamera properties implementation

      public override int CameraNumber { get; protected set; }

      public override string Name { get; protected set; }

      // Properties

      /// <summary>
      /// Gets or sets the id of the webcam to use.
      /// </summary>
      public int WebcamIds { get { return webcamId; } set { webcamId = value; } }

      /// <summary>
      /// Gets the used webcam.
      /// </summary>
      public WebCamDevice WebCamDevice { get; protected set; }

      /// <summary>
      /// Gets the textures of the used webcams.
      /// </summary>
      public WebCamTexture WebCamTexture { get; protected set; }

      // Variables

      protected bool startInitiated = false;
      protected int cameraId = 0;

      // MonoBehaviour methods

      /// <summary>
      /// If the camera has been started, wait for Unity to start the webcam to initialize the textures, the Unity camera backgrounds and to trigger
      /// the <see cref="ArucoCamera.Started"/> event.
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

            // Update state
            startInitiated = false;
            OnStarted();
          }
        }

        base.Update();
      }

      // ArucoCamera methods

      /// <summary>
      /// Configure the webcam and its properties with the id <see cref="WebcamIds"/>. The camera needs to be stopped before configured.
      /// </summary>
      public override void Configure()
      {
        if (IsStarted || startInitiated)
        {
          return;
        }

        // Reset state
        startInitiated = false;
        IsConfigured = false;

        // Try to load the webcam
        CameraNumber = 1;
        WebCamDevice = WebCamTexture.devices[webcamId];
        WebCamTexture = new WebCamTexture(WebCamDevice.name);
        Name = WebCamDevice.name;
        
        base.Configure();
      }

      /// <summary>
      /// Initiate the camera start and the associated webcam device.
      /// </summary>
      public override void StartCameras()
      {
        if (IsConfigured && !IsStarted && !startInitiated)
        {
          WebCamTexture.Play();
          startInitiated = true;
        }
      }

      /// <summary>
      /// Stop the camera and the associated webcam device.
      /// </summary>
      public override void StopCameras()
      {
        if (IsConfigured && (IsStarted || startInitiated))
        {
          WebCamTexture.Stop();
          startInitiated = false;
          OnStopped();
        }
      }

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, update every frame the <see cref="ArucoCamera.ImageTextures"/> and the
      /// <see cref="ArucoCamera.ImageDatas"/> with the <see cref="WebCamTexture"/> content.
      /// </summary>
      protected override void UpdateCameraImages()
      {
        ImageTextures[cameraId].SetPixels32(WebCamTexture.GetPixels32());
        Array.Copy(ImageTextures[cameraId].GetRawTextureData(), ImageDatas[cameraId], ImageDataSizes[cameraId]);

        OnImagesUpdated();
      }
    }
  }

  /// \} aruco_unity_package
}