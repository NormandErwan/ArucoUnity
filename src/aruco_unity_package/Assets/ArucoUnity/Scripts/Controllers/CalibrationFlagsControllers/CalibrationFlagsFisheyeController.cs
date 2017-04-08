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

      public bool RecomputeExtrinsic
      {
        get { return recomputeExtrinsic; }
        set
        {
          recomputeExtrinsic = value;
          UpdateCalibrationFlags();
        }
      }

      public bool CheckCond
      {
        get { return checkCond; }
        set
        {
          checkCond = value;
          UpdateCalibrationFlags();
        }
      }

      public bool FixSkew
      {
        get { return fixSkew; }
        set
        {
          fixSkew = value;
          UpdateCalibrationFlags();
        }
      }

      public bool FixIntrinsic
      {
        get { return fixIntrinsic; }
        set
        {
          fixIntrinsic = value;
          UpdateCalibrationFlags();
        }
      }

      public Cv.Calib3d.Fisheye.Calib CalibrationFlags
      {
        get { return calibrationFlags; }
        set
        {
          calibrationFlags = value;
          UpdateCalibrationOptions();
        }
      }

      public override int CalibrationFlagsValue
      {
        get { return (int)calibrationFlags; }
        set { calibrationFlags = (Cv.Calib3d.Fisheye.Calib)value; }
      }

      protected override int FixKLength { get { return 4; } set { } }

      // Variables

      private Cv.Calib3d.Fisheye.Calib calibrationFlags;

      // Methods

      protected override void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess)  { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.UseIntrinsicGuess; }
        if (FixPrincipalPoint)  { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixPrincipalPoint; }
        if (FixK[0])            { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixK1; }
        if (FixK[1])            { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixK2; }
        if (FixK[2])            { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixK3; }
        if (FixK[3])            { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixK4; }
        if (RecomputeExtrinsic) { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.UseIntrinsicGuess; }
        if (CheckCond)          { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.CheckCond; }
        if (FixSkew)            { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixPrincipalPoint; }
        if (FixIntrinsic)       { calibrationFlags |= Cv.Calib3d.Fisheye.Calib.FixIntrinsic; }
      }

      protected override void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess =  Enum.IsDefined(typeof(Cv.Calib3d.Fisheye.Calib), Cv.Calib3d.Calib.UseIntrinsicGuess);
        FixPrincipalPoint =  Enum.IsDefined(typeof(Cv.Calib3d.Fisheye.Calib), Cv.Calib3d.Calib.FixPrincipalPoint);
        FixK[0] =            Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.FixK1);
        FixK[1] =            Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.FixK2);
        FixK[2] =            Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.FixK3);
        FixK[3] =            Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.FixK4);
        RecomputeExtrinsic = Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.RecomputeExtrinsic);
        CheckCond =          Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.CheckCond);
        FixSkew =            Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.FixSkew);
        FixIntrinsic =       Enum.IsDefined(typeof(Cv.Calib3d.Calib), Cv.Calib3d.Fisheye.Calib.FixIntrinsic);
      }
    }
  }

  /// \} aruco_unity_package
}