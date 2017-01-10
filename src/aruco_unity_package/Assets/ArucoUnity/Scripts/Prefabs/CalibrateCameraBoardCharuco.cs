using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CalibrateCameraBoardCharuco : CameraDeviceMarkersDetector
    {
      [Header("ChArUco board configuration")]
      [SerializeField]
      [Tooltip("Number of markers in X direction")]
      private int squaresNumberX;

      [SerializeField]
      [Tooltip("Number of markers in Y direction")]
      private int squaresNumberY;

      [SerializeField]
      [Tooltip("Square side length (in meters)")]
      public float squareSideLength;

      [SerializeField]
      [Tooltip("Marker side length (in meters)")]
      private float markerSideLength;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [Header("Calibration configuration")]
      [SerializeField]
      private DetectorParametersManager detectorParametersManager;

      [SerializeField]
      private bool applyRefindStrategy = false;

      [SerializeField]
      private bool assumeZeroTangentialDistorsion = false;

      [SerializeField]
      private float fixAspectRatio;

      [SerializeField]
      private bool fixPrincipalPointAtCenter = false;

      [SerializeField]
      private string outputFilePath;

      [Header("Camera configuration")]
      [SerializeField]
      private CameraDeviceController cameraDeviceController;

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
      private Text arucoCalibrationReprojectionError;

      [SerializeField]
      private Text charucoCalibrationReprojectionError;

      // Configuration properties
      public CharucoBoard CharucoBoard { get; set; }
      public Dictionary Dictionary { get; set; }
      public DetectorParameters DetectorParameters { get; set; }

      public bool ApplyRefindStrategy { get { return applyRefindStrategy; } set { applyRefindStrategy = value; } }
      public bool AssumeZeroTangentialDistorsion { get { return assumeZeroTangentialDistorsion; } set { assumeZeroTangentialDistorsion = value; } }
      public float FixAspectRatio { get { return fixAspectRatio; } set { fixAspectRatio = value; } }
      public bool FixPrincipalPointAtCenter { get { return fixPrincipalPointAtCenter; } set { fixPrincipalPointAtCenter = value; } }
      public CALIB CalibrationFlags { get; set; }
      public string OutputFilePath { get { return outputFilePath; } set { outputFilePath = value; } }

      // Calibration properties
      public Utility.VectorVectorVectorPoint2f AllCorners { get; private set; }
      public Utility.VectorVectorInt AllIds { get; private set; }
      public Utility.VectorMat AllImages { get; private set; }
      public Utility.Size ImageSize { get; private set; }
      public Utility.Mat CameraMatrix { get; private set; }
      public Utility.Mat DistCoeffs { get; private set; }
      public Utility.VectorMat Rvecs { get; private set; }
      public Utility.VectorMat Tvecs { get; private set; }
      public Utility.VectorMat AllCharucoCorners { get; private set; }
      public Utility.VectorMat AllCharucoIds { get; private set; }
      public double ArucoCalibrationReprojectionError { get; private set; }
      public double CharucoCalibrationReprojectionError { get; private set; }

      private CameraParameters cameraParameters;
      private bool addNextFrame;
      private bool calibrate;

      public CalibrateCameraBoardCharuco(CameraDeviceController cameraDeviceController) 
        : base(cameraDeviceController)
      {
      }

      /// <summary>
      /// Add onClick functions to UI buttons.
      /// </summary>
      void Awake()
      {
        CameraDeviceController = cameraDeviceController;

        addFrameButton.onClick.AddListener(AddNextFrameForCalibration);
        calibrateButton.onClick.AddListener(CalibrateFromEditor);
        resetButton.onClick.AddListener(ResetCalibrationFromEditor);
      }

      protected override void Configurate()
      {
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        DetectorParameters = detectorParametersManager.detectorParameters;
        CharucoBoard = CharucoBoard.Create(squaresNumberX, squaresNumberY, squareSideLength, markerSideLength, Dictionary);

        ConfigurateCalibrationFlags();
        ResetCalibrationFromEditor();
      }

      void LateUpdate()
      {
        if (Configurated)
        {
          Utility.Mat image;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f corners, rejectedImgPoints;

          // Detect and draw markers
          Detect(out corners, out ids, out rejectedImgPoints, out image);

          // Add frame to calibration frame list
          if (addNextFrame && !calibrate)
          {
            AddFrameForCalibration(corners, ids, image);
            calibrateButton.interactable = true;
            UpdateImagesForCalibrationText();
          }
        }
      }

      public void ResetCalibration()
      {
        calibrate = false;
        AllCorners = new Utility.VectorVectorVectorPoint2f();
        AllIds = new Utility.VectorVectorInt();
        AllImages = new Utility.VectorMat();
        ImageSize = new Utility.Size();
      }

      public void Detect(out Utility.VectorVectorPoint2f corners, out Utility.VectorInt ids, out Utility.VectorVectorPoint2f rejectedImgPoints,
        out Utility.Mat image)
      {
        // Detect markers
        byte[] imageData = ImageTexture.GetRawTextureData();
        image = new Utility.Mat(ImageTexture.height, ImageTexture.width, TYPE.CV_8UC3, imageData);
        Methods.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

        if (applyRefindStrategy)
        {
          Methods.RefineDetectedMarkers(image, CharucoBoard, corners, ids, rejectedImgPoints);
        }

        if (ids.Size() > 0)
        {
          // Interpolate charuco corners
          Utility.Mat currentCharucoCorners, currentCharucoIds;
          Methods.InterpolateCornersCharuco(corners, ids, image, CharucoBoard, out currentCharucoCorners, out currentCharucoIds);

          // Draw results
          Methods.DrawDetectedMarkers(image, corners, ids);

          if (currentCharucoIds.Total() > 0)
          {
            Methods.DrawDetectedCornersCharuco(image, currentCharucoCorners, currentCharucoIds);
          }
        }

        // Copy back the image
        int imageDataSize = (int)(image.ElemSize() * image.Total());
        ImageTexture.LoadRawTextureData(image.data, imageDataSize);
        ImageTexture.Apply(false);
      }

      public void AddFrameForCalibration(Utility.VectorVectorPoint2f corners, Utility.VectorInt ids, Utility.Mat image)
      {
        if (!calibrate)
        {
          if (ids.Size() < 1)
          {
            Debug.LogError("Not enough markers detected to add the frame for calibration.");
            return;
          }

          addNextFrame = false;

          AllCorners.PushBack(corners);
          AllIds.PushBack(ids);
          AllImages.PushBack(image);
          ImageSize = image.size;
        }
      }

      public void Calibrate(string calibrationFilePath)
      {
        if (AllIds.Size() < 1)
        {
          Debug.LogError("Not enough captures for calibration.");
          return;
        }
        calibrate = true;

        CameraMatrix = new Utility.Mat();
        DistCoeffs = new Utility.Mat();

        if ((CalibrationFlags & CALIB.FIX_ASPECT_RATIO) == CALIB.FIX_ASPECT_RATIO)
        {
          CameraMatrix = new Utility.Mat(3, 3, TYPE.CV_64F, new double[9] { fixAspectRatio, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
        }

        // Prepare data for calibration
        Utility.VectorVectorPoint2f allCornersContenated = new Utility.VectorVectorPoint2f();
        Utility.VectorInt allIdsContanated = new Utility.VectorInt();
        Utility.VectorInt markerCounterPerFrame = new Utility.VectorInt();

        uint allCornersSize = AllCorners.Size();
        markerCounterPerFrame.Reserve(allCornersSize);
        for (uint i = 0; i < allCornersSize; i++)
        {
          Utility.VectorVectorPoint2f allCornersI = AllCorners.At(i);
          uint allCornersISize = allCornersI.Size();
          markerCounterPerFrame.PushBack((int)allCornersISize);
          for (uint j = 0; j < allCornersISize; j++)
          {
            allCornersContenated.PushBack(allCornersI.At(j));
            allIdsContanated.PushBack(AllIds.At(i).At(j));
          }
        }

        // Calibrate camera using aruco markers
        Utility.VectorMat rvecsAruco, tvecsAruco;
        ArucoCalibrationReprojectionError = Methods.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, 
          CharucoBoard, ImageSize, CameraMatrix, DistCoeffs, out rvecsAruco, out tvecsAruco, (int)CalibrationFlags);

        // Interpolate charuco corners using camera parameters
        AllCharucoCorners = new Utility.VectorMat();
        AllCharucoIds = new Utility.VectorMat();

        for (uint i = 0; i < AllIds.Size(); i++)
        {
          Utility.Mat charucoCorners, charucoIds;
          Methods.InterpolateCornersCharuco(AllCorners.At(i), AllIds.At(i), AllImages.At(i), CharucoBoard, out charucoCorners, out charucoIds);

          AllCharucoCorners.PushBack(charucoCorners);
          AllCharucoIds.PushBack(charucoIds);
        }

        if (AllCharucoIds.Size() < 4)
        {
          Debug.LogError("Not enough corners for calibration.");
          calibrate = false;
          return;
        }

        // Calibrate camera using charuco
        CharucoCalibrationReprojectionError = Methods.CalibrateCameraCharuco(AllCharucoCorners, AllCharucoIds, CharucoBoard, ImageSize,
          CameraMatrix, DistCoeffs);

        calibrate = true;

        // Save camera parameters
        cameraParameters = new CameraParameters(CameraMatrix, DistCoeffs)
        {
          ImageHeight = ImageSize.height,
          ImageWidth = ImageSize.width,
          CalibrationFlags = (int)CalibrationFlags,
          AspectRatio = fixAspectRatio,
          ReprojectionError = CharucoCalibrationReprojectionError
        };
        cameraParameters.SaveToXmlFile(calibrationFilePath);
      }

      // Editor button onclick listeners
      private void AddNextFrameForCalibration()
      {
        addNextFrame = true;
      }

      private void CalibrateFromEditor()
      {
        Calibrate(outputFilePath);

        if (calibrate == true)
        {
          UpdateCalibrationReprojectionErrorTexts();
        }
      }

      private void ResetCalibrationFromEditor()
      {
        addFrameButton.interactable = true;
        calibrateButton.interactable = false;

        ArucoCalibrationReprojectionError = CharucoCalibrationReprojectionError = 0f;

        ResetCalibration();
        UpdateImagesForCalibrationText();
        UpdateCalibrationReprojectionErrorTexts();
      }

      // Utilities
      private void UpdateImagesForCalibrationText()
      {
        imagesForCalibrationText.text = "Images for calibration: " + AllIds.Size();
      }

      private void UpdateCalibrationReprojectionErrorTexts()
      {
        arucoCalibrationReprojectionError.text = " - Aruco: " + ArucoCalibrationReprojectionError.ToString("F3");
        charucoCalibrationReprojectionError.text = " - Charuco: " + CharucoCalibrationReprojectionError.ToString("F3");
      }

      private void ConfigurateCalibrationFlags()
      {
        CalibrationFlags = 0;
        if (assumeZeroTangentialDistorsion)
        {
          CalibrationFlags |= CALIB.ZERO_TANGENT_DIST;
        }
        if (fixAspectRatio > 0)
        {
          CalibrationFlags |= CALIB.FIX_ASPECT_RATIO;
        }
        if (fixPrincipalPointAtCenter)
        {
          CalibrationFlags |= CALIB.FIX_PRINCIPAL_POINT;
        }
      }
    }
  }
}