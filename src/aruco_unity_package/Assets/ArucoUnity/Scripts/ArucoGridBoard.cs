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

    // Methods

    /// <summary>
    /// <see cref="ArucoBoard.UpdateBoard"/>
    /// </summary>
    protected override void UpdateBoard()
    {
      ImageSize.width = MarkersNumberX * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;
      ImageSize.height = MarkersNumberY * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;

      Board = GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);
    }
  }

  /// \} aruco_unity_package
}