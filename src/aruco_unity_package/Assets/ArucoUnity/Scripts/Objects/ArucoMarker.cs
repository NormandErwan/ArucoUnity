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
      /// Gets or sets the marker id in the used dictionary.
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

      protected override void AdjustGameObjectScale()
      {
        transform.localScale = MarkerSideLength * Vector3.one;
      }

      protected override void UpdateArucoHashCode()
      {
        ArucoHashCode = GetArucoHashCode(MarkerId);
      }

      // Methods

      /// <summary>
      /// Computes the hash code of a marker based on its id.
      /// </summary>
      /// <param name="markerId">The marker id.</param>
      /// <returns>The calculated ArUco hash code.</returns>
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