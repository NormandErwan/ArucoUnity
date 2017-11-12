using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.Utility;
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
    /// <remarks>
    /// If you want to use a custom physical camera not supported by Unity, you need to derive this class. See
    /// <see cref="ArucoCameraWebcam"/> as example. You will need to implement <see cref="StartCameras"/>, <see cref="StopCameras"/>,
    /// <see cref="Configure"/> and to set <see cref="ImageDatas"/> when <see cref="UpdateCameraImages"/> is called.
    /// Also read about the calibration and undistortion processes with OpenCV : https://docs.opencv.org/3.3.0/d9/d0c/group__calib3d.html,
    /// https://docs.opencv.org/3.3.0/d4/d94/tutorial_camera_calibration.html, https://docs.opencv.org/3.3.0/dd/d12/tutorial_omnidir_calib_main.html,
    /// https://docs.opencv.org/3.3.0/da/d54/group__imgproc__transform.html#ga7dfb72c9cf9780a347fbe3d1c47e5d5a
    /// </remarks>
    public abstract class ArucoCamera : MonoBehaviour
    {
      // Constants

      public const float defaultCameraBackgroundDistance = 1f;

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

      // Properties

      /// <summary>
      /// Gets or sets if automatically start the camera system after configured it.
      /// </summary>
      public bool AutoStart { get { return autoStart; } set { autoStart = value; } }

      /// <summary>
      /// Gets or sets if <see cref="ImageCameraBackgrounds"/> are actives.
      /// </summary>
      public bool DisplayImages { get { return displayImages; } set { displayImages = value; } }

      /// <summary>
      /// Gets or sets if automatically undistort each image according to the camera parameters.
      /// </summary>
      public bool AutoUndistortWithCameraParameters { get { return autoUndistortWithCameraParameters; } set { autoUndistortWithCameraParameters = value; } }

      /// <summary>
      /// Gets the algorithm to use for the undistortion of the images : `Pinhole` for standard pinhole cameras, `OmnidirPerspective` for fisheye and
      /// omnidirectional cameras.
      /// </summary>
      public UndistortionType UndistortionType { get { return undistortionType; } set { undistortionType = value; } }

      /// <summary>
      /// Gets the number of cameras in the system.
      /// </summary>
      public abstract int CameraNumber { get; protected set; }

      /// <summary>
      /// Gets the name of the camera system used.
      /// </summary>
      public abstract string Name { get; protected set; }

      /// <summary>
      /// Gets if the camera system is configured.
      /// </summary>
      public bool IsConfigured { get; protected set; }

      /// <summary>
      /// Gets if the camera system is started.
      /// </summary>
      public bool IsStarted { get; protected set; }

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
      /// Gets the the current images frame manipulated by Unity. They are updated at <see cref="LateUpdate"/> from the OpenCV <see cref="Images"/>.
      /// </summary>
      public Texture2D[] ImageTextures { get; protected set; }

      /// <summary>
      /// Gets the parameters of each camera from the calibration process.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      /// <summary>
      /// Gets the Unity virtual cameras. There is one for each physical camera (<see cref="CameraNumber"/> cameras). If <see cref="DisplayImages"/>
      /// is set, the <see cref="ImageTextures"/> will be set as background of these Unity cameras.
      /// </summary>
      public Camera[] ImageCameras { get; protected set; }

      /// <summary>
      /// Gets the <see cref="Images"/> ratios.
      /// </summary>
      public virtual float[] ImageRatios { get; protected set; }

      /// <summary>
      /// Gets the planes displaying the <see cref="Images"/> as background of the <see cref="ImageCameras"/>.
      /// </summary>
      public GameObject[] ImageCameraBackgrounds { get; protected set; }

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
      /// Configure the cameras and the properties, then calls <see cref="StartCameras"/> if <see cref="AutoStart"/> is true.
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
        ImageTextures = new Texture2D[CameraNumber];
        ImageRatios = new float[CameraNumber];
        ImageCameras = new Camera[CameraNumber];
        ImageCameraBackgrounds = new GameObject[CameraNumber];

        cameraMatricesSave = new Cv.Mat[CameraNumber];
        distCoeffsSave = new Cv.Mat[CameraNumber];

        if (CameraParameters != null)
        {
          undistordedImageMaps = new Cv.Mat[CameraNumber][];
          for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
          {
            undistordedImageMaps[cameraId] = new Cv.Mat[2]; // 2 maps: horizontal map and vertical map
          }
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
      /// Calls <see cref="InitializeImages"/>, <see cref="ConfigureUndistortion"/>, saves the state of <see cref="CameraParameters.CameraMatrices"/>
      /// and <see cref="CameraParameters.DistCoeffs"/>, and executes the <see cref="Started"/> action.
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

        // Configure images, undistortion of the images, cameras and camera backgrounds
        InitializeImages();
        if (CameraParameters != null)
        {
          ConfigureUndistortion();
          ConfigureCameras();
        }
        ConfigureCameraBackgrounds();

        // Execute started event
        IsStarted = true;
        Started();
      }

      /// <summary>
      /// Restores <see cref="CameraParameters.CameraMatrices"/> and <see cref="CameraParameters.DistCoeffs"/> to their original state before
      /// starting the cameras, deactivate <see cref="ImageCameraBackgrounds"/> and executes the <see cref="Stopped"/> action.
      /// </summary>
      protected void OnStopped()
      {
        // Deactivate backgrounds
        foreach (var cameraBackground in ImageCameraBackgrounds)
        {
          if (cameraBackground != null)
          {
            cameraBackground.SetActive(DisplayImages);
          }
        }

        // Restore camera parameters
        if (CameraParameters != null)
        {
          CameraParameters.CameraMatrices = cameraMatricesSave;
          CameraParameters.DistCoeffs = distCoeffsSave;
        }

        // Execute stopped event
        IsStarted = false;
        Stopped();
      }

      /// <summary>
      /// Updates <see cref="ImageDatas"/> with the current frame images and calls <see cref="OnImagesUpdated"/>.
      /// </summary>
      protected abstract void UpdateCameraImages();

      /// <summary>
      /// Undistorts the <see cref="Images"/> if <see cref="AutoUndistortWithCameraParameters"/> is true, and execute the <see cref="ImagesUpdated"/>
      /// action.
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
      /// Initializes the <see cref="Images"/>, <see cref="ImageDataSizes"/> and <see cref="ImageDatas"/> properties from the <see cref="ImageTextures"/>
      /// property.
      /// </summary>
      protected virtual void InitializeImages()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          Images[cameraId] = new Cv.Mat(ImageTextures[cameraId].height, ImageTextures[cameraId].width, CvMatUtility.ImageType(ImageTextures[cameraId].format));
          ImageDataSizes[cameraId] = (int)(Images[cameraId].ElemSize() * Images[cameraId].Total());
          ImageDatas[cameraId] = new byte[ImageDataSizes[cameraId]];
          Images[cameraId].DataByte = ImageDatas[cameraId];
          ImageRatios[cameraId] = ImageTextures[cameraId].width / ImageTextures[cameraId].height;
        }
      }

      /// <summary>
      /// Initializes the undistortions of all <see cref="Images"/> : calls <see cref="ConfigureUndistortion(int, Cv.Mat, Cv.Mat)"/> for camera
      /// calibrated in a stereo pair, otherwise calls <see cref="ConfigureUndistortion(int)"/>.
      /// </summary>
      // TODO: scale if there is a difference between camera image size and camera parameters image size (during calibration)
      protected virtual void ConfigureUndistortion()
      {
        // Set the undistortion maps for the cameras with stereo calibration results first
        List<int> monoCameraIds = Enumerable.Range(0, CameraNumber).ToList();
        foreach (var stereoCameraParameters in CameraParameters.StereoCameraParametersList)
        {
          for (int i = 0; i < StereoCameraParameters.CameraNumber; i++)
          {
            int cameraId = stereoCameraParameters.CameraIds[i];
            ConfigureUndistortion(cameraId, stereoCameraParameters.RotationMatrices[i], stereoCameraParameters.NewCameraMatrices[i]);
            monoCameraIds.Remove(cameraId);
          }
        }

        // Set the undistortion maps for cameras not in a stereo pair
        foreach (int cameraId in monoCameraIds)
        {
          ConfigureUndistortion(cameraId);
        }
      }

      /// <summary>
      /// Updates field of view and aspect ratio of the <see cref="ImageCameras"/> according to the <see cref="CameraParameters"/> and the
      /// <see cref="Images"/>.
      /// </summary>
      protected virtual void ConfigureCameras()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          Vector2 cameraF = CameraParameters.GetCameraFocalLengths(cameraId);
          float fovY = 2f * Mathf.Atan(0.5f * CameraParameters.ImageHeights[cameraId] / cameraF.y) * Mathf.Rad2Deg;
          ImageCameras[cameraId].fieldOfView = fovY;
        }
      }

      /// <summary>
      /// Calls <see cref="ConfigureUndistortion(int, Cv.Mat, Cv.Mat)"/> with no rectification and current corresponding camera matrix in 
      /// <see cref="CameraParameters.CameraMatrices"/> as new camera matrix.
      /// </summary>
      /// <param name="cameraId">The camera to use.</param>
      protected virtual void ConfigureUndistortion(int cameraId)
      {
        var noRectification = new Cv.Mat();
        var defaultCameraMatrix = CameraParameters.CameraMatrices[cameraId];
        ConfigureUndistortion(cameraId, noRectification, defaultCameraMatrix);
      }

      /// <summary>
      /// Initializes the undistortion for the camera image, and updates the camera matrix <see cref="CameraParameters.CameraMatrices"/>
      /// and distorsion parameters <see cref="CameraParameters.DistCoeffs"/> corresponding to the recitified camera image.
      /// </summary>
      /// <param name="cameraId">The camera to use.</param>
      /// <param name="rectificationMatrix">Optional rectification matrix.</param>
      /// <param name="newCameraMatrix">The desired new camera matrix, can be computed by a stereo rectification during calibration.</param>
      protected virtual void ConfigureUndistortion(int cameraId, Cv.Mat rectificationMatrix, Cv.Mat newCameraMatrix)
      {
        // Init the undistort rectify maps for pinhole camera
        if (UndistortionType == UndistortionType.Pinhole)
        {
          Cv.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId], rectificationMatrix,
            newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2, out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1]);
        }

        // Init the undistort rectify maps for omnidir camera
        else if (UndistortionType == UndistortionType.OmnidirPerspective || UndistortionType == UndistortionType.OmnidirCylindrical
          || UndistortionType == UndistortionType.OmnidirLonglati || UndistortionType == UndistortionType.OmnidirStereographic)
        {
          Cv.Omnidir.Rectifify flags = Cv.Omnidir.Rectifify.Perspective;
          if      (UndistortionType == UndistortionType.OmnidirCylindrical)   { flags = Cv.Omnidir.Rectifify.Cylindrical; }
          else if (UndistortionType == UndistortionType.OmnidirLonglati)      { flags = Cv.Omnidir.Rectifify.Longlati; }
          else if (UndistortionType == UndistortionType.OmnidirStereographic) { flags = Cv.Omnidir.Rectifify.Stereographic; }

          // If no newCameraMatrix, inititalize it with the recommended values by this tutorial : https://docs.opencv.org/3.3.1/dd/d12/tutorial_omnidir_calib_main.html
          if (newCameraMatrix.Total() == 0)
          {
            double width = ImageTextures[cameraId].width, height = ImageTextures[cameraId].height;
            if (flags == Cv.Omnidir.Rectifify.Perspective)
            {
              newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { width / 4, 0, width / 2, 0, height / 4, height / 2, 0, 0, 1 }).Clone();
            }
            else
            {
              newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { width / 3.1415, 0, 0, 0, height / 3.1415, 0, 0, 0, 1 }).Clone();
            }
          }

          Cv.Omnidir.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
            CameraParameters.OmnidirXis[cameraId], rectificationMatrix, newCameraMatrix, Images[cameraId].Size, Cv.Type.CV_16SC2,
            out undistordedImageMaps[cameraId][0], out undistordedImageMaps[cameraId][1], flags);
        }

        // Update camera intrinsic parameters for the undistorted images
        CameraParameters.CameraMatrices[cameraId] = newCameraMatrix;
        CameraParameters.DistCoeffs[cameraId] = new Cv.Mat();
      }

      /// <summary>
      /// Configures the <see cref="ImageCameraBackgrounds"/> facing the cameras, according to the <see cref="CameraParameters"/> if set, otherwise
      /// with default values.
      /// </summary>
      /// <param name="cameraBackgroundDistance">The camera-background distance.</param>
      // TODO: handle case of CameraParameters.ImageHeight != ImageTexture.height or CameraParameters.ImageWidth != ImageTexture.width
      // TODO: handle case of CameraParameters.FixAspectRatio != 0
      protected virtual void ConfigureCameraBackgrounds(float cameraBackgroundDistance = defaultCameraBackgroundDistance)
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          // Configure the background
          if (ImageCameraBackgrounds[cameraId] == null)
          {
            ImageCameraBackgrounds[cameraId] = GameObject.CreatePrimitive(PrimitiveType.Quad);
            ImageCameraBackgrounds[cameraId].name = "CameraBackground";
            ImageCameraBackgrounds[cameraId].transform.SetParent(ImageCameras[cameraId].transform);
            ImageCameraBackgrounds[cameraId].transform.localRotation = Quaternion.identity;

            var cameraBackgroundRenderer = ImageCameraBackgrounds[cameraId].GetComponent<Renderer>();
            cameraBackgroundRenderer.material = Resources.Load("UnlitImage") as Material;
            cameraBackgroundRenderer.material.mainTexture = ImageTextures[cameraId];
          }
          ImageCameraBackgrounds[cameraId].SetActive(DisplayImages);

          // Place background
          Vector2 position = Vector2.zero;
          Vector2 scale = Vector2.one;
          if (CameraParameters != null)
          {
            float imageWidth = CameraParameters.ImageWidths[cameraId];
            float imageHeight = CameraParameters.ImageHeights[cameraId];
            Vector2 cameraF = CameraParameters.GetCameraFocalLengths(cameraId);
            Vector2 cameraC = CameraParameters.GetCameraPrincipalPoint(cameraId);

            // Considering https://docs.opencv.org/3.3.0/d4/d94/tutorial_camera_calibration.html, we are looking for X=posX and Y=posY
            // with x=0.5*ImageWidth, y=0.5*ImageHeight (center of the camera projection) and w=Z=cameraBackgroundDistance 
            position.x = (0.5f * imageWidth - cameraC.x) / cameraF.x * cameraBackgroundDistance;
            position.y = -(0.5f * imageHeight - cameraC.y) / cameraF.y * cameraBackgroundDistance; // OpenCV(u = 0, v = 0) camera coordinates origin is top - left, but bottom-left in Unity

            // Considering https://stackoverflow.com/a/41137160
            // scale.x = 2 * cameraBackgroundDistance * tan(fovx / 2), fx = imageWidth / (2 * tan(fovx / 2)) = cameraF.x
            scale.x = imageWidth / cameraF.x * cameraBackgroundDistance;
            scale.y = imageHeight / cameraF.y * cameraBackgroundDistance;
          }
          else
          {
            // Default : place background centered on the camera and full size of the camera image
            float aspectRatioFactor = Mathf.Min(ImageRatios[cameraId], 1f);
            scale.y = 2 * cameraBackgroundDistance * aspectRatioFactor * Mathf.Tan(0.5f * ImageCameras[cameraId].fieldOfView * Mathf.Deg2Rad);
            scale.x = scale.y * ImageRatios[cameraId];
          }

          ImageCameraBackgrounds[cameraId].transform.localPosition = new Vector3(position.x, position.y, cameraBackgroundDistance);
          ImageCameraBackgrounds[cameraId].transform.localScale = new Vector3(scale.x, scale.y, 1);
        }
      }

      /// <summary>
      /// Undistorts the camera images. <see cref="Images"/> is immediatly updated. <see cref="ImageTextures"/> will be updated at
      /// <see cref="LateUpdate"/> from <see cref="Images"/>. It's time-consuming but it's often necessary for a well-aligned AR.
      /// </summary>
      protected virtual void Undistort()
      {
        for (int i = 0; i < CameraNumber; i++)
        {
          Cv.Remap(Images[i], Images[i], undistordedImageMaps[i][0], undistordedImageMaps[i][1], Cv.InterpolationFlags.Linear);
        }
      }
    }
  }

  /// \} aruco_unity_package
}