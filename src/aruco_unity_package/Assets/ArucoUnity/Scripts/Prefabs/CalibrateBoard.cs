using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CalibrateBoard : MonoBehaviour
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
      private DeviceCameraController deviceCameraController;

      [Header("UI")]
      [SerializeField]
      private Button addFrameButton;

      [SerializeField]
      private Button calibrateButton;

      [SerializeField]
      private Text imagesForCalibrationText;

      [SerializeField]
      private Text calibrationReprojectionError;

      public GridBoard Board { get; set; }
      public Dictionary Dictionary { get; set; }
      public DetectorParameters DetectorParameters { get; set; }

      public bool ApplyRefindStrategy { get { return applyRefindStrategy; } set { applyRefindStrategy = value; } }
      public float FixAspectRatio { get { return fixAspectRatio; } set { fixAspectRatio = value; } }
      public CALIB CalibrationFlags { get; set; }
      public string OutputFilePath { get { return outputFilePath; } set { outputFilePath = value; } }

      public Utility.VectorVectorVectorPoint2f AllCorners { get; private set; }
      public Utility.VectorVectorInt AllIds { get; private set; }
      public Utility.Size ImageSize { get; private set; }
      public Texture2D ImageTexture { get; private set; }
      public double CalibrationReprojectionError { get; private set; }

      private CameraParameters cameraParameters;
      private bool addNextFrame;
      private bool calibrate;

      void OnEnable()
      {
        DeviceCameraController.OnCameraStarted += Configurate;
      }

      void OnDisable()
      {
        DeviceCameraController.OnCameraStarted -= Configurate;
      }

      void LateUpdate()
      {
        if (deviceCameraController.cameraStarted)
        {
          Utility.Mat image;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f corners, rejectedImgPoints;

          // Detect and draw markers
          ImageTexture.SetPixels32(deviceCameraController.activeCameraTexture.GetPixels32());
          Detect(out corners, out ids, out rejectedImgPoints, out image);

          // Add frame to calibration frame list
          if (!calibrate)
          {
            AddFrameForCalibration(corners, ids, image);
            calibrateButton.enabled = true;
            UpdateImagesForCalibrationText();
          }
        }
      }

      void Configurate()
      {
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        DetectorParameters = detectorParametersManager.detectorParameters;
        Board = GridBoard.Create(markersNumberX, markersNumberY, markerSideLength, markerSeparation, Dictionary);

        ConfigurateCalibrationFlags();
        ConfigurateImageTexture(deviceCameraController);
        ResetCalibration();
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

      public void ConfigurateImageTexture(DeviceCameraController deviceCameraController)
      {
        ImageTexture = new Texture2D(deviceCameraController.activeCameraTexture.width, deviceCameraController.activeCameraTexture.height,
          TextureFormat.RGB24, false);
        deviceCameraController.SetActiveTexture(ImageTexture);
      }

      public void ResetCalibration()
      {
        calibrate = false;
        AllCorners = new Utility.VectorVectorVectorPoint2f();
        AllIds = new Utility.VectorVectorInt();
        ImageSize = new Utility.Size();
      }

      public void ResetCalibrationFromEditor()
      {
        addFrameButton.enabled = true;
        calibrateButton.enabled = false;

        ResetCalibration();
        UpdateImagesForCalibrationText();
        calibrationReprojectionError.text = "Calibration reprojection error: 0";
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

        int imageDataSize = (int)(image.ElemSize() * image.Total());
        ImageTexture.LoadRawTextureData(image.data, imageDataSize);
        ImageTexture.Apply(false);
      }

      public void AddNextFrameForCalibration()
      {
        addNextFrame = true;
      }

      public void AddFrameForCalibration(Utility.VectorVectorPoint2f corners, Utility.VectorInt ids, Utility.Mat image)
      {
        if (addNextFrame && !calibrate)
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
        calibrate = true;

        Utility.Mat cameraMatrix;
        Utility.Mat distCoeffs = new Utility.Mat();
        Utility.VectorMat rvecs, tvecs;

        if ((CalibrationFlags & CALIB.FIX_ASPECT_RATIO) == CALIB.FIX_ASPECT_RATIO)
        {
          cameraMatrix = new Utility.Mat(3, 3, TYPE.CV_64F, new double[9] { fixAspectRatio, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
        }
        else
        {
          cameraMatrix = new Utility.Mat();
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
        CalibrationReprojectionError = Methods.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, Board, ImageSize, 
          cameraMatrix, distCoeffs, out rvecs, out tvecs, (int)CalibrationFlags);

        // Save camera parameters
        cameraParameters = new CameraParameters(cameraMatrix, distCoeffs)
        {
          ImageHeight = ImageSize.height,
          ImageWidth = ImageSize.width,
          CalibrationFlags = (int)CalibrationFlags,
          AspectRatio = fixAspectRatio,
          ReprojectionError = CalibrationReprojectionError
        };
        cameraParameters.SaveToXmlFile(calibrationFilePath);
      }

      public void CalibrateFromEditor()
      {
        addFrameButton.enabled = false;
        calibrateButton.enabled = false;
        Calibrate(outputFilePath);
        calibrationReprojectionError.text = "Calibration reprojection error: " + CalibrationReprojectionError.ToString("F3");
      }

      void UpdateImagesForCalibrationText()
      {
        imagesForCalibrationText.text = "Images for calibration: " + AllIds.Size();
      }
    }
  }
}