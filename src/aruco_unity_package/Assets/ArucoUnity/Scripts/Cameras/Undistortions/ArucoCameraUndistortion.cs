using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers;
using ArucoUnity.Plugin;
using UnityEngine;
using System;
using ArucoUnity.Utilities;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Undistortions
  {
    /// <summary>
    /// Manages the processes of undistortion and rectification of <see cref="ArucoCamera.Images"/>. It's a time-consuming
    /// operation but it's necessary for cameras with an important distorsion for a good alignement of the images with the 3D content.
    /// </summary>
    public abstract class ArucoCameraUndistortion : ArucoCameraController, IArucoCameraUndistortion
    {
      // Constants

      public const int undistortionCameraMapsNumber = 2;

      // Editor fields

      [SerializeField]
      [Tooltip("The camera parameters associated with the ArucoCamera.")]
      private CameraParametersController CameraParametersController;

      // IArucoCameraUndistortion properties

      public CameraParameters CameraParameters { get; set; }
      public Cv.Mat[] RectifiedCameraMatrices { get; protected set; }
      public Cv.Mat[] RectificationMatrices { get; protected set; }
      public Cv.Mat[] UndistortedDistCoeffs { get; private set; }
      public Cv.Mat[][] UndistortionRectificationMaps { get; protected set; }

      // Variables

      protected Cv.Mat noRectificationMatrix;
      protected Cv.Mat noDistCoeffs;
      protected string CameraParametersFilePath;
      protected ArucoCameraSeparateThread remapThread;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the properties.
      /// </summary>
      protected override void Start()
      {
        noRectificationMatrix = new Cv.Mat();
        noDistCoeffs = new Cv.Mat();

        if (CameraParameters == null && CameraParametersController != null)
        {
          CameraParameters = CameraParametersController.CameraParameters;
        }

        base.Start();
      }

      // ArucoCameraController methods

      /// <summary>
      /// Checks if <see cref="CameraParameters"/> is set, if <see cref="CameraParameters.CameraNumber"/> and <see cref="ArucoCamera.CameraNumber"/>
      /// are equals.
      /// </summary>
      public override void Configure()
      {
        base.Configure();

        // Check properties
        if (CameraParameters == null)
        {
          throw new Exception("CameraParameters must be set to undistort the ArucoCamera images.");
        }
        if (CameraParameters.CameraNumber != ArucoCamera.CameraNumber)
        {
          throw new Exception("The number of cameras in CameraParameters must be equal to the number of cameras in ArucoCamera");
        }

        // Initialize properties
        RectifiedCameraMatrices = new Cv.Mat[CameraParameters.CameraNumber];
        RectificationMatrices = new Cv.Mat[CameraParameters.CameraNumber];
        UndistortedDistCoeffs = new Cv.Mat[CameraParameters.CameraNumber];
        UndistortionRectificationMaps = new Cv.Mat[CameraParameters.CameraNumber][];
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          UndistortedDistCoeffs[cameraId] = noDistCoeffs;
          UndistortionRectificationMaps[cameraId] = new Cv.Mat[undistortionCameraMapsNumber];
        }

        OnConfigured();
      }

      /// <summary>
      /// Calls <see cref="InitializeRectification"/> and <see cref="InitializeUndistortionMaps"/> and susbcribes to
      /// <see cref="ArucoCamera.UndistortRectifyImages"/>.
      /// </summary>
      public override void StartController()
      {
        base.StartController();

        InitializeRectification();
        InitializeUndistortionMaps();

        ArucoCamera.UndistortRectifyImages += ArucoCamera_UndistortRectifyImages;
        remapThread = new ArucoCameraSeparateThread(ArucoCamera, UndistortRectifyImages) { CopyBackImages = true };
        remapThread.Start();

        OnStarted();
      }

      /// <summary>
      /// Unsusbcribes from <see cref="ArucoCamera.UndistortRectifyImages"/>.
      /// </summary>
      public override void StopController()
      {
        base.StopController();
        remapThread.Stop();
        ArucoCamera.UndistortRectifyImages -= ArucoCamera_UndistortRectifyImages;
        OnStopped();
      }

      // Methods

      /// <summary>
      /// Updates the undistortion thread with the <paramref name="images"/> and stops if there was an exception from this thread.
      /// </summary>
      protected virtual void ArucoCamera_UndistortRectifyImages(Cv.Mat[] images, byte[][] imageDatas)
      {
        try
        {
          remapThread.Update(imageDatas);
        }
        catch (Exception e)
        {
          StopController();
          throw e;
        }
      }

      /// <summary>
      /// Undistorts and rectifies the <paramref name="images"/> using <see cref="UndistortionRectificationMaps"/> on a separate thread.
      /// </summary>
      protected virtual void UndistortRectifyImages(Cv.Mat[] images)
      {
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          Cv.Remap(images[cameraId], images[cameraId], UndistortionRectificationMaps[cameraId][0],
            UndistortionRectificationMaps[cameraId][1], Cv.InterpolationFlags.Linear);
        }
      }

      /// <summary>
      /// Initializes the <see cref="RectificationMatrices"/> of each camera image.
      /// </summary>
      protected abstract void InitializeRectification();

      /// <summary>
      /// Initializes the <see cref="UndistortionRectificationMaps"/> of each camera image.
      /// </summary>
      protected abstract void InitializeUndistortionMaps();
    }
  }

  /// \} aruco_unity_package
}