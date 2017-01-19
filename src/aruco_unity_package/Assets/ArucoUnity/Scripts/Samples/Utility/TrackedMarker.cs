using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
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

        // MonoBehaviour methods

        protected void Start()
        {
          markerObjectsController.AddTrackedMarker(this);
        }
      }
    }
  }

  /// \} aruco_unity_package
}