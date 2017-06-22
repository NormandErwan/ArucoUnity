using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CalibrationFlagsControllers;
using ArucoUnity.Controllers.Utility;
using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Calibrate a <see cref="ArucoObjectDetector.ArucoCamera"/> camera system with a <see cref="ArucoBoard"/>, and save the calibration results in a file to be used
    /// for tracking.
    /// 
    /// See the OpenCV documentation for more information about the calibration: http://docs.opencv.org/3.2.0/da/d13/tutorial_aruco_calibration.html
    /// </summary>
    public class ArucoCalibrator : ArucoObjectDetector
    {
      // Editor fields

      [Header("Calibration")]
      [SerializeField]
      [Tooltip("The ArUco board to use for calibration.")]
      private ArucoBoard calibrationBoard;

      [SerializeField]
      [Tooltip("Use a refine algorithm to find not detected markers based on the already detected and the board layout (if using a board).")]
      private bool refineMarkersDetection = false;

      [SerializeField]
      [Tooltip("The calibration flags to use.")]
      private CalibrationFlagsController calibrationFlagsController;

      [SerializeField]
      [Tooltip("The folder for the calibration files, relative to the Application.persistentDataPath folder.")]
      private string calibrationFolder = "ArucoUnity/Calibrations/";

      [SerializeField]
      [Tooltip("The xml calibration file to load as a guess, and where the calibration parameters will be saved. If empty, it will be generated"
        + " automatically from the camera name and the current datetime.")]
      private string calibrationFilename;

      [Header("Stereo Calibration")]
      [SerializeField]
      [Tooltip("The list of the camera pairs on which apply a stereo calibration.")]
      private StereoCalibrationCameraPair[] stereoCalibrationCameraPairs;

      // Properties

      /// <summary>
      /// The ArUco board to use for calibration.
      /// </summary>
      public ArucoBoard CalibrationBoard { get { return calibrationBoard; } set { calibrationBoard = value; } }

      /// <summary>
      /// Use a refine algorithm to find not detected markers based on the already detected and the board layout.
      /// </summary>
      public bool RefineMarkersDetection { get { return refineMarkersDetection; } set { refineMarkersDetection = value; } }

      /// <summary>
      /// The calibration flags to use.
      /// </summary>
      public CalibrationFlagsController CalibrationFlagsController { get { return calibrationFlagsController; } set { calibrationFlagsController = value; } }

      /// <summary>
      /// The folder for the calibration files, relative to the Application.persistentDataPath folder.
      /// </summary>
      public string CalibrationFolder { get { return calibrationFolder; } set { calibrationFolder = value; } }

      /// <summary>
      /// The xml calibration file to load as a guess, and where the calibration parameters will be saved. The extension (.xml) is added
      /// automatically. If empty, it will be generated automatically from the camera name and the current datetime.
      /// </summary>
      public string CalibrationFilename { get { return calibrationFilename; } set { calibrationFilename = value; } }

      /// <summary>
      /// The list of the camera pairs on which apply a stereo calibration.
      /// </summary>
      public StereoCalibrationCameraPair[] StereoCalibrationCameraPairs { get { return stereoCalibrationCameraPairs; } set { stereoCalibrationCameraPairs = value; } }

      /// <summary>
      /// The detected marker corners for each camera.
      /// </summary>
      public Std.VectorVectorVectorPoint2f[] MarkerCorners { get; protected set; }

      /// <summary>
      /// The detected marker ids for each camera.
      /// </summary>
      public Std.VectorVectorInt[] MarkerIds { get; protected set; }

      /// <summary>
      /// The images to use for the calibration.
      /// </summary>
      public Std.VectorMat[] CameraImages { get; protected set; }

      /// <summary>
      /// The estimated rotation vector for each detected markers in each camera.
      /// </summary>
      public Std.VectorVec3d[] Rvecs { get; protected set; }

      /// <summary>
      /// The estimated translation vector for each detected markers in each camera.
      /// </summary>
      public Std.VectorVec3d[] Tvecs { get; protected set; }

      /// <summary>
      /// The calibration results.
      /// </summary>
      public CameraParameters CameraParameters { get; protected set; }

      /// <summary>
      /// The detected marker corners on the current images of each camera.
      /// </summary>
      public Std.VectorVectorPoint2f[] MarkerCornersCurrentImage { get; protected set; }

      /// <summary>
      /// The detected marker ids on the current images of each camera.
      /// </summary>
      public Std.VectorInt[] MarkerIdsCurrentImage { get; protected set; }

      /// <summary>
      /// If the calibration has been done for the camera system.
      /// </summary>
      public bool IsCalibrated { get; protected set; }

      // Variables

      CalibrationFlagsPinholeController calibrationFlagsPinholeController;
      CalibrationFlagsOmnidirController calibrationFlagsOmnidirController;

      // ArucoDetector Methods

      /// <summary>
      /// Check if the properties are properly set and and reset the calibration.
      /// </summary>
      protected override void PreConfigure()
      {
        // Check for the board
        if (CalibrationBoard == null)
        {
          throw new ArgumentNullException("CalibrationBoard", "This property needs to be set to configure the calibrator.");
        }

        // Check for the calibration flags
        calibrationFlagsPinholeController = CalibrationFlagsController as CalibrationFlagsPinholeController;
        calibrationFlagsOmnidirController = CalibrationFlagsController as CalibrationFlagsOmnidirController;
        if (CalibrationFlagsController == null 
          || (calibrationFlagsPinholeController == null && calibrationFlagsOmnidirController == null))
        {
          throw new ArgumentNullException("CalibrationFlagsController", "This property needs to be set to configure the calibrator.");
        }
        else if (ArucoCamera.UndistortionType == UndistortionType.Pinhole && calibrationFlagsPinholeController == null)
        {
          throw new ArgumentException("CalibrationFlagsController", "The camera is set for the pinhole undistortion, but the calibration flags are"
            + " not. Use CalibrationFlagsPinholeController instead.");
        }
        else if (new[] { UndistortionType.OmnidirPerspective, UndistortionType.OmnidirCylindrical, UndistortionType.OmnidirLonglati,
          UndistortionType.OmnidirStereographic }.Contains(ArucoCamera.UndistortionType) && calibrationFlagsOmnidirController == null)
        {
          throw new ArgumentException("CalibrationFlagsController", "The camera is set for an omnidir undistortion, but the calibration flags are"
            + " not. Use CalibrationFlagsOmnidirController instead.");
        }

        // Check for the stereo calibration properties
        foreach (var stereoCameraPair in StereoCalibrationCameraPairs)
        {
          stereoCameraPair.PropertyCheck(ArucoCamera);
        }

        ResetCalibration();
      }

      /// <summary>
      /// Detect and draw the ArUco markers on the current images of the cameras.
      /// </summary>
      protected override void ArucoCamera_ImagesUpdated()
      {
        if (!IsConfigured || !IsStarted)
        {
          return;
        }

        Detect();
        Draw();
      }

      // Methods

      /// <summary>
      /// Reset the properties.
      /// </summary>
      public void ResetCalibration()
      {
        MarkerCorners = new Std.VectorVectorVectorPoint2f[ArucoCamera.CameraNumber];
        MarkerIds = new Std.VectorVectorInt[ArucoCamera.CameraNumber];
        CameraImages = new Std.VectorMat[ArucoCamera.CameraNumber];
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          MarkerCorners[cameraId] = new Std.VectorVectorVectorPoint2f();
          MarkerIds[cameraId] = new Std.VectorVectorInt();
          CameraImages[cameraId] = new Std.VectorMat();
        }
        
        Rvecs = new Std.VectorVec3d[ArucoCamera.CameraNumber];
        Tvecs = new Std.VectorVec3d[ArucoCamera.CameraNumber];
        MarkerCornersCurrentImage = new Std.VectorVectorPoint2f[ArucoCamera.CameraNumber];
        MarkerIdsCurrentImage = new Std.VectorInt[ArucoCamera.CameraNumber];

        CameraParameters = null;
        IsCalibrated = false;
      }

      /// <summary>
      /// Detect the Aruco markers on the current images of the cameras and store the results in the <see cref="MarkerCornersCurrentImage"/> and
      /// <see cref="MarkerIdsCurrentImage"/> properties.
      /// </summary>
      public void Detect()
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
      /// Draw the detected ArUco markers on the current images of the cameras.
      /// </summary>
      public void Draw()
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
      /// Add the current images of the cameras and the detected corners for the calibration.
      /// </summary>
      public void AddCurrentFrameForCalibration()
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
          CameraImages[cameraId].PushBack(ArucoCamera.Images[cameraId]);
        }
      }

      /// <summary>
      /// Calibrate each camera of the <see cref="ArucoObjectDetector.ArucoCamera"/> system using the detected markers added with
      /// <see cref="AddCurrentFrameForCalibration()"/>, the <see cref="CameraParameters"/>, the <see cref="CalibrationFlagsController"/> and save
      /// the results on a calibration file. Stereo calibrations will be additionally executed on these results for every camera pair in
      /// <see cref="StereoCalibrationCameraPairs"/>.
      /// </summary>
      public void Calibrate()
      {
        // Prepare data
        Aruco.CharucoBoard charucoBoard = CalibrationBoard.Board as Aruco.CharucoBoard;

        // Check if there is enough captured frames for calibration
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          if (charucoBoard == null && MarkerIds[cameraId].Size() < 1)
          {
            throw new Exception("Need at least one frame captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CameraNumber
              + " to calibrate.");
          }
          else if (charucoBoard != null && MarkerIds[cameraId].Size() < 4)
          {
            throw new Exception("Need at least four frames captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CameraNumber
              + " to calibrate with a ChAruco board.");
          }
        }

        // Load the camera parameters if they exist
        string cameraParametersFilePath;
        string cameraParametersFolderPath = Path.Combine((Application.isEditor) ? Application.dataPath : Application.persistentDataPath, CalibrationFolder);
        if (!Directory.Exists(cameraParametersFolderPath))
        {
          Directory.CreateDirectory(cameraParametersFolderPath);
        }
        if (CalibrationFilename != null && CalibrationFilename.Length > 0)
        {
          cameraParametersFilePath = cameraParametersFolderPath + CalibrationFilename;
          CameraParameters = CameraParameters.LoadFromXmlFile(cameraParametersFilePath);

          if (CameraParameters.CameraNumber != ArucoCamera.CameraNumber)
          {
            throw new ArgumentException("The loaded camera parameters from the file '" + cameraParametersFilePath + "' is for a system with " +
              CameraParameters.CameraNumber + " camera. But the current calibrating camera has " + ArucoCamera.CameraNumber + ". These numbers"
              + " must be equal.", "CalibrationFilename");
          }
        }
        // Or initialize the camera parameters
        else
        {
          CameraParameters = new CameraParameters(ArucoCamera.CameraNumber)
          {
            CalibrationFlagsValue = CalibrationFlagsController.CalibrationFlagsValue,
            FixAspectRatioValue = (calibrationFlagsPinholeController) ? calibrationFlagsPinholeController.FixAspectRatioValue : 0
          };
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            CameraParameters.ImageHeights[cameraId] = ArucoCamera.ImageTextures[cameraId].height;
            CameraParameters.ImageWidths[cameraId] = ArucoCamera.ImageTextures[cameraId].width;

            double cameraMatrixAspectRatio = (calibrationFlagsPinholeController && calibrationFlagsPinholeController.FixAspectRatio) 
              ? calibrationFlagsPinholeController.FixAspectRatioValue : 1.0;
            CameraParameters.CameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F,
              new double[9] { cameraMatrixAspectRatio, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
            CameraParameters.DistCoeffs[cameraId] = new Cv.Mat();
            CameraParameters.OmnidirXis[cameraId] = new Cv.Mat();
          }
        }

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
          Cv.Size imageSize = ArucoCamera.Images[cameraId].Size;
          Std.VectorVec3d rvecs, tvecs;
          if (calibrationFlagsPinholeController)
          {
            CameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(boardObjectPoints, boardImagePoints, imageSize,
              CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs,
              calibrationFlagsPinholeController.CalibrationFlags);
          }
          else if (calibrationFlagsOmnidirController)
          {
            CameraParameters.ReprojectionErrors[cameraId] = Cv.Omnidir.Calibrate(boardObjectPoints, boardImagePoints, imageSize,
              CameraParameters.CameraMatrices[cameraId], CameraParameters.OmnidirXis[cameraId], CameraParameters.DistCoeffs[cameraId], out rvecs,
              out tvecs, calibrationFlagsOmnidirController.CalibrationFlags);
          }
          else
          {
            rvecs = new Std.VectorVec3d();
            tvecs = new Std.VectorVec3d();
          }

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
              Aruco.InterpolateCornersCharuco(MarkerCorners[cameraId].At(frame), MarkerIds[cameraId].At(frame), CameraImages[cameraId].At(frame),
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
            if (calibrationFlagsPinholeController)
            {
              CameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(charucoObjectPoints, charucoImagePoints, imageSize,
                CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs,
                calibrationFlagsPinholeController.CalibrationFlags);
            }
            else if (calibrationFlagsOmnidirController)
            {
              CameraParameters.ReprojectionErrors[cameraId] = Cv.Omnidir.Calibrate(boardObjectPoints, boardImagePoints, imageSize,
                CameraParameters.CameraMatrices[cameraId], CameraParameters.OmnidirXis[cameraId], CameraParameters.DistCoeffs[cameraId], out rvecs,
                out tvecs, calibrationFlagsOmnidirController.CalibrationFlags);
            }
          }

          // Save calibration extrinsic parameters
          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }

        // If required, apply a stereo calibration and save the resuts in the camera parameters
        CameraParameters.StereoCameraParametersList = new StereoCameraParameters[StereoCalibrationCameraPairs.Length];
        for (int i = 0; i < StereoCalibrationCameraPairs.Length; i++)
        {
          CameraParameters.StereoCameraParametersList[i] = StereoCalibrationCameraPairs[i].Calibrate(ArucoCamera, CameraParameters, objectPoints, imagePoints);
        }

        IsCalibrated = true;

        // Save the camera parameters
        cameraParametersFilePath = cameraParametersFolderPath;
        if (CalibrationFilename != null && CalibrationFilename.Length > 0)
        {
          cameraParametersFilePath += CalibrationFilename;
        }
        else
        {
          cameraParametersFilePath += ArucoCamera.Name + " - " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xml";
        }
        CameraParameters.SaveToXmlFile(cameraParametersFilePath);
      }
    }
  }

  /// \} aruco_unity_package
}