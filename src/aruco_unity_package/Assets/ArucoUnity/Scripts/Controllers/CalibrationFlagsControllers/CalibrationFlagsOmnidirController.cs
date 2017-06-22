using ArucoUnity.Plugin;
using UnityEngine;
using System;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CalibrationFlagsControllers
  {
    /// <summary>
    /// Manages flags for the calibration process of fisheye or omnidir cameras.
    /// 
    /// See the OpenCV documentation for more information about these calibration flags:
    /// http://docs.opencv.org/3.2.0/dd/d12/tutorial_omnidir_calib_main.html
    /// </summary>
    public class CalibrationFlagsOmnidirController : CalibrationFlagsController
    {
      // Editor fields

      [SerializeField]
      private bool fixSkew = false;

      [SerializeField]
      private bool[] fixP;

      [SerializeField]
      private bool fixXi = false;

      [SerializeField]
      private bool fixGamma = false;

      [SerializeField]
      private bool fixCenter = false;

      // Properties

      public bool FixSkew { get { return fixSkew; } set { fixSkew = value; } }

      public bool[] FixP
      {
        get { return fixP; }
        set
        {
          if (value.Length == FixPLength)
          {
            fixP = value;
            UpdateCalibrationFlags();
          }
        }
      }

      public bool FixXi { get { return fixXi; } set { fixXi = value; } }

      public bool FixGamma { get { return fixGamma; } set { fixGamma = value; } }

      public bool FixCenter { get { return fixCenter; } set { fixCenter = value; } }

      /// <summary>
      /// The calibration flags enum.
      /// </summary>
      public Cv.Omnidir.Calib CalibrationFlags
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
        set { CalibrationFlags = (Cv.Omnidir.Calib)value; }
      }

      protected override int FixKLength { get { return 2; } }

      protected virtual int FixPLength { get { return 2; } set { } }

      // Variables

      private Cv.Omnidir.Calib calibrationFlags;

      // Methods

      protected override void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess)             { calibrationFlags |= Cv.Omnidir.Calib.UseGuess; }
        if (FixSkew)                       { calibrationFlags |= Cv.Omnidir.Calib.FixSkew; }
        if (FixKDistorsionCoefficients[0]) { calibrationFlags |= Cv.Omnidir.Calib.FixK1; }
        if (FixKDistorsionCoefficients[1]) { calibrationFlags |= Cv.Omnidir.Calib.FixK2; }
        if (FixP[0])                       { calibrationFlags |= Cv.Omnidir.Calib.FixP1; }
        if (FixP[1])                       { calibrationFlags |= Cv.Omnidir.Calib.FixP2; }
        if (FixXi)                         { calibrationFlags |= Cv.Omnidir.Calib.FixXi; }
        if (FixGamma)                      { calibrationFlags |= Cv.Omnidir.Calib.FixGamma; }
        if (FixCenter)                     { calibrationFlags |= Cv.Omnidir.Calib.FixCenter; }
      }

      protected override void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess =             Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.UseGuess);
        FixSkew =                       Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixSkew);
        FixKDistorsionCoefficients[0] = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixK1);
        FixKDistorsionCoefficients[1] = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixK2);
        FixP[0] =                       Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixP1);
        FixP[1] =                       Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixP2);
        FixXi =                         Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixXi);
        FixGamma =                      Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixGamma);
        FixCenter =                     Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixCenter);
      }

      protected override void OnValidate()
      {
        base.OnValidate();

        if (fixP.Length != FixPLength)
        {
          Array.Resize(ref fixP, FixPLength);
        }
      }
    }
  }

  /// \} aruco_unity_package
}