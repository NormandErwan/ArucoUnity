using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public abstract class CameraDeviceMarkersDetector : MonoBehaviour
    {
      // Properties
      public CameraDeviceController CameraDeviceController { get; set; }
      public Texture2D ImageTexture { get; private set; }
      public bool Configurated { get; protected set; }

      // Events
      public delegate void CameraDeviceMakersDetectorAction();
      public event CameraDeviceMakersDetectorAction OnConfigurated;

      public CameraDeviceMarkersDetector(CameraDeviceController cameraDeviceController)
      {
        CameraDeviceController = cameraDeviceController;
      }

      protected virtual void OnEnable()
      {
        Configurated = false;
        CameraDeviceController.OnActiveCameraStarted += WholeConfigurate;
      }

      protected virtual void OnDisable()
      {
        CameraDeviceController.OnActiveCameraStarted -= WholeConfigurate;
      }

      private void WholeConfigurate()
      {
        // Configurate start
        Configurated = false;
        ImageTexture = CameraDeviceController.ActiveCameraTexture2D;

        Configurate();

        // Configurate end
        if (OnConfigurated != null)
        {
          OnConfigurated();
        }
        Configurated = true;
      }

      protected abstract void Configurate();
    }
  }
}