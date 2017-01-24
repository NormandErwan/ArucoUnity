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
      private TrackedObjectsController trackedObjectsController;

      // Properties

      public int MarkerId { get { return markerId; } set { markerId = value; } }

      public TrackedObjectsController TrackedObjectsController { get { return trackedObjectsController; } set { trackedObjectsController = value; } }

      // MonoBehaviour methods

      protected void Start()
      {
        // TODO: update when MarkerId and MarkerObjectsController are changed
        if (TrackedObjectsController.isActiveAndEnabled)
        {
          TrackedObjectsController.AddTrackedMarker(this);
        }
      }
    }
  }

  /// \} aruco_unity_package
}