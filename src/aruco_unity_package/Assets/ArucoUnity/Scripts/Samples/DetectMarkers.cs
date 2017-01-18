using System.Collections.Generic;
using UnityEngine;
using ArucoUnity.Utility.cv;
using ArucoUnity.Utility.std;
using ArucoUnity.Samples.Utility;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    /// <summary>
    /// Detect markers, display results and place game objects on the detected markers transform.
    /// </summary>
    public class DetectMarkers : CameraDeviceMarkersDetector
    {
      // Editor fields

      [Header("Detection configuration")]
      [SerializeField]
      [Tooltip("The dictionary to use for the marker detection")]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("The parameters to use for the marker detection")]
      private DetectorParametersManager detectorParametersManager;

      [SerializeField]
      [Tooltip("The side length of the markers that will be detected (in meters). This also is the scale factor of the DetectedMarkersObject")]
      private float markerSideLength = 0.1f;

      [SerializeField]
      [Tooltip("Display the detected markers in the CameraImageTexture")]
      private bool showDetectedMarkers = true;

      [SerializeField]
      [Tooltip("Display the rejected markers candidates.")]
      private bool showRejectedCandidates = false;

      [Header("Camera configuration")]
      [SerializeField]
      [Tooltip("The parameters to use for the marker detection")]
      private CameraDeviceController cameraDeviceController;

      [SerializeField]
      [Tooltip("If estimatePose is false, the CameraImageTexture will be displayed on this canvas")]
      private CameraDeviceCanvasDisplay cameraDeviceCanvasDisplay;

      [SerializeField]
      [Tooltip("The Unity camera that will capture the CameraPlane display")]
      private new Camera camera;

      [SerializeField]
      [Tooltip("The plane facing the camera that display the CameraImageTexture")]
      private GameObject cameraPlane;

      [Header("Estimation configuration")]
      [SerializeField]
      [Tooltip("Estimate the detecte markers pose (position, rotation)")]
      private bool estimatePose;

      [SerializeField]
      [Tooltip("The file path to load the camera parameters.")]
      private string cameraParametersFilePath = "Assets/ArucoUnity/aruco-calibration.xml";

      [SerializeField]
      [Tooltip("The object to place above the detected markers")]
      private GameObject detectedMarkersObject;

      // Properties

      // Detection properties
      /// <summary>
      /// The dictionary to use for the marker detection.
      /// </summary>
      public Dictionary Dictionary { get; set; }

      /// <summary>
      /// The parameters to use for the marker detection.
      /// </summary>
      public DetectorParameters DetectorParameters { get; set; }

      /// <summary>
      /// The side length of the markers that will be detected (in meters). This also is the scale factor of the <see cref="DetectedMarkersObject"/>.
      /// </summary>
      public float MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }

      /// <summary>
      /// Display the detected markers in the <see cref="CameraDeviceMarkersDetector.CameraImageTexture"/>.
      /// </summary>
      public bool ShowDetectedMarkers { get { return showDetectedMarkers; } set { showDetectedMarkers = value; } }

      /// <summary>
      /// Display the rejected markers candidates.
      /// </summary>
      public bool ShowRejectedCandidates { get { return showRejectedCandidates; } set { showRejectedCandidates = value; } }

      // Estimation properties
      /// <summary>
      /// Estimate the detected markers pose (position, rotation).
      /// </summary>
      public bool EstimatePose { get { return estimatePose; } set { estimatePose = value; } }

      /// <summary>
      /// The object to place above the detected markers.
      /// </summary>
      public GameObject DetectedMarkersObject { get { return detectedMarkersObject; } set { detectedMarkersObject = value; } }

      // Variables

      protected CameraParameters cameraParameters;
      protected Dictionary<int, GameObject> markerObjects;
      protected bool displayMarkerObjects = false;

      // MonoBehaviour methods

      /// <summary>
      /// Populate the CameraDeviceMarkersDetector base class properties.
      /// </summary>
      private void Awake()
      {
        CameraDeviceController = cameraDeviceController;
        Camera = camera;
        CameraPlane = cameraPlane;
      }

      /// <summary>
      /// When configurated, detect markers and show results each frame.
      /// </summary>
      void LateUpdate()
      {
        if (Configurated)
        {
          VectorVectorPoint2f corners;
          VectorInt ids;
          VectorVectorPoint2f rejectedImgPoints;
          VectorVec3d rvecs, tvecs;

          Detect(out corners, out ids, out rejectedImgPoints, out rvecs, out tvecs);
        }
      }

      // Methods

      /// <summary>
      /// Set up the detection and the results display.
      /// </summary>
      protected override void Configurate()
      {
        // Set the detector parameters and the dictionary
        DetectorParameters = detectorParametersManager.detectorParameters;
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

        // Try to load the camera parameters
        if (estimatePose)
        {
          bool cameraParametersLoaded = CameraDeviceController.ActiveCameraDevice.LoadCameraParameters(cameraParametersFilePath);
          cameraParameters = CameraDeviceController.ActiveCameraDevice.CameraParameters;
          estimatePose &= cameraParametersLoaded;
        }

        // Configurate the camera-plane group xor the canvas
        if (estimatePose)
        {
          displayMarkerObjects = ConfigurateCameraPlane();
        }
        cameraPlane.gameObject.SetActive(estimatePose && displayMarkerObjects);
        cameraDeviceCanvasDisplay.gameObject.SetActive(!estimatePose || !displayMarkerObjects);
      }

      /// <summary>
      /// Detect the markers on the <see cref="CameraDeviceMarkersDetector.CameraImageTexture"/>, estimate their poses, and show results. Should be called during LateUpdate(),
      /// after the update of the CameraImageTexture.
      /// </summary>
      /// <param name="corners">Vector of the detected marker corners.</param>
      /// <param name="ids">Vector of identifiers of the detected markers.</param>
      /// <param name="rejectedImgPoints">Vector of the corners with not a correct identification.</param>
      /// <param name="rvecs">Vector of rotation vectors of the detected markers.</param>
      /// <param name="tvecs">Vector of translation vectors of the detected markers.</param>
      public void Detect(out VectorVectorPoint2f corners, out VectorInt ids, out VectorVectorPoint2f rejectedImgPoints, out VectorVec3d rvecs, 
        out VectorVec3d tvecs)
      {
        // Copy the bytes of the texture to the image
        byte[] imageData = CameraImageTexture.GetRawTextureData();

        // Detect markers
        Mat image = new Mat(CameraImageTexture.height, CameraImageTexture.width, TYPE.CV_8UC3, imageData);
        Functions.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

        // Estimate board pose
        if (estimatePose && ids.Size() > 0)
        {
          Functions.EstimatePoseSingleMarkers(corners, markerSideLength, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);
        }
        else
        {
          rvecs = null;
          tvecs = null;
        }

        // Hide the marker objects
        if (estimatePose && displayMarkerObjects)
        {
          DeactivateMarkerObjects();
        }

        // Draw the detected markers and display the marker objects
        if (ids.Size() > 0)
        {
          if (showDetectedMarkers)
          {
            Functions.DrawDetectedMarkers(image, corners, ids);
          }

          if (estimatePose && displayMarkerObjects)
          {
            DisplayMarkerObjects(ids, rvecs, tvecs);
          }
        }

        // Draw rejected marker candidates
        if (showRejectedCandidates && rejectedImgPoints.Size() > 0)
        {
          Functions.DrawDetectedMarkers(image, rejectedImgPoints, new Color(100, 0, 255));
        }

        // Undistord the image if calibrated
        Mat undistordedImage, finalImage;
        if (estimatePose)
        {
          Imgproc.Undistord(image, out undistordedImage, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs);
          finalImage = undistordedImage;
        }
        else
        {
          finalImage = image;
        }

        // Copy the bytes of the final image to the texture
        int imageDataSize = (int)(finalImage.ElemSize() * finalImage.Total());
        CameraImageTexture.LoadRawTextureData(finalImage.data, imageDataSize);
        CameraImageTexture.Apply(false);
      }

      /// <summary>
      /// Hide all the marker objects.
      /// </summary>
      private void DeactivateMarkerObjects()
      {
        if (markerObjects != null)
        {
          foreach (var markerObject in markerObjects)
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
      private void DisplayMarkerObjects(VectorInt ids, VectorVec3d rvecs, VectorVec3d tvecs)
      {
        if (markerObjects == null)
        {
          markerObjects = new Dictionary<int, GameObject>();
        }

        for (uint i = 0; i < ids.Size(); i++)
        {
          // Retrieve the associated object for this marker or create it
          GameObject markerObject;
          if (!markerObjects.TryGetValue(ids.At(i), out markerObject))
          {
            markerObject = Instantiate(DetectedMarkersObject);
            markerObject.name = ids.At(i).ToString();
            markerObject.transform.SetParent(this.transform);

            markerObject.transform.localScale = markerObject.transform.localScale * markerSideLength; // Rescale to the marker size

            markerObjects.Add(ids.At(i), markerObject);
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

  /// \} aruco_unity_package
}