using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    namespace Utility
    {
      // TODO: doc
      public abstract class CameraDeviceMarkersDetector : MonoBehaviour
      {
        // Properties
        public CameraDeviceController CameraDeviceController { get; set; }
        public Texture2D CameraImageTexture { get; private set; }
        public bool Configurated { get; protected set; }

        // Events
        public delegate void CameraDeviceMakersDetectorAction();
        public event CameraDeviceMakersDetectorAction OnConfigurated;

        // Internals
        protected int cameraImageResolutionX;
        protected int cameraImageResolutionY;

        // TODO: allow to add a CameraDeviceController after objet creation
        public CameraDeviceMarkersDetector(CameraDeviceController cameraDeviceController)
        {
          CameraDeviceController = cameraDeviceController;
        }

        protected virtual void OnEnable()
        {
          Configurated = false;
          CameraDeviceController.OnActiveCameraDeviceChanged += CameraDeviceController_OnActiveCameraChanged;
        }

        protected virtual void OnDisable()
        {
          CameraDeviceController.OnActiveCameraDeviceChanged -= CameraDeviceController_OnActiveCameraChanged;
          CameraDeviceController.ActiveCameraDevice.OnStarted -= CompleteConfigurate;
        }

        private void CameraDeviceController_OnActiveCameraChanged(CameraDevice previousCameraDevice)
        {
          if (previousCameraDevice != null)
          {
            previousCameraDevice.OnStarted -= CompleteConfigurate;
          }
          CameraDeviceController.ActiveCameraDevice.OnStarted += CompleteConfigurate;
        }

        private void CompleteConfigurate()
        {
          // Configurate start
          Configurated = false;
          CameraImageTexture = CameraDeviceController.ActiveCameraDevice.Texture2D;

          cameraImageResolutionX = CameraImageTexture.width;
          cameraImageResolutionY = CameraImageTexture.height;

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

  /// \} aruco_unity_package
}