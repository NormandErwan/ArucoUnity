using ArucoUnity.Plugin;
using UnityEngine;
using System;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationFlagsControllers
  {
    public class CalibrationFlagsController : CalibrationFlagsBaseController
    {
      // Constants

      const float DEFAULT_FIX_ASPECT_RATIO = 1f;

      // Editor fields

      [SerializeField]
      private bool fixAspectRatio = false;

      [SerializeField]
      private float fixAspectRatioValue = DEFAULT_FIX_ASPECT_RATIO;

      [SerializeField]
      private bool zeroTangentialDistorsion = false;

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

      [Header("Stereo only")]
      [SerializeField]
      private bool fixFocalLength = false;

      [SerializeField]
      private bool fixIntrinsic = false;

      [SerializeField]
      private bool sameFocalLength = false;

      // Properties

      public bool FixAspectRatio { get { return fixAspectRatio; } set { fixAspectRatio = value; } }

      public float FixAspectRatioValue { get { return fixAspectRatioValue; } set { fixAspectRatioValue = value; } }

      public bool ZeroTangentialDistorsion { get { return zeroTangentialDistorsion; } set { zeroTangentialDistorsion = value; } }

      public bool RationalModel { get { return rationalModel; } set { rationalModel = value; } }

      public bool ThinPrismModel { get { return thinPrismModel; } set { thinPrismModel = value; } }

      public bool FixS1_S2_S3_S4 { get { return fixS1_S2_S3_S4; } set { fixS1_S2_S3_S4 = value; } }

      public bool TiltedModel { get { return tiltedModel; } set { tiltedModel = value; } }

      public bool FixTauxTauy { get { return fixTauxTauy; } set { fixTauxTauy = value; } }

      public bool FixFocalLength { get { return fixFocalLength; } set { fixFocalLength = value; } }

      public bool FixIntrinsic { get { return fixIntrinsic; } set { fixIntrinsic = value; } }

      public bool SameFocalLength { get { return sameFocalLength; } set { sameFocalLength = value; } }

      public Cv.Calib3d.Calib CalibrationFlags
      {
        get
        {
          UpdateCalibrationFlags();
          return calibrationFlags;
        }
        set
        {
          calibrationFlags = value;
          UpdateCalibrationOptions();
        }
      }

      public override int CalibrationFlagsValue
      {
        get { return (int)CalibrationFlags; }
        set { CalibrationFlags = (Cv.Calib3d.Calib)value; }
      }

      protected override int FixKLength { get { return 6; } set { } }

      // Variables

      private Cv.Calib3d.Calib calibrationFlags;

      // Methods

      protected override void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess)             { calibrationFlags |= Cv.Calib3d.Calib.UseIntrinsicGuess; }
        if (FixPrincipalPoint)             { calibrationFlags |= Cv.Calib3d.Calib.FixPrincipalPoint; }
        if (FixKDistorsionCoefficients[0]) { calibrationFlags |= Cv.Calib3d.Calib.FixK1; }
        if (FixKDistorsionCoefficients[1]) { calibrationFlags |= Cv.Calib3d.Calib.FixK2; }
        if (FixKDistorsionCoefficients[2]) { calibrationFlags |= Cv.Calib3d.Calib.FixK3; }
        if (FixKDistorsionCoefficients[3]) { calibrationFlags |= Cv.Calib3d.Calib.FixK4; }
        if (FixKDistorsionCoefficients[4]) { calibrationFlags |= Cv.Calib3d.Calib.FixK5; }
        if (FixKDistorsionCoefficients[5]) { calibrationFlags |= Cv.Calib3d.Calib.FixK6; }
        if (FixAspectRatio)                { calibrationFlags |= Cv.Calib3d.Calib.FixAspectRatio; }
        if (ZeroTangentialDistorsion)      { calibrationFlags |= Cv.Calib3d.Calib.ZeroTangentDist; }
        if (RationalModel)                 { calibrationFlags |= Cv.Calib3d.Calib.RationalModel; }
        if (ThinPrismModel)                { calibrationFlags |= Cv.Calib3d.Calib.ThinPrismModel; }
        if (FixS1_S2_S3_S4)                { calibrationFlags |= Cv.Calib3d.Calib.FixS1S2S3S4; }
        if (TiltedModel)                   { calibrationFlags |= Cv.Calib3d.Calib.TiltedModel; }
        if (FixTauxTauy)                   { calibrationFlags |= Cv.Calib3d.Calib.FixTauxTauy; }
        if (FixFocalLength)                { calibrationFlags |= Cv.Calib3d.Calib.FixFocalLength; }
        if (FixIntrinsic)                  { calibrationFlags |= Cv.Calib3d.Calib.FixIntrinsic; }
        if (SameFocalLength)               { calibrationFlags |= Cv.Calib3d.Calib.SameFocalLength; }
      }

      protected override void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess =             Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.UseIntrinsicGuess);
        FixPrincipalPoint =             Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixPrincipalPoint);
        FixKDistorsionCoefficients[0] = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK1);
        FixKDistorsionCoefficients[1] = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK2);
        FixKDistorsionCoefficients[2] = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK3);
        FixKDistorsionCoefficients[3] = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK4);
        FixKDistorsionCoefficients[4] = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK5);
        FixKDistorsionCoefficients[5] = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK6);
        ZeroTangentialDistorsion =      Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.ZeroTangentDist);
        RationalModel =                 Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.RationalModel);
        ThinPrismModel =                Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.ThinPrismModel);
        FixS1_S2_S3_S4 =                Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixS1S2S3S4);
        TiltedModel =                   Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.TiltedModel);
        FixTauxTauy =                   Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixTauxTauy);
        FixFocalLength =                Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixFocalLength);
        FixIntrinsic =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixIntrinsic);
        SameFocalLength =               Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.SameFocalLength);
      }
    }
  }

  /// \} aruco_unity_package
}