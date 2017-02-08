using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Describes an ArUco marker.
    /// </summary>
    public class ArucoMarker : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The marker id in the used dictionary.")]
      private int id;

      // Properties

      /// <summary>
      /// The marker id in the used dictionary.
      /// </summary>
      public int Id { get { return id; } set { id = value; } }
    }
  }

  /// \} aruco_unity_package
}