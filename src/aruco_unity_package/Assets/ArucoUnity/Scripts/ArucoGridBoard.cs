using ArucoUnity.Plugin;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  /// <summary>
  /// Describes an ArUco grid board.
  /// </summary>
  public class ArucoGridBoard : ArucoBoard<GridBoard>
  {
    // Editor fields

    [SerializeField]
    [Tooltip("Number of markers in the X direction.")]
    private int markersNumberX;

    [SerializeField]
    [Tooltip("Number of markers in the Y direction.")]
    private int markersNumberY;

    [SerializeField]
    [Tooltip("Separation between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.")]
    private float markerSeparation;

    // Properties

    /// <summary>
    /// Number of markers in the X direction.
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
    /// Number of markers in the Y direction.
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
    /// Separation between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.
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

    public int MarkersUsedForEstimation { get; set; }

    // Methods

    /// <summary>
    /// <see cref="ArucoBoard.UpdateHashCode"/>
    /// </summary>
    protected override void UpdateHashCode()
    {
      hashCode = 17;
      hashCode = hashCode * 31 + typeof(ArucoGridBoard).GetHashCode();
      hashCode = hashCode * 31 + MarkersNumberX;
      hashCode = hashCode * 31 + MarkersNumberY;
      hashCode = hashCode * 31 + Mathf.RoundToInt(MarkerSideLength * 1000); // MarkerSideLength is not less than millimetres
      hashCode = hashCode * 31 + Mathf.RoundToInt(MarkerSeparation * 1000); // MarkerSeparation is not less than millimetres
    }

    /// <summary>
    /// <see cref="ArucoBoard.UpdateBoard"/>
    /// </summary>
    protected override void UpdateBoard()
    {
      ImageSize.width = MarkersNumberX * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;
      ImageSize.height = MarkersNumberY * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;

      AxisLength = 0.5f * (Mathf.Min(MarkersNumberX, MarkersNumberY) * (MarkerSideLength + MarkerSeparation) + markerSeparation);

      Board = GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);
    }
  }

  /// \} aruco_unity_package
}