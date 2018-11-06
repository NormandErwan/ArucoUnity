using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using UnityEngine;
using System;
using ArucoUnity.Utilities;

namespace ArucoUnity.Cameras.Undistortions
{
    /// <summary>
    /// Manages the processes of undistortion and rectification of <see cref="ArucoCamera.Images"/>. It's a time-consuming
    /// operation but it's necessary for cameras with an important distorsion for a good alignement of the images with
    /// the 3D content. Base class to reference in editor fields.
    /// </summary>
    public abstract class ArucoCameraUndistortion : ArucoCameraController, IArucoCameraUndistortion
    {
        // Constants

        public const int undistortionCameraMapsNumber = 2;

        // Editor fields

        [SerializeField]
        [Tooltip("The camera parameters associated with the ArucoCamera.")]
        private ArucoCameraParametersController CameraParametersController;

        // IArucoCameraUndistortion properties

        public ArucoCameraParameters CameraParameters { get; set; }
        public Cv.Mat[] RectifiedCameraMatrices { get; protected set; }
        public Cv.Mat[] RectificationMatrices { get; protected set; }
        public Cv.Mat[] UndistortedDistCoeffs { get; private set; }
        public Cv.Mat[][] UndistortionRectificationMaps { get; protected set; }

        // Variables

        protected Cv.Mat noRectificationMatrix = new Cv.Mat();
        protected Cv.Mat noDistCoeffs = new Cv.Mat();
        protected Cv.Rect noROI = new Cv.Rect();
        protected string CameraParametersFilePath;
        protected ArucoCameraSeparateThread remapThread;

        // MonoBehaviour methods

        /// <summary>
        /// Initializes the properties.
        /// </summary>
        protected override void Start()
        {
            if (CameraParameters == null && CameraParametersController != null)
            {
                CameraParameters = CameraParametersController.CameraParameters;
            }

            base.Start();
        }

        // ConfigurableController methods

        /// <summary>
        /// Initializes the properties from <see cref="CameraParameters"/>.
        /// </summary>
        protected override void Configuring()
        {
            base.Configuring();

            if (CameraParameters == null)
            {
                throw new ArgumentNullException("CameraParameters", "This property needs to be set for the configuration.");
            }
            if (CameraParameters.CameraNumber != ArucoCamera.CameraNumber)
            {
                throw new Exception("The number of cameras in CameraParameters must be equal to the number of cameras in ArucoCamera");
            }

            RectifiedCameraMatrices = new Cv.Mat[CameraParameters.CameraNumber];
            RectificationMatrices = new Cv.Mat[CameraParameters.CameraNumber];
            UndistortedDistCoeffs = new Cv.Mat[CameraParameters.CameraNumber];
            UndistortionRectificationMaps = new Cv.Mat[CameraParameters.CameraNumber][];
            for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
            {
                UndistortedDistCoeffs[cameraId] = noDistCoeffs;
                UndistortionRectificationMaps[cameraId] = new Cv.Mat[undistortionCameraMapsNumber];
            }
        }

        /// <summary>
        /// Calls <see cref="InitializeRectification"/> and <see cref="InitializeUndistortionMaps"/> and susbcribes to
        /// <see cref="ArucoCamera.UndistortRectifyImages"/>.
        /// </summary>
        protected override void Starting()
        {
            base.Starting();

            InitializeRectification();
            InitializeUndistortionMaps();

            ArucoCamera.UndistortRectifyImages += ArucoCamera_UndistortRectifyImages;
            remapThread = new ArucoCameraSeparateThread(ArucoCamera, UndistortRectifyImages) { CopyBackImages = true };
            remapThread.Start();
        }

        /// <summary>
        /// Unsusbcribes from <see cref="ArucoCamera.UndistortRectifyImages"/>.
        /// </summary>
        protected override void Stopping()
        {
            base.Stopping();
            remapThread.Stop();
            ArucoCamera.UndistortRectifyImages -= ArucoCamera_UndistortRectifyImages;
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