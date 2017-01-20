using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  // TODO: doc
  public class CalibrateCameraBoard : ArucoDetector
  {
    // Editor fields

    [Header("Board configuration")]
    [SerializeField]
    [Tooltip("Number of markers in X direction")]
    private int markersNumberX;

    [SerializeField]
    [Tooltip("Number of markers in Y direction")]
    private int markersNumberY;

    [SerializeField]
    [Tooltip("Marker side length (in meters)")]
    private float markerSideLength;

    [SerializeField]
    [Tooltip("Separation between two consecutive markers in the grid (in meters)")]
    private float markerSeparation;

    [SerializeField]
    private PREDEFINED_DICTIONARY_NAME dictionaryName;

    [Header("Calibration configuration")]
    [SerializeField]
    private ArucoDetectorParametersController detectorParametersManager;

    [SerializeField]
    private bool applyRefineStrategy = false;

    [SerializeField]
    private bool assumeZeroTangentialDistorsion = false;

    [SerializeField]
    private float fixAspectRatio;

    [SerializeField]
    private bool fixPrincipalPointAtCenter = false;

    [SerializeField]
    private string cameraParametersFilePath = "Assets/ArucoUnity/aruco-calibration.xml";

    [Header("Camera configuration")]
    [SerializeField]
    private ArucoCamera arucoCamera;

    [Header("UI")]
    [SerializeField]
    private Button addFrameButton;

    [SerializeField]
    private Button calibrateButton;

    [SerializeField]
    private Button resetButton;

    [SerializeField]
    private Text imagesForCalibrationText;

    [SerializeField]
    private Text calibrationReprojectionErrorText;

    // Properties

    // Calibration configuration properties
    public GridBoard Board { get; set; }
    public bool ApplyRefineStrategy { get { return applyRefineStrategy; } set { applyRefineStrategy = value; } } // TODO: to factor
    public float FixAspectRatio { get { return fixAspectRatio; } set { fixAspectRatio = value; } } // TODO: to factor
    public CALIB CalibrationFlags { get; set; } // TODO: to factor

    // Calibration results properties // TODO: to factor
    public VectorVectorVectorPoint2f AllCorners { get; private set; }
    public VectorVectorInt AllIds { get; private set; }
    public Size ImageSize { get; private set; }
    public VectorMat Rvecs { get; private set; }
    public VectorMat Tvecs { get; private set; }
    public CameraParameters CameraParameters { get; private set; }

    // Variables

    private bool addNextFrame; // TODO: to factor
    private bool calibrate; // TODO: to factor

    // MonoBehaviour methods

    /// <summary>
    /// Set up <see cref="ArucoDetector.ArucoCamera"/> and the UI. 
    /// </summary>
    protected override void OnEnable() // TODO: to factor
    {
      // Set up the parent class
      ArucoCamera = arucoCamera;
      base.OnEnable();

      // Set up onClick functions to UI buttons
      addFrameButton.onClick.AddListener(AddNextFrameForCalibration);
      calibrateButton.onClick.AddListener(CalibrateFromEditor);
      resetButton.onClick.AddListener(ResetCalibrationFromEditor);
    }

    void LateUpdate() // TODO: to factor
    {
      if (Configured)
      {
        Mat image;
        VectorInt ids;
        VectorVectorPoint2f corners, rejectedImgPoints;

        // Detect and draw markers
        Detect(out corners, out ids, out rejectedImgPoints, out image);

        // Add frame to calibration frame list
        if (addNextFrame && !calibrate)
        {
          AddFrameForCalibration(corners, ids, image);
          calibrateButton.enabled = true;
          UpdateImagesForCalibrationText();
        }
      }
    }

    // ArucoDetector Methods

    /// <summary>
    /// Set up the <see cref="ArucoDetector"/> parent class properties.
    /// </summary>
    protected override void PreConfigurate()
    {
      // Configurate detection properties
      Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
      DetectorParameters = detectorParametersManager.detectorParameters;
      MarkerSideLength = markerSideLength;

      // Configurate camera properties
      CameraParametersFilePath = cameraParametersFilePath;

      // Configurate pose estimation properties
      EstimatePose = false;

      // Configurate the board calibration
      Board = GridBoard.Create(markersNumberX, markersNumberY, markerSideLength, markerSeparation, Dictionary);
      ConfigurateCalibrationFlags(); // TODO: to factor
      ResetCalibrationFromEditor(); // TODO: to factor
    }

    // Methods

    public void ResetCalibration() // TODO: to factor
    {
      calibrate = false;
      AllCorners = new VectorVectorVectorPoint2f();
      AllIds = new VectorVectorInt();
      ImageSize = new Size();
    }

    public void Detect(out VectorVectorPoint2f corners, out VectorInt ids, out VectorVectorPoint2f rejectedImgPoints, out Mat image)
    {
      // Detect markers
      byte[] imageData = CameraImageTexture.GetRawTextureData();
      image = new Mat(CameraImageTexture.height, CameraImageTexture.width, TYPE.CV_8UC3, imageData);
      Functions.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

      if (ApplyRefineStrategy)
      {
        Functions.RefineDetectedMarkers(image, Board, corners, ids, rejectedImgPoints);
      }

      // Draw the markers on the image
      if (ids.Size() > 0)
      {
        Functions.DrawDetectedMarkers(image, corners, ids);
      }

      // Undistord the image if calibrated
      Mat undistordedImage, imageToDisplay;
      if (calibrate)
      {
        Imgproc.Undistord(image, out undistordedImage, CameraParameters.CameraMatrix, CameraParameters.DistCoeffs);
        imageToDisplay = undistordedImage;
      }
      else
      {
        imageToDisplay = image;
      }

      // Copy the bytes of the image to the texture
      int imageDataSize = (int)(imageToDisplay.ElemSize() * imageToDisplay.Total());
      CameraImageTexture.LoadRawTextureData(imageToDisplay.data, imageDataSize);
      CameraImageTexture.Apply(false);
    }

    public void AddFrameForCalibration(VectorVectorPoint2f corners, VectorInt ids, Mat image) // TODO: to factor
    {
      if (!calibrate)
      {
        if (ids.Size() < 1)
        {
          Debug.LogError(gameObject.name + ": Not enough markers detected to add the frame for calibration.");
          return;
        }

        addNextFrame = false;

        AllCorners.PushBack(corners);
        AllIds.PushBack(ids);
        ImageSize = image.size;
      }
    }

    public void Calibrate()
    {
      if (AllIds.Size() < 1)
      {
        Debug.LogError(gameObject.name + ": Not enough captures for the calibration.");
        return;
      }
      calibrate = true;

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
      VectorMat rvecs, tvecs;
      double reprojectionError = Functions.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, Board, ImageSize,
        cameraMatrix, distCoeffs, out rvecs, out tvecs, (int)CalibrationFlags);
      Rvecs = rvecs;
      Tvecs = tvecs;

      // Save camera parameters
      CameraParameters = new CameraParameters()
      {
        ImageHeight = ImageSize.height,
        ImageWidth = ImageSize.width,
        CalibrationFlags = (int)CalibrationFlags,
        FixAspectRatio = FixAspectRatio,
        ReprojectionError = reprojectionError,
        CameraMatrix = cameraMatrix,
        DistCoeffs = distCoeffs
      };
      CameraParameters.SaveToXmlFile(CameraParametersFilePath);
    }

    // Editor button onclick listeners
    private void AddNextFrameForCalibration() // TODO: to factor
    {
      addNextFrame = true;
    }

    private void CalibrateFromEditor() // TODO: to factor
    {
      addFrameButton.enabled = false;
      calibrateButton.enabled = false;
      Calibrate();
      UpdateCalibrationReprojectionErrorTexts();
    }

    private void ResetCalibrationFromEditor() // TODO: to factor
    {
      addFrameButton.enabled = true;
      calibrateButton.enabled = false;

      ResetCalibration();
      UpdateImagesForCalibrationText();
      UpdateCalibrationReprojectionErrorTexts();
    }

    // Utilities
    void UpdateImagesForCalibrationText() // TODO: to factor
    {
      imagesForCalibrationText.text = "Images for calibration: " + AllIds.Size();
    }

    private void UpdateCalibrationReprojectionErrorTexts()
    {
      calibrationReprojectionErrorText.text = "Calibration reprojection error: "
       + ((CameraParameters != null) ? CameraParameters.ReprojectionError.ToString("F3") : "");
    }

    void ConfigurateCalibrationFlags() // TODO: to factor
    {
      CalibrationFlags = 0;
      if (assumeZeroTangentialDistorsion)
      {
        CalibrationFlags |= CALIB.ZERO_TANGENT_DIST;
      }
      if (FixAspectRatio > 0)
      {
        CalibrationFlags |= CALIB.FIX_ASPECT_RATIO;
      }
      if (fixPrincipalPointAtCenter)
      {
        CalibrationFlags |= CALIB.FIX_PRINCIPAL_POINT;
      }
    }
  }

  /// \} aruco_unity_package
}