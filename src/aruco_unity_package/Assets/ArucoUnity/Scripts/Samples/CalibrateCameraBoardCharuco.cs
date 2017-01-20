using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Samples.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    public class CalibrateCameraBoardCharuco : ArucoDetector
    {
      // Editor fields

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
      private string arucoCameraParametersFilePath = "Assets/ArucoUnity/aruco-calibration.xml";

      [SerializeField]
      private string charucoCameraParametersFilePath = "Assets/ArucoUnity/charuco-calibration.xml";

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

      // Properties

      // Calibration configuration properties
      public CharucoBoard CharucoBoard { get; set; }
      public bool ApplyRefineStrategy { get { return applyRefineStrategy; } set { applyRefineStrategy = value; } }
      public bool AssumeZeroTangentialDistorsion { get { return assumeZeroTangentialDistorsion; } set { assumeZeroTangentialDistorsion = value; } }
      public float FixAspectRatio { get { return fixAspectRatio; } set { fixAspectRatio = value; } }
      public bool FixPrincipalPointAtCenter { get { return fixPrincipalPointAtCenter; } set { fixPrincipalPointAtCenter = value; } }
      public CALIB CalibrationFlags { get; set; }
      public string ArucoCameraParametersFilePath { get { return arucoCameraParametersFilePath; } set { arucoCameraParametersFilePath = value; } }
      public string CharucoCameraParametersFilePath { get { return charucoCameraParametersFilePath; } set { charucoCameraParametersFilePath = value; } }

      // Calibration results properties
      public VectorVectorVectorPoint2f AllCorners { get; private set; }
      public VectorVectorInt AllIds { get; private set; }
      public VectorMat AllImages { get; private set; }
      public Size ImageSize { get; private set; }
      public VectorMat Rvecs { get; private set; }
      public VectorMat Tvecs { get; private set; }
      public VectorMat AllCharucoCorners { get; private set; }
      public VectorMat AllCharucoIds { get; private set; }
      public CameraParameters ArucoCameraParameters { get; private set; }
      public CameraParameters CharucoCameraParameters { get; private set; }

      // Variables

      protected bool addNextFrame;
      protected bool calibrate;

      // MonoBehaviour methods

      /// <summary>
      /// Set up <see cref="ArucoDetector.CameraDeviceController"/> and the UI. 
      /// </summary>
      protected override void OnEnable()
      {
        // Set up the parent class
        CameraDeviceController = cameraDeviceController;
        base.OnEnable();

        // Set up onClick functions to UI buttons
        addFrameButton.onClick.AddListener(AddNextFrameForCalibration);
        calibrateButton.onClick.AddListener(CalibrateFromEditor);
        resetButton.onClick.AddListener(ResetCalibrationFromEditor);
      }

      void LateUpdate()
      {
        if (Configurated)
        {
          VectorInt ids;
          VectorVectorPoint2f corners, rejectedImgPoints;
          Mat image;

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

        // Configurate pose estimation properties
        EstimatePose = false;

        // Configurate the board calibration
        CharucoBoard = CharucoBoard.Create(squaresNumberX, squaresNumberY, squareSideLength, markerSideLength, Dictionary);
        ConfigurateCalibrationFlags();
        ResetCalibrationFromEditor();
      }

      // Methods

      public void ResetCalibration()
      {
        calibrate = false;
        AllCorners = new VectorVectorVectorPoint2f();
        AllIds = new VectorVectorInt();
        AllImages = new VectorMat();
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
          Functions.RefineDetectedMarkers(image, CharucoBoard, corners, ids, rejectedImgPoints);
        }

        if (ids.Size() > 0)
        {
          // Interpolate charuco corners
          Mat currentCharucoCorners, currentCharucoIds;
          Functions.InterpolateCornersCharuco(corners, ids, image, CharucoBoard, out currentCharucoCorners, out currentCharucoIds);

          // Draw the markers on the image
          Functions.DrawDetectedMarkers(image, corners, ids);

          if (currentCharucoIds.Total() > 0)
          {
            Functions.DrawDetectedCornersCharuco(image, currentCharucoCorners, currentCharucoIds);
          }
        }

        // Undistord the image if calibrated
        Mat undistordedImage, imageToDisplay;
        if (calibrate)
        {
          Imgproc.Undistord(image, out undistordedImage, CharucoCameraParameters.CameraMatrix, CharucoCameraParameters.DistCoeffs);
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

      public void AddFrameForCalibration(VectorVectorPoint2f corners, VectorInt ids, Mat image)
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
          AllImages.PushBack(image);
          ImageSize = image.size;
        }
      }

      public void Calibrate()
      {
        if (AllIds.Size() < 1)
        {
          Debug.LogError(gameObject.name + ": Not enough captures for the ArUco calibration.");
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

        // Calibrate camera using aruco markers
        VectorMat rvecsAruco, tvecsAruco;
        double arucoCalibrationReprojectionError = Functions.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, 
          CharucoBoard, ImageSize, cameraMatrix, distCoeffs, out rvecsAruco, out tvecsAruco, (int)CalibrationFlags);

        // Save aruco camera parameters
        ArucoCameraParameters = new CameraParameters()
        {
          ImageHeight = ImageSize.height,
          ImageWidth = ImageSize.width,
          CalibrationFlags = (int)CalibrationFlags,
          FixAspectRatio = FixAspectRatio,
          ReprojectionError = arucoCalibrationReprojectionError,
          CameraMatrix = cameraMatrix,
          DistCoeffs = distCoeffs
        };
        ArucoCameraParameters.SaveToXmlFile(ArucoCameraParametersFilePath);

        // Interpolate charuco corners using camera parameters
        AllCharucoCorners = new VectorMat();
        AllCharucoIds = new VectorMat();

        for (uint i = 0; i < AllIds.Size(); i++)
        {
          Mat charucoCorners, charucoIds;
          Functions.InterpolateCornersCharuco(AllCorners.At(i), AllIds.At(i), AllImages.At(i), CharucoBoard, out charucoCorners, out charucoIds);

          AllCharucoCorners.PushBack(charucoCorners);
          AllCharucoIds.PushBack(charucoIds);
        }

        if (AllCharucoIds.Size() < 4)
        {
          Debug.LogError(gameObject.name + ": Not enough corners for ChArUco calibration.");
          calibrate = false;
          return;
        }

        // Calibrate camera using charuco
        double charucoCalibrationReprojectionError = Functions.CalibrateCameraCharuco(AllCharucoCorners, AllCharucoIds, CharucoBoard, ImageSize,
          cameraMatrix, distCoeffs);

        // Save charuco camera parameters
        CharucoCameraParameters = new CameraParameters()
        {
          ImageHeight = ImageSize.height,
          ImageWidth = ImageSize.width,
          CalibrationFlags = (int)CalibrationFlags,
          FixAspectRatio = FixAspectRatio,
          ReprojectionError = charucoCalibrationReprojectionError,
          CameraMatrix = cameraMatrix,
          DistCoeffs = distCoeffs
        };
        CharucoCameraParameters.SaveToXmlFile(CharucoCameraParametersFilePath);
      }

      // Editor button onclick listeners
      private void AddNextFrameForCalibration()
      {
        addNextFrame = true;
      }

      private void CalibrateFromEditor()
      {
        Calibrate();

        if (calibrate == true)
        {
          UpdateCalibrationReprojectionErrorTexts();
        }
      }

      private void ResetCalibrationFromEditor()
      {
        addFrameButton.interactable = true;
        calibrateButton.interactable = false;

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
        arucoCalibrationReprojectionError.text = " - Aruco: "
          + ((ArucoCameraParameters != null) ? ArucoCameraParameters.ReprojectionError.ToString("F3") : "");

        charucoCalibrationReprojectionError.text = " - Charuco: " 
          + ((CharucoCameraParameters != null) ? CharucoCameraParameters.ReprojectionError.ToString("F3") : "");
      }

      private void ConfigurateCalibrationFlags()
      {
        CalibrationFlags = 0;
        if (AssumeZeroTangentialDistorsion)
        {
          CalibrationFlags |= CALIB.ZERO_TANGENT_DIST;
        }
        if (FixAspectRatio > 0)
        {
          CalibrationFlags |= CALIB.FIX_ASPECT_RATIO;
        }
        if (FixPrincipalPointAtCenter)
        {
          CalibrationFlags |= CALIB.FIX_PRINCIPAL_POINT;
        }
      }
    }
  }

  /// \} aruco_unity_package
}