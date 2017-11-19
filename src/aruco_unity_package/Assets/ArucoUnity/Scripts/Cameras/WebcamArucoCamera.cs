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
      [Tooltip("The ids of the webcams to use.")]
      private int[] webcamIds;

      // ArucoCamera properties implementation

      public override int CameraNumber { get; protected set; }

      public override string Name { get; protected set; }

      // Properties

      /// <summary>
      /// Gets or sets the id of the webcam to use.
      /// </summary>
      public int[] WebcamIds { get { return webcamIds; } set { webcamIds = value; } }

      /// <summary>
      /// Gets the used webcam.
      /// </summary>
      public WebCamDevice[] WebCamDevices { get; protected set; }

      /// <summary>
      /// Gets the textures of the used webcams.
      /// </summary>
      public WebCamTexture[] WebCamTextures { get; protected set; }

      // Variables

      protected bool startInitiated = false;

      // MonoBehaviour methods

      /// <summary>
      /// If the camera has been started, wait for Unity to start the webcam to initialize the textures, the Unity camera backgrounds and to trigger
      /// the <see cref="ArucoCamera.Started"/> event.
      /// </summary>
      protected override void Update()
      {
        if (startInitiated)
        {
          if (WebCamTextures[0].width < 100) // Wait the WebCamTexture initialization
          {
            return;
          }
          else
          {
            // Configure
            for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
            {
              ImageTextures[cameraId] = new Texture2D(WebCamTextures[cameraId].width, WebCamTextures[cameraId].height, TextureFormat.RGB24, false);
            }

            // Update state
            startInitiated = false;
            OnStarted();
          }
        }

        base.Update();
      }

      protected override void OnValidate()
      {
        CameraNumber = WebcamIds.Length;
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

        // Initializes the properties
        CameraNumber = WebcamIds.Length;
        WebCamDevices = new WebCamDevice[CameraNumber];
        WebCamTextures = new WebCamTexture[CameraNumber];
        Name = "";

        // Try to load the webcams
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          int webcamId = WebcamIds[cameraId];
          if (WebCamTexture.devices.Length <= webcamId)
          {
            throw new ArgumentException("The webcam with the id '" + WebcamIds + "' is not found.", "WebcamId");
          }
          WebCamDevices[cameraId] = WebCamTexture.devices[webcamId];
          WebCamTextures[cameraId] = new WebCamTexture(WebCamDevices[cameraId].name);

          if (cameraId > 0)
          {
            Name += "+";
          }
          Name += WebCamDevices[cameraId].name;
        }
        
        base.Configure();
      }

      /// <summary>
      /// Initiate the camera start and the associated webcam device.
      /// </summary>
      public override void StartCameras()
      {
        if (IsConfigured && !IsStarted && !startInitiated)
        {
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            WebCamTextures[cameraId].Play();
          }
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
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            WebCamTextures[cameraId].Stop();
          }
          startInitiated = false;
          OnStopped();
        }
      }

      /// <summary>
      /// Once the <see cref="WebCamTextures"/> is started, update every frame the <see cref="ArucoCamera.ImageTextures"/> and the
      /// <see cref="ArucoCamera.ImageDatas"/> with the <see cref="WebCamTextures"/> content.
      /// </summary>
      protected override void UpdateCameraImages()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          ImageTextures[cameraId].SetPixels32(WebCamTextures[cameraId].GetPixels32());
          Array.Copy(ImageTextures[cameraId].GetRawTextureData(), ImageDatas[cameraId], ImageDataSizes[cameraId]);
        }

        OnImagesUpdated();
      }
    }
  }

  /// \} aruco_unity_package
}