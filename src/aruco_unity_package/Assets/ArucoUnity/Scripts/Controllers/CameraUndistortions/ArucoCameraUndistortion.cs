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
    public abstract class ArucoCameraUndistortion : ArucoCameraController<ArucoCamera>, IArucoCameraUndistortion
    {
      // Constants

      public const int undistortionCameraMapsNumber = 2;

      // Editor fields

      [SerializeField]
      [Tooltip("The camera parameters to use.")]
      private CameraParametersController cameraParametersController;

      // IArucoCameraUndistortion properties

      public CameraParametersController CameraParametersController { get { return cameraParametersController; } set { cameraParametersController = value; } }
      public Cv.Mat[] RectifiedCameraMatrices { get; protected set; }
      public Cv.Mat[] RectificationMatrices { get; protected set; }
      public Cv.Mat UndistortedDistCoeffs { get { return noDistCoeffs; } }
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
      /// Checks if <see cref="CameraParameters.CameraNumber"/> and <see cref="ArucoCamera.CameraNumber"/> are equals and calls
      /// <see cref="InitializeUndistortionRectification(Cv.Mat[], Camera[])"/>.
      /// </summary>
      public override void Configure()
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
        OnConfigured();
      }

      /// <summary>
      /// Susbcribes to <see cref="ArucoCamera.UndistortRectifyImages"/>.
      /// </summary>
      public override void StartController()
      {
        base.StartController();
        ArucoCamera.UndistortRectifyImages += ArucoCamera_UndistortRectifyImages;
        OnStarted();
      }

      /// <summary>
      /// Unsusbcribes from <see cref="ArucoCamera.UndistortRectifyImages"/>.
      /// </summary>
      public override void StopController()
      {
        base.StopController();
        ArucoCamera.UndistortRectifyImages -= ArucoCamera_UndistortRectifyImages;
        OnStopped();
      }

      // Methods

      /// <summary>
      /// Initializes the <see cref="RectifiedCameraMatrices"/> and the <see cref="UndistortionRectificationMaps"/> of each camera image, and sets
      /// the undistorted distorsion coefficients to <see cref="UndistortedDistCoeffs"/>.
      /// </summary>
      // TODO: scale if there is a difference between camera image size and camera parameters image size (during calibration)
      protected virtual void InitializeUndistortionRectification()
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        
        RectifiedCameraMatrices = new Cv.Mat[cameraParameters.CameraNumber];
        RectificationMatrices = new Cv.Mat[cameraParameters.CameraNumber];
        UndistortionRectificationMaps = new Cv.Mat[cameraParameters.CameraNumber][];
        for (int cameraId = 0; cameraId < cameraParameters.CameraNumber; cameraId++)
        {
          RectifiedCameraMatrices[cameraId] = new Cv.Mat();
          UndistortionRectificationMaps[cameraId] = new Cv.Mat[undistortionCameraMapsNumber];
        }
      }

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