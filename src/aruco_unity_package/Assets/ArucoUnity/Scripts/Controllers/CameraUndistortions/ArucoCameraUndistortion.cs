using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using UnityEngine;
using System;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the processes of undistortion and rectification of <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public abstract class ArucoCameraUndistortion : ArucoCameraController<ArucoCamera>
    {
      // Constants

      public const int undistortionCameraMapsNumber = 2;

      // Editor fields

      [SerializeField]
      [Tooltip("The camera parameters to use.")]
      private CameraParametersController cameraParametersController;

      // Properties

      /// <summary>
      /// Gets or sets the camera parameters to use.
      /// </summary>
      public CameraParametersController CameraParametersController { get { return cameraParametersController; } set { cameraParametersController = value; } }

      /// <summary>
      /// Gets the camera matrices of the undistorted and rectified images of each camera.
      /// </summary>
      public Cv.Mat[] RectifiedCameraMatrices { get; protected set; }

      /// <summary>
      /// Gets the distorsion coefficients of the undistorted and rectified images of each camera.
      /// </summary>
      public Cv.Mat UndistortedDistCoeffs { get { return noDistCoeffs; } }

      /// <summary>
      /// Gets the undistortion and rectification images transformation map of each camera (two maps per camera).
      /// </summary>
      public Cv.Mat[][] UndistortionRectificationMaps { get; protected set; }

      // Variables

      protected Cv.Mat noRectificationMatrix;
      protected Cv.Mat noDistCoeffs;
      protected string cameraParametersFilePath;

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();
        noRectificationMatrix = new Cv.Mat();
        noDistCoeffs = new Cv.Mat();
      }

      // ArucoCameraController methods

      /// <summary>
      /// Susbcribes from <see cref="ArucoCamera.UndistortRectifyImages"/>.
      /// </summary>
      public override void StartController()
      {
        base.StartController();
        ArucoCamera.UndistortRectifyImages += ArucoCamera_UndistortRectifyImages;
      }

      /// <summary>
      /// Unsusbcribes from <see cref="ArucoCamera.UndistortRectifyImages"/>.
      /// </summary>
      public override void StopController()
      {
        base.StopController();
        ArucoCamera.UndistortRectifyImages -= ArucoCamera_UndistortRectifyImages;
      }

      /// <summary>
      /// Checks if <see cref="CameraParameters.CameraNumber"/> == <see cref="ArucoCamera.CameraNumber"/>, calls
      /// <see cref="InitializeUndistortionRectification(Cv.Mat[], Camera[])"/>.
      /// </summary>
      protected override void Configure()
      {
        if (CameraParametersController.CameraParameters.CameraNumber != ArucoCamera.CameraNumber)
        {
          throw new Exception("The number of cameras in CameraParameters must be equal to the number of cameras in ArucoCamera");
        }
        if (ArucoCamera is StereoArucoCamera && CameraParametersController.CameraParameters.StereoCameraParameters == null)
        {
          throw new Exception("The camera parameters must contains a valid StereoCameraParameters to undistort and rectify a StereoArucoCamera.");
        }

        InitializeUndistortionRectification();
      }

      // Methods

      /// <summary>
      /// Initializes the undistortion and rectification of each camera image, sets the rectified camera matrices to 
      /// <see cref="RectifiedCameraMatrices"/> and the undistorted distorsion coefficients to <see cref="UndistortedDistCoeffs"/>.
      /// </summary>
      // TODO: scale if there is a difference between camera image size and camera parameters image size (during calibration)
      protected virtual void InitializeUndistortionRectification()
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        var stereoCameraParameters = cameraParameters.StereoCameraParameters;

        // Initialize the undistortion maps and rectified camera matrices
        RectifiedCameraMatrices = new Cv.Mat[cameraParameters.CameraNumber];
        UndistortionRectificationMaps = new Cv.Mat[cameraParameters.CameraNumber][];
        for (int cameraId = 0; cameraId < cameraParameters.CameraNumber; cameraId++)
        {
          RectifiedCameraMatrices[cameraId] = cameraParameters.CameraMatrices[cameraId].Clone();
          UndistortionRectificationMaps[cameraId] = new Cv.Mat[undistortionCameraMapsNumber];
        }

        // Configure the undistortion maps
        for (int cameraId = 0; cameraId < cameraParameters.CameraNumber; cameraId++)
        {
          if (stereoCameraParameters != null)
          {
            ConfigureUndistortionRectification(cameraId, stereoCameraParameters.RotationMatrices[cameraId],
              stereoCameraParameters.NewCameraMatrices[cameraId]);
          }
          else
          {
            ConfigureUndistortionRectification(cameraId, noRectificationMatrix, cameraParameters.CameraMatrices[cameraId]);
          }
        }
      }

      protected abstract void ConfigureUndistortionRectification(int cameraId, Cv.Mat rectificationMatrix, Cv.Mat newCameraMatrix);

      /// <summary>
      /// Undistorts and rectifies the <see cref="ArucoCamera.Images"/> using <see cref="UndistortionRectificationMaps"/>. It's a time-consuming
      /// operation but it's necessary for cameras with an important distorsion for a good alignement of the images with the 3D content.
      /// </summary>
      protected virtual void ArucoCamera_UndistortRectifyImages()
      {
        for (int cameraId = 0; cameraId < CameraParametersController.CameraParameters.CameraNumber; cameraId++)
        {
          Cv.Remap(ArucoCamera.Images[cameraId], ArucoCamera.Images[cameraId], UndistortionRectificationMaps[cameraId][0],
            UndistortionRectificationMaps[cameraId][1], Cv.InterpolationFlags.Linear);
        }
      }
    }
  }

  /// \} aruco_unity_package
}