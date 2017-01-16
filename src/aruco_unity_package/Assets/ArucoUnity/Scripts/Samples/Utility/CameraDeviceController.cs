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
      /// Manage the available camera devices.
      /// Based on: http://answers.unity3d.com/answers/1155328/view.html
      /// </summary>
      public class CameraDeviceController : MonoBehaviour
      {
        // Configuration
        [SerializeField]
        [Tooltip("The id of the camera device to use.")]
        private int cameraId = 0;

        // Properties
        /// <summary>
        /// The current active camera device.
        /// </summary>
        public CameraDevice ActiveCameraDevice { get; private set; }

        // Events
        public delegate void CameraDeviceControllerAction(CameraDevice previousCameraDevice);
        public event CameraDeviceControllerAction OnActiveCameraDeviceChanged;

        public delegate void ActiveCameraDeviceAction(CameraDevice activeCameraDevice);
        public event ActiveCameraDeviceAction OnActiveCameraDeviceStarted;
        public event ActiveCameraDeviceAction OnActiveCameraDeviceStopped;

        /// <summary>
        /// Initialize the camera device with the index cameraId.
        /// </summary>
        void Start()
        {
          ActiveCameraDevice = gameObject.AddComponent<CameraDevice>();
          SwitchCamera(cameraId);
        }

        /// <summary>
        /// Switch between the different cameras devices.
        /// </summary>
        /// <param name="cameraId">The id of the camera device to use. Set to null to switch to the next camera.</param>
        public void SwitchCamera(int? cameraId = null)
        {
          // Check for camera devices
          WebCamDevice[] webcamDevices = WebCamTexture.devices;
          if (webcamDevices.Length == 0)
          {
            Debug.LogError(gameObject.name + ": No devices cameras found.");
          }

          // Stop the previous camera device
          CameraDevice previousCameraDevice = ActiveCameraDevice;
          if (previousCameraDevice != null)
          {
            previousCameraDevice.StopCamera();
          }

          // Switch to the next camera device
          this.cameraId = (cameraId != null) ? (int)cameraId : this.cameraId + 1;
          this.cameraId %= WebCamTexture.devices.Length;

          ActiveCameraDevice.ResetCamera(webcamDevices[this.cameraId]);
          ActiveCameraDevice.StartCamera();

          // Subscribe to the active camera device
          if (previousCameraDevice != null)
          {
            previousCameraDevice.OnStarted -= ActiveCameraDevice_OnStarted;
            previousCameraDevice.OnStopped -= ActiveCameraDevice_OnStopped;
          }
          ActiveCameraDevice.OnStarted += ActiveCameraDevice_OnStarted;
          ActiveCameraDevice.OnStopped += ActiveCameraDevice_OnStopped;

          // Notify about the camera device change
          if (OnActiveCameraDeviceChanged != null)
          {
            OnActiveCameraDeviceChanged(previousCameraDevice);
          }
        }

        /// <summary>
        /// Notify that the active camera has started.
        /// </summary>
        private void ActiveCameraDevice_OnStarted()
        {
          if (OnActiveCameraDeviceStarted != null)
          {
            OnActiveCameraDeviceStarted(ActiveCameraDevice);
          }
        }

        /// <summary>
        /// Notify that the active camera has stopped.
        /// </summary>
        private void ActiveCameraDevice_OnStopped()
        {
          if (OnActiveCameraDeviceStopped != null)
          {
            OnActiveCameraDeviceStopped(ActiveCameraDevice);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}