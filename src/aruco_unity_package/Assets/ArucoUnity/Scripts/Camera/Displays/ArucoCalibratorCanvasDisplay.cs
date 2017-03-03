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

        // Configure the arucoCameraImagesRect as a grid of images
        int gridCols = 1, gridRows = 1;
        for (int i = 0; i < arucoCamera.CamerasNumber; i++)
        {
          if (gridCols * gridRows > i)
          {
            continue;
          }
          else if (arucoCameraImagesRect.rect.width / gridCols >= arucoCameraImagesRect.rect.height / gridRows)
          {
            gridCols++;
          }
          else
          {
            gridRows++;
          }
        }
        Vector2 gridCellSize = new Vector2(1f / gridCols, 1f / gridRows);

        // Configure the cells of the grid of images
        for (int i = 0; i < arucoCamera.CamerasNumber; i++)
        {
          int cellCol = i % gridCols; // Range : 0 to (gridCols - 1), images from left ot right
          int cellRow = (gridRows - 1) - (i / gridCols); // Range : (gridRows - 1) to 0, images from top to bottom

          // Create a cell on the grid for each camera image
          GameObject cell = new GameObject("Image " + i + " display", typeof(RectTransform));
          RectTransform cellRect = cell.GetComponent<RectTransform>();
          cellRect.SetParent(arucoCameraImagesRect);
          cellRect.anchorMin = new Vector2(1f / gridCols * cellCol, 1f / gridRows * cellRow);
          cellRect.anchorMax = cellRect.anchorMin + gridCellSize;
          cellRect.offsetMin = cellRect.offsetMax = Vector2.zero;
          cellRect.localScale = Vector3.one;

          // Create an image display inside the cell
          GameObject cellDisplay = new GameObject("Image", typeof(RectTransform));
          RectTransform cellDisplayRect = cellDisplay.GetComponent<RectTransform>();
          cellDisplayRect.SetParent(cellRect);
          cellDisplayRect.localScale = arucoCamera.ImageScalesFrontFacing[i];

          RawImage cellDisplayImage = cellDisplay.AddComponent<RawImage>();
          cellDisplayImage.texture = arucoCamera.ImageTextures[i];
          cellDisplayImage.uvRect = arucoCamera.ImageUvRectFlips[i];

          AspectRatioFitter cellDisplayFitter = cellDisplay.AddComponent<AspectRatioFitter>();
          cellDisplayFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
          cellDisplayFitter.aspectRatio = arucoCamera.ImageRatios[i];
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