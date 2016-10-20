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
      private bool showRejectedCandidates;

      [SerializeField]
      private DetectorParametersManager detectorParametersManager;

      [Header("Camera configuration")]
      [SerializeField]
      private DeviceCameraController deviceCameraController;

      public Dictionary Dictionary { get; set; }
      public bool ShowRejectedCandidates { get { return showRejectedCandidates; } set { showRejectedCandidates = value; } }
      public DetectorParameters DetectorParameters { get; set; }
      public Texture2D ImageTexture { get; set; }

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
          Utility.VectorVectorPoint2f corners;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f rejectedImgPoints;
          Utility.Mat image;

          ImageTexture.SetPixels32(deviceCameraController.activeCameraTexture.GetPixels32());
          Detect(out corners, out ids, out rejectedImgPoints, out image);
        }
      }

      void Configurate()
      {
        Dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        DetectorParameters = detectorParametersManager.detectorParameters;
        ConfigurateImageTexture(deviceCameraController);
      }

      public void ConfigurateImageTexture(DeviceCameraController deviceCameraController)
      {
        ImageTexture = new Texture2D(deviceCameraController.activeCameraTexture.width, deviceCameraController.activeCameraTexture.height,
          TextureFormat.RGB24, false);
        deviceCameraController.SetActiveTexture(ImageTexture);
      }

      public void Detect(out Utility.VectorVectorPoint2f corners, out Utility.VectorInt ids, out Utility.VectorVectorPoint2f rejectedImgPoints, 
        out Utility.Mat image)
      {
        byte[] imageData = ImageTexture.GetRawTextureData();
        image = new Utility.Mat(ImageTexture.height, ImageTexture.width, TYPE.CV_8UC3, imageData);
        Methods.DetectMarkers(image, Dictionary, out corners, out ids, DetectorParameters, out rejectedImgPoints);

        if (ids.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, corners, ids);
        }

        if (showRejectedCandidates && rejectedImgPoints.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, rejectedImgPoints, new Color(100, 0, 255));
        }

        int imageDataSize = (int)(image.ElemSize() * image.Total());
        ImageTexture.LoadRawTextureData(image.data, imageDataSize);
        ImageTexture.Apply(false);
      }
    }
  }
}