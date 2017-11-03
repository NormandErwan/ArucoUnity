using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
  {
    /// <summary>
    /// Describes the shared properties of the ArUco boards.
    /// </summary>
    public abstract class ArucoBoard : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The size of the margins in pixels, used by Creators (default: 0).")]
      private int marginsSize;

      // Properties

      /// <summary>
      /// Gets or sets the size of the margins in pixels (default: 0). Used by the Creators.
      /// </summary>
      public int MarginsSize
      {
        get { return marginsSize; }
        set
        {
          OnPropertyUpdating();
          marginsSize = value;
          OnPropertyUpdated();
        }
      }

      /// <summary>
      /// Gets or sets the image size for drawing the board.
      /// </summary>
      public Cv.Size ImageSize { get; protected set; }

      /// <summary>
      /// Gets or sets the associated board from the ArucoUnity plugin library.
      /// </summary>
      public Aruco.Board Board { get; protected set; }

      /// <summary>
      /// Gets or sets the length of the axis lines when drawn on the board.
      /// </summary>
      public float AxisLength { get; protected set; }

      /// <summary>
      /// Gets or sets the estimated rotation vector of the board when tracked.
      /// </summary>
      public Cv.Vec3d Rvec { get; set; }

      /// <summary>
      /// Gets or sets the estimated translation vector of the board when tracked.
      /// </summary>
      public Cv.Vec3d Tvec { get; set; }

      // Variables

      protected Vector2Int imageSize = Vector2Int.one;

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();
        ImageSize = new Cv.Size();
        UpdateBoard();
      }

      // ArucoObject methods

      /// <summary>
      /// Calls the <see cref="UpdateBoard"/> method and calls <see cref="ArucoObject.OnPropertyUpdated"/>.
      /// </summary>
      protected override void OnPropertyUpdated()
      {
        UpdateBoard();
        base.OnPropertyUpdated();
      }

      /// <summary>
      /// Updates the <see cref="Board"/> property.
      /// </summary>
      protected virtual void UpdateBoard()
      {
        ImageSize.Width = imageSize.x;
        ImageSize.Height = imageSize.y;
      }
    }
  }

  /// \} aruco_unity_package
}