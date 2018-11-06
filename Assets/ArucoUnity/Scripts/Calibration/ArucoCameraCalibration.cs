using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Objects;
using ArucoUnity.Objects.Trackers;
using ArucoUnity.Plugin;
using System;
using System.Threading;
using UnityEngine;

namespace ArucoUnity.Calibration
{
    /// <summary>
    /// Calibrates a <see cref="Cameras.ArucoCamera"/> with a <see cref="ArucoBoard"/> and saves the calibrated camera
    /// parameters in a file managed by <see cref="ArucoCameraParametersController"/>. Base class to reference in editor
    /// fields.
    /// 
    /// See the OpenCV and the ArUco module documentations for more information about the calibration process:
    /// http://docs.opencv.org/3.4/da/d13/tutorial_aruco_calibration.html and
    /// https://docs.opencv.org/3.4/da/d13/tutorial_aruco_calibration.html
    /// </summary>
    public abstract class ArucoCameraCalibration : ArucoObjectDetector
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The ArUco board to use for calibration.")]
        private ArucoBoard calibrationBoard;

        [SerializeField]
        [Tooltip("Use a refine algorithm to find not detected markers based on the already detected and the board layout" +
            " (if using a board).")]
        private bool refineMarkersDetection = false;

        [SerializeField]
        [Tooltip("The camera parameters to use if CalibrationFlags.UseIntrinsicGuess is true. Otherwise, the camera" +
            " parameters file will be generated from the camera name and the calibration datetime.")]
        private ArucoCameraParametersController cameraParametersController;

        // Properties

        /// <summary>
        /// Gets or sets the ArUco board to use for calibration.
        /// </summary>
        public ArucoBoard CalibrationBoard { get { return calibrationBoard; } set { calibrationBoard = value; } }

        /// <summary>
        /// Gets or sets if need to use a refine algorithm to find not detected markers based on the already detected and
        /// the board layout.
        /// </summary>
        public bool RefineMarkersDetection { get { return refineMarkersDetection; } set { refineMarkersDetection = value; } }

        /// <summary>
        /// Gets or sets the camera parameters to use if <see cref="CalibrationFlags.UseIntrinsicGuess"/> is true.
        /// Otherwise, the camera parameters file will be generated from the camera name and the calibration datetime.
        /// </summary>
        public ArucoCameraParametersController CameraParametersController { get { return cameraParametersController; }
            set { cameraParametersController = value; } }

        /// <summary>
        /// Gets or sets the flags for the cameras calibration.
        /// </summary>
        public CalibrationFlags CalibrationFlags { get; set; }

        /// <summary>
        /// Gets the detected marker corners for each camera.
        /// </summary>
        public Std.VectorVectorVectorPoint2f[] AllMarkerCorners { get; protected set; }

        /// <summary>
        /// Gets the detected marker ids for each camera.
        /// </summary>
        public Std.VectorVectorInt[] AllMarkerIds { get; protected set; }

        /// <summary>
        /// Gets the images to use for the calibration.
        /// </summary>
        public Std.VectorMat[] Images { get; protected set; }

        /// <summary>
        /// Gets the estimated rotation vector for each detected markers in each camera.
        /// </summary>
        public Std.VectorVec3d[] Rvecs { get; protected set; }

        /// <summary>
        /// Gets the estimated translation vector for each detected markers in each camera.
        /// </summary>
        public Std.VectorVec3d[] Tvecs { get; protected set; }

        /// <summary>
        /// Gets the detected marker corners on the current images of each camera.
        /// </summary>
        public Std.VectorVectorPoint2f[] MarkerCorners { get; protected set; }

        /// <summary>
        /// Gets the detected marker ids on the current images of each camera.
        /// </summary>
        public Std.VectorInt[] MarkerIds { get; protected set; }

        /// <summary>
        /// Gets if the last <see cref="CalibrateAsync"/> call has been a success.
        /// </summary>
        public bool IsCalibrated { get; protected set; }

        /// <summary>
        /// Gets if <see cref="CalibrateAsync"/> has been called and hasn't completed yet.
        /// </summary>
        public bool CalibrationRunning { get; protected set; }

        // Events

        /// <summary>
        /// Called when <see cref="IsCalibrated"/> is set to true.
        /// </summary>
        public event Action Calibrated = delegate { };

        // Variables

        protected string applicationPath;
        protected Thread calibratingThread;
        protected Mutex calibratingMutex = new Mutex();
        protected Exception calibratingException;

        // MonoBehaviour methods

