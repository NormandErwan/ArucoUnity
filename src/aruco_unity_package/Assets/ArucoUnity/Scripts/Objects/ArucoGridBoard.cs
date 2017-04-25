using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
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

      public static int GetArucoHashCode(int markersNumberX, int markersNumberY, float markerSideLength, float markerSeparation)
      {
        int hashCode = 17;
        hashCode = hashCode * 31 + typeof(ArucoGridBoard).GetHashCode();
        hashCode = hashCode * 31 + markersNumberX;
        hashCode = hashCode * 31 + markersNumberY;
        hashCode = hashCode * 31 + Mathf.RoundToInt(markerSideLength * 1000); // MarkerSideLength is not less than millimetres
        hashCode = hashCode * 31 + Mathf.RoundToInt(markerSeparation * 1000); // MarkerSeparation is not less than millimetres
        return hashCode;
      }

      protected static int GetArucoHashCode(ArucoGridBoard arucoGridBoard)
      {
        return GetArucoHashCode(arucoGridBoard.MarkersNumberX, arucoGridBoard.MarkersNumberY, arucoGridBoard.MarkerSideLength,
          arucoGridBoard.MarkerSeparation);
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
        ImageSize.Width = MarkersNumberX * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;
        ImageSize.Height = MarkersNumberY * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;

        AxisLength = 0.5f * (Mathf.Min(MarkersNumberX, MarkersNumberY) * (MarkerSideLength + MarkerSeparation) + markerSeparation);

        Board = Aruco.GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);
      }
    }
  }

  /// \} aruco_unity_package
}