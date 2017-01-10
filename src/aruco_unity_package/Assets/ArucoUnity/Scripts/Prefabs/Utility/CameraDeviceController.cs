using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    // TODO: doc
    /// <summary>
    /// Based on: http://answers.unity3d.com/answers/1155328/view.html
    /// </summary>
    public class CameraDeviceController : MonoBehaviour
    {
      // Configuration
      [SerializeField]
      private int defaultCameraDeviceIndex;

      // Properties
      public CameraDevice ActiveCameraDevice { get; private set; }

      // Events
      public delegate void CameraDeviceControllerAction(CameraDevice previousCameraDevice);
      public event CameraDeviceControllerAction OnActiveCameraDeviceChanged;

      void Start()
      {
        ActiveCameraDevice = gameObject.AddComponent<CameraDevice>();
        SwitchCamera(defaultCameraDeviceIndex);
      }

      // Switch between cameras devices
      public void SwitchCamera(int? cameraIndex = null)
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
        defaultCameraDeviceIndex = (cameraIndex != null) ? (int)cameraIndex : defaultCameraDeviceIndex + 1;
        defaultCameraDeviceIndex %= WebCamTexture.devices.Length;

        ActiveCameraDevice.ResetCamera(webcamDevices[defaultCameraDeviceIndex]);
        ActiveCameraDevice.StartCamera();

        // Update the state
        if (OnActiveCameraDeviceChanged != null)
        {
          OnActiveCameraDeviceChanged(previousCameraDevice);
        }
      }
    }
  }
}