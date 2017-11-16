using ArucoUnity.Controllers.Utilities;
using ArucoUnity.Plugin;
using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Capture and display every frame the images of any camera system with a fixed number of cameras to use for calibration or ArUco object
    /// tracking. Each camera of the system is associated with a Unity camera that shots as background the current captured frame.
    /// </summary>
    /// <remarks>
    /// If you want to use a custom physical camera not supported by Unity, you need to derive this class. See
    /// <see cref="ArucoCameraWebcam"/> as example. You will need to implement <see cref="StartCameras"/>, <see cref="StopCameras"/>,
    /// <see cref="Configure"/> and to set <see cref="ImageDatas"/> when <see cref="UpdateCameraImages"/> is called.
    /// </remarks>
    public abstract class ArucoCamera : MonoBehaviour
    {
      // Constants

      protected readonly int? dontFlipCode = null;

      // Editor fields

      [SerializeField]
      [Tooltip("Start the cameras automatically after configured it. Call StartCameras() alternatively.")]
      private bool autoStart = true;

      // Events

      /// <summary>
      /// Called when the camera system is configured.
      /// </summary>
      public event Action Configured = delegate { };

      /// <summary>
      /// Called when the camera system starts.
      /// </summary>
      public event Action Started = delegate { };

      /// <summary>
      /// Called when the camera system stops.
      /// </summary>
      public event Action Stopped = delegate { };

      /// <summary>
      /// Called when the images has been updated.
      /// </summary>
      public event Action ImagesUpdated = delegate { };

      /// <summary>
      /// Callback to undistort the <see cref="Images"/>.
      /// </summary>
      public event Action UndistortRectifyImages = delegate { };

      // Properties

      /// <summary>
      /// Gets or sets if automatically start the camera system after configured it. Call StartCameras() alternatively.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// Gets the number of cameras in the system.
      /// </summary>
      public abstract int CameraNumber { get; protected set; }

      /// <summary>
      /// Gets the name of the camera system used.
      /// </summary>
      public abstract string Name { get; protected set; }

      /// <summary>
      /// Gets or sets the current images frame manipulated by OpenCV. They are updated at <see cref="UpdateCameraImages"/>.
      /// </summary>
      public virtual Cv.Mat[] Images
      {
        get
        {
          return images;
        }
        set
        {
          if (images != null && value != null && value.Length == CameraNumber)
          {
            for (int i = 0; i < CameraNumber; i++)
            {
              images[i] = value[i];
            }
          }
        }
      }

      /// <summary>
      /// Gets the <see cref="Images"/> content.
      /// </summary>
      public byte[][] ImageDatas { get; protected set; }

      /// <summary>
      /// Gets the size of each image in <see cref="ImageDatas"/>.
      /// </summary>
      public int[] ImageDataSizes { get; protected set; }

      /// <summary>
      /// Gets the <see cref="Images"/> ratios.
      /// </summary>
      public float[] ImageRatios { get; protected set; }

      /// <summary>
      /// Gets the the current images frame manipulated by Unity. They are updated at <see cref="LateUpdate"/> from the OpenCV <see cref="Images"/>.
      /// </summary>
      public Texture2D[] ImageTextures { get; protected set; }

      /// <summary>
      /// Gets the Unity virtual cameras. There is one for each physical camera (<see cref="CameraNumber"/> cameras). If <see cref="DisplayImages"/>
      /// is set, the <see cref="ImageTextures"/> will be set as background of these Unity cameras.
      /// </summary>
      public Camera[] ImageCameras { get; protected set; }

      /// <summary>
      /// Gets if the camera system is configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      /// <summary>
      /// Gets if the camera system is started.
      /// </summary>
      public bool IsStarted { get; protected set; }

      // Variables

      protected Cv.Mat[] images;
      protected bool imagesUpdatedThisFrame = false;

      protected bool flipHorizontallyImages = false,
                     flipVerticallyImages = false;
      protected int? preDetectflipCode, // Convert the images from Unity's left-handed coordinate system to OpenCV's right-handed coordinate system
                     postDetectflipCode; // Convert back the images

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the camera system state.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;
      }

      /// <summary>
      /// Configure the camera system at start if <see cref="AutoStart"/> is true.
      /// </summary>
      protected virtual void Start()
      {
        if (AutoStart)
        {
          Configure();
        }
      }

      /// <summary>
      /// Update <see cref="Images"/> with the new frame images.
      /// </summary>
      protected virtual void Update()
      {
        if (IsConfigured && IsStarted)
        {
          imagesUpdatedThisFrame = false;
          UpdateCameraImages();
        }
      }

      /// <summary>
      /// Apply the changes made on the <see cref="Images"/> during the frame to the <see cref="ImageTextures"/>.
      /// </summary>
      protected virtual void LateUpdate()
      {
        if (!IsConfigured || !IsStarted || !imagesUpdatedThisFrame)
        {
          return;
        }

        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          // Flip the Images if needed and load them to the textures
          if (postDetectflipCode != null)
          {
            Cv.Flip(Images[cameraId], Images[cameraId], (int)postDetectflipCode);
          }

          // Load the Images to the ImageTextures
          ImageTextures[cameraId].LoadRawTextureData(Images[cameraId].DataIntPtr, ImageDataSizes[cameraId]);
          ImageTextures[cameraId].Apply(false);
        }
      }

      /// <summary>
      /// Automatically stop the camera.
      /// </summary>
      protected virtual void OnDestroy()
      {
        StopCameras();
      }

      // Methods

      /// <summary>
      /// Configure the cameras and the properties, then calls <see cref="StartCameras"/> if <see cref="AutoStart"/> is true.
      /// </summary>
      public virtual void Configure()
      {
        // Initialize the properties and variables
        images = new Cv.Mat[CameraNumber];
        ImageDatas = new byte[CameraNumber][];
        ImageDataSizes = new int[CameraNumber];
        ImageRatios = new float[CameraNumber];
        ImageTextures = new Texture2D[CameraNumber];
        ImageCameras = new Camera[CameraNumber];

        // Configure the flip codes to transfer images from Unity to OpenCV and vice-versa
        // The raw bytes from a Texture to a Mat and from a Mat to a Texture needs to be vertically flipped to be in the correct orientation
        if (!flipHorizontallyImages && !flipVerticallyImages)
        {
          preDetectflipCode = postDetectflipCode = Cv.verticalFlipCode;
        }
        else if (flipHorizontallyImages && !flipVerticallyImages)
        {
          preDetectflipCode = Cv.verticalFlipCode;
          postDetectflipCode = Cv.bothAxesFlipCode;
        }
        else if (!flipHorizontallyImages && flipVerticallyImages)
        {
          preDetectflipCode = dontFlipCode; // Don't flip because texture image is already vertically flipped
          postDetectflipCode = Cv.verticalFlipCode;
        }
        else if (flipHorizontallyImages && flipVerticallyImages)
        {
          preDetectflipCode = dontFlipCode; // Don't flip because texture image is already vertically flipped
          postDetectflipCode = Cv.bothAxesFlipCode;
        }

        // Update state
        IsConfigured = true;
        Configured.Invoke();

        // AutoStart
        if (AutoStart)
        {
          StartCameras();
        }
      }

      /// <summary>
      /// Starts the camera system, and calls <see cref="OnStarted"/> with initialized <see cref="ImageTextures"/>.
      /// </summary>
      public abstract void StartCameras();

      /// <summary>
      /// Stops the camera system and calls <see cref="OnStopped"/>.
      /// </summary>
      public abstract void StopCameras();

      /// <summary>
      /// Calls <see cref="InitializeImages"/> and the <see cref="Started"/> event.
      /// </summary>
      protected void OnStarted()
      {
        InitializeImages();
        IsStarted = true;
        Started();
      }

      /// <summary>
      /// Calls the <see cref="Stopped"/> event.
      /// </summary>
      protected void OnStopped()
      {
        IsStarted = false;
        Stopped();
      }

      /// <summary>
      /// Updates <see cref="ImageDatas"/> with the current frame images and calls <see cref="OnImagesUpdated"/>.
      /// </summary>
      protected abstract void UpdateCameraImages();

      /// <summary>
      /// Calls the <see cref="UndistortRectifyImages"/> and <see cref="ImagesUpdated"/> events.
      /// </summary>
      protected void OnImagesUpdated()
      {
        // Flip the images if needed
        if (preDetectflipCode != null)
        {
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            Cv.Flip(Images[cameraId], Images[cameraId], (int)preDetectflipCode);
          }
        }

        UndistortRectifyImages();

        imagesUpdatedThisFrame = true;
        ImagesUpdated();
      }

      /// <summary>
      /// Initializes the <see cref="Images"/>, <see cref="ImageDataSizes"/> and <see cref="ImageDatas"/> properties from the <see cref="ImageTextures"/>
      /// property.
      /// </summary>
      protected virtual void InitializeImages()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          Images[cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, CvMatExtensions.ImageType(ImageTextures[cameraId].format));
          ImageDataSizes[cameraId] = (int)(Images[cameraId].ElemSize() * Images[cameraId].Total());
          ImageDatas[cameraId] = new byte[ImageDataSizes[cameraId]];
          Images[cameraId].DataByte = ImageDatas[cameraId];
          ImageRatios[cameraId] = ImageTextures[cameraId].width / (float)ImageTextures[cameraId].height;
        }
      }
    }
  }

  /// \} aruco_unity_package
}