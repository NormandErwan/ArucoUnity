using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
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
      private int markerSeparation;

      // Properties

      /// <summary>
      /// Number of markers in the X direction.
      /// </summary>
      public int MarkersNumberX {
        get { return markersNumberX; }
        set
        {
          PropertyPreUpdate();
          markersNumberX = value;
          PropertyUpdated();
        }
      }

      /// <summary>
      /// Number of markers in the Y direction.
      /// </summary>
      public int MarkersNumberY {
        get { return markersNumberY; }
        set
        {
          PropertyPreUpdate();
          markersNumberY = value;
          PropertyUpdated();
        }
      }

      /// <summary>
      /// Separation between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public int MarkerSeparation {
        get { return markerSeparation; }
        set
        {
          PropertyPreUpdate();
          markerSeparation = value;
          PropertyUpdated();
        }
      }

      public GridBoard Board { get; protected set; }

      // Methods

      protected override void UpdateBoard()
      {
        ImageSize.width = MarkersNumberX * ((int)MarkerSideLength + MarkerSeparation) - MarkerSeparation + 2 * MarginsSize;
        ImageSize.height = MarkersNumberY * ((int)MarkerSideLength + MarkerSeparation) - MarkerSeparation + 2 * MarginsSize;

        Board = GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);
      }
    }
  }

  /// \} aruco_unity_package
}