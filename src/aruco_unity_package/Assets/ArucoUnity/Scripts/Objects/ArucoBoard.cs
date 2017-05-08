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
      /// The size of the margins in pixels (default: 0). Used by the Creators.
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
      /// The image size for drawing the board.
      /// </summary>
      public Cv.Size ImageSize { get; protected set; }

      /// <summary>
      /// The associated board from the ArucoUnity plugin library.
      /// </summary>
      public Aruco.Board Board { get; protected set; }

      public float AxisLength { get; protected set; }

      public Cv.Vec3d Rvec { get; set; }

      public Cv.Vec3d Tvec { get; set; }

      // MonoBehaviour methods

      /// <summary>
      /// <see cref="ArucoObject.Awake()"/>
      /// </summary>
      protected override void Awake()
      {
        base.Awake();
        ImageSize = new Cv.Size();
        UpdateBoard();
        UpdateHashCode();
      }

      // ArucoObject methods

      /// <summary>
      /// <see cref="ArucoObject.OnPropertyUpdated"/>.
      /// </summary>
      protected override void OnPropertyUpdated()
      {
        UpdateBoard();
        base.OnPropertyUpdated();
      }

      /// <summary>
      /// Update the <see cref="Board"/> property.
      /// </summary>
      protected abstract void UpdateBoard();
    }
  }

  /// \} aruco_unity_package
}