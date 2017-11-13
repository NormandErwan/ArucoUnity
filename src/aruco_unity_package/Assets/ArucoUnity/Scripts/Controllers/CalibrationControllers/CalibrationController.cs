using ArucoUnity.Cameras.Parameters;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationControllers
  {
    /// <summary>
    /// Manages the flags of the calibration, undistortion and rectification processes.
    /// </summary>
    public abstract class CalibrationController : MonoBehaviour
    {
      // Editor fields

      [Header("Calibration flags")]
      [SerializeField]
      [Tooltip("Use and optimize the initial values (fx, fy), (cx, cy) of the camera matrix during the calibration process.")]
      private bool useIntrinsicGuess = false;

      [SerializeField]
      [Tooltip("The corresponding radial distortion coefficient is not changed during the calibration. If useIntrinsicGuess is set, the original" +
        " DistCoeffs value in the camera parameters are used, otherwise it's to 0.")]
      private bool[] fixKDistorsionCoefficients;

      // Properties

      /// <summary>
      /// Gets or sets if the <see cref="CameraParameters.CameraMatrices"/> has valid initial value that will be optimized by the calibration process.
      /// </summary>
      public bool UseIntrinsicGuess { get { return useIntrinsicGuess; } set { useIntrinsicGuess = value; } }

      /// <summary>
      /// Gets or sets if the corresponding radial distortion coefficients are not changed during the calibration. If useIntrinsicGuess is set, the
      /// original <see cref="CameraParameters.DistCoeffs"/> values in the camera parameters are used, otherwise they're set to 0.
      /// </summary>
      public bool[] FixKDistorsionCoefficients
      {
        get { return fixKDistorsionCoefficients; }
        set {
          if (value.Length == FixKLength)
          {
            fixKDistorsionCoefficients = value;
            UpdateCalibrationFlags();
          }
        }
      }

      /// <summary>
      /// Gets or sets if the equivalent int, used by OpenCV, of the calibration flags.
      /// </summary>
      public abstract int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// Gets the length of <see cref="FixKDistorsionCoefficients"/> array.
      /// </summary>
      protected abstract int FixKLength { get; }

      // Methods

      /// <summary>
      /// Updates <see cref="CalibrationFlagsValue"/> from the flag properties.
      /// </summary>
      protected abstract void UpdateCalibrationFlags();

      /// <summary>
      /// Updates the flag property values from <see cref="CalibrationFlagsValue"/>.
      /// </summary>
      protected abstract void UpdateCalibrationOptions();

      /// <summary>
      /// Keeps the <see cref="FixKDistorsionCoefficients"/> array to its fixed length <see cref="FixKLength"/> in the editor.
      /// </summary>
      protected virtual void OnValidate()
      {
        if (fixKDistorsionCoefficients.Length != FixKLength)
        {
          Array.Resize(ref fixKDistorsionCoefficients, FixKLength);
        }
      }
    }
  }

  /// \} aruco_unity_package
}