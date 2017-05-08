using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
  {
    /// <summary>
    /// Describes an ArUco marker.
    /// </summary>
    public class ArucoMarker : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The marker id in the used dictionary.")]
      private int markerId;

      // Properties

      /// <summary>
      /// The marker id in the used dictionary.
      /// </summary>
      public int MarkerId
      {
        get { return markerId; }
        set
        {
          OnPropertyUpdating();
          markerId = value;
          OnPropertyUpdated();
        }
      }

      // ArucoObject methods

      /// <summary>
      /// <see cref="ArucoObject.UpdateHashCode"/>
      /// </summary>
      protected override void UpdateHashCode()
      {
        HashCode = GetArucoHashCode(MarkerId);
      }

      // Methods

      public static int GetArucoHashCode(int markerId)
      {
        int hashCode = 17;
        hashCode = hashCode * 31 + typeof(ArucoMarker).GetHashCode();
        hashCode = hashCode * 31 + markerId;
        return hashCode;
      }
    }
  }

  /// \} aruco_unity_package
}