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
    protected ArucoBoard<Board> arucoBoard;

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

    public ArucoBoard<Board> ArucoBoard { get { return arucoBoard; } set { arucoBoard = value; } }

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

    public VectorVectorVectorPoint2f AllCorners { get; protected set; }

    public VectorVectorInt AllIds { get; protected set; }

    public VectorMat AllImages { get; protected set; }

    public VectorVectorPoint2f AllCharucoCorners { get; protected set; }

    public VectorVectorInt AllCharucoIds { get; protected set; }

    public VectorMat Rvecs { get; protected set; }

    public VectorMat Tvecs { get; protected set; }

    public CameraParameters CameraParameters { get; protected set; }

    public VectorVectorPoint2f MarkerCornersCurrentImage { get; protected set; }

    public VectorInt MarkerIdsCurrentFrame { get; protected set; }

    public bool IsCalibrated { get; protected set; }

    // Variables

    private CALIB calibrationFlags;

    // MonoBehaviour methods

    protected override void Awake()
    {
      base.Awake();

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
      AllCorners = new VectorVectorVectorPoint2f();
      AllIds = new VectorVectorInt();
      AllImages = new VectorMat();
      AllCharucoCorners = null;
      AllCharucoIds = null;
      Rvecs = new VectorMat();
      Tvecs = new VectorMat();
      CameraParameters = null;
      MarkerCornersCurrentImage = null;
      MarkerIdsCurrentFrame = null;
      IsCalibrated = false;
    }

    public void Detect()
    {
      if (!IsConfigured)
      {
        return;
      }

      VectorInt markerIds;
      VectorVectorPoint2f markerCorners, rejectedCandidateCorners;

      int cameraId = 0;
      Mat image = ArucoCamera.Images[cameraId];

      Functions.DetectMarkers(image, ArucoBoard.Dictionary, out markerCorners, out markerIds, DetectorParameters, out rejectedCandidateCorners);

      MarkerCornersCurrentImage = markerCorners;
      MarkerIdsCurrentFrame = markerIds;

      if (ApplyRefineStrategy)
      {
        Functions.RefineDetectedMarkers(image, ArucoBoard.Board, MarkerCornersCurrentImage, MarkerIdsCurrentFrame, rejectedCandidateCorners);
      }
    }

    public void Draw()
    {
      if (!IsConfigured)
      {
        return;
      }

      bool updatedCameraImage = false;

      int cameraId = 0;
      Mat[] cameraImages = ArucoCamera.Images;

      if (MarkerIdsCurrentFrame.Size() > 0)
      {
        Functions.DrawDetectedMarkers(cameraImages[cameraId], MarkerCornersCurrentImage, MarkerIdsCurrentFrame);
        updatedCameraImage = true;
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

      if (MarkerIdsCurrentFrame.Size() < 1)
      {
        throw new System.Exception("Not enough markers detected to add the frame for calibration.");
      }

      int cameraId = 0;
      Mat[] cameraImages = ArucoCamera.Images;

      AllCorners.PushBack(MarkerCornersCurrentImage);
      AllIds.PushBack(MarkerIdsCurrentFrame);
      AllImages.PushBack(ArucoCamera.Images[cameraId]);
    }

    public void Calibrate()
    {
      if (AllIds.Size() < 1)
      {
        throw new System.Exception("Need at least one frame captured to calibrate.");
      }

      IsCalibrated = true;
      int cameraId = 0;

      // Prepare camera parameters
      Mat cameraMatrix = new Mat();
      Mat distCoeffs = new Mat();

      if ((CalibrationFlags & CALIB.FIX_ASPECT_RATIO) == CALIB.FIX_ASPECT_RATIO)
      {
        cameraMatrix = new Mat(3, 3, TYPE.CV_64F, new double[9] { FixAspectRatio, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
      }

      // Prepare data for calibration
      VectorVectorPoint2f allCornersContenated = new VectorVectorPoint2f();
      VectorInt allIdsContanated = new VectorInt();
      VectorInt markerCounterPerFrame = new VectorInt();

      uint allCornersSize = AllCorners.Size();
      markerCounterPerFrame.Reserve(allCornersSize);
      for (uint i = 0; i < allCornersSize; i++)
      {
        VectorVectorPoint2f allCornersI = AllCorners.At(i);
        uint allCornersISize = allCornersI.Size();
        markerCounterPerFrame.PushBack((int)allCornersISize);
        for (uint j = 0; j < allCornersISize; j++)
        {
          allCornersContenated.PushBack(allCornersI.At(j));
          allIdsContanated.PushBack(AllIds.At(i).At(j));
        }
      }

      // Calibrate camera
      Size imageSize = ArucoCamera.Images[cameraId].size;
      VectorMat rvecs, tvecs;
      double reprojectionError = Functions.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, ArucoBoard.Board,
        imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, (int)CalibrationFlags);

      CharucoBoard charucoBoard = ArucoBoard.Board as CharucoBoard;
      if (charucoBoard != null)
      {
        AllCharucoCorners = new VectorVectorPoint2f();
        AllCharucoIds = new VectorVectorInt();

        // Interpolate charuco corners using camera parameters
        for (uint i = 0; i < AllIds.Size(); i++)
        {
          VectorPoint2f charucoCorners;
          VectorInt charucoIds;
          Functions.InterpolateCornersCharuco(AllCorners.At(i), AllIds.At(i), AllImages.At(i), charucoBoard, out charucoCorners, out charucoIds);

          AllCharucoCorners.PushBack(charucoCorners);
          AllCharucoIds.PushBack(charucoIds);
        }

        if (AllCharucoIds.Size() < 4)
        {
          IsCalibrated = false;
          throw new System.Exception("Need at least four frames captured to calibrate with a ChAruco board.");
        }

        // Calibrate camera using charuco
        reprojectionError = Functions.CalibrateCameraCharuco(AllCharucoCorners, AllCharucoIds, charucoBoard, imageSize, cameraMatrix, distCoeffs);
      }

      Rvecs = rvecs;
      Tvecs = tvecs;

      // Create camera parameters
      CameraParameters = new CameraParameters()
      {
        ImageHeight = ArucoCamera.ImageTextures[cameraId].height,
        ImageWidth = ArucoCamera.ImageTextures[cameraId].width,
        CalibrationFlags = (int)CalibrationFlags,
        FixAspectRatio = FixAspectRatio,
        ReprojectionError = reprojectionError,
        CameraMatrix = cameraMatrix,
        DistCoeffs = distCoeffs
      };

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