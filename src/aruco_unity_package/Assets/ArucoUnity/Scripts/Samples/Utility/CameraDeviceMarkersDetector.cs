using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    namespace Utility
    {
      /// <summary>
      /// Base for any markers detection class that use a <see cref="CameraDevice"/>.
      /// </summary>
      public abstract class CameraDeviceMarkersDetector : MonoBehaviour
      {
        // Events
        public delegate void CameraDeviceMakersDetectorAction();
        
        /// <summary>
        /// Executed when the markers detector is ready and configurated.
        /// </summary>
        public event CameraDeviceMakersDetectorAction OnConfigurated;

        // Properties
        /// <summary>
        /// The manipulated camera image texture by the marker detection class.
        /// </summary>
        public Texture2D CameraImageTexture { get; private set; }

        /// <summary>
        /// True when the markers detection class is ready and configurated.
        /// </summary>
        public bool Configurated { get; protected set; }

        /// <summary>
        /// The <see cref="CameraDeviceController"/> to use. When its active camera device has started, execute the configuration of the marker 
        /// detection class.
        /// </summary>
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

        // Variables

        /// <summary>
        /// The <see cref="CameraImageTexture"/>'s width.
        /// </summary>
        protected int cameraImageResolutionX;

        /// <summary>
        /// The <see cref="CameraImageTexture"/>'s height.
        /// </summary>
        protected int cameraImageResolutionY;

        private CameraDeviceController cameraDeviceControllerValue = null;

        // MonoBehaviour methods

        /// <summary>
        /// Subscribe to <see cref="CameraDeviceController"/> and execute the configuration if the active camera device has already started.
        /// </summary>
        protected virtual void OnEnable()
        {
          Configurated = false;

          if (cameraDeviceControllerValue != null)
          {
            cameraDeviceControllerValue.OnActiveCameraDeviceStarted += CompleteConfigurate;
            ConfigurateIfActiveCameraDeviceStarted();
          }
        }

        /// <summary>
        /// Unsubscribe from <see cref="CameraDeviceController"/>.
        /// </summary>
        protected virtual void OnDisable()
        {
          if (cameraDeviceControllerValue != null)
          {
            cameraDeviceControllerValue.OnActiveCameraDeviceStarted -= CompleteConfigurate;
          }
        }

        // Methods

        /// <summary>
        /// The configuration content of the marker detection class.
        /// </summary>
        protected abstract void Configurate();

        /// <summary>
        /// Configuration the the marker detection class.
        /// </summary>
        /// <param name="activeCameraDevice">The current active camera device.</param>
        private void CompleteConfigurate(CameraDevice activeCameraDevice)
        {
          // Configurate the CameraImageTexture
          Configurated = false;
          CameraImageTexture = activeCameraDevice.Texture2D;
          cameraImageResolutionX = CameraImageTexture.width;
          cameraImageResolutionY = CameraImageTexture.height;

          // Configurate content
          Configurate();

          // Update the state and notify
          if (OnConfigurated != null)
          {
            OnConfigurated();
          }
          Configurated = true;
        }

        /// <summary>
        /// If the camera is already started, execute the configuration.
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