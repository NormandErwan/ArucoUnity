using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraCalibrations.Flags;
using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using System;
using System.Threading;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraCalibrations
  {
    /// <summary>
    /// Calibrates a <see cref="ArucoCamera"/> or a <see cref="StereoArucoCamera"/> with a <see cref="ArucoBoard"/> and saves the calibration results
    /// in a file to use in <see cref="ObjectTrackers.ArucoObjectsTracker"/> for <see cref="ArucoObject"/> tracking.
    /// 
    /// See the OpenCV and the ArUco module documentations for more information about the calibration process:
    /// http://docs.opencv.org/3.3.0/da/d13/tutorial_aruco_calibration.html and https://docs.opencv.org/3.3.0/da/d13/tutorial_aruco_calibration.html
    /// </summary>
    public abstract class ArucoCameraCalibration : ArucoObjectDetector<ArucoCamera>
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The ArUco board to use for calibration.")]
      private ArucoBoard calibrationBoard;

      [SerializeField]
      [Tooltip("Use a refine algorithm to find not detected markers based on the already detected and the board layout (if using a board).")]
      private bool refineMarkersDetection = false;

      [SerializeField]
      [Tooltip("The camera parameters to use if CalibrationFlags.UseIntrinsicGuess is true. Otherwise, the camera parameters file will be generated" +
        " from the camera name and the calibration datetime.")]
      private CameraParametersController cameraParametersController;

      // Properties

      /// <summary>
      /// Gets or sets the ArUco board to use for calibration.
      /// </summary>
      public ArucoBoard CalibrationBoard { get { return calibrationBoard; } set { calibrationBoard = value; } }

      /// <summary>
      /// Gets or sets if need to use a refine algorithm to find not detected markers based on the already detected and the board layout.
      /// </summary>
      public bool RefineMarkersDetection { get { return refineMarkersDetection; } set { refineMarkersDetection = value; } }

      /// <summary>
      /// Gets or sets the camera parameters to use if <see cref="CameraCalibrationFlags.UseIntrinsicGuess"/> is true. Otherwise, the camera parameters
      /// file will be generated from the camera name and the calibration datetime.
      /// </summary>
      public CameraParametersController CameraParametersController { get { return cameraParametersController; } set { cameraParametersController = value; } }

      /// <summary>
      /// Gets the detected marker corners for each camera.
      /// </summary>
      public Std.VectorVectorVectorPoint2f[] MarkerCorners { get; protected set; }

      /// <summary>
      /// Gets the detected marker ids for each camera.
      /// </summary>
      public Std.VectorVectorInt[] MarkerIds { get; protected set; }

      /// <summary>
      /// Gets the images to use for the calibration.
      /// </summary>
      public Std.VectorMat[] CalibrationImages { get; protected set; }

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
      public Std.VectorVectorPoint2f[] MarkerCornersCurrentImage { get; protected set; }

      /// <summary>
      /// Gets the detected marker ids on the current images of each camera.
      /// </summary>
      public Std.VectorInt[] MarkerIdsCurrentImage { get; protected set; }

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

      protected int stereoCameraId1 = 0, stereoCameraId2 = 1;
      protected string applicationPath;
      protected Cv.Size[] calibrationImageSizes;
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

      // ArucoCameraController methods

      /// <summary>
      /// Checks if <see cref="CalibrationBoard"/> is set and calls <see cref="ResetCalibration"/>.
      /// </summary>
      protected override void Configure()
      {
        // Check for the board
        if (CalibrationBoard == null)
        {
          throw new ArgumentNullException("CalibrationBoard", "This property needs to be set to configure the calibrator.");
        }

        ResetCalibration();
      }

      /// <summary>
      /// Detects and draw the ArUco markers on the current images of the cameras.
      /// </summary>
      protected override void ArucoCamera_ImagesUpdated()
      {
        DetectMarkers();
        DrawDetectedMarkers();
      }

      // Methods

      /// <summary>
      /// Resets the properties.
      /// </summary>
      public virtual void ResetCalibration()
      {
        MarkerCorners = new Std.VectorVectorVectorPoint2f[ArucoCamera.CameraNumber];
        MarkerIds = new Std.VectorVectorInt[ArucoCamera.CameraNumber];
        CalibrationImages = new Std.VectorMat[ArucoCamera.CameraNumber];
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          MarkerCorners[cameraId] = new Std.VectorVectorVectorPoint2f();
          MarkerIds[cameraId] = new Std.VectorVectorInt();
          CalibrationImages[cameraId] = new Std.VectorMat();
        }
        
        Rvecs = new Std.VectorVec3d[ArucoCamera.CameraNumber];
        Tvecs = new Std.VectorVec3d[ArucoCamera.CameraNumber];
        MarkerCornersCurrentImage = new Std.VectorVectorPoint2f[ArucoCamera.CameraNumber];
        MarkerIdsCurrentImage = new Std.VectorInt[ArucoCamera.CameraNumber];

        calibrationImageSizes = new Cv.Size[ArucoCamera.CameraNumber];

        IsCalibrated = false;
      }

      /// <summary>
      /// Detects the Aruco markers on the current images of the cameras and store the results in the <see cref="MarkerCornersCurrentImage"/> and
      /// <see cref="MarkerIdsCurrentImage"/> properties.
      /// </summary>
      public virtual void DetectMarkers()
      {
        if (!IsConfigured)
        {
          return;
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          Std.VectorInt markerIds;
          Std.VectorVectorPoint2f markerCorners, rejectedCandidateCorners;

          Cv.Mat image = ArucoCamera.Images[cameraId];

          Aruco.DetectMarkers(image, CalibrationBoard.Dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);

          MarkerCornersCurrentImage[cameraId] = markerCorners;
          MarkerIdsCurrentImage[cameraId] = markerIds;

          if (RefineMarkersDetection)
          {
            Aruco.RefineDetectedMarkers(image, CalibrationBoard.Board, MarkerCornersCurrentImage[cameraId], MarkerIdsCurrentImage[cameraId],
              rejectedCandidateCorners);
          }
        }
      }

      /// <summary>
      /// Draws the detected ArUco markers on the current images of the cameras.
      /// </summary>
      public virtual void DrawDetectedMarkers()
      {
        if (!IsConfigured)
        {
          return;
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          if (MarkerIdsCurrentImage[cameraId] != null && MarkerIdsCurrentImage[cameraId].Size() > 0)
          {
            Aruco.DrawDetectedMarkers(ArucoCamera.Images[cameraId], MarkerCornersCurrentImage[cameraId], MarkerIdsCurrentImage[cameraId]);
          }
        }
      }

      /// <summary>
      /// Adds the current images of the cameras and the detected corners for the calibration.
      /// </summary>
      public virtual void AddCurrentFrameForCalibration()
      {
        if (!IsConfigured)
        {
          return;
        }

        // Check for validity
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          if (MarkerIdsCurrentImage[cameraId] == null || MarkerIdsCurrentImage[cameraId].Size() < 1)
          {
            throw new Exception("No markers detected for the camera " + (cameraId + 1) + "/" + ArucoCamera.CameraNumber + " to add the"
              + " current images for calibration. It requires at least one marker detected.");
          }
        }

        // Save the images and the detected corners
        Cv.Mat[] cameraImages = ArucoCamera.Images;
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          MarkerCorners[cameraId].PushBack(MarkerCornersCurrentImage[cameraId]);
          MarkerIds[cameraId].PushBack(MarkerIdsCurrentImage[cameraId]);
          CalibrationImages[cameraId].PushBack(ArucoCamera.Images[cameraId].Clone());

          if (calibrationImageSizes[cameraId] == null)
          {
            print(ArucoCamera.Images[cameraId].Size.Width + "+" + ArucoCamera.Images[cameraId].Size.Height);
            calibrationImageSizes[cameraId] = new Cv.Size(ArucoCamera.Images[cameraId].Size.Width, ArucoCamera.Images[cameraId].Size.Height);
          }
        }
      }

      /// <summary>
      /// Calls <see cref="Calibrate"/> in a background thread.
      /// </summary>
      public virtual void CalibrateAsync()
      {
        bool calibrationRunning = false;
        calibratingMutex.WaitOne();
        {
          calibrationRunning = CalibrationRunning;
        }
        calibratingMutex.ReleaseMutex();

        if (calibrationRunning)
        {
          throw new Exception("A calibration is already running. Wait its completion or call CancelCalibrateAsync() before starting a new calibration.");
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
        bool calibrationRunning = false;
        calibratingMutex.WaitOne();
        {
          calibrationRunning = CalibrationRunning;
        }
        calibratingMutex.ReleaseMutex();

        if (calibrationRunning)
        {
          calibratingThread.Abort();
        }
      }

      /// <summary>
      /// Calibrates each camera of the <see cref="ArucoObjectDetector.ArucoCamera"/> system using the detected markers added with
      /// <see cref="AddCurrentFrameForCalibration()"/>, the <see cref="CameraParameters"/>, the <see cref="ArucoCameraUndistortion"/> and save
      /// the results on a calibration file. Stereo calibrations will be additionally executed on these results for every camera pair in
      /// <see cref="StereoCalibrationCameraPairs"/>.
      /// </summary>
      public virtual void Calibrate()
      {
        calibratingMutex.WaitOne();
        {
          IsCalibrated = false;
          CalibrationRunning = true;
        }
        calibratingMutex.ReleaseMutex();

        // Check if there is enough captured frames for calibration
        Aruco.CharucoBoard charucoBoard = CalibrationBoard.Board as Aruco.CharucoBoard;
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          if (charucoBoard == null && MarkerIds[cameraId].Size() < 3)
          {
            throw new Exception("Need at least three frames captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CameraNumber
              + " to calibrate.");
          }
          else if (charucoBoard != null && MarkerIds[cameraId].Size() < 4)
          {
            throw new Exception("Need at least four frames captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CameraNumber
              + " to calibrate with a ChAruco board.");
          }
        }

        // Initialize and configure the camera parameters
        InitializeCameraParameters();

        // Calibrate each camera
        Std.VectorVectorPoint3f[] objectPoints = new Std.VectorVectorPoint3f[ArucoCamera.CameraNumber];
        Std.VectorVectorPoint2f[] imagePoints = new Std.VectorVectorPoint2f[ArucoCamera.CameraNumber];
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          // Get objet and image calibration points from detected ids and corners
          Std.VectorVectorPoint3f boardObjectPoints = new Std.VectorVectorPoint3f();
          Std.VectorVectorPoint2f boardImagePoints = new Std.VectorVectorPoint2f();
          uint frameCount = MarkerCorners[cameraId].Size();
          for (uint frame = 0; frame < frameCount; frame++)
          {
            Std.VectorPoint3f frameObjectPoints;
            Std.VectorPoint2f frameImagePoints;
            Aruco.GetBoardObjectAndImagePoints(CalibrationBoard.Board, MarkerCorners[cameraId].At(frame), MarkerIds[cameraId].At(frame),
              out frameObjectPoints, out frameImagePoints);
            boardObjectPoints.PushBack(frameObjectPoints);
            boardImagePoints.PushBack(frameImagePoints);
          }
          objectPoints[cameraId] = boardObjectPoints;
          imagePoints[cameraId] = boardImagePoints;

          // Calibrate the camera
          Std.VectorVec3d rvecs, tvecs;
          Calibrate(cameraId, boardObjectPoints, boardImagePoints, calibrationImageSizes[cameraId], out rvecs,
            out tvecs);

          // If the used board is a charuco board, refine the calibration
          if (charucoBoard != null)
          {
            // Prepare data to refine the calibration
            Std.VectorVectorPoint3f charucoObjectPoints = new Std.VectorVectorPoint3f();
            Std.VectorVectorPoint2f charucoImagePoints = new Std.VectorVectorPoint2f();
            for (uint frame = 0; frame < frameCount; frame++)
            {
              // Interpolate charuco corners using camera parameters
              Std.VectorPoint2f charucoCorners;
              Std.VectorInt charucoIds;
              Aruco.InterpolateCornersCharuco(MarkerCorners[cameraId].At(frame), MarkerIds[cameraId].At(frame), CalibrationImages[cameraId].At(frame),
                charucoBoard, out charucoCorners, out charucoIds);
              charucoImagePoints.PushBack(charucoCorners);

              // Join the object points corresponding to the detected markers
              charucoObjectPoints.PushBack(new Std.VectorPoint3f());
              uint markerCount = charucoIds.Size();
              for (uint marker = 0; marker < markerCount; marker++)
              {
                uint pointId = (uint)charucoIds.At(marker);
                Cv.Point3f objectPoint = charucoBoard.ChessboardCorners.At(pointId);
                charucoObjectPoints.At(frame).PushBack(objectPoint);
              }
            }
            objectPoints[cameraId] = boardObjectPoints;

            // Refine the calibration
            Calibrate(cameraId, charucoObjectPoints, charucoImagePoints, calibrationImageSizes[cameraId], out rvecs,
              out tvecs);
          }

          // Save calibration extrinsic parameters
          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }

        // If ArucoCamera is a stereo camera, apply a stereo calibration and save the resuts in the camera parameters
        if (ArucoCamera is StereoArucoCamera)
        {
          CameraParametersController.CameraParameters.StereoCameraParameters = new StereoCameraParameters();
          StereoCalibrate(stereoCameraId1, stereoCameraId2, objectPoints, imagePoints, calibrationImageSizes,
            CameraParametersController.CameraParameters.StereoCameraParameters);
        }

        // Save the camera parameters
        CameraParametersController.CameraParametersFilename = ArucoCamera.Name + " - "
          + CameraParametersController.CameraParameters.CalibrationDateTime + ".xml";
        CameraParametersController.Save();

        // Update state
        calibratingMutex.WaitOne();
        {
          IsCalibrated = true;
        }
        calibratingMutex.ReleaseMutex();
      }

      /// <summary>
      /// Initializes and configure the <see cref="CameraParametersController.CameraParameters"/>.
      /// </summary>
      /// <param name="calibrationFlags">The calibration flags that will be used in <see cref="Calibrate"/>.</param>
      protected virtual void InitializeCameraParameters(CameraCalibrationFlags calibrationFlags = null)
      {
        if (calibrationFlags != null && calibrationFlags.UseIntrinsicGuess)
        {
          if (CameraParametersController.CameraParameters == null || CameraParametersController.CameraParameters.CameraMatrices == null)
          {
            throw new Exception("CalibrationFlags.UseIntrinsicGuess flag is set but CameraParameters is null or has no valid values. Set" +
              " CameraParametersFilename or deactivate this flag.");
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

        CameraParametersController.CameraParameters.CalibrationFlagsValue = (calibrationFlags != null) ? calibrationFlags.CalibrationFlagsValue : default(int);

        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          CameraParametersController.CameraParameters.ImageHeights[cameraId] = calibrationImageSizes[cameraId].Height;
          CameraParametersController.CameraParameters.ImageWidths[cameraId] = calibrationImageSizes[cameraId].Width;
        }
      }

      /// <summary>
      /// Applies a calibration to a camera, saves the intrinsic parameters in <see cref="CameraParametersController.CameraParameters"/> and output
      /// the extrinsic parameters.
      /// </summary>
      /// <param name="cameraId">The id of the camera to calibrate.</param>
      /// <param name="objectPoints">The object points corresponding the imagePoints.</param>
      /// <param name="imagePoints">The detected image points for this camera.</param>
      /// <param name="imageSize">The size of the camera images.</param>
      /// <param name="rvecs">Output rotations for each calibration images.</param>
      /// <param name="tvecs">Output rotations for each calibration images.</param>
      protected abstract void Calibrate(int cameraId, Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Cv.Size imageSize,
        out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs);

      /// <summary>
      /// Applies a stereo calibration to a stereo camera and saves the extrinsincs parameters between the two cameras in the
      /// <see cref="stereoCalibrationCameraPairs"/> argument.
      /// </summary>
      /// <param name="cameraId1">The id of first camera in the camera pair to calibrate.</param>
      /// <param name="cameraId2">The id of second camera in the camera pair to calibrate.</param>
      /// <param name="objectPoints">The object points of the camera pair.</param>
      /// <param name="imagePoints">The detected image points of the camera pair.</param>
      /// <param name="imageSizes">The size of the images of each camera in the camera pair.</param>
      /// <param name="stereoCameraParameters">The parameters containing the results of the stereo calibration.</param>
      protected abstract void StereoCalibrate(int cameraId1, int cameraId2, Std.VectorVectorPoint3f[] objectPoints,
        Std.VectorVectorPoint2f[] imagePoints, Cv.Size[] imageSizes, StereoCameraParameters stereoCameraParameters);
    }
  }

  /// \} aruco_unity_package
}