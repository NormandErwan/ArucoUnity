using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationFlagsControllers
  {
    public abstract class CalibrationFlagsController : MonoBehaviour
    {
      // Editor fields

      [Header("Calibration flags")]
      [SerializeField]
      private bool useIntrinsicGuess = false;

      [SerializeField]
      private bool[] fixKDistorsionCoefficients;

      // Properties

      public bool UseIntrinsicGuess { get { return useIntrinsicGuess; } set { useIntrinsicGuess = value; } }

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

      public abstract int CalibrationFlagsValue { get; set; }

      protected abstract int FixKLength { get; }

      // Methods

      protected abstract void UpdateCalibrationFlags();

      protected abstract void UpdateCalibrationOptions();

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