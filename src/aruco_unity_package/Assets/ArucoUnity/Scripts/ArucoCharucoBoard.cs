using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Describes a ChArUco board.
    /// </summary>
    public class ArucoCharucoBoard : ArucoObject
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
      private int squareSideLength;

      // Properties

      /// <summary>
      /// Number of squares in the X direction.
      /// </summary>
      public int SquaresNumberX { get { return squaresNumberX; } set { squaresNumberX = value; } }

      /// <summary>
      /// Number of squares in the Y direction.
      /// </summary>
      public int SquaresNumberY { get { return squaresNumberY; } set { squaresNumberY = value; } }

      /// <summary>
      /// Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public int SquareSideLength { get { return squareSideLength; } set { squareSideLength = value; } }
    }
  }

  /// \} aruco_unity_package
}