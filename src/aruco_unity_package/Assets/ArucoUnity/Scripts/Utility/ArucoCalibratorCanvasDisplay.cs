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
      private Button addFrameButton;

      [SerializeField]
      private Button calibrateButton;

      [SerializeField]
      private Button resetButton;

      [SerializeField]
      private Text imagesForCalibrationText;

      [SerializeField]
      private Text calibrationReprojectionErrorText;

      // MonoBehaviour methods

      /// <summary>
      /// Set up the UI. 
      /// </summary>
      protected void Awake()
      {
        addFrameButton.onClick.AddListener(AddFrameForCalibration);
        calibrateButton.onClick.AddListener(Calibrate);
        resetButton.onClick.AddListener(ResetCalibration);
      }

      private void AddFrameForCalibration()
      {
        if (arucoCalibrator.IsConfigured || arucoCalibrator.IsCalibrated)
        {
          return;
        }

        arucoCalibrator.AddFrameForCalibration();

        UpdateImagesForCalibrationText();
      }

      private void Calibrate()
      {
        if (arucoCalibrator.IsConfigured)
        {
          return;
        }

        arucoCalibrator.Calibrate();

        addFrameButton.enabled = false;
        calibrateButton.enabled = false;
        UpdateCalibrationReprojectionErrorText();
      }

      private void ResetCalibration()
      {
        arucoCalibrator.ResetCalibration();

        addFrameButton.enabled = true;
        calibrateButton.enabled = false;
        UpdateImagesForCalibrationText();
        UpdateCalibrationReprojectionErrorText();
      }

      void UpdateImagesForCalibrationText()
      {
        imagesForCalibrationText.text = "Images for calibration: " + arucoCalibrator.AllIds.Size();
      }

      private void UpdateCalibrationReprojectionErrorText()
      {
        calibrationReprojectionErrorText.text = "Calibration reprojection error: "
         + ((arucoCalibrator.CameraParameters != null) ? arucoCalibrator.CameraParameters.ReprojectionError.ToString("F3") : "");
      }
    }
  }

  /// \} aruco_unity_package
}