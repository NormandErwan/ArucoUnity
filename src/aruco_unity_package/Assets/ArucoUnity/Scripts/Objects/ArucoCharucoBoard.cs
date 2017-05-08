using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
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
      /// Number of squares in the X direction.
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
      /// Number of squares in the Y direction.
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
      /// Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
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

      public Std.VectorPoint2f DetectedCorners { get; set; }

      public Std.VectorInt DetectedIds { get; set; }

      public int InterpolatedCorners { get; set; }

      public bool ValidTransform { get; set; }

      // ArucoObject methods

      /// <summary>
      /// <see cref="ArucoObject.UpdateHashCode"/>
      /// </summary>
      protected override void UpdateHashCode()
      {
        HashCode = GetArucoHashCode(SquaresNumberX, SquaresNumberY, MarkerSideLength, SquareSideLength);
      }

      // ArucoBoard methods

      /// <summary>
      /// <see cref="ArucoBoard.UpdateBoard"/>
      /// </summary>
      protected override void UpdateBoard()
      {
        ImageSize.Width = SquaresNumberX * (int)SquareSideLength + 2 * MarginsSize;
        ImageSize.Height = SquaresNumberY * (int)SquareSideLength + 2 * MarginsSize;

        AxisLength = 0.5f * (Mathf.Min(SquaresNumberX, SquaresNumberY) * SquareSideLength);

        Board = Aruco.CharucoBoard.Create(SquaresNumberX, SquaresNumberY, SquareSideLength, MarkerSideLength, Dictionary);
      }

      // Methods

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
  
  /// \} aruco_unity_package
}