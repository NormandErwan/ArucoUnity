using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Base for any Aruco detection class.
    /// </summary>
    public abstract class ArucoDetector : MonoBehaviour
    {
      // Events

      public delegate void CameraDeviceMakersDetectorAction();

      /// <summary>
      /// Executed when the detector is ready and configured.
      /// </summary>
      public event CameraDeviceMakersDetectorAction OnConfigurated;

      // Properties

      // State properties
      /// <summary>
      /// True when the detector is ready and configured.
      /// </summary>
      public bool Configured { get; protected set; }

      /// <summary>
      /// The result of the last <see cref="ConfigureCameraPlane"/> call.
      /// </summary>
      public bool CameraPlaneConfigured { get; protected set; }

      // Detection configuration properties
      /// <summary>
      /// The dictionary to use for the detection.
      /// </summary>
      public Dictionary Dictionary { get; set; }

      /// <summary>
      /// The parameters to use for the detection.
      /// </summary>
      public DetectorParameters DetectorParameters { get; set; }

      /// <summary>
      /// The side length of the markers that will be detected (in meters).
      /// </summary>
      public float MarkerSideLength { get; set; }

      // CameraDeviceController related properties
      /// <summary>
      /// The manipulated camera image texture for the detection. You can set your own during Update(), or use a CameraDeviceController.
      /// </summary>
      public Texture2D CameraImageTexture { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public ArucoCamera ArucoCamera
      {
        get { return arucoCameraValue; }
        set
        {
          // Reset configuration
          Configured = false;
          CameraPlaneConfigured = false;

          // Unsubscribe from the previous ArucoCamera
          if (arucoCameraValue != null)
          {
            arucoCameraValue.OnStarted -= Configure;
          }

          // Subscribe to the new ArucoCamera
          arucoCameraValue = value;
          arucoCameraValue.OnStarted += Configure;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configure();
          }
        }
      }

      /// <summary>
      /// The file path to load the camera parameters
      /// </summary>
      public string CameraParametersFilePath { get; set; }

      // Camera properties
      /// <summary>
      /// The Unity camera that will capture the <see cref="CameraPlane"/> display.
      /// </summary>
      public Camera Camera { get; set; }

      /// <summary>
      /// The plane facing the camera that display the <see cref="ArucoDetector.CameraImageTexture"/>.
      /// </summary>
      public GameObject CameraPlane { get; set; }

      /// <summary>
      /// If <see cref="EstimatePose "/> or <see cref="CameraPlaneConfigured"/> is false, the CameraImageTexture will be displayed on this canvas.
      /// </summary>
      public CameraDeviceCanvasDisplay CameraDeviceCanvasDisplay { get; set; }

      // Pose estimation properties
      /// <summary>
      /// Estimate the detected markers pose (position, rotation).
      /// </summary>
      public bool EstimatePose { get; set; }

      public MarkerObjectsController MarkerObjectsController { get; set; }

      // Variables

      private ArucoCamera arucoCameraValue = null;

      // MonoBehaviour methods

      /// <summary>
      /// Subscribe to <see cref="ArucoCamera"/> and execute the configuration if the active camera device has already started.
      /// </summary>
      protected virtual void OnEnable()
      {
        if (ArucoCamera != null)
        {
          ArucoCamera.OnStarted += Configure;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configure();
          }
        }
      }

      /// <summary>
      /// Unsubscribe from <see cref="ArucoCamera"/>.
      /// </summary>
      protected virtual void OnDisable()
      {
        Configured = false;
        CameraPlaneConfigured = false;
        CameraImageTexture = null;

        if (ArucoCamera != null)
        {
          ArucoCamera.OnStarted -= Configure;
        }
      }

      // Methods

      /// <summary>
      /// The configuration content of derived classes.
      /// </summary>
      protected abstract void PreConfigurate();

      /// <summary>
      /// Configure the detection and the results display.
      /// </summary>
      /// <param name="activeCameraDevice">The current active camera device.</param>
      private void Configure()
      {
        // Set properties
        Configured = false;
        CameraPlaneConfigured = false;
        CameraImageTexture = ArucoCamera.Texture2D;

        // Execute the derived classes' configuration
        PreConfigurate();

        // Try to load the camera parameters
        if (EstimatePose)
        {
          bool cameraParametersLoaded = ArucoCamera.LoadCameraParameters(CameraParametersFilePath);
          EstimatePose &= cameraParametersLoaded;
        }

        // Configurate the camera-plane group xor the canvas
        if (EstimatePose)
        {
          ConfigureCameraPlane();
          if (CameraPlaneConfigured)
          {
            MarkerObjectsController.SetCamera(Camera, ArucoCamera.CameraParameters);
            MarkerObjectsController.MarkerSideLength = MarkerSideLength;
          }
        }
        if (CameraPlane != null)
        {
          CameraPlane.gameObject.SetActive(EstimatePose && CameraPlaneConfigured);
        }
        if (CameraDeviceCanvasDisplay != null)
        {
          CameraDeviceCanvasDisplay.gameObject.SetActive(!EstimatePose || !CameraPlaneConfigured);
        }

        // Update the state and notify
        if (OnConfigurated != null)
        {
          OnConfigurated();
        }
        Configured = true;
      }

      /// <summary>
      /// Configurate from the camera parameters the <see cref="Camera"/> and a the <see cref="CameraPlane"/> that display the 
      /// <see cref="CameraImageTexture"/> facing the camera.
      /// </summary>
      /// <returns>If the configuration has been successful.</returns>
      public bool ConfigureCameraPlane()
      {
        if (Camera == null || CameraPlane == null || ArucoCamera.CameraParameters == null)
        {
          Debug.LogError(gameObject.name + ": unable to configure the camera and the facing plane. The following properties must be set: Camera"
            + " and CameraPlane.");
          return CameraPlaneConfigured = false;
        }

        // Configurate the camera according to the camera parameters
        float farClipPlaneNewValueFactor = 1.01f;
        float vFov = 2f * Mathf.Atan(0.5f * CameraImageTexture.height / ArucoCamera.CameraParameters.CameraFy) * Mathf.Rad2Deg;
        Camera.fieldOfView = vFov;
        Camera.farClipPlane = ArucoCamera.CameraParameters.CameraFy * farClipPlaneNewValueFactor;
        Camera.aspect = ArucoCamera.ImageRatio;
        Camera.transform.position = Vector3.zero;
        Camera.transform.rotation = Quaternion.identity;

        // Configurate the plane facing the camera that display the texture
        CameraPlane.transform.position = new Vector3(0, 0, Camera.farClipPlane);
        CameraPlane.transform.rotation = ArucoCamera.ImageRotation;
        CameraPlane.transform.localScale = new Vector3(CameraImageTexture.width, CameraImageTexture.height, 1);
        CameraPlane.transform.localScale = Vector3.Scale(CameraPlane.transform.localScale, ArucoCamera.ImageScaleFrontFacing);
        CameraPlane.GetComponent<MeshFilter>().mesh = ArucoCamera.ImageMesh;
        CameraPlane.GetComponent<Renderer>().material.mainTexture = CameraImageTexture;

        return CameraPlaneConfigured = true;
      }
    }
  }

  /// \} aruco_unity_package
}