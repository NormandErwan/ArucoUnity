using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
  {
    /// <summary>
    /// Describes an ChArUco diamond marker.
    /// </summary>
    public class ArucoDiamond : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.")]
      private float squareSideLength;

      [SerializeField]
      [Tooltip("The four ids of the four markers of the diamond.")]
      private int[] ids;

      // Properties

      /// <summary>
      /// Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public float SquareSideLength
      {
        get { return squareSideLength; }
        set
        {
          OnPropertyUpdating();
          squareSideLength = value;
          OnPropertyUpdated();
        }
      }

      /// <summary>
      /// The four ids of the four markers of the diamond.
      /// </summary>
      public int[] Ids
      {
        get
        {
          if (ids.Length != 4)
          {
            Debug.LogError("Invalid number of Ids: ArucoDiamond requires 4 ids.");
          }

          return ids;
        }
        set
        {
          if (value.Length != 4)
          {
            Debug.LogError("Invalid number of Ids: ArucoDiamond requires 4 ids.");
            return;
          }

          OnPropertyUpdating();
          ids = value;
          OnPropertyUpdated();
        }
      }

      // ArucoObject methods

      /// <summary>
      /// <see cref="ArucoObject.UpdateHashCode"/>
      /// </summary>
      protected override void UpdateHashCode()
      {
        HashCode = GetArucoHashCode(Ids);
      }

      // Methods

      public static int GetArucoHashCode(int[] ids)
      {
        int hashCode = 17;
        hashCode = hashCode * 31 + typeof(ArucoDiamond).GetHashCode();
        foreach (var id in ids)
        {
          hashCode = hashCode * 31 + id;
        }
        return hashCode;
      }
    }
  }

  /// \} aruco_unity_package
}