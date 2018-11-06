using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity.Objects
{
    /// <summary>
    /// Describes an ArUco grid board.
    /// </summary>
    public class ArucoGridBoard : ArucoBoard
    {
        // Editor fields

        [SerializeField]
        [Tooltip("Number of markers in the X direction.")]
        private int markersNumberX;

        [SerializeField]
        [Tooltip("Number of markers in the Y direction.")]
        private int markersNumberY;

        [SerializeField]
        [Tooltip("Separation length between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.")]
        private float markerSeparation;

        // Properties

        /// <summary>
        /// Gets or sets the number of markers in the X direction.
        /// </summary>
        public int MarkersNumberX
        {
            get { return markersNumberX; }
            set
            {
                OnPropertyUpdating();
                markersNumberX = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the number of markers in the Y direction.
        /// </summary>
        public int MarkersNumberY
        {
            get { return markersNumberY; }
            set
            {
                OnPropertyUpdating();
                markersNumberY = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the separation between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.
        /// </summary>
        public float MarkerSeparation
        {
            get { return markerSeparation; }
            set
            {
                OnPropertyUpdating();
                markerSeparation = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the number of markers employed by the tracker the last frame for the estimation of the transform of the board.
        /// </summary>
        public int MarkersUsedForEstimation { get; internal set; }

        // ArucoObject methods

        public override Vector3 GetGameObjectScale()
        {
            ImageSize = new Vector2(
                x: MarkersNumberX * (MarkerSideLength + MarkerSeparation) - MarkerSeparation + 2 * MarginsLength,
                y: MarkersNumberY * (MarkerSideLength + MarkerSeparation) - MarkerSeparation + 2 * MarginsLength
            );
            return new Vector3(ImageSize.x, MarkerSideLength, ImageSize.y);
        }

        protected override void UpdateArucoHashCode()
        {
            ArucoHashCode = GetArucoHashCode(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation);
        }

        // ArucoBoard methods

        public override Cv.Mat Draw()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (MarkersNumberX <= 0 || MarkersNumberY <= 0 || MarkerSideLength <= 0
                || MarkerSeparation <= 0 || MarkerBorderBits <= 0 || Dictionary == null))
            {
                return null;
            }
#endif
            int markerSideLength = GetInPixels(MarkerSideLength);
            int markerSeparation = GetInPixels(MarkerSeparation);
            Aruco.GridBoard board = Aruco.GridBoard.Create(MarkersNumberX, MarkersNumberY, markerSideLength, markerSeparation, Dictionary);

            Cv.Size imageSize = new Cv.Size();
            imageSize.Width = GetInPixels(MarkersNumberX * (markerSideLength + markerSeparation) - markerSeparation + 2 * MarginsLength);
            imageSize.Height = GetInPixels(MarkersNumberY * (markerSideLength + markerSeparation) - markerSeparation + 2 * MarginsLength);

            Cv.Mat image;
            board.Draw(imageSize, out image, MarginsLength, (int)MarkerBorderBits);

            return image;
        }

        public override string GenerateName()
        {
            return "ArUcoUnity_GridBoard_" + Dictionary.Name + "_X_" + MarkersNumberX + "_Y_" + MarkersNumberY + "_MarkerSize_" + MarkerSideLength;
        }

        protected override void UpdateBoard()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (MarkersNumberX <= 0 || MarkersNumberY <= 0 || MarkerSideLength <= 0
                || MarkerSeparation <= 0))
            {
                return;
            }
#endif

            AxisLength = 0.5f * (Mathf.Min(MarkersNumberX, MarkersNumberY) * (MarkerSideLength + MarkerSeparation) + MarkerSeparation);
            Board = Aruco.GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);
        }

        // Methods

        /// <summary>
        /// Computes the hash code of a grid board.
        /// </summary>
        /// <param name="markersNumberX">The number of markers in the X direction.</param>
        /// <param name="markersNumberY">The number of markers in the Y direction.</param>
        /// <param name="markerSideLength">The side length of each marker.</param>
        /// <param name="markerSeparation">The separation between two consecutive markers in the grid.</param>
        /// <returns>The calculated ArUco hash code.</returns>
        public static int GetArucoHashCode(int markersNumberX, int markersNumberY, float markerSideLength, float markerSeparation)
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + typeof(ArucoGridBoard).GetHashCode();
            hashCode = hashCode * 31 + markersNumberX;
            hashCode = hashCode * 31 + markersNumberY;
            hashCode = hashCode * 31 + Mathf.RoundToInt(markerSideLength * 1000); // MarkerSideLength is not less than millimeters
            hashCode = hashCode * 31 + Mathf.RoundToInt(markerSeparation * 1000); // MarkerSeparation is not less than millimeters
            return hashCode;
        }
    }
}