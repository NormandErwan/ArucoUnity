using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationFlagsControllers
  {
    public abstract class CalibrationFlagsBaseController : MonoBehaviour
    {
      // Editor fields

      [HideInInspector]
      private bool useIntrinsicGuess = false;

      [SerializeField]
      private bool fixPrincipalPoint = false;

      [SerializeField]
      private bool[] fixK;

      // Properties

      public bool UseIntrinsicGuess
      {
        get { return useIntrinsicGuess; }
        set
        {
          useIntrinsicGuess = value;
          UpdateCalibrationFlags();
        }
      }

      public bool FixPrincipalPoint
      {
        get { return fixPrincipalPoint; }
        set
        {
          fixPrincipalPoint = value;
          UpdateCalibrationFlags();
        }
      }

      public bool[] FixK
      {
        get { return fixK; }
        set
        {
          if (value.Length == FixKLength)
          {
            fixK = value;
            UpdateCalibrationFlags();
          }
        }
      }

      public abstract int CalibrationFlagsValue { get; set; }

      protected abstract int FixKLength { get; set; }

      // Methods

      protected abstract void UpdateCalibrationFlags();

      protected abstract void UpdateCalibrationOptions();

      protected virtual void OnValidate()
      {
        if (fixK.Length != FixKLength)
        {
          Array.Resize(ref fixK, FixKLength);
        }
      }
    }
  }

  /// \} aruco_unity_package
}