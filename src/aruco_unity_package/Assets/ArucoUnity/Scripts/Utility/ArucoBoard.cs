using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Describes an ChArUco diamond marker.
    /// </summary>
    public class ArucoBoard : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The size of the margins in pixels, used by Creators (default: 0).")]
      private int marginsSize;

      // Properties

      /// <summary>
      /// The size of the margins in pixels (default: 0).
      /// </summary>
      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }
    }
  }

  /// \} aruco_unity_package
}