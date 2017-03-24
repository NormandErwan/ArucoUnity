using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    public class CalibrationFlagsController : MonoBehaviour
    {
      // Constants

      const float DEFAULT_FIX_ASPECT_RATIO = 1f;
      const int FIX_K_SIZE = 6;

      // Editor fields

      [SerializeField]
      private bool useIntrinsicGuess = false;

      [SerializeField]
      private bool fixPrincipalPoint = false;

      [SerializeField]
      private bool fixAspectRatio = false;

      [SerializeField]
      private float fixAspectRatioValue = DEFAULT_FIX_ASPECT_RATIO;

      [SerializeField]
      private bool zeroTangentialDistorsion = false;

      [SerializeField]
      private bool[] fixK = new bool[FIX_K_SIZE];

      [SerializeField]
      private bool rationalModel = false;

      [SerializeField]
      private bool thinPrismModel = false;

      [SerializeField]
      private bool fixS1_S2_S3_S4 = false;

      [SerializeField]
      private bool tiltedModel = false;

      [SerializeField]
      private bool fixTauxTauy = false;

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

      public bool FixAspectRatio
      {
        get { return fixAspectRatio; }
        set
        {
          fixAspectRatio = value;
          UpdateCalibrationFlags();
        }
      }

      public float FixAspectRatioValue
      {
        get { return fixAspectRatioValue; }
        set
        {
          fixAspectRatioValue = value;
          UpdateCalibrationFlags();
        }
      }

      public bool ZeroTangentialDistorsion
      {
        get { return zeroTangentialDistorsion; }
        set
        {
          zeroTangentialDistorsion = value;
          UpdateCalibrationFlags();
        }
      }

      public bool[] FixK
      {
        get { return fixK; }
        set
        {
          if (value.Length == FIX_K_SIZE)
          {
            fixK = value;
            UpdateCalibrationFlags();
          }
        }
      }

      public bool RationalModel
      {
        get { return rationalModel; }
        set
        {
          rationalModel = value;
          UpdateCalibrationFlags();
        }
      }

      public bool ThinPrismModel
      {
        get { return thinPrismModel; }
        set
        {
          thinPrismModel = value;
          UpdateCalibrationFlags();
        }
      }

      public bool FixS1_S2_S3_S4
      {
        get { return fixS1_S2_S3_S4; }
        set
        {
          fixS1_S2_S3_S4 = value;
          UpdateCalibrationFlags();
        }
      }

      public bool TiltedModel
      {
        get { return tiltedModel; }
        set
        {
          tiltedModel = value;
          UpdateCalibrationFlags();
        }
      }

      public bool FixTauxTauy
      {
        get { return fixTauxTauy; }
        set
        {
          fixTauxTauy = value;
          UpdateCalibrationFlags();
        }
      }

      public Cv.CALIB CalibrationFlags
      {
        get { return calibrationFlags; }
        set
        {
          calibrationFlags = value;
          UpdateCalibrationOptions();
        }
      }

      // Variables

      private Cv.CALIB calibrationFlags;

      // Methods

      protected void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess) { calibrationFlags |= Cv.CALIB.USE_INTRINSIC_GUESS; }
        if (FixAspectRatio) { calibrationFlags |= Cv.CALIB.FIX_ASPECT_RATIO; }
        if (FixPrincipalPoint) { calibrationFlags |= Cv.CALIB.FIX_PRINCIPAL_POINT; }
        if (ZeroTangentialDistorsion) { calibrationFlags |= Cv.CALIB.ZERO_TANGENT_DIST; }
        if (FixK[0]) { calibrationFlags |= Cv.CALIB.FIX_K1; }
        if (FixK[1]) { calibrationFlags |= Cv.CALIB.FIX_K2; }
        if (FixK[2]) { calibrationFlags |= Cv.CALIB.FIX_K3; }
        if (FixK[3]) { calibrationFlags |= Cv.CALIB.FIX_K4; }
        if (FixK[4]) { calibrationFlags |= Cv.CALIB.FIX_K5; }
        if (FixK[5]) { calibrationFlags |= Cv.CALIB.FIX_K6; }
        if (RationalModel) { calibrationFlags |= Cv.CALIB.RATIONAL_MODEL; }
        if (ThinPrismModel) { calibrationFlags |= Cv.CALIB.THIN_PRISM_MODEL; }
        if (FixS1_S2_S3_S4) { calibrationFlags |= Cv.CALIB.FIX_S1_S2_S3_S4; }
        if (TiltedModel) { calibrationFlags |= Cv.CALIB.TILTED_MODEL; }
        if (FixTauxTauy) { calibrationFlags |= Cv.CALIB.FIX_TAUX_TAUY; }
      }

      protected void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess = CalibrationFlagsContain(Cv.CALIB.USE_INTRINSIC_GUESS);
        FixAspectRatio = CalibrationFlagsContain(Cv.CALIB.FIX_ASPECT_RATIO);
        FixPrincipalPoint = CalibrationFlagsContain(Cv.CALIB.FIX_PRINCIPAL_POINT);
        ZeroTangentialDistorsion = CalibrationFlagsContain(Cv.CALIB.ZERO_TANGENT_DIST);
        FixK[0] = CalibrationFlagsContain(Cv.CALIB.FIX_K1);
        FixK[1] = CalibrationFlagsContain(Cv.CALIB.FIX_K2);
        FixK[2] = CalibrationFlagsContain(Cv.CALIB.FIX_K3);
        FixK[3] = CalibrationFlagsContain(Cv.CALIB.FIX_K4);
        FixK[4] = CalibrationFlagsContain(Cv.CALIB.FIX_K5);
        FixK[5] = CalibrationFlagsContain(Cv.CALIB.FIX_K6);
        RationalModel = CalibrationFlagsContain(Cv.CALIB.RATIONAL_MODEL);
        ThinPrismModel = CalibrationFlagsContain(Cv.CALIB.THIN_PRISM_MODEL);
        FixS1_S2_S3_S4 = CalibrationFlagsContain(Cv.CALIB.FIX_S1_S2_S3_S4);
        TiltedModel = CalibrationFlagsContain(Cv.CALIB.TILTED_MODEL);
        FixTauxTauy = CalibrationFlagsContain(Cv.CALIB.FIX_TAUX_TAUY);
      }

      protected bool CalibrationFlagsContain(Cv.CALIB calibrationFlag)
      {
        return (CalibrationFlags & calibrationFlag) == calibrationFlag;
      }

      protected void OnValidate()
      {
        if (fixK.Length != FIX_K_SIZE)
        {
          Array.Resize(ref fixK, FIX_K_SIZE);
        }
      }
    }
  }

  /// \} aruco_unity_package
}