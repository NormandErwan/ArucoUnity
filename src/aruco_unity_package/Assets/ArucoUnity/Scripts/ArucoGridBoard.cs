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
    public class ArucoGridBoard : ArucoObject
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
      public int MarkersNumberX { get { return markersNumberX; } set { markersNumberX = value; } }

      /// <summary>
      /// Number of markers in the Y direction.
      /// </summary>
      public int MarkersNumberY { get { return markersNumberY; } set { markersNumberY = value; } }

      /// <summary>
      /// Separation between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public int MarkerSeparation { get { return markerSeparation; } set { markerSeparation = value; } }
    }
  }

  /// \} aruco_unity_package
}