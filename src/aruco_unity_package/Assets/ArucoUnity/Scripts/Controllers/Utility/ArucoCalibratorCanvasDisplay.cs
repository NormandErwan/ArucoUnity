using ArucoUnity.Cameras;
using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.Utility
  {
    public class ArucoCalibratorCanvasDisplay : MonoBehaviour
    {
      // Editor fields

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

      // Variables

      private Text[] calibrationReprojectionErrorTexts;

      // MonoBehaviour methods

      /// <summary>
      /// Prepare the buttons and subscribe to ArucoCalibrator configured event to set the image display.
      /// </summary>
      protected void Awake()
      {
        // Deactivate the buttons
        addFrameButton.enabled = false;
        calibrateButton.enabled = false;
        resetButton.enabled = false;

        // Bind the button clicks
        addFrameButton.onClick.AddListener(AddFrameForCalibration);
        calibrateButton.onClick.AddListener(Calibrate);
        resetButton.onClick.AddListener(ResetCalibration);

        // Suscribe to ArucoCalibrator events
        arucoCalibrator.Configured += ConfigureUI;
        if (arucoCalibrator.IsConfigured)
        {
          ConfigureUI();
        }
      }

      /// <summary>
      /// Configure the images display.
      /// </summary>
      protected void ConfigureUI()
      {
        // Configure the buttons
        addFrameButton.enabled = true;
        calibrateButton.enabled = false;
        resetButton.enabled = false;

        // Configure the images display
        ArucoCamera arucoCamera = arucoCalibrator.ArucoCamera;
        calibrationReprojectionErrorTexts = new Text[arucoCamera.CameraNumber];

        // Configure the arucoCameraImagesRect as a grid of images
        int gridCols = 1, gridRows = 1;
        for (int i = 0; i < arucoCamera.CameraNumber; i++)
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
        for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
        {
          int cellCol = cameraId % gridCols; // Range : 0 to (gridCols - 1), images from left ot right
          int cellRow = (gridRows - 1) - (cameraId / gridCols); // Range : (gridRows - 1) to 0, images from top to bottom

          // Create a cell on the grid for each camera image
          GameObject cell = new GameObject("Image " + cameraId + " display");
          RectTransform cellRect = cell.AddComponent<RectTransform>();
          cellRect.SetParent(arucoCameraImagesRect);
          cellRect.anchorMin = new Vector2(1f / gridCols * cellCol, 1f / gridRows * cellRow); // Cell's position
          cellRect.anchorMax = cellRect.anchorMin + gridCellSize; // All cells have the same size
          cellRect.offsetMin = cellRect.offsetMax = Vector2.zero; // No margins
          cellRect.localScale = Vector3.one;

          // Create an image display inside the cell
          GameObject cellDisplay = new GameObject("Image");
          cellDisplay.transform.SetParent(cellRect);
          cellDisplay.transform.localScale = Vector3.one;

          RawImage cellDisplayImage = cellDisplay.AddComponent<RawImage>();
          cellDisplayImage.texture = arucoCamera.ImageTextures[cameraId];

          AspectRatioFitter cellDisplayFitter = cellDisplay.AddComponent<AspectRatioFitter>(); // Fit the image inside the cell
          cellDisplayFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
          cellDisplayFitter.aspectRatio = arucoCamera.ImageRatios[cameraId];

          // Create a text for calibration reprojection error inside the cell
          GameObject reproError = new GameObject("CalibrationReprojectionErrorText");
          RectTransform reproErrorRect = reproError.AddComponent<RectTransform>();
          reproErrorRect.SetParent(cellRect);
          reproErrorRect.pivot = Vector2.zero;
          reproErrorRect.anchorMin = reproErrorRect.anchorMax = Vector2.zero;
          reproErrorRect.offsetMin = Vector2.one * 5; // Pos X and pos Y margins
          reproErrorRect.offsetMax = new Vector2(70, 60); // width and Height
          reproErrorRect.localScale = Vector3.one;

          Text reproErrorText = reproError.AddComponent<Text>();
          reproErrorText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
          reproErrorText.fontSize = 12;
          reproErrorText.color = Color.red;
          calibrationReprojectionErrorTexts[cameraId] = reproErrorText;
        }

        // Configure the text
        UpdateFramesForCalibrationText();
        UpdateCalibrationReprojectionErrorText();
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

        arucoCalibrator.AddCurrentFrameForCalibration();

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
        string frames = (arucoCalibrator.MarkerIds != null && arucoCalibrator.MarkerIds[0] != null) ? "" + arucoCalibrator.MarkerIds[0].Size() : "0";
        framesForCalibrationText.text = "Frames for calibration: " + frames;
      }

      /// <summary>
      /// Update text for of the calibration result.
      /// </summary>
      private void UpdateCalibrationReprojectionErrorText()
      {
        for (int cameraId = 0; cameraId < arucoCalibrator.ArucoCamera.CameraNumber; cameraId++)
        {
          calibrationReprojectionErrorTexts[cameraId].text = "Camera " + (cameraId + 1) + "/" + arucoCalibrator.ArucoCamera.CameraNumber + "\n"
           + "Reprojection error: " 
           + ((arucoCalibrator.CameraParameters != null) ? arucoCalibrator.CameraParameters.ReprojectionErrors[cameraId].ToString("F3") : "0.000");
        }
      }
    }
  }

  /// \} aruco_unity_package
}