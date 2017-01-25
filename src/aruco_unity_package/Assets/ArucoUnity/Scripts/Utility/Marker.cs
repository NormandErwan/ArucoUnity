using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public class Marker : ArucoObject
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The marker id")]
      private int id;

      // Properties

      public int Id { get { return id; } set { id = value; } }
    }
  }

  /// \} aruco_unity_package
}