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
      /// Executed when the detector is ready and configurated.
      /// </summary>
      public event CameraDeviceMakersDetectorAction OnConfigurated;

      // Properties

      // State properties
      /// <summary>
      /// True when the detector is ready and configurated.
      /// </summary>
      public bool Configurated { get; protected set; }

      /// <summary>
      /// The result of the last <see cref="CameraPlaneConfigurated"/> call.
      /// </summary>
      public bool CameraPlaneConfigurated { get; protected set; }

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
          Configurated = false;
          CameraPlaneConfigurated = false;

          // Unsubscribe from the previous ArucoCamera
          if (arucoCameraValue != null)
          {
            arucoCameraValue.OnStarted -= Configurate;
          }

          // Subscribe to the new ArucoCamera
          arucoCameraValue = value;
          arucoCameraValue.OnStarted += Configurate;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configurate();
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
      /// If <see cref="EstimatePose "/> or <see cref="CameraPlaneConfigurated"/> is false, the CameraImageTexture will be displayed on this canvas.
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
          ArucoCamera.OnStarted += Configurate;
          if (ArucoCamera != null && ArucoCamera.Started)
          {
            Configurate();
          }
        }
      }

      /// <summary>
      /// Unsubscribe from <see cref="ArucoCamera"/>.
      /// </summary>
      protected virtual void OnDisable()
      {
        Configurated = false;
        CameraPlaneConfigurated = false;
        CameraImageTexture = null;

        if (ArucoCamera != null)
        {
          ArucoCamera.OnStarted -= Configurate;
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
      private void Configurate()
      {
        // Set properties
        Configurated = false;
        CameraPlaneConfigurated = false;
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
          ConfigurateCameraPlane();
          if (CameraPlaneConfigurated)
          {
            MarkerObjectsController.SetCamera(Camera, ArucoCamera.CameraParameters);
            MarkerObjectsController.MarkerSideLength = MarkerSideLength;
          }
        }
        if (CameraPlane != null)
        {
          CameraPlane.gameObject.SetActive(EstimatePose && CameraPlaneConfigurated);
        }
        if (CameraDeviceCanvasDisplay != null)
        {
          CameraDeviceCanvasDisplay.gameObject.SetActive(!EstimatePose || !CameraPlaneConfigurated);
        }

        // Update the state and notify
        if (OnConfigurated != null)
        {
          OnConfigurated();
        }
        Configurated = true;
      }

      /// <summary>
      /// Configurate from the camera parameters the <see cref="Camera"/> and a the <see cref="CameraPlane"/> that display the 
      /// <see cref="CameraImageTexture"/> facing the camera.
      /// </summary>
      /// <returns>If the configuration has been successful.</returns>
      public bool ConfigurateCameraPlane()
      {
        if (Camera == null || CameraPlane == null || ArucoCamera.CameraParameters == null)
        {
          Debug.LogError(gameObject.name + ": unable to configurate the camera and the facing plane. The following properties must be set: Camera"
            + " and CameraPlane.");
          return CameraPlaneConfigurated = false;
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

        return CameraPlaneConfigurated = true;
      }
    }
  }

  /// \} aruco_unity_package
}