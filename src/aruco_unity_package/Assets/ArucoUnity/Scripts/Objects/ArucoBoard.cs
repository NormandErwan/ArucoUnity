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
      /// <see cref="ArucoObject.HashCode"/>.
      /// </summary>
      public override int HashCode { get { return hashCode; } }

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
      public Cv.Core.Size ImageSize { get; protected set; }

      /// <summary>
      /// The associated board from the ArucoUnity plugin library.
      /// </summary>
      public Aruco.Board Board { get; protected set; }

      public float AxisLength { get; protected set; }

      public Cv.Core.Vec3d Rvec { get; set; }

      public Cv.Core.Vec3d Tvec { get; set; }

      // Variables

      protected int hashCode;

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties and suscribe to <see cref="ArucoObject.PropertyUpdated"/>.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();

        ImageSize = new Cv.Core.Size();
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
        UpdateHashCode();
        base.OnPropertyUpdated();
      }

      // Methods

      protected abstract void UpdateHashCode();

      /// <summary>
      /// Update the <see cref="Board"/> property.
      /// </summary>
      protected abstract void UpdateBoard();
    }
  }

  /// \} aruco_unity_package
}