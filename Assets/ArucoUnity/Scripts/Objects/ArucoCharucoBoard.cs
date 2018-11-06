using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity.Objects
{
    /// <summary>
    /// Describes a ChArUco board.
    /// </summary>
    public class ArucoCharucoBoard : ArucoBoard
    {
        // Editor fields

        [SerializeField]
        [Tooltip("Number of squares in the X direction.")]
        private int squaresNumberX;

        [SerializeField]
        [Tooltip("Number of squares in the Y direction.")]
        private int squaresNumberY;

        [SerializeField]
        [Tooltip("Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.")]
        private float squareSideLength;

        // Properties

        /// <summary>
        /// Gets or sets the number of squares in the X direction.
        /// </summary>
        public int SquaresNumberX
        {
            get { return squaresNumberX; }
            set
            {
                OnPropertyUpdating();
                squaresNumberX = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the number of squares in the Y direction.
        /// </summary>
        public int SquaresNumberY
        {
            get { return squaresNumberY; }
            set
            {
                OnPropertyUpdating();
                squaresNumberY = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
        /// </summary>
        public float SquareSideLength
        {
            get { return squareSideLength; }
            set
            {
                OnPropertyUpdating();
                squareSideLength = value;
                OnPropertyUpdated();
            }
        }

        /// <summary>
        /// Gets or sets the list of the detected marker by the tracker the last frame.
        /// </summary>
        public Std.VectorPoint2f DetectedCorners { get; internal set; }

        /// <summary>
        /// Gets or sets the list of the ids of the detected marker by the tracker the last frame.
        /// </summary>
        public Std.VectorInt DetectedIds { get; internal set; }

        /// <summary>
        /// Gets or sets if the transform of the board has been correctly estimated by the tracker the last frame.
        /// </summary>
        public bool ValidTransform { get; internal set; }

        // ArucoObject methods

        public override Vector3 GetGameObjectScale()
        {
            ImageSize = new Vector2(
                x: SquaresNumberX * SquareSideLength + 2 * MarginsLength,
                y: SquaresNumberY * SquareSideLength + 2 * MarginsLength
            );
            return new Vector3(ImageSize.x, SquareSideLength, ImageSize.y);
        }

        protected override void UpdateArucoHashCode()
        {
            ArucoHashCode = GetArucoHashCode(SquaresNumberX, SquaresNumberY, MarkerSideLength, SquareSideLength);
        }

        // ArucoBoard methods

        public override Cv.Mat Draw()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (SquaresNumberX <= 1 || SquaresNumberY <= 1 || SquareSideLength <= 0
                || MarkerSideLength <= 0 || SquareSideLength <= MarkerSideLength || MarkerBorderBits <= 0 || Dictionary == null))
            {
                return null;
            }
#endif
            int squareSideLength = GetInPixels(SquareSideLength);
            int markerSideLength = GetInPixels(MarkerSideLength);
            Aruco.CharucoBoard board = Aruco.CharucoBoard.Create(SquaresNumberX, SquaresNumberY, squareSideLength, markerSideLength, Dictionary);

            Cv.Size imageSize = new Cv.Size();
            imageSize.Width = GetInPixels(SquaresNumberX * squareSideLength + 2 * MarginsLength);
            imageSize.Height = GetInPixels(SquaresNumberY * squareSideLength + 2 * MarginsLength);

            Cv.Mat image;
            board.Draw(imageSize, out image, MarginsLength, (int)MarkerBorderBits);

            return image;
        }

        public override string GenerateName()
        {
            return "ArUcoUnity_ChArUcoBoard_" + Dictionary.Name + "_X_" + SquaresNumberX + "_Y_" + SquaresNumberY + "_SquareSize_" + SquareSideLength
                    + "_MarkerSize_" + MarkerSideLength;
        }

        protected override void UpdateBoard()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (SquaresNumberX <= 1 || SquaresNumberY <= 1 || SquareSideLength <= 0
                || MarkerSideLength <= 0 || SquareSideLength <= MarkerSideLength))
            {
                return;
            }
#endif

            AxisLength = 0.5f * (Mathf.Min(SquaresNumberX, SquaresNumberY) * SquareSideLength);
            Board = Aruco.CharucoBoard.Create(SquaresNumberX, SquaresNumberY, SquareSideLength, MarkerSideLength, Dictionary);
        }

        // Methods

        /// <summary>
        /// Computes the hash code of a ChAruco board.
        /// </summary>
        /// <param name="squaresNumberX">The number of squares in the X direction.</param>
        /// <param name="squaresNumberY">The number of squares in the Y direction.</param>
        /// <param name="markerSideLength">The side length of each marker.</param>
        /// <param name="squareSideLength">The side length of each square.</param>
        /// <returns>The calculated ArUco hash code.</returns>
        public static int GetArucoHashCode(int squaresNumberX, int squaresNumberY, float markerSideLength, float squareSideLength)
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + typeof(ArucoCharucoBoard).GetHashCode();
            hashCode = hashCode * 31 + squaresNumberX;
            hashCode = hashCode * 31 + squaresNumberY;
            hashCode = hashCode * 31 + Mathf.RoundToInt(markerSideLength * 1000); // MarkerSideLength is not less than millimetres
            hashCode = hashCode * 31 + Mathf.RoundToInt(markerSideLength * 1000); // SquareSideLength is not less than millimetres
            return hashCode;
        }
    }
}