using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using System;
using System.IO;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  public class ArucoCalibrator : ArucoObjectDetector
  {
    // Constants

    protected const float DEFAULT_FIX_ASPECT_RATIO = 1f;

    // Editor fields

    [SerializeField]
    [Tooltip("The ArUco board to use for calibrate.")]
    private ArucoBoard arucoBoard;

    [SerializeField]
    private bool applyRefineStrategy = false;

    [SerializeField]
    private bool assumeZeroTangentialDistorsion = false;

    [SerializeField]
    private float fixAspectRatio;

    [SerializeField]
    private bool fixPrincipalPointAtCenter = false;

    [SerializeField]
    [Tooltip("The output folder for the calibration files, relative to the Application.persistentDataPath folder.")]
    private string outputFolder = "ArucoUnity/Calibrations/";

    [SerializeField]
    [Tooltip("The saved calibration name. The extension (.xml) is added automatically. If empty, it will be generated automatically from the "
      + "camera name and the current datetime.")]
    private string calibrationFilename;

    // Properties

    public ArucoBoard ArucoBoard { get { return arucoBoard; } set { arucoBoard = value; } }

    public bool ApplyRefineStrategy { get { return applyRefineStrategy; } set { applyRefineStrategy = value; } }

    public bool AssumeZeroTangentialDistorsion 
    {
      get { return assumeZeroTangentialDistorsion; }
      set
      {
        assumeZeroTangentialDistorsion = value;
        UpdateCalibrationFlags();
      }
    }

    public float FixAspectRatio 
    {
      get { return fixAspectRatio; }
      set
      {
        fixAspectRatio = value;
        UpdateCalibrationFlags();
      }
    }

    public bool FixPrincipalPointAtCenter 
    {
      get { return fixPrincipalPointAtCenter; }
      set
      {
        fixPrincipalPointAtCenter = value;
        UpdateCalibrationFlags();
      }
    }

    public CALIB CalibrationFlags 
    {
      get { return calibrationFlags; }
      set
      {
        calibrationFlags = value;
        UpdateCalibrationOptions();
      }
    }

    /// <summary>
    /// The output folder for the calibration files, relative to the Application.persistentDataPath folder.
    /// </summary>
    public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }

    /// <summary>
    /// The saved calibration name. The extension (.xml) is added automatically. If empty, it will be generated automatically from the camera name
    /// and the current datetime.
    /// </summary>
    public string CalibrationFilename { get { return calibrationFilename; } set { calibrationFilename = value; } }

    public TermCriteria CalibrationTermCriteria { get; set; }

    public VectorVectorVectorPoint2f[] AllCorners { get; protected set; }

    public VectorVectorInt[] AllIds { get; protected set; }

    public VectorMat[] AllImages { get; protected set; }

    public VectorVectorPoint2f[] AllCharucoCorners { get; protected set; }

    public VectorVectorInt[] AllCharucoIds { get; protected set; }

    public VectorMat[] Rvecs { get; protected set; }

    public VectorMat[] Tvecs { get; protected set; }

    public CameraParameters CameraParameters { get; protected set; }

    public VectorVectorPoint2f[] MarkerCornersCurrentImage { get; protected set; }

    public VectorInt[] MarkerIdsCurrentImage { get; protected set; }

    public bool IsCalibrated { get; protected set; }

    // Variables

    private CALIB calibrationFlags;

    // MonoBehaviour methods

    protected override void Awake()
    {
      base.Awake();

      if (CalibrationTermCriteria == null)
      {
        CalibrationTermCriteria = new TermCriteria(TermCriteria.Type.COUNT | TermCriteria.Type.EPS, 100, 1E-5);
      }

      UpdateCalibrationFlags();
    }

    // ArucoDetector Methods

    protected override void PreConfigure()
    {
      if (ArucoBoard == null)
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
      AllCorners = new VectorVectorVectorPoint2f[ArucoCamera.CamerasNumber];
      AllIds = new VectorVectorInt[ArucoCamera.CamerasNumber];
      AllImages = new VectorMat[ArucoCamera.CamerasNumber];
      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        AllCorners[cameraId] = new VectorVectorVectorPoint2f();
        AllIds[cameraId] = new VectorVectorInt();
        AllImages[cameraId] = new VectorMat();
      }

      AllCharucoCorners = new VectorVectorPoint2f[ArucoCamera.CamerasNumber];
      AllCharucoIds = new VectorVectorInt[ArucoCamera.CamerasNumber];
      Rvecs = new VectorMat[ArucoCamera.CamerasNumber];
      Tvecs = new VectorMat[ArucoCamera.CamerasNumber];
      MarkerCornersCurrentImage = new VectorVectorPoint2f[ArucoCamera.CamerasNumber];
      MarkerIdsCurrentImage = new VectorInt[ArucoCamera.CamerasNumber];

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
        VectorInt markerIds;
        VectorVectorPoint2f markerCorners, rejectedCandidateCorners;

        Mat image = ArucoCamera.Images[cameraId];

        Functions.DetectMarkers(image, ArucoBoard.Dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);

        MarkerCornersCurrentImage[cameraId] = markerCorners;
        MarkerIdsCurrentImage[cameraId] = markerIds;

        if (ApplyRefineStrategy)
        {
          Functions.RefineDetectedMarkers(image, ArucoBoard.Board, MarkerCornersCurrentImage[cameraId], MarkerIdsCurrentImage[cameraId], rejectedCandidateCorners);
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
      Mat[] cameraImages = ArucoCamera.Images;

      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        if (MarkerIdsCurrentImage[cameraId] != null && MarkerIdsCurrentImage[cameraId].Size() > 0)
        {
          Functions.DrawDetectedMarkers(cameraImages[cameraId], MarkerCornersCurrentImage[cameraId], MarkerIdsCurrentImage[cameraId]);
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

      Mat[] cameraImages = ArucoCamera.Images;
      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        if (MarkerIdsCurrentImage[cameraId] != null && MarkerIdsCurrentImage[cameraId].Size() < 1)
        {
          Debug.LogWarning("Not enough markers detected for the camera " + (cameraId+1) + "/" + ArucoCamera.CamerasNumber + " to add the frame for "
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
      CharucoBoard charucoBoard = ArucoBoard.Board as CharucoBoard;

      // Check if there is enough captured frames for calibration
      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        // Calibration with a grid board
        if (charucoBoard == null)
        {
          if (AllIds[cameraId].Size() < 1)
          {
            throw new System.Exception("Need at least one frame captured for the camera " + (cameraId+1) + "/" + ArucoCamera.CamerasNumber 
              + " to calibrate.");
          }
        }
        // Calibration with a charuco board
        else
        {
          if (AllCharucoIds[cameraId].Size() < 4)
          {
            IsCalibrated = false;
            throw new System.Exception("Need at least four frames captured for the camera " + (cameraId+1) + "/" + ArucoCamera.CamerasNumber 
              + " to calibrate with a ChAruco board.");
          }
        }
      }

      // Prepare camera parameters
      Mat[] camerasMatrix = new Mat[ArucoCamera.CamerasNumber],
            distCoeffs = new Mat[ArucoCamera.CamerasNumber];
      double[] reprojectionErrors = new double[ArucoCamera.CamerasNumber];

      // Calibrate each camera
      for (int cameraId = 0; cameraId < ArucoCamera.CamerasNumber; cameraId++)
      {
        // Prepare camera parameters
        if ((CalibrationFlags & CALIB.FIX_ASPECT_RATIO) == CALIB.FIX_ASPECT_RATIO)
        {
          camerasMatrix[cameraId] = new Mat(3, 3, TYPE.CV_64F, new double[9] { FixAspectRatio, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
        }
        else
        {
          camerasMatrix[cameraId] = new Mat();
        }
        distCoeffs[cameraId] = new Mat();

        // Prepare data for calibration
        VectorVectorPoint2f allCornersContenated = new VectorVectorPoint2f();
        VectorInt allIdsContenated = new VectorInt();
        VectorInt markerCounterPerFrame = new VectorInt();

        uint allCornersSize = AllCorners[cameraId].Size();
        markerCounterPerFrame.Reserve(allCornersSize);
        for (uint i = 0; i < allCornersSize; i++)
        {
          VectorVectorPoint2f allCornersI = AllCorners[cameraId].At(i);
          uint allCornersISize = allCornersI.Size();
          markerCounterPerFrame.PushBack((int)allCornersISize);
          for (uint j = 0; j < allCornersISize; j++)
          {
            allCornersContenated.PushBack(allCornersI.At(j));
            allIdsContenated.PushBack(AllIds[cameraId].At(i).At(j));
          }
        }

        // Calibrate camera with aruco
        Size imageSize = ArucoCamera.Images[cameraId].size;
        VectorMat rvecs, tvecs;
        reprojectionErrors[cameraId] = Functions.CalibrateCameraAruco(allCornersContenated, allIdsContenated, markerCounterPerFrame, 
          ArucoBoard.Board, imageSize, camerasMatrix[cameraId], distCoeffs[cameraId], out rvecs, out tvecs, (int)CalibrationFlags, 
          CalibrationTermCriteria);

        // If the used board is a charuco board, refine the calibration
        if (charucoBoard != null)
        {
          AllCharucoCorners[cameraId] = new VectorVectorPoint2f();
          AllCharucoIds[cameraId] = new VectorVectorInt();

          // Interpolate charuco corners using camera parameters
          for (uint i = 0; i < AllIds[cameraId].Size(); i++)
          {
            VectorPoint2f charucoCorners;
            VectorInt charucoIds;
            Functions.InterpolateCornersCharuco(AllCorners[cameraId].At(i), AllIds[cameraId].At(i), AllImages[cameraId].At(i), charucoBoard, out charucoCorners, out charucoIds);

            AllCharucoCorners[cameraId].PushBack(charucoCorners);
            AllCharucoIds[cameraId].PushBack(charucoIds);
          }

          // Calibrate camera using charuco
          reprojectionErrors[cameraId] = Functions.CalibrateCameraCharuco(AllCharucoCorners[cameraId], AllCharucoIds[cameraId], charucoBoard, 
            imageSize, camerasMatrix[cameraId], distCoeffs[cameraId], out rvecs, out tvecs, (int)CalibrationFlags, CalibrationTermCriteria);
        }

        // Save calibration parameters
        Rvecs[cameraId] = rvecs;
        Tvecs[cameraId] = tvecs;
      }

      IsCalibrated = true;

      // Create camera parameters
      CameraParameters = new CameraParameters(ArucoCamera.CamerasNumber)
      {
        CalibrationFlags = (int)CalibrationFlags,
        FixAspectRatio = FixAspectRatio,
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

    protected void UpdateCalibrationFlags()
    {
      calibrationFlags = 0;
      if (AssumeZeroTangentialDistorsion)
      {
        calibrationFlags |= CALIB.ZERO_TANGENT_DIST;
      }
      if (FixAspectRatio > 0)
      {
        calibrationFlags |= CALIB.FIX_ASPECT_RATIO;
      }
      if (fixPrincipalPointAtCenter)
      {
        calibrationFlags |= CALIB.FIX_PRINCIPAL_POINT;
      }
    }

    protected void UpdateCalibrationOptions()
    {
      if ((CalibrationFlags & CALIB.ZERO_TANGENT_DIST) == CALIB.ZERO_TANGENT_DIST)
      {
        AssumeZeroTangentialDistorsion = true;
      }
      if ((CalibrationFlags & CALIB.FIX_ASPECT_RATIO) == CALIB.FIX_ASPECT_RATIO)
      {
        FixAspectRatio = DEFAULT_FIX_ASPECT_RATIO;
      }
      if ((CalibrationFlags & CALIB.FIX_PRINCIPAL_POINT) == CALIB.FIX_PRINCIPAL_POINT)
      {
        fixPrincipalPointAtCenter = true;
      }
    }
  }

  /// \} aruco_unity_package
}