        /// <summary>
        /// Calls the <see cref="Calibrated"/> event when a calibration has just completed.
        /// </summary>
        protected virtual void LateUpdate()
        {
            Exception e = null;
            bool calibrationDone = false;
            calibratingMutex.WaitOne();
            {
                e = calibratingException;
                calibratingException = null;

                calibrationDone = CalibrationRunning && IsCalibrated;
            }
            calibratingMutex.ReleaseMutex();

            // Check for exception in calibrating thread
            if (e != null)
            {
                calibratingThread.Abort();
                CalibrationRunning = false;
                throw e;
            }

            // Check for calibration done
            if (calibrationDone)
            {
                CalibrationRunning = false;
                Calibrated.Invoke();
            }
        }

        // ConfigurableController methods

        /// <summary>
        /// Checks if <see cref="CalibrationBoard"/> is set and calls <see cref="ResetCalibration"/>.
        /// </summary>
        protected override void Configuring()
        {
            base.Configuring();
            if (CalibrationBoard == null)
            {
                throw new ArgumentNullException("CalibrationBoard", "This property needs to be set to configure the" +
                    " calibration controller.");
            }
            ResetCalibration();
        }

        /// <summary>
        /// Susbcribes to <see cref="Cameras.ArucoCamera.UndistortRectifyImages"/>.
        /// </summary>
        protected override void Starting()
        {
            base.Starting();
            ArucoCamera.ImagesUpdated += ArucoCamera_ImagesUpdated;
        }

        /// <summary>
        /// Unsusbcribes from <see cref="Cameras.ArucoCamera.UndistortRectifyImages"/>.
        /// </summary>
        protected override void Stopping()
        {
            base.Stopping();
            ArucoCamera.ImagesUpdated -= ArucoCamera_ImagesUpdated;
        }

        // Methods

        /// <summary>
        /// Resets the properties.
        /// </summary>
        public virtual void ResetCalibration()
        {
            AllMarkerCorners = new Std.VectorVectorVectorPoint2f[ArucoCamera.CameraNumber];
            AllMarkerIds = new Std.VectorVectorInt[ArucoCamera.CameraNumber];
            Images = new Std.VectorMat[ArucoCamera.CameraNumber];
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                AllMarkerCorners[cameraId] = new Std.VectorVectorVectorPoint2f();
                AllMarkerIds[cameraId] = new Std.VectorVectorInt();
                Images[cameraId] = new Std.VectorMat();
            }

            Rvecs = new Std.VectorVec3d[ArucoCamera.CameraNumber];
            Tvecs = new Std.VectorVec3d[ArucoCamera.CameraNumber];
            MarkerCorners = new Std.VectorVectorPoint2f[ArucoCamera.CameraNumber];
            MarkerIds = new Std.VectorInt[ArucoCamera.CameraNumber];

