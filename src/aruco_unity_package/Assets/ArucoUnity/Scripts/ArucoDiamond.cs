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
    public class ArucoDiamond : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The size of the margins in pixels, used by Creators (default: 0).")]
      private int marginsSize;

      [SerializeField]
      [Tooltip("Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.")]
      private int squareSideLength;

      [SerializeField]
      [Tooltip("The four ids of the four markers of the diamond.")]
      private int[] ids;

      // Properties

      /// <summary>
      /// The size of the margins in pixels (default: 0).
      /// </summary>
      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }

      /// <summary>
      /// Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public int SquareSideLength { get { return squareSideLength; } set { squareSideLength = value; } }

      /// <summary>
      /// The four ids of the four markers of the diamond.
      /// </summary>
      public int[] Ids { get { return ids; } set { ids = value; } }

      // MonoBehaviour methods

      protected override void Awake()
      {
        base.Awake();

        if (Ids.Length != 4)
        {
          Debug.LogWarning("ArucoDiamond requires 4 ids.");
        }
      }
    }
  }

  /// \} aruco_unity_package
}