using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationFlagsControllers
  {
    /// <summary>
    /// Manages flags for the calibration process.
    /// </summary>
    public abstract class CalibrationFlagsController : MonoBehaviour
    {
      // Editor fields

      [Header("Calibration flags")]
      [SerializeField]
      [Tooltip("The CameraMatrix in the camera parameters has valid initial value that will be optimized by the calibration process.")]
      private bool useIntrinsicGuess = false;

      [SerializeField]
      [Tooltip("The corresponding radial distortion coefficient is not changed during the calibration. If useIntrinsicGuess is set, the DistCoeffs"
        + " values in the camera parameters are used. Otherwise, it is set to 0.")]
      private bool[] fixKDistorsionCoefficients;

      // Properties

      /// <summary>
      /// The CameraMatrix in the camera parameters has valid initial value that will be optimized by the calibration process.
      /// </summary>
      public bool UseIntrinsicGuess { get { return useIntrinsicGuess; } set { useIntrinsicGuess = value; } }

      /// <summary>
      /// The corresponding radial distortion coefficient is not changed during the calibration. If useIntrinsicGuess is set, the DistCoeffs
      /// values in the camera parameters are used. Otherwise, it is set to 0.
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
      /// The equivalent int of the calibration flags.
      /// </summary>
      public abstract int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// The lenght of <see cref="FixKDistorsionCoefficients"/> array.
      /// </summary>
      protected abstract int FixKLength { get; }

      // Methods

      /// <summary>
      /// Update the calibration flags from the property values.
      /// </summary>
      protected abstract void UpdateCalibrationFlags();

      /// <summary>
      /// Update the property values from the calibration flags.
      /// </summary>
      protected abstract void UpdateCalibrationOptions();

      /// <summary>
      /// Keep the <see cref="FixKDistorsionCoefficients"/> array to its fixed size in the editor.
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