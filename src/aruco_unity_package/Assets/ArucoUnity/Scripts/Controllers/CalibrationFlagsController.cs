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

      public Cv.Calib3d.Calib CalibrationFlags
      {
        get { return calibrationFlags; }
        set
        {
          calibrationFlags = value;
          UpdateCalibrationOptions();
        }
      }

      // Variables

      private Cv.Calib3d.Calib calibrationFlags;

      // Methods

      protected void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess) { calibrationFlags |= Cv.Calib3d.Calib.UseIntrinsicGuess; }
        if (FixAspectRatio) { calibrationFlags |= Cv.Calib3d.Calib.FixAspectRatio; }
        if (FixPrincipalPoint) { calibrationFlags |= Cv.Calib3d.Calib.FixPrincipalPoint; }
        if (ZeroTangentialDistorsion) { calibrationFlags |= Cv.Calib3d.Calib.ZeroTangentDist; }
        if (FixK[0]) { calibrationFlags |= Cv.Calib3d.Calib.FixK1; }
        if (FixK[1]) { calibrationFlags |= Cv.Calib3d.Calib.FixK2; }
        if (FixK[2]) { calibrationFlags |= Cv.Calib3d.Calib.FixK3; }
        if (FixK[3]) { calibrationFlags |= Cv.Calib3d.Calib.FixK4; }
        if (FixK[4]) { calibrationFlags |= Cv.Calib3d.Calib.FixK5; }
        if (FixK[5]) { calibrationFlags |= Cv.Calib3d.Calib.FixK6; }
        if (RationalModel) { calibrationFlags |= Cv.Calib3d.Calib.RationalModel; }
        if (ThinPrismModel) { calibrationFlags |= Cv.Calib3d.Calib.ThinPrismModel; }
        if (FixS1_S2_S3_S4) { calibrationFlags |= Cv.Calib3d.Calib.FixS1S2S3S4; }
        if (TiltedModel) { calibrationFlags |= Cv.Calib3d.Calib.TiltedModel; }
        if (FixTauxTauy) { calibrationFlags |= Cv.Calib3d.Calib.FixTauxTauy; }
      }

      protected void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess = CalibrationFlagsContain(Cv.Calib3d.Calib.UseIntrinsicGuess);
        FixAspectRatio = CalibrationFlagsContain(Cv.Calib3d.Calib.FixAspectRatio);
        FixPrincipalPoint = CalibrationFlagsContain(Cv.Calib3d.Calib.FixPrincipalPoint);
        ZeroTangentialDistorsion = CalibrationFlagsContain(Cv.Calib3d.Calib.ZeroTangentDist);
        FixK[0] = CalibrationFlagsContain(Cv.Calib3d.Calib.FixK1);
        FixK[1] = CalibrationFlagsContain(Cv.Calib3d.Calib.FixK2);
        FixK[2] = CalibrationFlagsContain(Cv.Calib3d.Calib.FixK3);
        FixK[3] = CalibrationFlagsContain(Cv.Calib3d.Calib.FixK4);
        FixK[4] = CalibrationFlagsContain(Cv.Calib3d.Calib.FixK5);
        FixK[5] = CalibrationFlagsContain(Cv.Calib3d.Calib.FixK6);
        RationalModel = CalibrationFlagsContain(Cv.Calib3d.Calib.RationalModel);
        ThinPrismModel = CalibrationFlagsContain(Cv.Calib3d.Calib.ThinPrismModel);
        FixS1_S2_S3_S4 = CalibrationFlagsContain(Cv.Calib3d.Calib.FixS1S2S3S4);
        TiltedModel = CalibrationFlagsContain(Cv.Calib3d.Calib.TiltedModel);
        FixTauxTauy = CalibrationFlagsContain(Cv.Calib3d.Calib.FixTauxTauy);
      }

      protected bool CalibrationFlagsContain(Cv.Calib3d.Calib calibrationFlag)
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