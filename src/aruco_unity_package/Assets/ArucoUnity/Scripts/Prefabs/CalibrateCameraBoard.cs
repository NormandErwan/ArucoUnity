using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CalibrateCameraBoard : MonoBehaviour
    {
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
      private CameraDeviceCanvasDisplay cameraDeviceCanvasDisplay;

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

      // Configuration properties
      public GridBoard Board { get; set; }
      public Dictionary Dictionary { get; set; }
      public DetectorParameters DetectorParameters { get; set; }
      public bool ApplyRefindStrategy { get { return applyRefindStrategy; } set { applyRefindStrategy = value; } }
      public float FixAspectRatio { get { return fixAspectRatio; } set { fixAspectRatio = value; } }
      public CALIB CalibrationFlags { get; set; }
      public string OutputFilePath { get { return outputFilePath; } set { outputFilePath = value; } }

      // Calibration properties
      public Utility.VectorVectorVectorPoint2f AllCorners { get; private set; }
      public Utility.VectorVectorInt AllIds { get; private set; }
      public Utility.Size ImageSize { get; private set; }
      public Texture2D ImageTexture { get; private set; }
      public Utility.Mat CameraMatrix { get; private set; }
      public Utility.Mat DistCoeffs { get; private set; }
      public Utility.VectorMat Rvecs { get; private set; }
      public Utility.VectorMat Tvecs { get; private set; }
      public double CalibrationReprojectionError { get; private set; }

      private CameraParameters cameraParameters;
      private bool addNextFrame;
      private bool calibrate;
      private CameraDeviceController cameraController;

      void Awake()
      {
        cameraController = CameraDeviceController.Instance;

        addFrameButton.onClick.AddListener(AddNextFrameForCalibration);
        calibrateButton.onClick.AddListener(CalibrateFromEditor);
        resetButton.onClick.AddListener(ResetCalibrationFromEditor);
      }

      void OnEnable()
      {
        cameraController.OnCameraStarted += Configurate;
      }

      void OnDisable()
      {
        cameraController.OnCameraStarted -= Configurate;
      }

      private void Configurate()
      {
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        DetectorParameters = detectorParametersManager.detectorParameters;
        Board = GridBoard.Create(markersNumberX, markersNumberY, markerSideLength, markerSeparation, Dictionary);
        ImageTexture = cameraController.ActiveCameraTexture2D;

        ConfigurateCalibrationFlags();
        ResetCalibration();
      }

      void LateUpdate()
      {
        if (cameraController.CameraStarted)
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
            calibrateButton.enabled = true;
            UpdateImagesForCalibrationText();
          }
        }
      }

      public void ResetCalibration()
      {
        calibrate = false;
        AllCorners = new Utility.VectorVectorVectorPoint2f();
        AllIds = new Utility.VectorVectorInt();
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
          Methods.RefineDetectedMarkers(image, Board, corners, ids, rejectedImgPoints);
        }

        // Draw results
        if (ids.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, corners, ids);
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

        // Calibrate camera
        Utility.VectorMat rvecs, tvecs;
        CalibrationReprojectionError = Methods.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, Board, ImageSize, 
          CameraMatrix, DistCoeffs, out rvecs, out tvecs, (int)CalibrationFlags);
        Rvecs = rvecs;
        Tvecs = tvecs;

        // Save camera parameters
        cameraParameters = new CameraParameters(CameraMatrix, DistCoeffs)
        {
          ImageHeight = ImageSize.height,
          ImageWidth = ImageSize.width,
          CalibrationFlags = (int)CalibrationFlags,
          AspectRatio = fixAspectRatio,
          ReprojectionError = CalibrationReprojectionError
        };
        cameraParameters.SaveToXmlFile(calibrationFilePath);

        Debug.Log(gameObject.name + ": Camera parameters successfully saved to '" + calibrationFilePath + "'.");
      }

      // Editor button onclick listeners
      private void AddNextFrameForCalibration()
      {
        addNextFrame = true;
      }

      private void CalibrateFromEditor()
      {
        addFrameButton.enabled = false;
        calibrateButton.enabled = false;
        Calibrate(outputFilePath);
        UpdateCalibrationReprojectionErrorTexts();
      }

      private void ResetCalibrationFromEditor()
      {
        addFrameButton.enabled = true;
        calibrateButton.enabled = false;

        CalibrationReprojectionError = 0;

        ResetCalibration();
        UpdateImagesForCalibrationText();
        UpdateCalibrationReprojectionErrorTexts();
      }

      // Utilities
      void UpdateImagesForCalibrationText()
      {
        imagesForCalibrationText.text = "Images for calibration: " + AllIds.Size();
      }

      private void UpdateCalibrationReprojectionErrorTexts()
      {
        calibrationReprojectionErrorText.text = "Calibration reprojection error: " + CalibrationReprojectionError.ToString("F3");
      }

      void ConfigurateCalibrationFlags()
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