            IsCalibrated = false;
        }

        /// <summary>
        /// Detects the Aruco markers on the current images of the cameras and store the results in the
        /// <see cref="MarkerCorners"/> and <see cref="MarkerIds"/> properties.
        /// </summary>
        public virtual void DetectMarkers()
        {
            if (!IsConfigured || !IsStarted)
            {
                throw new Exception("Configure and start the calibration controller before detect markers.");
            }

            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                Std.VectorInt markerIds;
                Std.VectorVectorPoint2f markerCorners, rejectedCandidateCorners;

                Aruco.DetectMarkers(ArucoCamera.Images[cameraId], CalibrationBoard.Dictionary, out markerCorners,
                    out markerIds, DetectorParameters, out rejectedCandidateCorners);

                MarkerCorners[cameraId] = markerCorners;
                MarkerIds[cameraId] = markerIds;

                if (RefineMarkersDetection)
                {
                    Aruco.RefineDetectedMarkers(ArucoCamera.Images[cameraId], CalibrationBoard.Board, MarkerCorners[cameraId],
                        MarkerIds[cameraId], rejectedCandidateCorners);
                }
            }
        }

        /// <summary>
        /// Draws the detected ArUco markers on the current images of the cameras.
        /// </summary>
        public virtual void DrawDetectedMarkers()
        {
            if (!IsConfigured || !IsStarted)
            {
                throw new Exception("Configure and start the calibration controller before drawing detected markers.");
            }

            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                if (MarkerIds[cameraId] != null && MarkerIds[cameraId].Size() > 0)
                {
                    Aruco.DrawDetectedMarkers(ArucoCamera.Images[cameraId], MarkerCorners[cameraId],
                        MarkerIds[cameraId]);
                }
            }
        }

        /// <summary>
        /// Adds the current images of the cameras and the detected corners for the calibration.
        /// </summary>
        public virtual void AddImages()
        {
            if (!IsConfigured)
            {
                throw new Exception("Configure the calibration controller before adding the current images for calibration.");
            }

            // Check for validity
            uint markerIdsNumber = (MarkerIds[0] != null) ? MarkerIds[0].Size() : 0;
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                if (MarkerIds[cameraId] == null || MarkerIds[cameraId].Size() < 1)
                {
                    throw new Exception("No markers detected for the camera " + (cameraId + 1) + "/" + ArucoCamera.CameraNumber +
                        " to add the current images for the calibration. At least one marker detected is required for" +
                        " calibrating the camera.");
                }

                if (markerIdsNumber != MarkerIds[cameraId].Size())
                {
                    throw new Exception("The cameras must have detected the same number of markers to add the current images" +
                        " for the calibration.");
                }
            }

            // Save the images and the detected corners
            Cv.Mat[] cameraImages = ArucoCamera.Images;
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                AllMarkerCorners[cameraId].PushBack(MarkerCorners[cameraId]);
                AllMarkerIds[cameraId].PushBack(MarkerIds[cameraId]);
                Images[cameraId].PushBack(ArucoCamera.Images[cameraId].Clone());
            }
        }

        /// <summary>
        /// Calls <see cref="Calibrate"/> in a background thread.
        /// </summary>
        public virtual void CalibrateAsync()
        {
            if (!IsConfigured)
            {
                throw new Exception("Configure the calibration controller before starting the async calibration.");
            }

            bool calibrationRunning = false;
            calibratingMutex.WaitOne();
            {
                calibrationRunning = CalibrationRunning;
            }
            calibratingMutex.ReleaseMutex();

            if (calibrationRunning)
            {
                throw new Exception("A calibration is already running. Wait its completion or call CancelCalibrateAsync()" +
                    " before starting a new calibration.");
            }

            calibratingThread = new Thread(() =>
            {
                try
                {
                    Calibrate();
                }
                catch (Exception e)
                {
                    calibratingMutex.WaitOne();
                    {
                        calibratingException = e;
                    }
                    calibratingMutex.ReleaseMutex();
                }
            });
            calibratingThread.IsBackground = true;
            calibratingThread.Start();
        }

        /// <summary>
        /// Stops the calibration if <see cref="CalibrationRunning"/> is true.
        /// </summary>
        public virtual void CancelCalibrateAsync()
        {
            if (!IsConfigured)
            {
                throw new Exception("Configure the calibration controller before starting or canceling the calibration.");
            }

            bool calibrationRunning = false;
            calibratingMutex.WaitOne();
            {
                calibrationRunning = CalibrationRunning;
            }
            calibratingMutex.ReleaseMutex();

            if (!calibrationRunning)
            {
                throw new Exception("Start the async calibration before canceling it.");
            }

            calibratingThread.Abort();
        }

        /// <summary>
        /// Calibrates each mono camera in <see cref="Cameras.ArucoCameraController.ArucoCamera"/> using the detected
        /// markers added with <see cref="AddImages()"/>, the <see cref="ArucoCameraParameters"/>, the
        /// <see cref="Cameras.Undistortions.ArucoCameraUndistortion"/> and save the results on a calibration file. Stereo
        /// calibrations will be additionally executed on these results for every camera pair.
        /// </summary>
        public virtual void Calibrate()
        {
            if (!IsConfigured)
            {
                throw new Exception("Configure the calibration controller before starting the calibration.");
            }

            // Update state
            calibratingMutex.WaitOne();
            {
                IsCalibrated = false;
                CalibrationRunning = true;
            }
            calibratingMutex.ReleaseMutex();

            // Check if there is enough captured images for calibration
            Aruco.CharucoBoard charucoBoard = CalibrationBoard.Board as Aruco.CharucoBoard;
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                if (charucoBoard == null && AllMarkerIds[cameraId].Size() < 3)
                {
                    throw new Exception("Need at least three images captured for the camera " + (cameraId + 1) + "/" +
                        ArucoCamera.CameraNumber + " to calibrate.");
                }
                else if (charucoBoard != null && AllMarkerIds[cameraId].Size() < 4)
                {
                    throw new Exception("Need at least four images captured for the camera " + (cameraId + 1) + "/" +
                        ArucoCamera.CameraNumber + " to calibrate with a ChAruco board.");
                }
            }

            InitializeCameraParameters(); // Initialize and configure the camera parameters

            // Get objet and image calibration points from detected ids and corners
            Std.VectorVectorPoint2f[] imagePoints = new Std.VectorVectorPoint2f[ArucoCamera.CameraNumber];
            Std.VectorVectorPoint3f[] objectPoints = new Std.VectorVectorPoint3f[ArucoCamera.CameraNumber];
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                imagePoints[cameraId] = new Std.VectorVectorPoint2f();
                objectPoints[cameraId] = new Std.VectorVectorPoint3f();

                uint imagesCount = AllMarkerCorners[cameraId].Size();
                for (uint imageId = 0; imageId < imagesCount; imageId++)
                {
                    Std.VectorPoint2f currentImagePoints;
                    Std.VectorPoint3f currentObjectPoints;

                    if (charucoBoard == null)
                    {
                        // Using a grid board
                        Aruco.GetBoardObjectAndImagePoints(CalibrationBoard.Board, AllMarkerCorners[cameraId].At(imageId),
                            AllMarkerIds[cameraId].At(imageId), out currentObjectPoints, out currentImagePoints);
                    }
                    else
                    {
                        // Using a charuco board
                        Std.VectorInt charucoIds;
                        Aruco.InterpolateCornersCharuco(AllMarkerCorners[cameraId].At(imageId), AllMarkerIds[cameraId].At(imageId),
                            Images[cameraId].At(imageId), charucoBoard, out currentImagePoints, out charucoIds);

                        // Join the object points corresponding to the detected markers
                        currentObjectPoints = new Std.VectorPoint3f();
                        uint markerCount = charucoIds.Size();
                        for (uint marker = 0; marker < markerCount; marker++)
                        {
                            uint pointId = (uint)charucoIds.At(marker);
                            Cv.Point3f objectPoint = charucoBoard.ChessboardCorners.At(pointId);
                            currentObjectPoints.PushBack(objectPoint);
                        }
                    }

                    imagePoints[cameraId].PushBack(currentImagePoints);
                    objectPoints[cameraId].PushBack(currentObjectPoints);
                }
            }

            // Calibrate the Aruco camera
            Calibrate(imagePoints, objectPoints);

            // Save the camera parameters
            CameraParametersController.CameraParametersFilename = ArucoCamera.Name + " - "
                + CameraParametersController.CameraParameters.CalibrationDateTime.ToString("yyyy-MM-dd_HH-mm-ss") + ".xml";
            CameraParametersController.Save();

            // Update state
            calibratingMutex.WaitOne();
            {
                IsCalibrated = true;
            }
            calibratingMutex.ReleaseMutex();
        }

        /// <summary>
        /// Detects and draw the ArUco markers on the current images of the cameras.
        /// </summary>
        protected virtual void ArucoCamera_ImagesUpdated()
        {
            DetectMarkers();
            DrawDetectedMarkers();
        }

        /// <summary>
        /// Initializes and configure the <see cref="ArucoCameraParametersController.CameraParameters"/>.
        /// </summary>
        protected virtual void InitializeCameraParameters()
        {
            if (CalibrationFlags != null && CalibrationFlags.UseIntrinsicGuess)
            {
                if (CameraParametersController.CameraParameters == null || CameraParametersController.CameraParameters.CameraMatrices == null)
                {
                    throw new Exception("CalibrationFlags.UseIntrinsicGuess flag is set but CameraParameters is null or has no" +
                        " valid values. Set CameraParametersFilename or deactivate this flag.");
                }
            }
            else
            {
                CameraParametersController.Initialize(ArucoCamera.CameraNumber);
                for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
                {
                    CameraParametersController.CameraParameters.CameraMatrices[cameraId] = new Cv.Mat();
                    CameraParametersController.CameraParameters.DistCoeffs[cameraId] = new Cv.Mat();
                    CameraParametersController.CameraParameters.OmnidirXis[cameraId] = new Cv.Mat();
                }
            }

            CameraParametersController.CameraParameters.CalibrationFlagsValue =
                (CalibrationFlags != null) ? CalibrationFlags.Value : default(int);

            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                CameraParametersController.CameraParameters.ImageHeights[cameraId] = ArucoCamera.Images[cameraId].Size.Height;
                CameraParametersController.CameraParameters.ImageWidths[cameraId] = ArucoCamera.Images[cameraId].Size.Width;
            }
        }

        /// <summary>
        /// Applies a calibration to the <see cref="Cameras.ArucoCameraController.ArucoCamera"/>, set the extrinsic camera
        /// parameters to <see cref="Rvecs"/> and <see cref="Tvecs"/> and saves the camera parameters in
        /// <see cref="ArucoCameraParametersController.CameraParameters"/>.
        /// </summary>
        /// <param name="imagePoints">The detected image points of each camera.</param>
        /// <param name="objectPoints">The corresponding object points of each camera.</param>
        protected abstract void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints);
    }
}