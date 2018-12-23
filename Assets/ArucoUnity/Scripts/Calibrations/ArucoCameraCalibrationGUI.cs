using ArucoUnity.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArucoUnity.Calibration
{
    public class ArucoCameraCalibrationGUI : MonoBehaviour
    {
        // Editor fields

        [SerializeField]
        private ArucoCameraCalibration arucoCameraCalibration;

        [SerializeField]
        private RectTransform arucoCameraImagesRect;

        [SerializeField]
        private Button addImagesButton;

        [SerializeField]
        private Text imagesCountText;

        [SerializeField]
        private Button calibrateButton;

        [SerializeField]
        private Text calibrationStatusText;

        [SerializeField]
        private Button resetButton;

        // Variables

        private Text[] calibrationReprojectionErrorTexts;
        private Text calibrateButtonText;

        // MonoBehaviour methods

        /// <summary>
        /// Prepares the buttons and subscribes to ArucoCalibrator started event to set the image display.
        /// </summary>
        protected void Awake()
        {
            calibrateButtonText = calibrateButton.transform.GetChild(0).GetComponent<Text>();

            // Configure the buttons
            addImagesButton.enabled = false;
            calibrateButton.enabled = false;
            resetButton.enabled = false;

            // Bind the button clicks
            addImagesButton.onClick.AddListener(AddImagesForCalibration);
            calibrateButton.onClick.AddListener(Calibrate);
            resetButton.onClick.AddListener(ResetCalibration);

            // Suscribe to ArucoCalibrator events
            if (arucoCameraCalibration.IsStarted)
            {
                ConfigureUI(this, EventArgs.Empty);
            }
            arucoCameraCalibration.Started += ConfigureUI;
            arucoCameraCalibration.Calibrated += Calibrated;
        }

        /// <summary>
        /// Unsubscribes from ArucoCalibrator events.
        /// </summary>
        protected void OnDestroy()
        {
            arucoCameraCalibration.Started -= ConfigureUI;
            arucoCameraCalibration.Calibrated -= Calibrated;
        }

        /// <summary>
        /// Configures the images display.
        /// </summary>
        protected void ConfigureUI(object sender, EventArgs e)
        {
            // Configure the buttons
            addImagesButton.enabled = true;
            calibrateButton.enabled = false;
            resetButton.enabled = false;

            // Configure the images display
            var arucoCamera = arucoCameraCalibration.ArucoCamera;
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
                int cellCol = cameraId % gridCols; // Range : 0 to (gridCols - 1), images from left to right
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
                cellDisplayImage.texture = arucoCamera.Textures[cameraId];

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
                reproErrorRect.offsetMax = new Vector2(120, 60); // width and Height
                reproErrorRect.localScale = Vector3.one;

                Text reproErrorText = reproError.AddComponent<Text>();
                reproErrorText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                reproErrorText.fontSize = 12;
                reproErrorText.color = Color.red;
                calibrationReprojectionErrorTexts[cameraId] = reproErrorText;
            }

            // Configure the text
            UpdateImagesCountText();
            UpdateCalibrationReprojectionErrorText();
        }

        /// <summary>
        /// Adds the current images to the calibration, and update the UI.
        /// </summary>
        private void AddImagesForCalibration()
        {
            if (!arucoCameraCalibration.IsConfigured)
            {
                return;
            }

            arucoCameraCalibration.AddImages();

            calibrateButton.enabled = true;
            resetButton.enabled = true;
            UpdateImagesCountText();
        }

        /// <summary>
        /// Calibrates and updates the UI.
        /// </summary>
        private void Calibrate()
        {
            if (!arucoCameraCalibration.IsConfigured)
            {
                return;
            }

            if (!arucoCameraCalibration.CalibrationRunning)
            {
                arucoCameraCalibration.CalibrateAsync();
                calibrateButtonText.text = "Stop calibration";
                calibrationStatusText.text = "Calibration status : running";
            }
            else
            {
                arucoCameraCalibration.CancelCalibrateAsync();
                calibrateButtonText.text = "Calibrate";
                calibrationStatusText.text = "Calibration status : stopped";
            }
        }

        /// <summary>
        /// Updates the UI with the calibration results.
        /// </summary>
        private void Calibrated()
        {
            calibrateButtonText.text = "Calibrate";
            calibrationStatusText.text = "Calibration status : finished";
            UpdateCalibrationReprojectionErrorText();
        }

        /// <summary>
        /// Resets the calibration and update the UI.
        /// </summary>
        private void ResetCalibration()
        {
            arucoCameraCalibration.ResetCalibration();

            calibrateButton.enabled = false;
            resetButton.enabled = false;
            UpdateImagesCountText();
            UpdateCalibrationReprojectionErrorText();
        }

        /// <summary>
        /// Updates the text of the number of images added for calibration.
        /// </summary>
        private void UpdateImagesCountText()
        {
            string imagesCount = (arucoCameraCalibration.AllMarkerIds != null && arucoCameraCalibration.AllMarkerIds[0] != null) ? "" + arucoCameraCalibration.AllMarkerIds[0].Size() : "0";
            imagesCountText.text = "Images count: " + imagesCount;
        }

        /// <summary>
        /// Updates text for of the calibration results.
        /// </summary>
        private void UpdateCalibrationReprojectionErrorText()
        {
            for (int cameraId = 0; cameraId < arucoCameraCalibration.ArucoCamera.CameraNumber; cameraId++)
            {
                double reprojectionError = (arucoCameraCalibration.CameraParametersController.CameraParameters != null)
                    ? arucoCameraCalibration.CameraParametersController.CameraParameters.ReprojectionErrors[cameraId]
                    : 0.0;

                calibrationReprojectionErrorTexts[cameraId].text = "Camera " + (cameraId + 1) + "/" + arucoCameraCalibration.ArucoCamera.CameraNumber + "\n"
                 + "Reprojection error: " + reprojectionError.ToString("F3");
            }
        }
    }
}