using ArucoUnity.Plugin;
using UnityEngine;
using System;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationFlagsControllers
  {
    public class CalibrationFlagsFisheyeController : CalibrationFlagsBaseController
    {
      // Editor fields

      [SerializeField]
      private bool recomputeExtrinsic = false;

      [SerializeField]
      private bool checkCond = false;

      [SerializeField]
      private bool fixSkew = false;

      [Header("Stereo only")]
      [SerializeField]
      private bool fixIntrinsic = false;

      // Properties

      public bool RecomputeExtrinsic { get { return recomputeExtrinsic; } set { recomputeExtrinsic = value; } }

      public bool CheckCond { get { return checkCond; } set { checkCond = value; } }

      public bool FixSkew { get { return fixSkew; } set { fixSkew = value; } }

      public bool FixIntrinsic { get { return fixIntrinsic; } set { fixIntrinsic = value; } }

      public Cv.Fisheye.Calib CalibrationFlags
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
        set { CalibrationFlags = (Cv.Fisheye.Calib)value; }
      }

      protected override int FixKLength { get { return 4; } set { } }

      // Variables

      Cv.Fisheye.Calib calibrationFlags;

      // Methods

      protected override void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess)             { calibrationFlags |= Cv.Fisheye.Calib.UseIntrinsicGuess; }
        if (FixPrincipalPoint)             { calibrationFlags |= Cv.Fisheye.Calib.FixPrincipalPoint; }
        if (FixKDistorsionCoefficients[0]) { calibrationFlags |= Cv.Fisheye.Calib.FixK1; }
        if (FixKDistorsionCoefficients[1]) { calibrationFlags |= Cv.Fisheye.Calib.FixK2; }
        if (FixKDistorsionCoefficients[2]) { calibrationFlags |= Cv.Fisheye.Calib.FixK3; }
        if (FixKDistorsionCoefficients[3]) { calibrationFlags |= Cv.Fisheye.Calib.FixK4; }
        if (RecomputeExtrinsic)            { calibrationFlags |= Cv.Fisheye.Calib.UseIntrinsicGuess; }
        if (CheckCond)                     { calibrationFlags |= Cv.Fisheye.Calib.CheckCond; }
        if (FixSkew)                       { calibrationFlags |= Cv.Fisheye.Calib.FixPrincipalPoint; }
        if (FixIntrinsic)                  { calibrationFlags |= Cv.Fisheye.Calib.FixIntrinsic; }
      }

      protected override void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess =             Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.UseIntrinsicGuess);
        FixPrincipalPoint =             Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixPrincipalPoint);
        FixKDistorsionCoefficients[0] = Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixK1);
        FixKDistorsionCoefficients[1] = Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixK2);
        FixKDistorsionCoefficients[2] = Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixK3);
        FixKDistorsionCoefficients[3] = Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixK4);
        RecomputeExtrinsic =            Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.RecomputeExtrinsic);
        CheckCond =                     Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.CheckCond);
        FixSkew =                       Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixSkew);
        FixIntrinsic =                  Enum.IsDefined(typeof(Cv.Fisheye.Calib), Cv.Fisheye.Calib.FixIntrinsic);
      }
    }
  }

  /// \} aruco_unity_package
}