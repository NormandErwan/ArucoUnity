using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DetectMarkers : MonoBehaviour
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
      private DeviceCameraCanvasDisplay deviceCameraCanvasDisplay;

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

      // Detection configuration
      public Dictionary Dictionary { get; set; }
      public DetectorParameters DetectorParameters { get; set; }
      public float MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }
      public bool ShowDetectedMarkers { get { return showDetectedMarkers; } set { showDetectedMarkers = value; } }
      public bool ShowRejectedCandidates { get { return showRejectedCandidates; } set { showRejectedCandidates = value; } }

      // Camera configuration
      public Texture2D ImageTexture { get; set; }
      public Camera Camera { get { return camera; } set { camera = value; } }
      public GameObject CameraPlane { get { return cameraPlane; } set { cameraPlane = value; } }

      // Estimation configuration
      public bool EstimatePose { get { return estimatePose; } set { estimatePose = value; } }
      public Utility.Mat CameraMatrix { get; set; }
      public Utility.Mat DistCoeffs { get; set; }
      public GameObject DetectedMarkersObject { get { return detectedMarkersObject; } set { detectedMarkersObject = value; } }

      private Dictionary<int, GameObject> markerObjects;
      private bool configurated;
      private DeviceCameraController deviceCameraController;

      void OnEnable()
      {
        deviceCameraController = DeviceCameraController.Instance;

        configurated = false;
        deviceCameraController.OnCameraStarted += Configurate;
      }

      void OnDisable()
      {
        deviceCameraController.OnCameraStarted -= Configurate;
      }

      void LateUpdate()
      {
        if (deviceCameraController.cameraStarted && configurated)
        {
          Utility.Mat image;
          Utility.VectorVectorPoint2f corners;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f rejectedImgPoints;
          Utility.VectorVec3d rvecs, tvecs;
          
          Detect(out image, out corners, out ids, out rejectedImgPoints, out rvecs, out tvecs);
        }
      }

      private void Configurate()
      {
        DetectorParameters = detectorParametersManager.detectorParameters;
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        ImageTexture = deviceCameraController.activeCameraTexture2D;

        // Configurate the camera-plane group or the canvas
        bool configurateCameraPlaneSuccess = false;
        if (estimatePose)
        {
          configurateCameraPlaneSuccess = ConfigurateCameraPlane(cameraParametersFilePath);
        }

        // Activate the camera-plane group or the canvas
        bool useCanvas = !estimatePose || !configurateCameraPlaneSuccess;
        deviceCameraCanvasDisplay.gameObject.SetActive(useCanvas);
        cameraPlane.gameObject.SetActive(!useCanvas);

        configurated = true;
      }

      public bool ConfigurateCameraPlane(string cameraParametersFilePath)
      {
        // Camera parameters
        CameraParameters cameraParameters = CameraParameters.LoadFromXmlFile(cameraParametersFilePath);
        if (cameraParameters == null)
        {
          return false;
        }

        Utility.Mat cameraMatrix, distCoeffs;
        cameraParameters.ExportArrays(out cameraMatrix, out distCoeffs);
        CameraMatrix = cameraMatrix;
        DistCoeffs = distCoeffs;

        // Camera configuration
        float cameraFy = (float)cameraMatrix.AtDouble(1, 1);
        float vFov = 2f * Mathf.Atan(0.5f * ImageTexture.height / cameraFy) * Mathf.Rad2Deg;
        camera.fieldOfView = vFov;
        camera.aspect = ImageTexture.width / (float)ImageTexture.height;
        camera.transform.position = Vector3.zero;
        camera.transform.rotation = Quaternion.identity;
        camera.farClipPlane = cameraFy;

        // Plane configuration
        float cameraPlaneDistance = 0.5f * ImageTexture.height / Mathf.Tan(0.5f * vFov * Mathf.Deg2Rad);
        print(cameraPlaneDistance + " " + cameraFy);

        cameraPlane.transform.position = new Vector3(0, 0, cameraFy);
        cameraPlane.transform.rotation = Quaternion.identity;
        cameraPlane.transform.localScale = new Vector3(ImageTexture.width, ImageTexture.height, 1);
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
          GameObject markerObject;
          if (!markerObjects.TryGetValue(ids.At(i), out markerObject))
          {
            markerObject = Instantiate(DetectedMarkersObject);
            markerObject.name = ids.At(i).ToString();
            markerObject.transform.SetParent(this.transform);
            markerObjects.Add(ids.At(i), markerObject);
          }

          // TODO: shift the position withe the optical center values from the camera parameters
          markerObject.transform.position = tvecs.At(i).ToPosition();
          markerObject.transform.rotation = rvecs.At(i).ToRotation();
          markerObject.SetActive(true);
          print(i + ": " + rvecs.At(i).ToRotation().eulerAngles + " " + tvecs.At(i).ToPosition());
        }
      }
    }
  }
}