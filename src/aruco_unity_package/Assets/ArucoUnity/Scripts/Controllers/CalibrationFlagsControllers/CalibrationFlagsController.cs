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

      // Properties

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

      protected override int FixKLength { get { return 6; } set { } }

      // Variables

      private Cv.Calib3d.Calib calibrationFlags;

      // Methods

      protected override void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess)        { calibrationFlags |= Cv.Calib3d.Calib.UseIntrinsicGuess; }
        if (FixPrincipalPoint)        { calibrationFlags |= Cv.Calib3d.Calib.FixPrincipalPoint; }
        if (FixK[0])                  { calibrationFlags |= Cv.Calib3d.Calib.FixK1; }
        if (FixK[1])                  { calibrationFlags |= Cv.Calib3d.Calib.FixK2; }
        if (FixK[2])                  { calibrationFlags |= Cv.Calib3d.Calib.FixK3; }
        if (FixK[3])                  { calibrationFlags |= Cv.Calib3d.Calib.FixK4; }
        if (FixK[4])                  { calibrationFlags |= Cv.Calib3d.Calib.FixK5; }
        if (FixK[5])                  { calibrationFlags |= Cv.Calib3d.Calib.FixK6; }
        if (FixAspectRatio)           { calibrationFlags |= Cv.Calib3d.Calib.FixAspectRatio; }
        if (ZeroTangentialDistorsion) { calibrationFlags |= Cv.Calib3d.Calib.ZeroTangentDist; }
        if (RationalModel)            { calibrationFlags |= Cv.Calib3d.Calib.RationalModel; }
        if (ThinPrismModel)           { calibrationFlags |= Cv.Calib3d.Calib.ThinPrismModel; }
        if (FixS1_S2_S3_S4)           { calibrationFlags |= Cv.Calib3d.Calib.FixS1S2S3S4; }
        if (TiltedModel)              { calibrationFlags |= Cv.Calib3d.Calib.TiltedModel; }
        if (FixTauxTauy)              { calibrationFlags |= Cv.Calib3d.Calib.FixTauxTauy; }
      }

      protected override void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess =        Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.UseIntrinsicGuess);
        FixPrincipalPoint =        Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixPrincipalPoint);
        FixK[0] =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK1);
        FixK[1] =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK2);
        FixK[2] =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK3);
        FixK[3] =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK4);
        FixK[4] =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK5);
        FixK[5] =                  Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixK6);
        ZeroTangentialDistorsion = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.ZeroTangentDist);
        RationalModel =            Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.RationalModel);
        ThinPrismModel =           Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.ThinPrismModel);
        FixS1_S2_S3_S4 =           Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixS1S2S3S4);
        TiltedModel =              Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.TiltedModel);
        FixTauxTauy =              Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Calib.FixTauxTauy);
      }
    }
  }

  /// \} aruco_unity_package
}