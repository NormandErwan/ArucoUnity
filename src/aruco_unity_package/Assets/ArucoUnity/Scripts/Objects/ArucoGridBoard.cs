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
      /// Gets or sets the number of markers in the X direction.
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
      /// Gets or sets the number of markers in the Y direction.
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
      /// Gets or sets the separation between two consecutive markers in the grid. In pixels for Creators. In meters for Trackers and Calibrators.
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

      /// <summary>
      /// Gets or sets the number of markers employed by the tracker the last frame for the estimation of the transform of the board.
      /// </summary>
      public int MarkersUsedForEstimation { get; internal set; }

      // ArucoObject methods

      protected override void AdjustGameObjectScale()
      {
        imageSize.x = MarkersNumberX * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;
        imageSize.y = MarkersNumberY * (int)(MarkerSideLength + MarkerSeparation) - (int)MarkerSeparation + 2 * MarginsSize;
        transform.localScale = new Vector3(imageSize.x, imageSize.y, 1);
      }

      protected override void UpdateArucoHashCode()
      {
        ArucoHashCode = GetArucoHashCode(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation);
      }

      // ArucoBoard methods

      protected override void UpdateBoard()
      {
        base.UpdateBoard();
        AxisLength = 0.5f * (Mathf.Min(MarkersNumberX, MarkersNumberY) * (MarkerSideLength + MarkerSeparation) + markerSeparation);
        //Board = Aruco.GridBoard.Create(MarkersNumberX, MarkersNumberY, MarkerSideLength, MarkerSeparation, Dictionary);
      }

      // Methods

      /// <summary>
      /// Computes the hash code of a grid board.
      /// </summary>
      /// <param name="markersNumberX">The number of markers in the X direction.</param>
      /// <param name="markersNumberY">The number of markers in the Y direction.</param>
      /// <param name="markerSideLength">The side length of each marker.</param>
      /// <param name="markerSeparation">The separation between two consecutive markers in the grid.</param>
      /// <returns>The calculated ArUco hash code.</returns>
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
    }
  }

  /// \} aruco_unity_package
}