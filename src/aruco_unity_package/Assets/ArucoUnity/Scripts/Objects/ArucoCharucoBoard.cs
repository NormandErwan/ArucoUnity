using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

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

    // MonoBehaviour methods

    protected override void Awake()
    {
      base.Awake();

      DetectedCorners = null;
      DetectedIds = null;
    }

    // Methods

    public static int GetArucoHashCode(int squaresNumberX, int squaresNumberY, float markerSideLength, float squareSideLength)
    {
      int hashCode = 17;
      hashCode = hashCode * 31 + typeof(ArucoCharucoBoard).GetHashCode();
      hashCode = hashCode * 31 + squaresNumberX;
      hashCode = hashCode * 31 + squaresNumberY;
      hashCode = hashCode * 31 + Mathf.RoundToInt(markerSideLength * 1000); // MarkerSideLength is not less than millimetres
      hashCode = hashCode * 31 + Mathf.RoundToInt(squareSideLength * 1000); // SquareSideLength is not less than millimetres
      return hashCode;
    }

    protected static int GetArucoHashCode(ArucoCharucoBoard arucoCharucoBoard)
    {
      return GetArucoHashCode(arucoCharucoBoard.SquaresNumberX, arucoCharucoBoard.SquaresNumberY, arucoCharucoBoard.MarkerSideLength, arucoCharucoBoard.SquareSideLength);
    }

    /// <summary>
    /// <see cref="ArucoBoard.UpdateHashCode"/>
    /// </summary>
    protected override void UpdateHashCode()
    {
      hashCode = GetArucoHashCode(this);
    }

    /// <summary>
    /// <see cref="ArucoBoard.UpdateBoard"/>
    /// </summary>
    protected override void UpdateBoard()
    {
      ImageSize.width = SquaresNumberX * (int)SquareSideLength + 2 * MarginsSize;
      ImageSize.height = SquaresNumberY * (int)SquareSideLength + 2 * MarginsSize;

      AxisLength = 0.5f * (Mathf.Min(SquaresNumberX, SquaresNumberY) * SquareSideLength);

      Board = Aruco.CharucoBoard.Create(SquaresNumberX, SquaresNumberY, SquareSideLength, MarkerSideLength, Dictionary);
    }
  }

  /// \} aruco_unity_package
}