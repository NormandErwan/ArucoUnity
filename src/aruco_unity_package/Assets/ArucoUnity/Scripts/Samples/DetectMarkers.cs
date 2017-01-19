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
    public class DetectMarkers : ArucoDetector
    {
      // Editor fields

      [Header("Detection configuration")]
      [SerializeField]
      [Tooltip("The dictionary to use for the marker detection")]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("The parameters to use for the marker detection")]
      private ArucoDetectorParametersController detectorParametersManager;

      [SerializeField]
      [Tooltip("The side length of the markers that will be detected (in meters). This also is the scale factor of the DetectedMarkersObject")]
      private float markerSideLength = 0.1f;

      [SerializeField]
      [Tooltip("Display the detected markers in the CameraImageTexture")]
      private bool showDetectedMarkers = true;

      [SerializeField]
      [Tooltip("Display the rejected markers candidates")]
      private bool showRejectedCandidates = false;

      [Header("Camera configuration")]
      [SerializeField]
      [Tooltip("The parameters to use for the marker detection")]
      private CameraDeviceController cameraDeviceController;

      [SerializeField]
      [Tooltip("The file path to load the camera parameters.")]
      private string cameraParametersFilePath = "Assets/ArucoUnity/aruco-calibration.xml";

      [SerializeField]
      [Tooltip("If estimatePose is false, the CameraImageTexture will be displayed on this canvas")]
      private CameraDeviceCanvasDisplay cameraDeviceCanvasDisplay;

      [SerializeField]
      [Tooltip("The Unity camera that will capture the CameraPlane display")]
      private new Camera camera;

      [SerializeField]
      [Tooltip("The plane facing the camera that display the CameraImageTexture")]
      private GameObject cameraPlane;

      [Header("Pose estimation configuration")]
      [SerializeField]
      [Tooltip("Estimate the detected markers pose (position, rotation)")]
      private bool estimatePose;

      [SerializeField]
      private MarkerObjectsController markerObjectsController;

      // Events
      // TODO: OnMarkerDetected, OnMarkerTracked, OnMarkerLost 

      // Properties

      // Detection configuration properties
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
      /// Display the detected markers in the <see cref="ArucoDetector.CameraImageTexture"/>.
      /// </summary>
      public bool ShowDetectedMarkers { get { return showDetectedMarkers; } set { showDetectedMarkers = value; } }

      /// <summary>
      /// Display the rejected markers candidates.
      /// </summary>
      public bool ShowRejectedCandidates { get { return showRejectedCandidates; } set { showRejectedCandidates = value; } }

      // Pose estimation properties
      /// <summary>
      /// Estimate the detected markers pose (position, rotation).
      /// </summary>
      public bool EstimatePose { get { return estimatePose; } set { EstimatePose = value; } }

      public MarkerObjectsController MarkerObjectsController { get { return markerObjectsController; } set { markerObjectsController = value; } }

      // Variables

      protected CameraParameters cameraParameters;
      protected bool displayMarkerObjects = false;

      // MonoBehaviour methods

      /// <summary>
      /// Populate the <see cref="ArucoDetector"/> parent class properties.
      /// </summary>
      protected void Awake()
      {
        CameraDeviceController = cameraDeviceController;
        Camera = camera;
        CameraPlane = cameraPlane;
      }

      /// <summary>
      /// When configurated, detect markers and show results each frame.
      /// </summary>
      protected void LateUpdate()
      {
        if (Configurated)
        {
          VectorVectorPoint2f corners;
          VectorInt ids;
          VectorVectorPoint2f rejectedImgPoints;
          VectorVec3d rvecs, tvecs;
          Mat image;

          Detect(out corners, out ids, out rejectedImgPoints, out rvecs, out tvecs, out image);
          ShowResults(corners, ids, rejectedImgPoints, rvecs, tvecs, image);
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
        if (EstimatePose)
        {
          bool cameraParametersLoaded = CameraDeviceController.ActiveCameraDevice.LoadCameraParameters(cameraParametersFilePath);
          cameraParameters = CameraDeviceController.ActiveCameraDevice.CameraParameters;
          EstimatePose &= cameraParametersLoaded;
        }

        // Configurate the camera-plane group xor the canvas
        if (EstimatePose)
        {
          ConfigurateCameraPlane();
          if (CameraPlaneConfigurated)
          {
            MarkerObjectsController.SetCamera(Camera, cameraParameters);
            MarkerObjectsController.MarkerSideLength = MarkerSideLength;
          }
        }
        cameraPlane.gameObject.SetActive(EstimatePose && CameraPlaneConfigurated);
        cameraDeviceCanvasDisplay.gameObject.SetActive(!EstimatePose || !CameraPlaneConfigurated);
      }

      /// <summary>
      /// Detect the markers on the <see cref="ArucoDetector.CameraImageTexture"/> and estimate their poses. Should be called during LateUpdate(),
      /// after the update of the CameraImageTexture.
      /// </summary>
      /// <param name="corners">Vector of the detected marker corners.</param>
      /// <param name="ids">Vector of identifiers of the detected markers.</param>
      /// <param name="rejectedImgPoints">Vector of the corners with not a correct identification.</param>
      /// <param name="rvecs">Vector of rotation vectors of the detected markers.</param>
      /// <param name="tvecs">Vector of translation vectors of the detected markers.</param>
      /// <param name="image">The OpenCV's Mat image used for the detection, created from <see cref="ArucoDetector.CameraImageTexture"/>.</param>
      public void Detect(out VectorVectorPoint2f corners, out VectorInt ids, out VectorVectorPoint2f rejectedImgPoints, out VectorVec3d rvecs,
        out VectorVec3d tvecs, out Mat image)
      {
        // Copy the bytes of the texture to the image
        byte[] imageData = CameraImageTexture.GetRawTextureData();

        // Detect markers
        image = new Mat(CameraImageTexture.height, CameraImageTexture.width, TYPE.CV_8UC3, imageData);
        Functions.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

        // Estimate markers pose
        if (EstimatePose && ids.Size() > 0)
        {
          Functions.EstimatePoseSingleMarkers(corners, MarkerSideLength, cameraParameters.CameraMatrix, cameraParameters.DistCoeffs, out rvecs, out tvecs);
        }
        else
        {
          rvecs = null;
          tvecs = null;
        }
      }

      /// <summary>
      /// Show the results of the detected markers 
      /// from <see cref="Detect(out VectorVectorPoint2f, out VectorInt, out VectorVectorPoint2f, out VectorVec3d, out VectorVec3d, out Mat)"/>.
      /// </summary>
      /// <param name="corners">Vector of the detected marker corners by Detect().</param>
      /// <param name="ids">Vector of identifiers of the detected markers by Detect().</param>
      /// <param name="rejectedImgPoints">Vector of the corners with not a correct identification by Detect().</param>
      /// <param name="rvecs">Vector of rotation vectors of the detected markers by Detect().</param>
      /// <param name="tvecs">Vector of translation vectors of the detected markers by Detect().</param>
      /// <param name="image">The image used for the detection returned by Detect().</param>
      public void ShowResults(VectorVectorPoint2f corners, VectorInt ids, VectorVectorPoint2f rejectedImgPoints, VectorVec3d rvecs, 
        VectorVec3d tvecs, Mat image)
      {
        // Draw the detected markers and the rejected marker candidates
        if (ShowDetectedMarkers && ids.Size() > 0)
        {
          Functions.DrawDetectedMarkers(image, corners, ids);
        }

        // Draw rejected marker candidates
        if (ShowRejectedCandidates && rejectedImgPoints.Size() > 0)
        {
          Functions.DrawDetectedMarkers(image, rejectedImgPoints, new Color(100, 0, 255));
        }

        // Show the marker objects
        MarkerObjectsController.DeactivateMarkerObjects();
        if (EstimatePose && CameraPlaneConfigurated)
        {
          MarkerObjectsController.UpdateTransforms(ids, rvecs, tvecs);
        }

        // Undistord the image if calibrated
        Mat undistordedImage, finalImage;
        if (EstimatePose)
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
    }
  }

  /// \} aruco_unity_package
}