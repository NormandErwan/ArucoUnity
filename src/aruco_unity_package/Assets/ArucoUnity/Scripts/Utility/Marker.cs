using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public class Marker : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The marker id")]
      private int id;

      [SerializeField]
      [Tooltip("")]
      private ArucoObjectController arucoObjectController;

      // Properties

      public int Id { get { return id; } set { id = value; } }

      public ArucoObjectController ArucoObjectController { get { return arucoObjectController; } set { arucoObjectController = value; } }

      // MonoBehaviour methods

      protected void Start()
      {
        // TODO: update when Id and ArucoObjectController are changed
        if (ArucoObjectController.isActiveAndEnabled)
        {
          ArucoObjectController.Add(this);
        }
      }
    }
  }

  /// \} aruco_unity_package
}