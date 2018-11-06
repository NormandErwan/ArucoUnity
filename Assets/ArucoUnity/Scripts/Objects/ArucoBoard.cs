using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity.Objects
{
    /// <summary>
    /// Describes the shared properties of the ArUco boards.
    /// </summary>
    public abstract class ArucoBoard : ArucoObject
    {
        // Editor fields

        [SerializeField]
        [Tooltip("The length of the margins around the board in pixels, used by Creators (default: 0).")]
        private int marginsLength;

        // Properties

        /// <summary>
        /// Gets or sets the length of the margins around the board in pixels, used by the Creators (default: 0).
        /// </summary>
        public int MarginsLength
        {
            get { return marginsLength; }
            set
            {
                OnPropertyUpdating();
                marginsLength = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the image size for drawing the board.
        /// </summary>
        public Vector2 ImageSize { get; protected set; }

        /// <summary>
        /// Gets or sets the associated board from the ArucoUnity plugin library.
        /// </summary>
        public Aruco.Board Board { get; protected set; }

        /// <summary>
        /// Gets or sets the length of the axis lines when drawn on the board.
        /// </summary>
        public float AxisLength { get; protected set; }

        /// <summary>
        /// Gets or sets the estimated rotation vector of the board when tracked.
        /// </summary>
        public Cv.Vec3d Rvec { get; set; }

        /// <summary>
        /// Gets or sets the estimated translation vector of the board when tracked.
        /// </summary>
        public Cv.Vec3d Tvec { get; set; }

        // MonoBehaviour methods

        /// <summary>
        /// Calls <see cref="UpdateBoard"/>.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            UpdateBoard();
        }

        // ArucoObject methods

        /// <summary>
        /// Calls <see cref="ArucoObject.OnPropertyUpdated"/> and calls <see cref="UpdateBoard"/>.
        /// </summary>
        protected override void UpdateProperties()
        {
            base.UpdateProperties();
            UpdateBoard();
        }

        /// <summary>
        /// Updates the <see cref="Board"/> properties.
        /// </summary>
        protected abstract void UpdateBoard();
    }
}