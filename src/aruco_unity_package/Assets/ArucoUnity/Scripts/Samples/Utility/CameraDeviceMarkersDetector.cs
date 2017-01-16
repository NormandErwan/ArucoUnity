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
        // Events
        public delegate void CameraDeviceMakersDetectorAction();

        public event CameraDeviceMakersDetectorAction OnConfigurated;

        // Properties
        public Texture2D CameraImageTexture { get; private set; }

        public bool Configurated { get; protected set; }

        public CameraDeviceController CameraDeviceController {
          get { return cameraDeviceControllerValue; }
          set
          {
            // Reset configuration
            Configurated = false;

            // Unsubscribe from the previous cameraDeviceController
            if (cameraDeviceControllerValue != null)
            {
              cameraDeviceControllerValue.OnActiveCameraDeviceStarted -= CompleteConfigurate;
            }

            // Subscribe to the new cameraDeviceController
            cameraDeviceControllerValue = value;
            cameraDeviceControllerValue.OnActiveCameraDeviceStarted += CompleteConfigurate;

            ConfigurateIfActiveCameraDeviceStarted();
          }
        }

        // Internals
        protected int cameraImageResolutionX;
        protected int cameraImageResolutionY;

        private CameraDeviceController cameraDeviceControllerValue = null;

        protected virtual void OnEnable()
        {
          if (cameraDeviceControllerValue != null)
          {
            cameraDeviceControllerValue.OnActiveCameraDeviceStarted += CompleteConfigurate;
            ConfigurateIfActiveCameraDeviceStarted();
          }
        }

        protected virtual void OnDisable()
        {
          if (cameraDeviceControllerValue != null)
          {
            cameraDeviceControllerValue.OnActiveCameraDeviceStarted -= CompleteConfigurate;
          }
        }

        protected abstract void Configurate();

        private void CompleteConfigurate(CameraDevice activeCameraDevice)
        {
          // Configurate start
          Configurated = false;
          CameraImageTexture = activeCameraDevice.Texture2D;

          cameraImageResolutionX = CameraImageTexture.width;
          cameraImageResolutionY = CameraImageTexture.height;

          // Configurate main
          Configurate();

          // Configurate end
          if (OnConfigurated != null)
          {
            OnConfigurated();
          }
          Configurated = true;
        }

        /// <summary>
        /// If the camera is already started, start the configuration.
        /// </summary>
        private void ConfigurateIfActiveCameraDeviceStarted()
        {
          if (cameraDeviceControllerValue != null)
          {
            CameraDevice activeCameraDevice = cameraDeviceControllerValue.ActiveCameraDevice;
            if (activeCameraDevice != null && activeCameraDevice.Started)
            {
              CompleteConfigurate(activeCameraDevice);
            }
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}