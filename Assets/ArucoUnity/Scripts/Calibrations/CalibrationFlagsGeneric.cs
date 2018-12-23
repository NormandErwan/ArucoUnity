using System;

namespace ArucoUnity.Calibration
{
    /// <summary>
    /// Manages the flags of the <see cref="ArucoCameraCalibration"/> process. Generic class to use in scripts.
    /// </summary>
    public abstract class CalibrationFlagsGeneric<T> : CalibrationFlags
        where T : struct, IConvertible, IComparable, IFormattable
    {
        // CameraCalibrationFlags properties

        public override int Value
        {
            get { return Convert.ToInt32(Enum.Parse(typeof(T), Flags.ToString()) as Enum); }
            set { Flags = (T)Enum.ToObject(typeof(T), value); }
        }

        // Properties

        /// <summary>
        /// Gets or sets the calibration flags enum and keeps updated the flag properties.
        /// </summary>
        public T Flags
        {
            get
            {
                UpdateCalibrationFlags();
                return flags;
            }
            set
            {
                flags = value;
                UpdateCalibrationOptions();
            }
        }

        // Variables

        protected T flags;
    }
}