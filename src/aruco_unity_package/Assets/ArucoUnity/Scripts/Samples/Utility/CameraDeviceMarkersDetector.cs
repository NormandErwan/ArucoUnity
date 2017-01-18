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
        /// True when the markers detection class is ready and configurated.
        /// </summary>
        public bool Configurated { get; protected set; }

        // CameraDeviceController related properties
        /// <summary>
        /// The manipulated camera image texture by the marker detection class.
        /// </summary>
        public Texture2D CameraImageTexture { get; protected set; }

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

        // Camera properties
        /// <summary>
        /// The Unity camera that will capture the <see cref="CameraPlane"/> display.
        /// </summary>
        public Camera Camera { get; protected set; }

        /// <summary>
        /// The plane facing the camera that display the <see cref="CameraDeviceMarkersDetector.CameraImageTexture"/>.
        /// </summary>
        public GameObject CameraPlane { get; protected set; }

        // Variables

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

        /// <summary>
        /// Configurate from the camera parameters the <see cref="Camera"/> and a the <see cref="CameraPlane"/> that display the 
        /// <see cref="CameraImageTexture"/> facing the camera.
        /// </summary>
        /// <returns>If the configuration has been successful.</returns>
        public bool ConfigurateCameraPlane()
        {
          CameraParameters cameraParameters = CameraDeviceController.ActiveCameraDevice.CameraParameters;

          if (Camera == null || CameraPlane == null || cameraParameters == null)
          {
            Debug.LogError(gameObject.name + ": unable to configurate the camera and the facing plane. The following properties must be set: Camera"
              + " and CameraPlane.");
            return false;
          }

          // Configurate the camera according to the camera parameters
          float vFov = 2f * Mathf.Atan(0.5f * CameraImageTexture.height / cameraParameters.CameraFy) * Mathf.Rad2Deg;
          Camera.fieldOfView = vFov;
          Camera.farClipPlane = cameraParameters.CameraFy;
          Camera.aspect = CameraDeviceController.ActiveCameraDevice.ImageRatio;
          Camera.transform.position = Vector3.zero;
          Camera.transform.rotation = Quaternion.identity;

          // Configurate the plane facing the camera that display the texture
          CameraPlane.transform.position = new Vector3(0, 0, Camera.farClipPlane);
          CameraPlane.transform.rotation = CameraDeviceController.ActiveCameraDevice.ImageRotation;
          CameraPlane.transform.localScale = new Vector3(CameraImageTexture.width, CameraImageTexture.height, 1);
          CameraPlane.transform.localScale = Vector3.Scale(CameraPlane.transform.localScale, CameraDeviceController.ActiveCameraDevice.ImageScaleFrontFacing);
          CameraPlane.GetComponent<MeshFilter>().mesh = CameraDeviceController.ActiveCameraDevice.ImageMesh;
          CameraPlane.GetComponent<Renderer>().material.mainTexture = CameraImageTexture;

          return true;
        }
      }
    }
  }

  /// \} aruco_unity_package
}