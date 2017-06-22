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
    /// Manages flags for the calibration process of pinhole cameras.
    /// 
    /// See the OpenCV documentation for more information about these calibration flags:
    /// http://docs.opencv.org/3.2.0/d9/d0c/group__calib3d.html#ga3207604e4b1a1758aa66acb6ed5aa65d
    /// </summary>
    public class CalibrationFlagsPinholeController : CalibrationFlagsController
    {
      // Constants

      const float DefaultFixAspectRatio = 1f;

      // Editor fields

      [SerializeField]
      private bool fixPrincipalPoint = false;

      [SerializeField]
      private bool fixAspectRatio = false;

      [SerializeField]
      private float fixAspectRatioValue = DefaultFixAspectRatio;

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

      [Header("Additional flags for stereo calibration")]
      [SerializeField]
      private bool fixFocalLength = false;

      [SerializeField]
      private bool fixIntrinsic = false;

      [SerializeField]
      private bool sameFocalLength = false;

      [Header("Stereo rectification flags")]
      [SerializeField]
      [Tooltip("If true (default), the principal points of the images have the same pixel coordinates in the rectified views.")]
      private bool zeroDisparity = true;

      [SerializeField]
      [Tooltip("Free scaling parameter (alpha coefficient) between 0 and 1, or -1 (default) for default scaling: 0 to zoom the images so that only"
        + " valid pixels are visible, 1 to shift the images so that no source image pixels are lost.")]
      [Range(-1,1)]
      private double skew = -1;

      // Properties

      public bool FixPrincipalPoint { get { return fixPrincipalPoint; } set { fixPrincipalPoint = value; } }

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

      /// <summary>
      /// If true (default), the principal points of the images have the same pixel coordinates in the rectified views.
      /// </summary>
      public bool ZeroDisparity { get { return zeroDisparity; } set { zeroDisparity = value; } }

      /// <summary>
      /// Free scaling parameter (alpha coefficient) between 0 and 1, or -1 (default) for default scaling: 0 to zoom the images so that only valid
      /// pixels are visible, 1 to shift the images so that no source image pixels are lost.
      /// </summary>
      public double Skew { get { return skew; } set { skew = value; } }

      /// <summary>
      /// The calibration flags enum.
      /// </summary>
      public Cv.Calib CalibrationFlags
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
        set { CalibrationFlags = (Cv.Calib)value; }
      }

      protected override int FixKLength { get { return 6; } }

      // Variables

      private Cv.Calib calibrationFlags;

      // Methods

      protected override void UpdateCalibrationFlags()
      {
        calibrationFlags = 0;
        if (UseIntrinsicGuess)             { calibrationFlags |= Cv.Calib.UseIntrinsicGuess; }
        if (FixPrincipalPoint)             { calibrationFlags |= Cv.Calib.FixPrincipalPoint; }
        if (FixKDistorsionCoefficients[0]) { calibrationFlags |= Cv.Calib.FixK1; }
        if (FixKDistorsionCoefficients[1]) { calibrationFlags |= Cv.Calib.FixK2; }
        if (FixKDistorsionCoefficients[2]) { calibrationFlags |= Cv.Calib.FixK3; }
        if (FixKDistorsionCoefficients[3]) { calibrationFlags |= Cv.Calib.FixK4; }
        if (FixKDistorsionCoefficients[4]) { calibrationFlags |= Cv.Calib.FixK5; }
        if (FixKDistorsionCoefficients[5]) { calibrationFlags |= Cv.Calib.FixK6; }
        if (FixAspectRatio)                { calibrationFlags |= Cv.Calib.FixAspectRatio; }
        if (ZeroTangentialDistorsion)      { calibrationFlags |= Cv.Calib.ZeroTangentDist; }
        if (RationalModel)                 { calibrationFlags |= Cv.Calib.RationalModel; }
        if (ThinPrismModel)                { calibrationFlags |= Cv.Calib.ThinPrismModel; }
        if (FixS1_S2_S3_S4)                { calibrationFlags |= Cv.Calib.FixS1S2S3S4; }
        if (TiltedModel)                   { calibrationFlags |= Cv.Calib.TiltedModel; }
        if (FixTauxTauy)                   { calibrationFlags |= Cv.Calib.FixTauxTauy; }
        if (FixFocalLength)                { calibrationFlags |= Cv.Calib.FixFocalLength; }
        if (FixIntrinsic)                  { calibrationFlags |= Cv.Calib.FixIntrinsic; }
        if (SameFocalLength)               { calibrationFlags |= Cv.Calib.SameFocalLength; }
      }

      protected override void UpdateCalibrationOptions()
      {
        UseIntrinsicGuess =             Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.UseIntrinsicGuess);
        FixPrincipalPoint =             Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixPrincipalPoint);
        FixKDistorsionCoefficients[0] = Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixK1);
        FixKDistorsionCoefficients[1] = Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixK2);
        FixKDistorsionCoefficients[2] = Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixK3);
        FixKDistorsionCoefficients[3] = Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixK4);
        FixKDistorsionCoefficients[4] = Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixK5);
        FixKDistorsionCoefficients[5] = Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixK6);
        ZeroTangentialDistorsion =      Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.ZeroTangentDist);
        RationalModel =                 Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.RationalModel);
        ThinPrismModel =                Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.ThinPrismModel);
        FixS1_S2_S3_S4 =                Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixS1S2S3S4);
        TiltedModel =                   Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.TiltedModel);
        FixTauxTauy =                   Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixTauxTauy);
        FixFocalLength =                Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixFocalLength);
        FixIntrinsic =                  Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.FixIntrinsic);
        SameFocalLength =               Enum.IsDefined(typeof(Cv.Calib), Cv.Calib.SameFocalLength);
      }
    }
  }

  /// \} aruco_unity_package
}