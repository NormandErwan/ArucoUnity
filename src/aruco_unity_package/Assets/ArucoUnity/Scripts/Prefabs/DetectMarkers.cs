using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DetectMarkers : CameraDeviceMarkersDetector
    {
      [Header("Detection configuration")]
      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      private DetectorParametersManager detectorParametersManager;

      [SerializeField]
      [Tooltip("Marker side length (in meters)")]
      private float markerSideLength = 0.1f;

      [SerializeField]
      private bool showDetectedMarkers = true;

      [SerializeField]
      private bool showRejectedCandidates = false;

      [Header("Camera configuration")]
      [SerializeField]
      private CameraDeviceController cameraDeviceController;

      [SerializeField]
      private CameraDeviceCanvasDisplay cameraDeviceCanvasDisplay;

      [SerializeField]
      private new Camera camera;

      [SerializeField]
      private GameObject cameraPlane;

      [Header("Estimation configuration")]
      [SerializeField]
      private bool estimatePose;

      [SerializeField]
      private string cameraParametersFilePath;

      [SerializeField]
      private GameObject detectedMarkersObject;

      public Vector3 userPositionShift; // TODO: remove

      // Detection properties
      public Dictionary Dictionary { get; set; }
      public DetectorParameters DetectorParameters { get; set; }
      public float MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }
      public bool ShowDetectedMarkers { get { return showDetectedMarkers; } set { showDetectedMarkers = value; } }
      public bool ShowRejectedCandidates { get { return showRejectedCandidates; } set { showRejectedCandidates = value; } }

      // Camera properties
      public Camera Camera { get { return camera; } set { camera = value; } }
      public GameObject CameraPlane { get { return cameraPlane; } set { cameraPlane = value; } }

      // Estimation properties
      public bool EstimatePose { get { return estimatePose; } set { estimatePose = value; } }
      public Utility.Mat CameraMatrix { get; set; }
      public Utility.Mat DistCoeffs { get; set; }
      public Vector3 OpticalCenter { get; private set; }
      public GameObject DetectedMarkersObject { get { return detectedMarkersObject; } set { detectedMarkersObject = value; } }

      private Dictionary<int, GameObject> markerObjects;

      public DetectMarkers(CameraDeviceController cameraDeviceController) 
        : base(cameraDeviceController)
      {
      }

      private void Awake()
      {
        CameraDeviceController = cameraDeviceController;
      }

      void LateUpdate()
      {
        if (Configurated)
        {
          Utility.Mat image;
          Utility.VectorVectorPoint2f corners;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f rejectedImgPoints;
          Utility.VectorVec3d rvecs, tvecs;
          
          Detect(out image, out corners, out ids, out rejectedImgPoints, out rvecs, out tvecs);
        }
      }

      protected override void Configurate()
      {
        DetectorParameters = detectorParametersManager.detectorParameters;
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

        // Configurate the camera-plane group or the canvas
        if (estimatePose)
        {
          bool configurateCameraPlaneSuccess = ConfigurateCameraPlane(cameraParametersFilePath);
          estimatePose &= configurateCameraPlaneSuccess;
        }

        // Activate the camera-plane group or the canvas
        cameraDeviceCanvasDisplay.gameObject.SetActive(!estimatePose);
        cameraPlane.gameObject.SetActive(estimatePose);

        Configurated = true;
      }

      public bool ConfigurateCameraPlane(string cameraParametersFilePath)
      {
        // Retrieve camera parameters
        CameraParameters cameraParameters = CameraParameters.LoadFromXmlFile(cameraParametersFilePath);
        if (cameraParameters == null)
        {
          Debug.LogError(gameObject.name + ": Unable to load the camera parameters from the '" + cameraParametersFilePath + "' file."
            + " Can't estimate pose of the detected markers.");
          return false;
        }

        // Prepare the camera parameters
        Utility.Mat cameraMatrix, distCoeffs;
        cameraParameters.ExportArrays(out cameraMatrix, out distCoeffs);
        CameraMatrix = cameraMatrix;
        DistCoeffs = distCoeffs;

        float cameraCx = (float)CameraMatrix.AtDouble(0, 2);
        float cameraCy = (float)CameraMatrix.AtDouble(1, 2);
        float cameraFy = (float)CameraMatrix.AtDouble(1, 1);

        float resolutionX = ImageTexture.width;
        float resolutionY = ImageTexture.height;

        // Calculate the optical center; based on: http://stackoverflow.com/a/36580522
        OpticalCenter = new Vector3(cameraCx / resolutionX, cameraCy / resolutionY, cameraFy);

        // Configurate the camera according to the camera parameters
        float vFov = 2f * Mathf.Atan(0.5f * resolutionY / cameraFy) * Mathf.Rad2Deg;
        camera.fieldOfView = vFov;
        camera.farClipPlane = cameraFy;
        camera.aspect = CameraDeviceController.ActiveCameraDevice.ImageRatio;
        camera.transform.position = Vector3.zero;
        camera.transform.rotation = Quaternion.identity;

        // Configurate the plane facing the camera that display the texture
        cameraPlane.transform.position = new Vector3(0, 0, camera.farClipPlane);
        cameraPlane.transform.rotation = CameraDeviceController.ActiveCameraDevice.ImageRotation;
        cameraPlane.transform.localScale = new Vector3(resolutionX, resolutionY, 1); 
        cameraPlane.transform.localScale = Vector3.Scale(cameraPlane.transform.localScale, CameraDeviceController.ActiveCameraDevice.ImageScaleFrontFacing);
        cameraPlane.GetComponent<MeshFilter>().mesh = CameraDeviceController.ActiveCameraDevice.ImageMesh;
        cameraPlane.GetComponent<Renderer>().material.mainTexture = ImageTexture;

        return true;
      }

      public void Detect(out Utility.Mat image, out Utility.VectorVectorPoint2f corners, out Utility.VectorInt ids, 
        out Utility.VectorVectorPoint2f rejectedImgPoints, out Utility.VectorVec3d rvecs, out Utility.VectorVec3d tvecs)
      {
        // Copy the bytes of the texture to the image
        byte[] imageData = ImageTexture.GetRawTextureData();

        // Detect markers
        image = new Utility.Mat(ImageTexture.height, ImageTexture.width, TYPE.CV_8UC3, imageData);
        Functions.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

        // Estimate board pose
        if (estimatePose && ids.Size() > 0)
        {
          Functions.EstimatePoseSingleMarkers(corners, markerSideLength, CameraMatrix, DistCoeffs, out rvecs, out tvecs);
        }
        else
        {
          rvecs = null;
          tvecs = null;
        }

        // Draw results
        if (estimatePose && DetectedMarkersObject)
        {
          DeactivateMarkerObjects();
        }

        if (ids.Size() > 0)
        {
          if (showDetectedMarkers)
          {
            Functions.DrawDetectedMarkers(image, corners, ids);
          }

          if (estimatePose && DetectedMarkersObject)
          {
            DisplayMarkerObjects(ids, rvecs, tvecs);
          }
        }

        if (showRejectedCandidates && rejectedImgPoints.Size() > 0)
        {
          Functions.DrawDetectedMarkers(image, rejectedImgPoints, new Color(100, 0, 255));
        }

        // Undistord the image
        Utility.Mat undistordedImage;
        Utility.Imgproc.Undistord(image, out undistordedImage, CameraMatrix, DistCoeffs);

        // Copy the bytes of the image to the texture
        int undistordedImageDataSize = (int)(undistordedImage.ElemSize() * undistordedImage.Total());
        ImageTexture.LoadRawTextureData(undistordedImage.data, undistordedImageDataSize);
        ImageTexture.Apply(false);
      }

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

      private void DisplayMarkerObjects(Utility.VectorInt ids, Utility.VectorVec3d rvecs, Utility.VectorVec3d tvecs)
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

          // Calculate the position shift of the object
          Vector3 imageCenterMarkerObject = new Vector3(0.5f, 0.5f, markerObject.transform.position.z);
          Vector3 opticalCenterMarkerObject = new Vector3(OpticalCenter.x, OpticalCenter.y, markerObject.transform.position.z);
          Vector3 positionShift = camera.ViewportToWorldPoint(imageCenterMarkerObject) - camera.ViewportToWorldPoint(opticalCenterMarkerObject);
          print(markerObject.name + " - imageCenter: " + imageCenterMarkerObject.ToString("F3") + "; opticalCenter: " + opticalCenterMarkerObject.ToString("F3")
            + "; positionShift: " + positionShift.ToString("F3"));

          // Place and orient the object to match the marker
          markerObject.transform.rotation = rvecs.At(i).ToRotation();
          markerObject.transform.position = tvecs.At(i).ToPosition();
          //markerObject.transform.localPosition += markerObject.transform.up * markerObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
          //markerObject.transform.localPosition -= markerObject.transform.rotation * positionShift; // Adjust the position taking account the optical center
          markerObject.transform.localPosition += markerObject.transform.rotation * userPositionShift; // TODO: remove

          markerObject.SetActive(true);
        }
      }
    }
  }
}