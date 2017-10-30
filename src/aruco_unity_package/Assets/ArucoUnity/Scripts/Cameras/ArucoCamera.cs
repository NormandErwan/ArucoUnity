using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// The different algorithms to use for the undistortion of the images.
    /// </summary>
    public enum UndistortionType
    {
      Pinhole,
      OmnidirPerspective,
      OmnidirCylindrical,
      OmnidirLonglati,
      OmnidirStereographic
    }

    /// <summary>
    /// Capture and display every frame the images of any camera system with a fixed number of cameras to use for calibration or ArUco object
    /// tracking. Each camera of the system is associated with a Unity camera that shots as background the current captured frame.
    /// </summary>
    /// <remarks>If you want to use a custom physical camera not supported by Unity, you need to derive this class. See
    /// <see cref="ArucoCameraWebcam"/> as example. You will need to implement <see cref="StartCameras"/>, <see cref="StopCameras"/> and
    /// <see cref="Configure"/> and to set <see cref="ImageDatas"/> when <see cref="UpdateCameraImages"/> is called.</remarks>
    public abstract class ArucoCamera : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Start the cameras automatically after configured it. Call StartCameras() alternatively.")]
      private bool autoStart = true;

      [SerializeField]
      [Tooltip("Display automatically or not the camera images on screen.")]
      private bool displayImages = true;

      [SerializeField]
      [Tooltip("Undistort automatically each image according to the camera parameters.")]
      private bool autoUndistortWithCameraParameters = true;

      [SerializeField]
      [Tooltip("The algorithm to use for the undistortion of the images. Use `Pinhole` for standard pinhole cameras, and `OmnidirPerspective` for"
        + "fisheye cameras.")]
      private UndistortionType undistortionType = UndistortionType.Pinhole;

      // Events

      /// <summary>
      /// Executed when the camera system is configured.
      /// </summary>
      public event Action Configured = delegate { };

      /// <summary>
      /// Executed when the camera system starts.
      /// </summary>
      public event Action Started = delegate { };

      /// <summary>
      /// Executed when the camera system stops.
      /// </summary>
      public event Action Stopped = delegate { };

      /// <summary>
      /// Executed when the images has been updated.
      /// </summary>
      public event Action ImagesUpdated = delegate { };

      // Properties

      /// <summary>
      /// Start the camera system automatically after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// Display automatically the camera images on screen.
      /// </summary>
      public bool DisplayImages { get { return displayImages; } set { displayImages = value; } }

      /// <summary>
      /// Undistort automatically each image according to the camera parameters.
      /// </summary>
      public bool AutoUndistortWithCameraParameters { get { return autoUndistortWithCameraParameters; } set { autoUndistortWithCameraParameters = value; } }

      /// <summary>
      /// The number of cameras in the system.
      /// </summary>
      public abstract int CameraNumber { get; protected set; }

      /// <summary>
      /// The name of the camera system used.
      /// </summary>
      public abstract string Name { get; protected set; }

      /// <summary>
      /// True when the camera system is configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      /// <summary>
      /// True when the camera system is started.
      /// </summary>
      public bool IsStarted { get; protected set; }

      /// <summary>
      /// The algorithm to use for the undistortion of the images. Use `Pinhole` for standard pinhole cameras, and `OmnidirPerspective` for fisheye
      /// cameras.
      /// </summary>
      public UndistortionType UndistortionType { get { return undistortionType; } set { undistortionType = value; } }

      /// <summary>
      /// The images manipulated by OpenCV.
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
      /// The <see cref="Images"/> content.
      /// </summary>
      public byte[][] ImageDatas { get; protected set; }

      /// <summary>
      /// The size of each image in <see cref="ImageDatas"/>.
      /// </summary>
      public int[] ImageDataSizes { get; protected set; }

      /// <summary>
      /// The image textures used by Unity. They are updated at <see cref="LateUpdate"/> from the OpenCV <see cref="Images"/>.
      /// </summary>
      public Texture2D[] ImageTextures { get; protected set; }

      /// <summary>
      /// The parameters of each camera.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      /// <summary>
      /// The Unity camera components. There is one for each physical camera (<see cref="CameraNumber"/> cameras). If <see cref="DisplayImages"/> is
      /// set, the <see cref="ImageTextures"/> will be set as background of these Unity cameras.
      /// </summary>
      public Camera[] ImageCameras { get; protected set; }

      /// <summary>
      /// The image ratios.
      /// </summary>
      public virtual float[] ImageRatios { get; protected set; }

      // Variables

      protected Cv.Mat[] images;
      protected bool imagesUpdatedThisFrame = false;

      protected Cv.Mat[][] undistordedImageMaps;
      protected bool flipHorizontallyImages = false, 
                     flipVerticallyImages = false;
      protected int? preDetectflipCode, // Convert the images from Unity's left-handed coordinate system to OpenCV's right-handed coordinate system
                     postDetectflipCode; // Convert back the images

      protected Cv.Mat[] cameraMatricesSave, distCoeffsSave;

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
      /// Configure the cameras and their properties.
      /// </summary>
      public virtual void Configure()
      {
        // Configure the flip codes to transfer images from Unity to OpenCV and vice-versa
        // The raw bytes from a Texture to a Mat and from a Mat to a Texture needs to be vertically flipped to be in the correct orientation
        if (!flipHorizontallyImages && !flipVerticallyImages)
        {
          preDetectflipCode = 0; // Vertical flip
          postDetectflipCode = 0;
        }
        else if (flipHorizontallyImages && !flipVerticallyImages)
        {
          preDetectflipCode = 0;
          postDetectflipCode = -1; // Flip on both axis
        }
        else if (!flipHorizontallyImages && flipVerticallyImages)
        {
          preDetectflipCode = null; // Don't flip, texture image is already flipped
          postDetectflipCode = 0;
        }
        else if (flipHorizontallyImages && flipVerticallyImages)
        {
          preDetectflipCode = null;
          postDetectflipCode = -1;
        }

        // Initialize the properties and variables
        images = new Cv.Mat[CameraNumber];
        ImageDatas = new byte[CameraNumber][];
        ImageDataSizes = new int[CameraNumber];
        ImageCameras = new Camera[CameraNumber];
        ImageTextures = new Texture2D[CameraNumber];

        cameraMatricesSave = new Cv.Mat[CameraNumber];
        distCoeffsSave = new Cv.Mat[CameraNumber];

        if (CameraParameters != null)
        {
          undistordedImageMaps = new Cv.Mat[CameraNumber][];
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            undistordedImageMaps[cameraId] = new Cv.Mat[2]; // map1 and map2
          }
        }

        // Update state
        IsConfigured = true;
        OnConfigured();

        // AutoStart
        if (AutoStart)
        {
          StartCameras();
        }
      }

      /// <summary>
      /// Start the camera system.
      /// </summary>
      public abstract void StartCameras();

      /// <summary>
      /// Stop the camera system.
      /// </summary>
      public abstract void StopCameras();

      /// <summary>
      /// Undistort the images according to the <see cref="Utility.CameraParameters"/>, if not null. <see cref="Images"/> is immediatly updated. 
      /// <see cref="ImageTextures"/> will be updated at <see cref="LateUpdate"/> from <see cref="Images"/>. It's time-consuming but it'ss often
      /// necessary for a well-aligned AR.
      /// </summary>
      public virtual void Undistort()
      {
        for (int i = 0; i < CameraNumber; i++)
        {
          Cv.Remap(Images[i], Images[i], undistordedImageMaps[i][0], undistordedImageMaps[i][1], Cv.InterpolationFlags.Linear);
        }
      }

      /// <summary>
      /// Update <see cref="ImageTextures"/> if the camera system has started.
      /// </summary>
      protected abstract void UpdateCameraImages();

      /// <summary>
      /// Execute the <see cref="Started"/> action.
      /// </summary>
      protected void OnStarted()
      {
        // Save the CameraParameters property as the rectification in the undistortion process may alter it
        if (CameraParameters != null)
        {
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            cameraMatricesSave[cameraId] = new Cv.Mat(CameraParameters.CameraMatrices[cameraId].CppPtr);
            CameraParameters.CameraMatrices[cameraId].DeleteResponsibility = Utility.DeleteResponsibility.False;

            distCoeffsSave[cameraId] = new Cv.Mat(CameraParameters.DistCoeffs[cameraId].CppPtr);
            CameraParameters.DistCoeffs[cameraId].DeleteResponsibility = Utility.DeleteResponsibility.False;
          }
        }

        InitializeMatImages();

        IsStarted = true;
        Started();
      }

      /// <summary>
      /// Execute the <see cref="Stopped"/> action.
      /// </summary>
      protected void OnStopped()
      {
        IsStarted = false;

        // Restore the CameraParameters property to its original state
        if (CameraParameters != null)
        {
          CameraParameters.CameraMatrices = cameraMatricesSave;
          CameraParameters.DistCoeffs = distCoeffsSave;
        }

        Stopped();
      }

      /// <summary>
      /// Execute the <see cref="Configured"/> action.
      /// </summary>
      protected void OnConfigured()
      {
        Configured();
      }

      /// <summary>
      /// Undistort the <see cref="Images"/> if required and execute the <see cref="ImagesUpdated"/> action.
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

        // Undistort the images if required
        if (AutoUndistortWithCameraParameters && CameraParameters != null)
        {
          Undistort();
        }

        imagesUpdatedThisFrame = true;
        ImagesUpdated();
      }

      /// <summary>
      /// Returns the OpenCV type equivalent to the format of the texture.
      /// </summary>
      /// <param name="textureFormat">The Unity texture format.</param>
      /// <returns>The equivalent OpenCV type.</returns>
      public Cv.Type ImageType(TextureFormat textureFormat)
      {
        Cv.Type type;
        switch (textureFormat)
        {
          case TextureFormat.RGB24:
            type = Cv.Type.CV_8UC3;
            break;
          case TextureFormat.BGRA32:
          case TextureFormat.ARGB32:
          case TextureFormat.RGBA32:
            type = Cv.Type.CV_8UC4;
            break;
          default:
            throw new ArgumentException("This type of texture is actually not supported: " + textureFormat + ".", "textureFormat");
        }
        return type;
      }

      /// <summary>
      /// Initialize the <see cref="Images"/> property, the undistortion maps and the undistorded images.
      /// </summary>
      protected void InitializeMatImages()
      {
        // Initialize the images
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          Images[cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, ImageType(ImageTextures[cameraId].format));
          ImageDataSizes[cameraId] = (int)(Images[cameraId].ElemSize() * Images[cameraId].Total());
          ImageDatas[cameraId] = new byte[ImageDataSizes[cameraId]];
          Images[cameraId].DataByte = ImageDatas[cameraId];
        }

        // Initialize the undistorted images
        if (CameraParameters != null)
        {
          // TODO: scale if there is a difference between camera image size and camera parameters image size (during calibration)

          // Set the undistortion maps for the cameras with stereo calibration results first
          List<int> monoCameraIds = Enumerable.Range(0, CameraNumber).ToList();
          foreach (var stereoCameraParameters in CameraParameters.StereoCameraParametersList)
          {
            for (int i = 0; i < StereoCameraParameters.CameraNumber; i++)
            {
              int cameraId = stereoCameraParameters.CameraIds[i];
              InitUndistortRectifyMap(cameraId, stereoCameraParameters.RotationMatrices[i], stereoCameraParameters.NewCameraMatrices[i]);
              monoCameraIds.Remove(stereoCameraParameters.CameraIds[i]);
            }
          }

          // Set the undistortion maps for the cameras not in a stereo pair
          Cv.Mat monoCameraRectification = new Cv.Mat();
          foreach (int cameraId in monoCameraIds)
          {
            InitUndistortRectifyMap(cameraId, monoCameraRectification, CameraParameters.CameraMatrices[cameraId]);
          }
        }
      }

      private void InitUndistortRectifyMap(int cameraId, Cv.Mat rotationMatrix, Cv.Mat newCameraMatrix)
      {
        // Init the undistort rectify maps
        if (UndistortionType == UndistortionType.Pinhole)
        {
          Cv.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId], rotationMatrix,
            newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2, out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1]);
        }
        else if (new[] { UndistortionType.OmnidirPerspective, UndistortionType.OmnidirCylindrical, UndistortionType.OmnidirLonglati,
          UndistortionType.OmnidirStereographic }.Contains(UndistortionType))
        {
          Cv.Omnidir.Rectifify flags = Cv.Omnidir.Rectifify.Perspective;
          if      (UndistortionType == UndistortionType.OmnidirCylindrical)   { flags = Cv.Omnidir.Rectifify.Cylindrical; }
          else if (UndistortionType == UndistortionType.OmnidirLonglati)      { flags = Cv.Omnidir.Rectifify.Longlati; }
          else if (UndistortionType == UndistortionType.OmnidirStereographic) { flags = Cv.Omnidir.Rectifify.Stereographic; }

          // If no newCameraMatrix, inititalize it with the recommended values
          if (newCameraMatrix.Total() == 0)
          {
            double width = ImageTextures[cameraId].width, height = ImageTextures[cameraId].height;
            if (flags == Cv.Omnidir.Rectifify.Perspective)
            {
              newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { width / 2, 0, width / 2, 0, height / 2, height / 2, 0, 0, 1 }).Clone();
            }
            else
            {
              newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { width / 3.1415, 0, 0, 0, height / 3.1415, 0, 0, 0, 1 }).Clone();
            }
          }

          Cv.Omnidir.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
            CameraParameters.OmnidirXis[cameraId], rotationMatrix, newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2,
            out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1], flags);
        }
        else
        {
          throw new Exception("Unable to initialize the undistort rectify maps with this UndistortionType: " + UndistortionType);
        }

        // Update camera intrinsic parameters for the undistorted images
        CameraParameters.CameraMatrices[cameraId] = newCameraMatrix;
        CameraParameters.DistCoeffs[cameraId] = new Cv.Mat();
      }
    }
  }

  /// \} aruco_unity_package
}