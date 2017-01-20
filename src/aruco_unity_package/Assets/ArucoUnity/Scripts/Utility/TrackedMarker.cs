using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public class TrackedMarker : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The associated marker id to track")]
      private int markerId;

      [SerializeField]
      [Tooltip("")]
      private MarkerObjectsController markerObjectsController;

      // Properties

      public int MarkerId { get { return markerId; } set { markerId = value; } }

      public MarkerObjectsController MarkerObjectsController { get { return markerObjectsController; } set { markerObjectsController = value; } }

      // MonoBehaviour methods

      protected void Start()
      {
        // TODO: update when MarkerId and MarkerObjectsController are changed
        if (MarkerObjectsController.isActiveAndEnabled)
        {
          MarkerObjectsController.AddTrackedMarker(this);
        }
      }
    }
  }

  /// \} aruco_unity_package
}