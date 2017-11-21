using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Captures image frames of two webcam cameras.
    /// </summary>
    public class WebcamStereoArucoCamera : StereoArucoCamera
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of the first webcam to use.")]
      private int webcamId1;

      [SerializeField]
      [Tooltip("The id of the second webcam to use.")]
      private int webcamId2;

      // Properties

      public override string Name { get; protected set; }

      /// <summary>
      /// Gets or sets the id of the first webcam to use.
      /// </summary>
      public int WebcamId1 { get { return webcamId1; } set { webcamId1 = value; } }

      /// <summary>
      /// Gets or sets the id of the second webcam to use.
      /// </summary>
      public int WebcamId2 { get { return webcamId2; } set { webcamId2 = value; } }

      /// <summary>
      /// Gets the used webcams.
      /// </summary>
      public WebCamDevice[] WebCamDevices { get; protected set; }

      /// <summary>
      /// Gets the textures of the used webcams.
      /// </summary>
      public WebCamTexture[] WebCamTextures { get; protected set; }

      // Variables

      protected bool startInitiated = false;
      protected int cameraId1 = 0, cameraId2 = 1;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the properties.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();
        WebCamDevices = new WebCamDevice[CameraNumber];
        WebCamTextures = new WebCamTexture[CameraNumber];
      }

      /// <summary>
      /// If the cameras has been started, waits for Unity to start the webcam to initialize the <see cref="ImageTextures"/> and to
      /// call the <see cref="ArucoCamera.Started"/> event.
      /// </summary>
      protected override void Update()
      {
        if (startInitiated)
        {
          if (WebCamTextures[cameraId1].width < 100 || WebCamTextures[cameraId2].width < 100) // Wait the WebCamTexture initialization
          {
            return;
          }
          else
          {
            // Configure
            ImageTextures[cameraId1] = new Texture2D(WebCamTextures[cameraId1].width, WebCamTextures[cameraId1].height, TextureFormat.RGB24, false);
            ImageTextures[cameraId2] = new Texture2D(WebCamTextures[cameraId2].width, WebCamTextures[cameraId2].height, TextureFormat.RGB24, false);

            // Update state
            startInitiated = false;
            OnStarted();
          }
        }

        base.Update();
      }

      // ArucoCamera methods

      /// <summary>
      /// Configures the webcams and the properties with the ids <see cref="WebcamId1"/> and <see cref="WebcamId2"/>.
      /// </summary>
      public override void Configure()
      {
        base.Configure();

        // Reset state
        startInitiated = false;

        // Try to load the webcam
        WebCamDevices[cameraId1] = WebCamTexture.devices[WebcamId1];
        WebCamDevices[cameraId2] = WebCamTexture.devices[WebcamId2];
        WebCamTextures[cameraId1] = new WebCamTexture(WebCamDevices[cameraId1].name);
        WebCamTextures[cameraId2] = new WebCamTexture(WebCamDevices[cameraId2].name);
        Name = WebCamDevices[cameraId1].name + "+" + WebCamDevices[cameraId2].name;

        OnConfigured();
      }

      /// <summary>
      /// Initiates the cameras start and the associated webcam devices.
      /// </summary>
      public override void StartCameras()
      {
        base.StartCameras();
        if (startInitiated)
        {
          throw new Exception("Cameras have already been started.");
        }

        WebCamTextures[cameraId1].Play();
        WebCamTextures[cameraId2].Play();
        startInitiated = true;
      }

      /// <summary>
      /// Stops the cameras and the associated webcam devices.
      /// </summary>
      public override void StopCameras()
      {
        base.StopCameras();
        WebCamTextures[cameraId1].Stop();
        WebCamTextures[cameraId2].Stop();
        startInitiated = false;
        OnStopped();
      }

      /// <summary>
      /// Once the <see cref="WebCamTexture"/> is started, updates every frame the <see cref="ArucoCamera.ImageTextures"/> and the
      /// <see cref="ArucoCamera.ImageDatas"/> with the <see cref="WebCamTexture"/> content.
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