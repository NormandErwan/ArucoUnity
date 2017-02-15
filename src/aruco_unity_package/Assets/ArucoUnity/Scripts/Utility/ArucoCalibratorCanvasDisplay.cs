using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public class ArucoCalibratorCanvasDisplay : MonoBehaviour
    {
      [SerializeField]
      private ArucoCalibrator arucoCalibrator;

      [SerializeField]
      private RectTransform arucoCameraImagesRect;

      [SerializeField]
      private Button addFrameButton;

      [SerializeField]
      private Button calibrateButton;

      [SerializeField]
      private Button resetButton;

      [SerializeField]
      private Text framesForCalibrationText;

      [SerializeField]
      private Text calibrationReprojectionErrorText;

      // MonoBehaviour methods

      /// <summary>
      /// Configure the UI.
      /// </summary>
      protected void Awake()
      {
        // Configure the buttons
        addFrameButton.enabled = true;
        calibrateButton.enabled = false;
        resetButton.enabled = false;

        addFrameButton.onClick.AddListener(AddFrameForCalibration);
        calibrateButton.onClick.AddListener(Calibrate);
        resetButton.onClick.AddListener(ResetCalibration);

        // Configure the text
        UpdateFramesForCalibrationText();
        UpdateCalibrationReprojectionErrorText();
      }

      /// <summary>
      /// Subscribe to ArucoCalibrator configured to set the image display.
      /// </summary>
      protected void Start()
      {
        arucoCalibrator.Configured += ConfigureImagesDisplay;
        if (arucoCalibrator.IsConfigured)
        {
          ConfigureImagesDisplay();
        }
      }

      /// <summary>
      /// Configure the images display.
      /// </summary>
      protected void ConfigureImagesDisplay()
      {
        ArucoCamera arucoCamera = arucoCalibrator.ArucoCamera;

        // Configure the grid layout on the arucoCameraImagesRect
        GridLayoutGroup arucoCameraImagesGrid = arucoCameraImagesRect.GetComponent<GridLayoutGroup>();
        if (arucoCameraImagesGrid == null)
        {
          arucoCameraImagesGrid = arucoCameraImagesRect.gameObject.AddComponent<GridLayoutGroup>();
        }

        int arucoCameraImagesGridCols = 1;
        int arucoCameraImagesGridRows = 1;
        for (int i = 1; i < arucoCamera.CamerasNumber; i += 2)
        {
          if (arucoCameraImagesRect.rect.width / arucoCameraImagesGridCols >= arucoCameraImagesRect.rect.height / arucoCameraImagesGridRows)
          {
            arucoCameraImagesGridCols++;
          }
          else
          {
            arucoCameraImagesGridRows++;
          }
        }
        arucoCameraImagesGrid.cellSize = new Vector2(arucoCameraImagesRect.rect.width / arucoCameraImagesGridCols, arucoCameraImagesRect.rect.height / arucoCameraImagesGridRows);

        for (int i = 0; i < arucoCamera.CamerasNumber; i++)
        {
          // Create a cell on the grid layout for each camera image
          GameObject imageDisplayParent = new GameObject("Image " + i + " display", typeof(RectTransform));
          RectTransform imageDisplayParentRect = imageDisplayParent.GetComponent<RectTransform>();
          imageDisplayParentRect.SetParent(arucoCameraImagesRect);
          imageDisplayParentRect.anchorMin = Vector2.zero;
          imageDisplayParentRect.anchorMax = Vector2.one;
          imageDisplayParentRect.offsetMin = imageDisplayParentRect.offsetMax = Vector2.zero;
          imageDisplayParentRect.localScale = Vector3.one;

          // Create an image display inside the cell
          GameObject imageDisplay = new GameObject("Image", typeof(RectTransform));
          RectTransform imageDisplayRect = imageDisplay.GetComponent<RectTransform>();
          imageDisplayRect.SetParent(imageDisplayParentRect);
          imageDisplayRect.localScale = arucoCamera.ImageScalesFrontFacing[i];

          RawImage imageDisplayImage = imageDisplay.AddComponent<RawImage>();
          imageDisplayImage.texture = arucoCamera.ImageTextures[i];
          imageDisplayImage.uvRect = arucoCamera.ImageUvRectFlips[i];

          AspectRatioFitter imageDisplayFitter = imageDisplay.AddComponent<AspectRatioFitter>();
          imageDisplayFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
          imageDisplayFitter.aspectRatio = arucoCamera.ImageRatios[i];
        }
      }

      /// <summary>
      /// Add the images of the next frame for the calibration, and update the UI.
      /// </summary>
      private void AddFrameForCalibration()
      {
        if (!arucoCalibrator.IsConfigured)
        {
          return;
        }

        arucoCalibrator.AddFrameForCalibration();

        calibrateButton.enabled = true;
        resetButton.enabled = true;
        UpdateFramesForCalibrationText();
      }

      /// <summary>
      /// Calibrate and update the UI.
      /// </summary>
      private void Calibrate()
      {
        if (!arucoCalibrator.IsConfigured)
        {
          return;
        }

        arucoCalibrator.Calibrate();
        UpdateCalibrationReprojectionErrorText();
      }

      /// <summary>
      /// Reset the calibration and update the UI.
      /// </summary>
      private void ResetCalibration()
      {
        arucoCalibrator.ResetCalibration();

        calibrateButton.enabled = false;
        resetButton.enabled = false;
        UpdateFramesForCalibrationText();
        UpdateCalibrationReprojectionErrorText();
      }

      /// <summary>
      /// Update the text of the number of images added for calibration.
      /// </summary>
      void UpdateFramesForCalibrationText()
      {
        string frames = (arucoCalibrator.AllIds != null) ? "" + arucoCalibrator.AllIds.Size() : "0";
        framesForCalibrationText.text = "Frames for calibration: " + frames;
      }

      /// <summary>
      /// Update text for of the calibration result.
      /// </summary>
      private void UpdateCalibrationReprojectionErrorText()
      {
        calibrationReprojectionErrorText.text = "Calibration reprojection error: "
         + ((arucoCalibrator.CameraParameters != null) ? arucoCalibrator.CameraParameters.ReprojectionError.ToString("F3") : "");
      }
    }
  }

  /// \} aruco_unity_package
}