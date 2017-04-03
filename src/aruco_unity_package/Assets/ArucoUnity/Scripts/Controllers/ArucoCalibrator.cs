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

      public Std.VectorVectorVectorPoint2f[] AllCorners { get; protected set; }

      public Std.VectorVectorInt[] AllIds { get; protected set; }

      public Std.VectorMat[] AllImages { get; protected set; }

      public Std.VectorVectorPoint2f[] AllCharucoCorners { get; protected set; }

      public Std.VectorVectorInt[] AllCharucoIds { get; protected set; }

      public Std.VectorMat[] Rvecs { get; protected set; }

      public Std.VectorMat[] Tvecs { get; protected set; }

      public CameraParameters CameraParameters { get; protected set; }

      public Std.VectorVectorPoint2f[] MarkerCornersCurrentImage { get; protected set; }

      public Std.VectorInt[] MarkerIdsCurrentImage { get; protected set; }

      public bool IsCalibrated { get; protected set; }

      // ArucoDetector Methods

      protected override void PreConfigure()
      {
        if (CalibrationBoard == null)
        {
          throw new ArgumentNullException("ArucoBoard", "ArucoBoard property needs to be set to configure the calibrator.");
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
        AllCorners = new Std.VectorVectorVectorPoint2f[ArucoCamera.CamerasNumber];
        AllIds = new Std.VectorVectorInt[ArucoCamera.CamerasNumber];
        AllImages = new Std.VectorMat[ArucoCamera.CamerasNumber];
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          AllCorners[cameraId] = new Std.VectorVectorVectorPoint2f();
          AllIds[cameraId] = new Std.VectorVectorInt();
          AllImages[cameraId] = new Std.VectorMat();
        }

        AllCharucoCorners = new Std.VectorVectorPoint2f[ArucoCamera.CamerasNumber];
        AllCharucoIds = new Std.VectorVectorInt[ArucoCamera.CamerasNumber];
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

          AllCorners[cameraId].PushBack(MarkerCornersCurrentImage[cameraId]);
          AllIds[cameraId].PushBack(MarkerIdsCurrentImage[cameraId]);
          AllImages[cameraId].PushBack(ArucoCamera.Images[cameraId]);
        }
      }

      public void Calibrate()
      {
        Aruco.CharucoBoard charucoBoard = CalibrationBoard.Board as Aruco.CharucoBoard;

        // Check if there is enough captured frames for calibration
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          // Calibration with a grid board
          if (charucoBoard == null)
          {
            if (AllIds[cameraId].Size() < 1)
            {
              throw new System.Exception("Need at least one frame captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CamerasNumber
                + " to calibrate.");
            }
          }
          // Calibration with a charuco board
          else
          {
            if (AllCharucoIds[cameraId].Size() < 4)
            {
              IsCalibrated = false;
              throw new System.Exception("Need at least four frames captured for the camera " + (cameraId + 1) + "/" + ArucoCamera.CamerasNumber
                + " to calibrate with a ChAruco board.");
            }
          }
        }

        // Prepare camera parameters
        Cv.Core.Mat[] camerasMatrix = new Cv.Core.Mat[ArucoCamera.CamerasNumber],
              distCoeffs = new Cv.Core.Mat[ArucoCamera.CamerasNumber];
        double[] reprojectionErrors = new double[ArucoCamera.CamerasNumber];

        // Calibrate each camera
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          // Prepare camera parameters
          if (CalibrationFlagsController.FixAspectRatio)
          {
            camerasMatrix[cameraId] = new Cv.Core.Mat(3, 3, Cv.Core.Type.CV_64F, new double[9] { CalibrationFlagsController.FixAspectRatioValue, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
          }
          else
          {
            camerasMatrix[cameraId] = new Cv.Core.Mat();
          }
          distCoeffs[cameraId] = new Cv.Core.Mat();

          // Prepare data for calibration
          Std.VectorVectorPoint2f allCornersContenated = new Std.VectorVectorPoint2f();
          Std.VectorInt allIdsContenated = new Std.VectorInt();
          Std.VectorInt markerCounterPerFrame = new Std.VectorInt();

          uint allCornersSize = AllCorners[cameraId].Size();
          markerCounterPerFrame.Reserve(allCornersSize);
          for (uint i = 0; i < allCornersSize; i++)
          {
            Std.VectorVectorPoint2f allCornersI = AllCorners[cameraId].At(i);
            uint allCornersISize = allCornersI.Size();
            markerCounterPerFrame.PushBack((int)allCornersISize);
            for (uint j = 0; j < allCornersISize; j++)
            {
              allCornersContenated.PushBack(allCornersI.At(j));
              allIdsContenated.PushBack(AllIds[cameraId].At(i).At(j));
            }
          }

          // Calibrate camera with aruco
          Cv.Core.Size imageSize = ArucoCamera.Images[cameraId].size;
          Std.VectorMat rvecs, tvecs;
          reprojectionErrors[cameraId] = Aruco.CalibrateCameraAruco(allCornersContenated, allIdsContenated, markerCounterPerFrame,
            CalibrationBoard.Board, imageSize, camerasMatrix[cameraId], distCoeffs[cameraId], out rvecs, out tvecs, CalibrationFlagsController.CalibrationFlags, CalibrationTermCriteria);

          // If the used board is a charuco board, refine the calibration
          if (charucoBoard != null)
          {
            AllCharucoCorners[cameraId] = new Std.VectorVectorPoint2f();
            AllCharucoIds[cameraId] = new Std.VectorVectorInt();

            // Interpolate charuco corners using camera parameters
            for (uint i = 0; i < AllIds[cameraId].Size(); i++)
            {
              Std.VectorPoint2f charucoCorners;
              Std.VectorInt charucoIds;
              Aruco.InterpolateCornersCharuco(AllCorners[cameraId].At(i), AllIds[cameraId].At(i), AllImages[cameraId].At(i), charucoBoard, out charucoCorners, out charucoIds);

              AllCharucoCorners[cameraId].PushBack(charucoCorners);
              AllCharucoIds[cameraId].PushBack(charucoIds);
            }

            // Calibrate camera using charuco
            reprojectionErrors[cameraId] = Aruco.CalibrateCameraCharuco(AllCharucoCorners[cameraId], AllCharucoIds[cameraId], charucoBoard,
              imageSize, camerasMatrix[cameraId], distCoeffs[cameraId], out rvecs, out tvecs, CalibrationFlagsController.CalibrationFlags, CalibrationTermCriteria);
          }

          // Save calibration parameters
          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }

        IsCalibrated = true;

        // Create camera parameters
        CameraParameters = new CameraParameters(ArucoCamera.CamerasNumber)
        {
          CalibrationFlags = (int)CalibrationFlagsController.CalibrationFlags,
          FixAspectRatioValue = CalibrationFlagsController.FixAspectRatioValue,
          ReprojectionError = reprojectionErrors,
          CamerasMatrix = camerasMatrix,
          DistCoeffs = distCoeffs
        };
        for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
        {
          CameraParameters.ImagesHeight[cameraId] = ArucoCamera.ImageTextures[cameraId].height;
          CameraParameters.ImagesWidth[cameraId] = ArucoCamera.ImageTextures[cameraId].width;
        }

        // Save camera parameters
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