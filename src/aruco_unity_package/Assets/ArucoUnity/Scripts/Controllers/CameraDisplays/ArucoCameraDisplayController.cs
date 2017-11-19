using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraUndistortions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraDisplays
  {
    /// <summary>
    /// Creates <see cref="ArucoCameraDisplay"/> for each <see cref="ArucoCamera.CameraNumber"/>.
    /// </summary>
    [ExecuteInEditMode]
    public class ArucoCameraDisplayController : ArucoCameraController
    {
      // Constants

      protected const float cameraBackgroundDistance = 1f;

      // Editor fields

      [SerializeField]
      [Tooltip("The prefab to display each camera in ArucoCamera. If null, default will be loaded" +
        " `Assets/ArucoUnity/Prefabs/Resources/ArucoCameraDisplay.prefab`.")]
      private ArucoCameraDisplay arucoCameraDisplayPrefab;

      [SerializeField]
      [Tooltip("Optional undistortion process associated with the ArucoCamera.")]
      private ArucoCameraUndistortion arucoCameraUndistortion;

      // Properties

      /// <summary>
      /// Gets or sets the prefab to display each camera in ArucoCamera. If null, default will be loaded
      /// `Assets/ArucoUnity/Prefabs/Resources/ArucoCameraDisplay.prefab`.
      /// </summary>
      public ArucoCameraDisplay ArucoCameraDisplayPrefab { get { return arucoCameraDisplayPrefab; } set { arucoCameraDisplayPrefab = value; } }

      /// <summary>
      /// Gets the list of display for each camera in ArucoCamera.
      /// </summary>
      public ArucoCameraDisplay[] ArucoCameraDisplays { get; protected set; }

      /// <summary>
      /// Gets or sets the optional undistortion process associated with the ArucoCamera.
      /// </summary>
      public ArucoCameraUndistortion ArucoCameraUndistortion { get { return arucoCameraUndistortion; } set { arucoCameraUndistortion = value; } }

      // MonoBehaviour methods

      /// <summary>
      /// In edit mode, calls <see cref="Configure"/> is ArucoCamera is set, otherwise clear <see cref="ArucoCameraDisplays"/>.
      /// </summary>
      protected virtual void Update()
      {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
          if (ArucoCamera != null)
          {
            Configure();
          }
          else if (ArucoCameraDisplays != null)
          {
            foreach (var arucoCameraDisplay in ArucoCameraDisplays)
            {
              DestroyImmediate(arucoCameraDisplay.gameObject);
            }
            ArucoCameraDisplays = null;
          }
        }
#endif
      }

      // ArucoCameraController methods

      /// <summary>
      /// Configures and activates the <see cref="ArucoCameraDisplays"/>.
      /// </summary>
      public override void StartController()
      {
        base.StartController();

        for (int cameraId = 0; cameraId < ArucoCameraDisplays.Length; cameraId++)
        {
          if (ArucoCameraUndistortion != null)
          {
            ArucoCameraDisplays[cameraId].Configure(cameraId, ArucoCamera, ArucoCameraUndistortion.CameraParametersController.CameraParameters,
              ArucoCameraUndistortion.RectifiedCameraMatrices);
          }
          else
          {
            ArucoCameraDisplays[cameraId].Configure(cameraId, ArucoCamera);
          }
        }
        
        ArucoCameraDisplaysSetActive(true);
      }

      /// <summary>
      /// Deactivates the <see cref="ArucoCameraDisplays"/>s.
      /// </summary>
      public override void StopController()
      {
        base.StopController();
        ArucoCameraDisplaysSetActive(false);
      }

      /// <summary>
      /// Loads <see cref="ArucoCameraDisplayPrefab"/> and instantiate from it displays for <see cref="ArucoCameraDisplays"/>.
      /// </summary>
      protected override void Configure()
      {
        // Load the default camera display prefab if not set
        if (ArucoCameraDisplayPrefab == null)
        {
#if UNITY_EDITOR
          if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
          {
#endif
            ArucoCameraDisplayPrefab = Resources.Load("ArucoCameraDisplay") as ArucoCameraDisplay;
#if UNITY_EDITOR
          }
          else
          {
            var arucoCameraDisplayPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ArucoUnity/Prefabs/Resources/ArucoCameraDisplay.prefab");
            ArucoCameraDisplayPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<ArucoCameraDisplay>("Assets/ArucoUnity/Prefabs/Resources/ArucoCameraDisplay.prefab");
          }
#endif
        }

        // Adjust the number of camera display
        if (ArucoCameraDisplays == null)
        {
          ArucoCameraDisplays = GetComponentsInChildren<ArucoCameraDisplay>();
        }
        var arucoCameraDisplaysNotNull = new List<ArucoCameraDisplay>(ArucoCameraDisplays).Where(x => x != null).ToArray(); // In edit mode, if the user has deleted one camera
        var arucoCameraDisplays = new Stack<ArucoCameraDisplay>(arucoCameraDisplaysNotNull);

        if (arucoCameraDisplays.Count != ArucoCamera.CameraNumber)
        {
          // Add camera displays if missing
          while (arucoCameraDisplays.Count < ArucoCamera.CameraNumber)
          {
            ArucoCameraDisplay arucoCameraDisplay;
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
#endif
              arucoCameraDisplay = Instantiate(ArucoCameraDisplayPrefab, transform);

#if UNITY_EDITOR
            }
            else
            {
              arucoCameraDisplay = UnityEditor.PrefabUtility.InstantiatePrefab(ArucoCameraDisplayPrefab) as ArucoCameraDisplay;
              arucoCameraDisplay.transform.SetParent(transform);
            }
#endif
            int cameraId = arucoCameraDisplays.Count;
            arucoCameraDisplay.name += cameraId;

            arucoCameraDisplay.transform.localPosition = Vector3.zero;
            arucoCameraDisplay.transform.localRotation = Quaternion.identity;
            arucoCameraDisplay.transform.localScale = Vector3.one;

            arucoCameraDisplays.Push(arucoCameraDisplay);
          }

          // Remove camera displays if there is more than the cameras in ArucoCamera
          while (arucoCameraDisplays.Count > ArucoCamera.CameraNumber)
          {
            var arucoCameraDisplay = arucoCameraDisplays.Pop();
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
#endif
              Destroy(arucoCameraDisplay.gameObject);
#if UNITY_EDITOR
            }
            else
            {
              DestroyImmediate(arucoCameraDisplay.gameObject);
            }
#endif
          }
        }
        ArucoCameraDisplays = arucoCameraDisplays.ToArray();

        // Activates the camera displays in edit mode, deactivates them in playing mode
        bool activate = false;
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
          activate = true;
        }
#endif
        ArucoCameraDisplaysSetActive(activate);
      }

      /// <summary>
      /// Activates/deactivates each camera display in <see cref="ArucoCameraDisplays"/>.
      /// </summary>
      protected void ArucoCameraDisplaysSetActive(bool value)
      {
        foreach (var arucoCameraDisplay in ArucoCameraDisplays)
        {
          arucoCameraDisplay.gameObject.SetActive(value);
        }
      }
    }
  }

  /// \} aruco_unity_package
}
