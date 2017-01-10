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

      public Vector3 positionShift;

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
      public Vector3 PositionShift { get; private set; }
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
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);

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

        float resolutionX = ImageTexture.width,
              resolutionY = ImageTexture.height,
              cameraCx = 0.5f, cameraCy = 0.5f,
              cameraFy = camera.farClipPlane;
        if (cameraParameters != null)
        {
          Utility.Mat cameraMatrix, distCoeffs;
          cameraParameters.ExportArrays(out cameraMatrix, out distCoeffs);
          CameraMatrix = cameraMatrix;
          DistCoeffs = distCoeffs;

          cameraCx = (float)CameraMatrix.AtDouble(0, 2);
          cameraCy = (float)CameraMatrix.AtDouble(1, 2);
          cameraFy = (float)CameraMatrix.AtDouble(1, 1);
        }
        
        // Calculate the position shift; based on: http://stackoverflow.com/a/36580522
        if (cameraParameters != null)
        {
          Vector3 imageCenter = new Vector3(0.5f, 0.5f, cameraFy);
          Vector3 opticalCenter = new Vector3(0.5f + cameraCx / resolutionX, 0.5f + cameraCy / resolutionY, cameraFy);
          PositionShift = camera.ViewportToWorldPoint(imageCenter) - camera.ViewportToWorldPoint(opticalCenter);
          print("cx: " + cameraCx + "; cy: " + cameraCy + "; imageCenter: " + imageCenter + "; opticalCenter: " + opticalCenter
            + "; positionShift: " + PositionShift);
        }

        // Configurate the camera according to the camera parameters
        if (cameraParameters != null)
        {
          float vFov = 2f * Mathf.Atan(0.5f * resolutionY / cameraFy) * Mathf.Rad2Deg;
          camera.fieldOfView = vFov;
          camera.farClipPlane = cameraFy;
        }
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
        Methods.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

        // Estimate board pose
        if (estimatePose && ids.Size() > 0)
        {
          Methods.EstimatePoseSingleMarkers(corners, markerSideLength, CameraMatrix, DistCoeffs, out rvecs, out tvecs);
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
            Methods.DrawDetectedMarkers(image, corners, ids);
          }

          if (estimatePose && DetectedMarkersObject)
          {
            DisplayMarkerObjects(ids, rvecs, tvecs);
          }
        }

        if (showRejectedCandidates && rejectedImgPoints.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, rejectedImgPoints, new Color(100, 0, 255));
        }

        // Copy the bytes of the image to the texture
        int imageDataSize = (int)(image.ElemSize() * image.Total());
        ImageTexture.LoadRawTextureData(image.data, imageDataSize);
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
            markerObjects.Add(ids.At(i), markerObject);
          }

          // Place and orient the object to match the marker
          markerObject.transform.localScale = new Vector3(markerSideLength, markerSideLength, markerSideLength);
          markerObject.transform.rotation = rvecs.At(i).ToRotation();

          markerObject.transform.position = tvecs.At(i).ToPosition();
          markerObject.transform.localPosition += markerObject.transform.forward * markerSideLength / 2; // Move up the object to coincide with the marker
          //markerObject.transform.position += PositionShift; // TODO: fix
          markerObject.transform.localPosition += (markerObject.transform.rotation * positionShift); // TODO: remove

          markerObject.SetActive(true);
        }
      }
    }
  }
}