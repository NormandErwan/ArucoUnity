using ArucoUnity.Plugin;
using UnityEngine;
using System;

namespace ArucoUnity.Calibration.Omnidir
{
    /// <summary>
    /// Manages the flags of the omnidir calibration process.
    /// </summary>
    public class OmnidirCameraCalibrationFlags : CalibrationFlagsGeneric<Cv.Omnidir.Calib>
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

        protected override int FixKLength { get { return 2; } }

        protected int FixPLength { get { return 2; } }

        // Methods

        protected override void UpdateCalibrationFlags()
        {
            flags = 0;
            if (UseIntrinsicGuess) { flags |= Cv.Omnidir.Calib.UseGuess; }
            if (FixSkew) { flags |= Cv.Omnidir.Calib.FixSkew; }
            if (FixKDistorsionCoefficients[0]) { flags |= Cv.Omnidir.Calib.FixK1; }
            if (FixKDistorsionCoefficients[1]) { flags |= Cv.Omnidir.Calib.FixK2; }
            if (FixP[0]) { flags |= Cv.Omnidir.Calib.FixP1; }
            if (FixP[1]) { flags |= Cv.Omnidir.Calib.FixP2; }
            if (FixXi) { flags |= Cv.Omnidir.Calib.FixXi; }
            if (FixGamma) { flags |= Cv.Omnidir.Calib.FixGamma; }
            if (FixCenter) { flags |= Cv.Omnidir.Calib.FixCenter; }
        }

        protected override void UpdateCalibrationOptions()
        {
            UseIntrinsicGuess = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.UseGuess);
            FixSkew = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixSkew);
            FixKDistorsionCoefficients[0] = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixK1);
            FixKDistorsionCoefficients[1] = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixK2);
            FixP[0] = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixP1);
            FixP[1] = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixP2);
            FixXi = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixXi);
            FixGamma = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixGamma);
            FixCenter = Enum.IsDefined(typeof(Cv.Omnidir.Calib), Cv.Omnidir.Calib.FixCenter);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (fixP != null && fixP.Length != FixPLength)
            {
                Array.Resize(ref fixP, FixPLength);
            }
        }
    }
}