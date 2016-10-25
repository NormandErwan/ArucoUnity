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
      private bool showRejectedCandidates;

      [Header("Camera configuration")]
      [SerializeField]
      private DeviceCameraController deviceCameraController;

      [Header("Camera parameters")]
      [SerializeField]
      private bool estimatePose;

      [SerializeField]
      private string cameraParametersFilePath;

      // Detection configuration
      public Dictionary Dictionary { get; set; }
      public DetectorParameters DetectorParameters { get; set; }
      public float MarkerSideLength { get { return markerSideLength; } set { markerSideLength = value; } }
      public bool ShowRejectedCandidates { get { return showRejectedCandidates; } set { showRejectedCandidates = value; } }

      // Camera configuration
      public Texture2D ImageTexture { get; set; }

      // Camera parameters
      public bool EstimatePose { get { return estimatePose; } set { estimatePose = value; } }
      public Utility.Mat CameraMatrix { get; set; }
      public Utility.Mat DistCoeffs { get; set; }

      void OnEnable()
      {
        DeviceCameraController.OnCameraStarted += Configurate;
      }

      void OnDisable()
      {
        DeviceCameraController.OnCameraStarted -= Configurate;
      }

      void LateUpdate()
      {
        if (deviceCameraController.cameraStarted)
        {
          Utility.Mat image;
          Utility.VectorVectorPoint2f corners;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f rejectedImgPoints;
          Utility.VectorVec3d rvecs, tvecs;

          ImageTexture.SetPixels32(deviceCameraController.activeCameraTexture.GetPixels32());
          Detect(out image, out corners, out ids, out rejectedImgPoints, out rvecs, out tvecs);
        }
      }

      void Configurate()
      {
        DetectorParameters = detectorParametersManager.detectorParameters;
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);

        ConfigurateImageTexture(deviceCameraController);

        if (estimatePose)
        {
          ConfigurateCameraParameters(cameraParametersFilePath);
        }
      }

      public void ConfigurateImageTexture(DeviceCameraController deviceCameraController)
      {
        ImageTexture = new Texture2D(deviceCameraController.activeCameraTexture.width, deviceCameraController.activeCameraTexture.height,
          TextureFormat.RGB24, false);
        deviceCameraController.SetActiveTexture(ImageTexture);
      }

      public void ConfigurateCameraParameters(string cameraParametersFilePath)
      {
        CameraParameters cameraParameters = CameraParameters.LoadFromXmlFile(cameraParametersFilePath);

        Utility.Mat cameraMatrix, distCoeffs;
        cameraParameters.ExportArrays(out cameraMatrix, out distCoeffs);
        CameraMatrix = cameraMatrix;
        DistCoeffs = distCoeffs;
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
        if (ids.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, corners, ids);
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
    }
  }
}