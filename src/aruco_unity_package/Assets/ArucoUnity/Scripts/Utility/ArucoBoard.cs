using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
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
      /// The size of the margins in pixels (default: 0).
      /// </summary>
      public int MarginsSize {
        get { return marginsSize; }
        set
        {
          PropertyPreUpdate();
          marginsSize = value;
          PropertyUpdated();
        }
      }

      public Plugin.cv.Size ImageSize { get; protected set; }

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();

        ImageSize = new Plugin.cv.Size();
        UpdateBoard();
      }

      // ArucoObject methods

      /// <summary>
      /// <see cref="ArucoObject.PropertyUpdated"/>
      /// </summary>
      protected override void PropertyUpdated()
      {
        base.PropertyUpdated();

        UpdateBoard();
      }

      protected abstract void UpdateBoard();
    }
  }

  /// \} aruco_unity_package
}