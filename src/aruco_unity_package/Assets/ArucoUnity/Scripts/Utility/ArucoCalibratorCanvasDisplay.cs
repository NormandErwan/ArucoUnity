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
      /// Set up the images display.
      /// </summary>
      protected void ConfigureImagesDisplay()
      {
        ArucoCamera arucoCamera = arucoCalibrator.ArucoCamera;

        for (int i = 0; i < arucoCamera.CamerasNumber; i++)
        {
          GameObject imageDisplay = new GameObject("Image " + i + " display", typeof(RectTransform));
          RectTransform imageDisplayRect = imageDisplay.GetComponent<RectTransform>();
          imageDisplayRect.SetParent(arucoCameraImagesRect);
          imageDisplayRect.anchorMin = Vector2.zero;
          imageDisplayRect.anchorMax = Vector2.one;
          imageDisplayRect.offsetMin = imageDisplayRect.offsetMax = Vector2.zero;
          imageDisplayRect.localScale = arucoCalibrator.ArucoCamera.ImageScalesFrontFacing[i];

          RawImage imageDisplayImage = imageDisplay.AddComponent<RawImage>();
          imageDisplayImage.texture = arucoCalibrator.ArucoCamera.ImageTextures[i];
          imageDisplayImage.uvRect = arucoCalibrator.ArucoCamera.ImageUvRectFlips[i];
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