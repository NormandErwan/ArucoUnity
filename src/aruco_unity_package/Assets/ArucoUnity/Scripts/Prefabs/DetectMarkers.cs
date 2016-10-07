using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DetectMarkers : MonoBehaviour
    {
      public Dictionary dictionary;
      public DetectorParameters detectorParameters;

      public Utility.VectorVectorPoint2f corners;
      public Utility.VectorInt ids;
      public Utility.VectorVectorPoint2f rejectedImgPoints;
      
      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      private bool showRejectedCandidates;

      [SerializeField]
      private DeviceCameraController deviceCameraController;

      // TODO : detector parameters manager

      private Texture2D imageTexture;

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
          Detect(deviceCameraController.activeCameraTexture, showRejectedCandidates);
        }
      }

      private void Configurate()
      {
        ConfigurateDetection(dictionaryName);
        ConfigurateImageTexture(deviceCameraController);
      }

      public void ConfigurateDetection(PREDEFINED_DICTIONARY_NAME dictionaryName)
      {
        dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        detectorParameters = new DetectorParameters();
      }

      private void ConfigurateImageTexture(DeviceCameraController deviceCameraController)
      {
        imageTexture = new Texture2D(deviceCameraController.activeCameraTexture.width, deviceCameraController.activeCameraTexture.height, 
          TextureFormat.RGB24, false);
        deviceCameraController.SetActiveTexture(imageTexture);
      }

      public Texture2D Detect(WebCamTexture camTexture, bool showRejectedCandidates)
      {
        imageTexture.SetPixels32(camTexture.GetPixels32());
        Detect(ref imageTexture, showRejectedCandidates);
        return imageTexture;
      }

      public void Detect(ref Texture2D imageTexture, bool showRejectedCandidates)
      {
        byte[] imageData = imageTexture.GetRawTextureData();
        Utility.Mat image = new Utility.Mat(imageTexture.height, imageTexture.width, imageData);
        Methods.DetectMarkers(image, dictionary, out corners, out ids, detectorParameters, out rejectedImgPoints);

        if (ids.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, corners, ids);
        }

        if (showRejectedCandidates && rejectedImgPoints.Size() > 0)
        {
          Methods.DrawDetectedMarkers(image, rejectedImgPoints, new Color(100, 0, 255));
        }

        int imageDataSize = (int)(image.ElemSize() * image.Total());
        imageTexture.LoadRawTextureData(image.data, imageDataSize);
        imageTexture.Apply(false);
      }
    }
  }
}