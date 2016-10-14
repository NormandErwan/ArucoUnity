using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ArucoUnity
{
  namespace Examples
  {
    public class CalibrateCamera : MonoBehaviour
    {
      public Dictionary dictionary;
      public DetectorParameters detectorParameters;
      public GridBoard board;

      [HideInInspector]
      public Texture2D imageTexture;

      [HideInInspector]
      public CALIB calibrationFlags;

      public Utility.VectorVectorVectorPoint2f allCorners;
      public Utility.VectorVectorInt allIds;
      public Utility.Size imageSize;

      [Header("Board configuration")]
      [SerializeField]
      [Tooltip("Number of markers in X direction")]
      private int markersNumberX;

      [SerializeField]
      [Tooltip("Number of markers in Y direction")]
      private int markersNumberY;

      [SerializeField]
      [Tooltip("Marker side length (in meters)")]
      private float markerLength;

      [SerializeField]
      [Tooltip("Separation between two consecutive markers in the grid (in meters)")]
      private float markerSeparation;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [Header("Calibration configuration")]
      [SerializeField]
      private DetectorParametersManager detectorParametersManager;

      [SerializeField]
      public bool applyRefindStrategy = false;

      [SerializeField]
      public bool assumeZeroTangentialDistorsion = false;

      [SerializeField]
      public float fixAspectRatio;

      [SerializeField]
      public bool fixPrincipalPointAtCenter = false;

      [Header("Camera configuration")]
      [SerializeField]
      private DeviceCameraController deviceCameraController;

      [Header("UI")]
      [SerializeField]
      private Button AddFrameButton;

      [SerializeField]
      private Button CalibrateButton;

      [SerializeField]
      private Text ImagesForCalibrationText;

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
          // Detect and draw markers
          Utility.VectorVectorPoint2f corners;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f rejectedImgPoints;
          Utility.Mat image;

          imageTexture.SetPixels32(deviceCameraController.activeCameraTexture.GetPixels32());
          Detect(out corners, out ids, out rejectedImgPoints, out image);

          // Add frame to calibration frame list
          if (addNextFrame && !calibrate)
          {
            addNextFrame = false;
            CalibrateButton.enabled = true;

            allCorners.PushBack(corners);
            allIds.PushBack(ids);
            imageSize = image.size;
            
            UpdateImagesForCalibrationText();
          }
        }
      }

      void Configurate()
      {
        dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        detectorParameters = detectorParametersManager.detectorParameters;
        board = GridBoard.Create(markersNumberX, markersNumberY, markerLength, markerSeparation, dictionary);

        ConfigurateCalibrationFlags();
        ConfigurateImageTexture(deviceCameraController);
        ResetCalibration();
      }

      // Call it first if you're using the Script alone, not with the Prefab.
      void Configurate(Dictionary dictionary, DetectorParameters detectorParameters, GridBoard board, Texture2D imageTexture, bool applyRefindStrategy, 
        bool assumeZeroTangentialDistorsion, float fixAspectRatio, bool fixPrincipalPointAtCenter)
      {
        this.dictionary = dictionary;
        this.detectorParameters = detectorParameters;
        this.board = board;

        this.imageTexture = imageTexture;
        this.applyRefindStrategy = applyRefindStrategy;
        this.assumeZeroTangentialDistorsion = assumeZeroTangentialDistorsion;
        this.fixAspectRatio = fixAspectRatio;
        this.fixPrincipalPointAtCenter = fixPrincipalPointAtCenter;

        ConfigurateCalibrationFlags();
        ResetCalibration();
      }

      void ConfigurateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (assumeZeroTangentialDistorsion)
        {
          calibrationFlags |= CALIB.ZERO_TANGENT_DIST;
        }
        if (fixAspectRatio > 0)
        {
          calibrationFlags |= CALIB.FIX_ASPECT_RATIO;
        }
        if (fixPrincipalPointAtCenter)
        {
          calibrationFlags |= CALIB.FIX_PRINCIPAL_POINT;
        }
      }

      public void ConfigurateImageTexture(DeviceCameraController deviceCameraController)
      {
        imageTexture = new Texture2D(deviceCameraController.activeCameraTexture.width, deviceCameraController.activeCameraTexture.height,
          TextureFormat.RGB24, false);
        deviceCameraController.SetActiveTexture(imageTexture);
      }

      public void ResetCalibration()
      {
        calibrate = false;
        AddFrameButton.enabled = true;
        CalibrateButton.enabled = false;

        allCorners = new Utility.VectorVectorVectorPoint2f();
        allIds = new Utility.VectorVectorInt();
        imageSize = new Utility.Size();

        UpdateImagesForCalibrationText();
      }

      public void Detect(out Utility.VectorVectorPoint2f corners, out Utility.VectorInt ids, 
        out Utility.VectorVectorPoint2f rejectedImgPoints, out Utility.Mat image)
      {
        // Detect markers
        byte[] imageData = imageTexture.GetRawTextureData();
        image = new Utility.Mat(imageTexture.height, imageTexture.width, imageData);
        Methods.DetectMarkers(image, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);

        if (applyRefindStrategy)
        {
          //Methods.RefineDetectedMarkers(image, board, corners, ids, rejectedImgPoints); // TODO: fix the method
        }

        // Draw results
        if (ids.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, corners, ids);
        }

        int imageDataSize = (int)(image.ElemSize() * image.Total());
        imageTexture.LoadRawTextureData(image.data, imageDataSize);
        imageTexture.Apply(false);
      }

      public void AddNextFrameForCalibration()
      {
        addNextFrame = true;
      }

      public double Calibrate()
      {
        calibrate = true;
        AddFrameButton.enabled = false;
        CalibrateButton.enabled = false;

        // Prepare data for calibration
        Utility.VectorVectorPoint2f allCornersContenated = new Utility.VectorVectorPoint2f();
        Utility.VectorInt allIdsContanated = new Utility.VectorInt();
        Utility.VectorInt markerCounterPerFrame = new Utility.VectorInt();

        uint allCornersSize = allCorners.Size();
        markerCounterPerFrame.Reserve(allCornersSize);
        for (uint i = 0; i < allCornersSize; i++)
        {
          Utility.VectorVectorPoint2f allCornersI = allCorners.At(i);
          uint allCornersISize = allCornersI.Size();
          markerCounterPerFrame.PushBack((int)allCornersISize);
          for (uint j = 0; j < allCornersISize; j++)
          {
            allCornersContenated.PushBack(allCornersI.At(j));
            allIdsContanated.PushBack(allIds.At(i).At(j));
          }
        }

        // Calibrate camera
        Utility.Mat cameraMatrix = new Utility.Mat();
        Utility.Mat distCoeffs = new Utility.Mat();
        Utility.VectorMat rvecs, tvecs;
        double repError = Methods.CalibrateCameraAruco(allCornersContenated, allIdsContanated, markerCounterPerFrame, board, imageSize, cameraMatrix, 
          distCoeffs, out rvecs, out tvecs, (int)calibrationFlags);

        // Save camera parameters
        // TODO

        return repError;
      }

      void UpdateImagesForCalibrationText()
      {
        ImagesForCalibrationText.text = "Images for calibration: " + allIds.Size();
      }
    }
  }
}