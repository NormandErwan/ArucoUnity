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
    public enum UndistortionType
    {
      Pinhole,
      Fisheye,
      OmnidirPerspective,
      OmnidirCylindrical,
      OmnidirLonglati,
      OmnidirStereographic
    }

    /// <summary>
    /// Manages to retrieve and display every frame the images of any system with a fixed number of cameras to use with ArucoUnity.
    /// </summary>
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
      [Tooltip("The algorithm to use for the undistortion of the images: pinhole camera (default, calib3d module), fisheye (calib3d module) or omnidir (ccalib module).")]
      private UndistortionType undistortionType = Cameras.UndistortionType.Pinhole;

      // Events

      public delegate void CameraEventHandler();

      /// <summary>
      /// Executed when the camera system is configured.
      /// </summary>
      public event CameraEventHandler Configured = delegate { };

      /// <summary>
      /// Executed when the camera system starts.
      /// </summary>
      public event CameraEventHandler Started = delegate { };

      /// <summary>
      /// Executed when the camera system stops.
      /// </summary>
      public event CameraEventHandler Stopped = delegate { };

      /// <summary>
      /// Executed when the images has been updated.
      /// </summary>
      public event CameraEventHandler ImagesUpdated = delegate { };

      // Properties

      /// <summary>
      /// Start the camera system automatically after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// Display automatically or not the camera images on screen.
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
      /// The algorithm to use for the undistortion of the images: pinhole camera (default, calib3d module), fisheye (calib3d module) or omnidir
      /// (ccalib module).
      /// </summary>
      public UndistortionType UndistortionType { get { return undistortionType; } set { undistortionType = value; } }

      /// <summary>
      /// The images in a OpenCV format. When getting the property, a new Cv.Mat is created for each image from the corresponding 
      /// <see cref="ImageTextures"/> content. When setting, the <see cref="ImageTextures"/> content is updated for each image from the Cv.Mat array.
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
      /// Image textures, updated each frame.
      /// </summary>
      public Texture2D[] ImageTextures { get; protected set; }

      /// <summary>
      /// The parameters of each camera.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      /// <summary>
      /// The Unity camera components that will capture the <see cref="ImageTextures"/>.
      /// </summary>
      public Camera[] ImageCameras { get; protected set; }

      /// <summary>
      /// The image ratios.
      /// </summary>
      public virtual float[] ImageRatios { get; protected set; }

      /// <summary>
      /// The recommended mesh plane to display the <see cref="ImageTextures"/>.
      /// </summary>
      public virtual Mesh[] ImageMeshes { get; protected set; }

      // Variables

      protected Cv.Mat[] images;
      protected int[] imageDataSizes;
      protected Cv.Mat[][] undistordedImageMaps;
      protected bool flipHorizontallyImages = false, 
                     flipVerticallyImages = false;
      protected int? preDetectflipCode, // Convert the images from Unity's left-handed coordinate system to OpenCV's right-handed coordinate system
                     postDetectflipCode; // Convert back the images
      protected Cv.Mat[] cameraMatricesSave, distCoeffsSave;

      // MonoBehaviour methods

      /// <summary>
      /// Initialize camera system state.
      /// </summary>
      protected virtual void Awake()
      {
        IsConfigured = false;
        IsStarted = false;
      }

      /// <summary>
      /// Configure the camera at start if <see cref="AutoStart"/> is true.
      /// </summary>
      protected virtual void Start()
      {
        if (AutoStart)
        {
          Configure();
        }
      }

      /// <summary>
      /// Reset <see cref="Images"/> and retrieve the new images for this frame.
      /// </summary>
      protected virtual void Update()
      {
        if (IsConfigured && IsStarted)
        {
          UpdateCameraImages();
        }
      }

      /// <summary>
      /// Apply the changes made on the <see cref="Images"/> during the frame to the <see cref="ImageTextures"/>.
      /// </summary>
      protected virtual void LateUpdate()
      {
        if (!IsConfigured || !IsStarted)
        {
          return;
        }

        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          // Flip the Images if needed, load them back to the textures and revert the flip to keep the correct orientation on both image and texture
          if (postDetectflipCode != null)
          {
            Cv.Flip(Images[cameraId], Images[cameraId], (int)postDetectflipCode);
          }
          ImageTextures[cameraId].LoadRawTextureData(Images[cameraId].DataIntPtr, imageDataSizes[cameraId]);
          ImageTextures[cameraId].Apply(false);
          if (postDetectflipCode != null)
          {
            Cv.Flip(Images[cameraId], Images[cameraId], (int)postDetectflipCode);
          }
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

        // Initialize the variables
        images = new Cv.Mat[CameraNumber];
        imageDataSizes = new int[CameraNumber];
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
      /// <see cref="ImageTextures"/> will be updated at LateUpdate().
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
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          cameraMatricesSave[cameraId] = new Cv.Mat(CameraParameters.CameraMatrices[cameraId].CppPtr);
          CameraParameters.CameraMatrices[cameraId].DeleteResponsibility = Utility.DeleteResponsibility.False;

          distCoeffsSave[cameraId] = new Cv.Mat(CameraParameters.DistCoeffs[cameraId].CppPtr);
          CameraParameters.DistCoeffs[cameraId].DeleteResponsibility = Utility.DeleteResponsibility.False;
        }

        // Initialize and execute the action
        InitializeMatImages();
        Started();
      }

      /// <summary>
      /// Execute the <see cref="Stopped"/> action.
      /// </summary>
      protected void OnStopped()
      {
        // Execute the action
        Stopped();

        // Restore the CameraParameters property to its original state
        CameraParameters.CameraMatrices = cameraMatricesSave;
        CameraParameters.DistCoeffs = distCoeffsSave;
      }

      /// <summary>
      /// Execute the <see cref="Configured"/> action.
      /// </summary>
      protected void OnConfigured()
      {
        Configured();
      }

      /// <summary>
      /// Update the <see cref="Images"/> property from the <see cref="ImageTextures"/> property.
      /// </summary>
      protected void OnImagesUpdated()
      {
        // Load the texture contents to the images, then flip the images if needed
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          images[cameraId].DataByte = ImageTextures[cameraId].GetRawTextureData();
          if (preDetectflipCode != null)
          {
            Cv.Flip(Images[cameraId], Images[cameraId], (int)preDetectflipCode);
          }
        }

        // Undistort the images if required
        if (AutoUndistortWithCameraParameters && CameraParameters != null)
        {
          Undistort();
        }

        ImagesUpdated();
      }

      /// <summary>
      /// Returns the OpenCV type equivalent to the format of the texture.
      /// </summary>
      /// <param name="imageTexture">The texture to analyze.</param>
      /// <returns>The equivalent OpenCV type.</returns>
      protected Cv.Type ImageType(Texture2D imageTexture)
      {
        Cv.Type type;
        var format = imageTexture.format;
        switch (format)
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
            throw new ArgumentException("This type of texture is actually not supported: " + imageTexture.format + ".", "imageTexture");
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
          byte[] imageData = ImageTextures[cameraId].GetRawTextureData();
          images[cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, ImageType(ImageTextures[cameraId]),
            imageData);
          imageDataSizes[cameraId] = (int)(images[cameraId].ElemSize() * images[cameraId].Total());
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
        if (UndistortionType == UndistortionType.Pinhole)
        {
          Cv.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId], rotationMatrix,
            newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2, out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1]);
        }
        else if (UndistortionType == UndistortionType.Fisheye)
        {
          Cv.Fisheye.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId], rotationMatrix,
            newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2, out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1]);
        }
        else if (new[] { UndistortionType.OmnidirPerspective, UndistortionType.OmnidirCylindrical, UndistortionType.OmnidirLonglati, UndistortionType.OmnidirStereographic }.Contains(UndistortionType))
        {
          Cv.Omnidir.Rectifify flags = Cv.Omnidir.Rectifify.Perspective;
          if      (UndistortionType == UndistortionType.OmnidirCylindrical)   { flags = Cv.Omnidir.Rectifify.Cylindrical; }
          else if (UndistortionType == UndistortionType.OmnidirLonglati)      { flags = Cv.Omnidir.Rectifify.Longlati; }
          else if (UndistortionType == UndistortionType.OmnidirStereographic) { flags = Cv.Omnidir.Rectifify.Stereographic; }

          if (newCameraMatrix.Total() == 0)
          {
            double width = ImageTextures[cameraId].width, height = ImageTextures[cameraId].height;
            if (flags == Cv.Omnidir.Rectifify.Perspective)
            {
              newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { width / 2, 0, width / 2, 0, height / 2, height / 2, 0, 0, 1 });
            }
            else
            {
              newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { width / 3.1415, 0, 0, 0, height / 3.1415, 0, 0, 0, 1 });
            }
          }

          Cv.Omnidir.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
            CameraParameters.OmnidirXis[cameraId], rotationMatrix, newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2,
            out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1], flags);

          CameraParameters.CameraMatrices[cameraId] = newCameraMatrix;
          CameraParameters.DistCoeffs[cameraId] = new Cv.Mat();
        }
      }
    }
  }

  /// \} aruco_unity_package
}