using UnityEngine;
using System.Collections.Generic;
using ArucoUnity.Utility.std;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    namespace Utility
    {
      public class MarkerObjectsController : MonoBehaviour
      {
        // Editor fields

        [SerializeField]
        [Tooltip("The default game object to place above the detected markers")]
        private GameObject defaultTrackedGameObject;

        // Properties

        /// <summary>
        /// The default game object to place above the detected markers.
        /// </summary>
        public GameObject DefaultTrackedGameObject { get { return defaultTrackedGameObject; } set { defaultTrackedGameObject = value; } }

        public float MarkerSideLength
        {
          get { return markerSideLength; }
          set
          {
            // Update value
            float oldMarkerSideLength = markerSideLength;
            markerSideLength = value;

            // Resize all the marker objects
            foreach (var markerObject in trackedMarkers)
            {
              UpdateLocalScales(markerSideLength, markerObject.Value.gameObject.transform, oldMarkerSideLength);
            }
            foreach (var markerObject in defaultTrackedMarkerObjects)
            {
              UpdateLocalScales(markerSideLength, markerObject.Value.gameObject.transform, oldMarkerSideLength);
            }
          }
        }

        // Variables

        protected Dictionary<int, TrackedMarker> trackedMarkers;
        protected Dictionary<int, GameObject> defaultTrackedMarkerObjects;
        protected new Camera camera;
        protected CameraParameters cameraParameters;
        private float markerSideLength;

        // MonoBehaviour methods

        /// <summary>
        /// Initialize the variables.
        /// </summary>
        protected void Awake()
        {
          trackedMarkers = new Dictionary<int, TrackedMarker>();
          defaultTrackedMarkerObjects = new Dictionary<int, GameObject>();
        }

        // Methods

        public void SetCamera(Camera camera, CameraParameters cameraParameters)
        {
          this.camera = camera;
          this.cameraParameters = cameraParameters;
        }

        public void AddTrackedMarker(TrackedMarker newTrackedMarker)
        {
          // Return if already added
          foreach(var trackedMarker in trackedMarkers)
          {
            if (trackedMarker.Value == newTrackedMarker)
            {
              return;
            }
          }

          // Add the new tracked marker, rescale to the marker size and hide it
          trackedMarkers.Add(newTrackedMarker.MarkerId, newTrackedMarker);
          UpdateLocalScales(MarkerSideLength, newTrackedMarker.gameObject.transform);
          newTrackedMarker.gameObject.SetActive(false);
        }

        /// <summary>
        /// Hide all the marker objects.
        /// </summary>
        public void DeactivateMarkerObjects()
        {
          foreach (var markerObject in trackedMarkers)
          {
            markerObject.Value.gameObject.SetActive(false);
          }
          foreach (var markerObject in defaultTrackedMarkerObjects)
          {
            markerObject.Value.gameObject.SetActive(false);
          }
        }

        /// <summary>
        /// Place and orient the object to match the marker.
        /// </summary>
        /// <param name="ids">Vector of identifiers of the detected markers.</param>
        /// <param name="rvecs">Vector of rotation vectors of the detected markers.</param>
        /// <param name="tvecs">Vector of translation vectors of the detected markers.</param>
        public void UpdateTransforms(VectorInt ids, VectorVec3d rvecs, VectorVec3d tvecs)
        {
          for (uint i = 0; i < ids.Size(); i++)
          {
            int markerId = ids.At(i);
            GameObject markerObject = null;

            // Try to retrieve the associated tracked marker
            TrackedMarker trackedMarker;
            if (trackedMarkers.TryGetValue(markerId, out trackedMarker))
            {
              markerObject = trackedMarker.gameObject;
            }

            if (markerObject == null)
            {
              // Instantiate the default tracked game object for the current tracked marker
              if (!defaultTrackedMarkerObjects.TryGetValue(markerId, out markerObject))
              {
                // If not found, instantiate it
                markerObject = Instantiate(DefaultTrackedGameObject);
                markerObject.name = markerId.ToString();
                markerObject.transform.SetParent(this.transform);

                UpdateLocalScales(MarkerSideLength, markerObject.transform);

                defaultTrackedMarkerObjects.Add(markerId, markerObject);
              }
            }

            // Place and orient the object to match the marker
            markerObject.transform.rotation = rvecs.At(i).ToRotation();
            markerObject.transform.position = tvecs.At(i).ToPosition();

            // Adjust the object position
            Vector3 imageCenterMarkerObject = new Vector3(0.5f, 0.5f, markerObject.transform.position.z);
            Vector3 opticalCenterMarkerObject = new Vector3(cameraParameters.OpticalCenter.x, cameraParameters.OpticalCenter.y, markerObject.transform.position.z);
            Vector3 opticalShift = camera.ViewportToWorldPoint(opticalCenterMarkerObject) - camera.ViewportToWorldPoint(imageCenterMarkerObject);

            Vector3 positionShift = opticalShift // Take account of the optical center not in the image center
              + markerObject.transform.up * markerObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
            markerObject.transform.localPosition += positionShift;

            print(markerObject.name + " - imageCenter: " + imageCenterMarkerObject.ToString("F3") + "; opticalCenter: " + opticalCenterMarkerObject.ToString("F3")
              + "; positionShift: " + (markerObject.transform.rotation * opticalShift).ToString("F4"));

            markerObject.SetActive(true);
          }
        }

        private void UpdateLocalScales(float markerSideLength, Transform markerObjectTransform, float oldMarkerSideLength = 1f)
        {
          if (oldMarkerSideLength != 0)
          {
            markerObjectTransform.localScale /= oldMarkerSideLength;
          }

          if (markerSideLength != 0)
          {
            markerObjectTransform.localScale *= markerSideLength;
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}
