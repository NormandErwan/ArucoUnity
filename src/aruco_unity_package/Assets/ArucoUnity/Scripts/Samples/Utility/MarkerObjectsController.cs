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
        [Tooltip("The default object to place above the detected markers")]
        private GameObject defaultMarkerObject;

        // Properties

        /// <summary>
        /// The object to place above the detected markers.
        /// </summary>
        public GameObject DefaultMarkerObject { get { return defaultMarkerObject; } set { defaultMarkerObject = value; } }

        // Variables

        protected Dictionary<int, GameObject> activeMarkerObjects;
        protected new Camera camera;
        protected CameraParameters cameraParameters;

        // Methods

        public void SetCamera(Camera camera, CameraParameters cameraParameters)
        {
          this.camera = camera;
          this.cameraParameters = cameraParameters;
        }

        /// <summary>
        /// Hide all the marker objects.
        /// </summary>
        public void DeactivateMarkerObjects()
        {
          if (activeMarkerObjects != null)
          {
            foreach (var markerObject in activeMarkerObjects)
            {
              markerObject.Value.SetActive(false);
            }
          }
        }

        /// <summary>
        /// Place and orient the object to match the marker.
        /// </summary>
        /// <param name="ids">Vector of identifiers of the detected markers.</param>
        /// <param name="rvecs">Vector of rotation vectors of the detected markers.</param>
        /// <param name="tvecs">Vector of translation vectors of the detected markers.</param>
        public void UpdateTransforms(VectorInt ids, float markerSideLength, VectorVec3d rvecs, VectorVec3d tvecs)
        {
          // Create the marker objects table
          if (activeMarkerObjects == null)
          {
            activeMarkerObjects = new Dictionary<int, GameObject>();
          }

          // Show the active marker objects
          for (uint i = 0; i < ids.Size(); i++)
          {
            // Retrieve the associated object for this marker or create it
            GameObject markerObject;
            if (!activeMarkerObjects.TryGetValue(ids.At(i), out markerObject))
            {
              markerObject = Instantiate(DefaultMarkerObject);
              markerObject.name = ids.At(i).ToString();
              markerObject.transform.SetParent(this.transform);

              markerObject.transform.localScale = markerObject.transform.localScale * markerSideLength; // Rescale to the marker size

              activeMarkerObjects.Add(ids.At(i), markerObject);
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
      }
    }
  }

  /// \} aruco_unity_package
}
