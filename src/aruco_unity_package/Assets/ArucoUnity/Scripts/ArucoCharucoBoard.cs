using ArucoUnity.Plugin;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  /// <summary>
  /// Describes a ChArUco board.
  /// </summary>
  public class ArucoCharucoBoard : ArucoBoard<CharucoBoard>
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
    public int SquareSideLength
    {
      get { return squareSideLength; }
      set
      {
        OnPropertyUpdating();
        squareSideLength = value;
        OnPropertyUpdated();
      }
    }

    // Methods

    /// <summary>
    /// <see cref="ArucoBoard.UpdateBoard"/>
    /// </summary>
    protected override void UpdateBoard()
    {
      ImageSize.width = SquaresNumberX * SquareSideLength + 2 * MarginsSize;
      ImageSize.height = SquaresNumberY * SquareSideLength + 2 * MarginsSize;

      Board = CharucoBoard.Create(SquaresNumberX, SquaresNumberY, SquareSideLength, MarkerSideLength, Dictionary);
    }
  }

  /// \} aruco_unity_package
}