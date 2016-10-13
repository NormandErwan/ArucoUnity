using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public class DetectMarkers : MonoBehaviour
    {
      public Dictionary dictionary;
      public DetectorParameters detectorParameters;

      [HideInInspector]
      public Texture2D imageTexture;

      [Header("Detection configuration")]
      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      public bool showRejectedCandidates;

      [SerializeField]
      private DetectorParametersManager detectorParametersManager;

      [Header("Camera configuration")]
      [SerializeField]
      private DeviceCameraController deviceCameraController;

      void OnEnable()
      {
        DeviceCameraController.OnCameraStarted += ConfigurateFromEditorValues;
      }

      void OnDisable()
      {
        DeviceCameraController.OnCameraStarted -= ConfigurateFromEditorValues;
      }

      void LateUpdate()
      {
        if (deviceCameraController.cameraStarted)
        {
          Utility.VectorVectorPoint2f corners;
          Utility.VectorInt ids;
          Utility.VectorVectorPoint2f rejectedImgPoints;
          Utility.Mat image;

          imageTexture.SetPixels32(deviceCameraController.activeCameraTexture.GetPixels32());
          Detect(out corners, out ids, out rejectedImgPoints, out image);
        }
      }

      // Call it first if you're using the Script alone, not with the Prefab.
      public void Configurate(Dictionary dictionary, DetectorParameters detectorParameters, Texture2D imageTexture)
      {
        this.dictionary = dictionary;
        this.detectorParameters = detectorParameters;
        this.imageTexture = imageTexture;
      }

      void ConfigurateFromEditorValues()
      {
        dictionary = Methods.GetPredefinedDictionary(dictionaryName);
        detectorParameters = detectorParametersManager.detectorParameters;

        ConfigurateImageTexture(deviceCameraController);
      }

      public void ConfigurateImageTexture(DeviceCameraController deviceCameraController)
      {
        imageTexture = new Texture2D(deviceCameraController.activeCameraTexture.width, deviceCameraController.activeCameraTexture.height,
          TextureFormat.RGB24, false);
        deviceCameraController.SetActiveTexture(imageTexture);
      }

      public void Detect(out Utility.VectorVectorPoint2f corners, out Utility.VectorInt ids, out Utility.VectorVectorPoint2f rejectedImgPoints, 
        out Utility.Mat image)
      {
        byte[] imageData = imageTexture.GetRawTextureData();
        image = new Utility.Mat(imageTexture.height, imageTexture.width, imageData);
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