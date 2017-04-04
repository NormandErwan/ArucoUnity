using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CalibrationFlagsControllers;
using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using System;
using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    public class ArucoCalibrator : ArucoObjectDetector
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The ArUco board to use for calibrate.")]
      private ArucoBoard calibrationBoard;

      [SerializeField]
      private bool refineMarkersDetection = false;

      [SerializeField]
      private CalibrationFlagsBaseController calibrationFlagsController;

      [SerializeField]
      [Tooltip("The output folder for the calibration files, relative to the Application.persistentDataPath folder.")]
      private string outputFolder = "ArucoUnity/Calibrations/";

      [SerializeField]
      [Tooltip("The saved calibration name. The extension (.xml) is added automatically. If empty, it will be generated automatically from the "
        + "camera name and the current datetime.")]
      private string calibrationFilename;

      // Properties

      public ArucoBoard CalibrationBoard { get { return calibrationBoard; } set { calibrationBoard = value; } }

      public bool RefineMarkersDetection { get { return refineMarkersDetection; } set { refineMarkersDetection = value; } }

      public CalibrationFlagsBaseController CalibrationFlagsController { get { return calibrationFlagsController; } set { calibrationFlagsController = value; } }

      /// <summary>
      /// The output folder for the calibration files, relative to the Application.persistentDataPath folder.
      /// </summary>
      public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }

      /// <summary>
      /// The saved calibration name. The extension (.xml) is added automatically. If empty, it will be generated automatically from the camera name
      /// and the current datetime.
      /// </summary>
      public string CalibrationFilename { get { return calibrationFilename; } set { calibrationFilename = value; } }

      public Cv.Core.TermCriteria CalibrationTermCriteria { get; set; }

      public Std.VectorVectorVectorPoint2f[] MarkerCorners { get; protected set; }

      public Std.VectorVectorInt[] MarkerIds { get; protected set; }

      public Std.VectorMat[] CameraImages { get; protected set; }

      public Std.VectorMat[] Rvecs { get; protected set; }

      public Std.VectorMat[] Tvecs { get; protected set; }

      public CameraParameters CameraParameters { get; protected set; }

      public Std.VectorVectorPoint2f[] MarkerCornersCurrentImage { get; protected set; }

      public Std.VectorInt[] MarkerIdsCurrentImage { get; protected set; }

      public bool IsCalibrated { get; protected set; }

      // Variables

      CalibrationFlagsController calibrationFlagsNonFisheyeController;
      CalibrationFlagsFisheyeController calibrationFlagsFisheyeController;

      // ArucoDetector Methods

      protected override void PreConfigure()
      {
        if (CalibrationBoard == null)
        {
          throw new ArgumentNullException("CalibrationBoard", "The CalibrationBoard property needs to be set to configure the calibrator.");
        }

        calibrationFlagsNonFisheyeController = CalibrationFlagsController as CalibrationFlagsController;
        calibrationFlagsFisheyeController = CalibrationFlagsController as CalibrationFlagsFisheyeController;
        if (CalibrationFlagsController == null || calibrationFlagsNonFisheyeController == null || calibrationFlagsFisheyeController == null)
        {
          throw new ArgumentNullException("CalibrationFlagsController", "The CalibrationFlagsController property needs to be set to configure the calibrator.");
        }
        if (!ArucoCamera.IsFisheye && calibrationFlagsNonFisheyeController == null)
        {
          throw new ArgumentNullException("CalibrationFlagsController", "The camera used if non fisheye, but the calibration flags are for fisheye camera. Use CalibrationFlagsController instead.");
        }
        if (ArucoCamera.IsFisheye && calibrationFlagsFisheyeController == null)
        {
          throw new ArgumentNullException("CalibrationFlagsController", "The camera used if fisheye, but the calibration flags are for non-fisheye camera. Use CalibrationFlagsFisheyeController instead.");
        }

        ResetCalibration();
      }

      protected override void ArucoCameraImageUpdated()
      {
        if (!IsConfigured || !IsStarted)
        {
          return;
        }

        Detect();
        Draw();
      }

      // Methods

      public void ResetCalibration()
      {
        MarkerCorners = new Std.VectorVectorVectorPoint2f[ArucoCamera.CamerasNumber];
        MarkerIds = new Std.VectorVectorInt[ArucoCamera.CamerasNumber];
        CameraImages = new Std.VectorMat[ArucoCamera.CamerasNumber];
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          MarkerCorners[cameraId] = new Std.VectorVectorVectorPoint2f();
          MarkerIds[cameraId] = new Std.VectorVectorInt();
          CameraImages[cameraId] = new Std.VectorMat();
        }
        
        Rvecs = new Std.VectorMat[ArucoCamera.CamerasNumber];
        Tvecs = new Std.VectorMat[ArucoCamera.CamerasNumber];
        MarkerCornersCurrentImage = new Std.VectorVectorPoint2f[ArucoCamera.CamerasNumber];
        MarkerIdsCurrentImage = new Std.VectorInt[ArucoCamera.CamerasNumber];

        CameraParameters = null;
        IsCalibrated = false;
      }

      public void Detect()
      {
        if (!IsConfigured)
        {
          return;
        }

        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          Std.VectorInt markerIds;
          Std.VectorVectorPoint2f markerCorners, rejectedCandidateCorners;

          Cv.Core.Mat image = ArucoCamera.Images[cameraId];

          Aruco.DetectMarkers(image, CalibrationBoard.Dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);

          MarkerCornersCurrentImage[cameraId] = markerCorners;
          MarkerIdsCurrentImage[cameraId] = markerIds;

          if (RefineMarkersDetection)
          {
            Aruco.RefineDetectedMarkers(image, CalibrationBoard.Board, MarkerCornersCurrentImage[cameraId], MarkerIdsCurrentImage[cameraId], rejectedCandidateCorners);
          }
        }
      }

      public void Draw()
      {
        if (!IsConfigured)
        {
          return;
        }

        bool updatedCameraImage = false;
        Cv.Core.Mat[] cameraImages = ArucoCamera.Images;

        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          if (MarkerIdsCurrentImage[cameraId] != null && MarkerIdsCurrentImage[cameraId].Size() > 0)
          {
            Aruco.DrawDetectedMarkers(cameraImages[cameraId], MarkerCornersCurrentImage[cameraId], MarkerIdsCurrentImage[cameraId]);
            updatedCameraImage = true;
          }
        }

        if (updatedCameraImage)
        {
          ArucoCamera.Images = cameraImages;
        }
      }

      public void AddFrameForCalibration()
      {
        if (!IsConfigured || IsCalibrated)
        {
          return;
        }

        Cv.Core.Mat[] cameraImages = ArucoCamera.Images;
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          if (MarkerIdsCurrentImage[cameraId] != null && MarkerIdsCurrentImage[cameraId].Size() < 1)
          {
            Debug.LogWarning("Not enough markers detected for the camera " + (cameraId + 1) + "/" + ArucoCamera.CamerasNumber + " to add the frame for "
              + "calibration. It requires more than one marker detected.");
            continue;
          }

          MarkerCorners[cameraId].PushBack(MarkerCornersCurrentImage[cameraId]);
          MarkerIds[cameraId].PushBack(MarkerIdsCurrentImage[cameraId]);
          CameraImages[cameraId].PushBack(ArucoCamera.Images[cameraId]);
        }
      }

      public void Calibrate()
      {
        Aruco.CharucoBoard charucoBoard = CalibrationBoard.Board as Aruco.CharucoBoard;

        // Check if there is enough captured frames for calibration
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          if (charucoBoard == null && MarkerIds[cameraId].Size() < 1)
          {
            throw new Exception("Need at least one frame captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CamerasNumber
              + " to calibrate.");
          }
          else if (charucoBoard == null && MarkerIds[cameraId].Size() < 4)
          {
            throw new Exception("Need at least four frames captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CamerasNumber
              + " to calibrate with a ChAruco board.");
          }
        }

        // Prepare the camera parameters
        // TODO : check if the camera parameters file can be read and load it for intrinsic guessing
        Cv.Core.Mat[] camerasMatrix = new Cv.Core.Mat[ArucoCamera.CamerasNumber], 
                      distCoeffs = new Cv.Core.Mat[ArucoCamera.CamerasNumber];
        double[] reprojectionErrors = new double[ArucoCamera.CamerasNumber];

        // Calibrate each camera
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          // Prepare the camera parameters
          if (!ArucoCamera.IsFisheye && calibrationFlagsNonFisheyeController.FixAspectRatio)
          {
            camerasMatrix[cameraId] = new Cv.Core.Mat(3, 3, Cv.Core.Type.CV_64F, new double[9] {
              calibrationFlagsNonFisheyeController.FixAspectRatioValue, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
          }
          else
          {
            camerasMatrix[cameraId] = new Cv.Core.Mat();
          }
          distCoeffs[cameraId] = new Cv.Core.Mat();

          // Prepare data for calibration
          Std.VectorVectorPoint3f boardOjectPoints = new Std.VectorVectorPoint3f();
          Std.VectorVectorPoint2f boardImagePoints = new Std.VectorVectorPoint2f();
          uint frameCount = MarkerCorners[cameraId].Size();
          for (uint frame = 0; frame < frameCount; frame++)
          {
            Std.VectorPoint3f frameObjectPoints;
            Std.VectorPoint2f frameImagePoints;
            Aruco.GetBoardObjectAndImagePoints(CalibrationBoard.Board, MarkerCorners[cameraId].At(frame), MarkerIds[cameraId].At(frame),
              out frameObjectPoints, out frameImagePoints);
            boardOjectPoints.PushBack(frameObjectPoints);
            boardImagePoints.PushBack(frameImagePoints);
          }

          // Calibrate the camera
          Cv.Core.Size imageSize = ArucoCamera.Images[cameraId].size;
          Std.VectorMat rvecs, tvecs;
          if (!ArucoCamera.IsFisheye)
          {
            reprojectionErrors[cameraId] = Cv.Calib3d.CalibrateCamera(boardOjectPoints, boardImagePoints, imageSize, camerasMatrix[cameraId],
              distCoeffs[cameraId], out rvecs, out tvecs, calibrationFlagsNonFisheyeController.CalibrationFlags, CalibrationTermCriteria);
          }
          else
          {
            reprojectionErrors[cameraId] = Cv.Calib3d.Fisheye.Calibrate(boardOjectPoints, boardImagePoints, imageSize, camerasMatrix[cameraId],
              distCoeffs[cameraId], out rvecs, out tvecs, calibrationFlagsFisheyeController.CalibrationFlags, CalibrationTermCriteria);
          }

          // If the used board is a charuco board, refine the calibration
          if (charucoBoard != null)
          {
            // Prepare the data for calibration
            Std.VectorVectorPoint3f charucoOjectPoints = new Std.VectorVectorPoint3f();
            Std.VectorVectorPoint2f charucoImagePoints = new Std.VectorVectorPoint2f();
            for (uint frame = 0; frame < frameCount; frame++)
            {
              // Interpolate charuco corners using camera parameters
              Std.VectorPoint2f charucoCorners;
              Std.VectorInt charucoIds;
              Aruco.InterpolateCornersCharuco(MarkerCorners[cameraId].At(frame), MarkerIds[cameraId].At(frame),
                CameraImages[cameraId].At(frame), charucoBoard, out charucoCorners, out charucoIds);
              charucoImagePoints.PushBack(charucoCorners);

              // Join the object points corresponding to the detected markers
              charucoOjectPoints.PushBack(new Std.VectorPoint3f());
              uint markerCount = charucoIds.Size();
              for (uint marker = 0; marker < markerCount; marker++)
              {
                uint pointId = (uint)charucoIds.At(marker);
                Cv.Core.Point3f objectPoint = charucoBoard.chessboardCorners.At(pointId);
                charucoOjectPoints.At(frame).PushBack(objectPoint);
              }
            }

            // Refine the calibration
            if (!ArucoCamera.IsFisheye)
            {
              reprojectionErrors[cameraId] = Cv.Calib3d.CalibrateCamera(charucoOjectPoints, charucoImagePoints, imageSize, camerasMatrix[cameraId],
                distCoeffs[cameraId], out rvecs, out tvecs, calibrationFlagsNonFisheyeController.CalibrationFlags, CalibrationTermCriteria);
            }
            else
            {
              reprojectionErrors[cameraId] = Cv.Calib3d.Fisheye.Calibrate(boardOjectPoints, boardImagePoints, imageSize, camerasMatrix[cameraId],
                distCoeffs[cameraId], out rvecs, out tvecs, calibrationFlagsFisheyeController.CalibrationFlags, CalibrationTermCriteria);
            }
          }

          // Save calibration parameters
          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }

        IsCalibrated = true;

        // Create the camera parameters
        CameraParameters = new CameraParameters(ArucoCamera.CamerasNumber)
        {
          CalibrationFlagsValue = CalibrationFlagsController.CalibrationFlagsValue,
          FixAspectRatioValue = (!ArucoCamera.IsFisheye) ? calibrationFlagsNonFisheyeController.FixAspectRatioValue : 0,
          ReprojectionError = reprojectionErrors,
          CamerasMatrix = camerasMatrix,
          DistCoeffs = distCoeffs
        };
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          CameraParameters.ImagesHeight[cameraId] = ArucoCamera.ImageTextures[cameraId].height;
          CameraParameters.ImagesWidth[cameraId] = ArucoCamera.ImageTextures[cameraId].width;
        }

        // Save the camera parameters
        string outputFolderPath = Path.Combine((Application.isEditor) ? Application.dataPath : Application.persistentDataPath, OutputFolder);
        if (!Directory.Exists(outputFolderPath))
        {
          Directory.CreateDirectory(outputFolderPath);
        }

        string calibrationFilePath = outputFolderPath;
        calibrationFilePath += (CalibrationFilename == null || CalibrationFilename.Length == 0)
          ? ArucoCamera.Name + " - " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")
          : CalibrationFilename;
        calibrationFilePath += ".xml";
        CameraParameters.SaveToXmlFile(calibrationFilePath);
      }
    }
  }

  /// \} aruco_unity_package
}