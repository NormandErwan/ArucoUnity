using ArucoUnity.Plugin;
using UnityEngine;
using ArucoUnity.Objects;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    [ExecuteInEditMode]
    public class ArucoObjectDisplayer : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The ArUco object to display.")]
      private ArucoObject arucoObject;

      [SerializeField]
      [Tooltip("Display the image in at start in play mode?")]
      private bool displayInPlayMode = false;

      // Properties

      /// <summary>
      /// Get or sets the ArUco object to display.
      /// </summary>
      protected ArucoObject ArucoObject { get { return arucoObject; } set { SetArucoObject(value); } }

      /// <summary>
      /// Gets or sets if <see cref="ImagePlane"/> is visible at start in play mode.
      /// </summary>
      protected bool DisplayInPlayMode { get { return displayInPlayMode; } set { displayInPlayMode = value; } }

      /// <summary>
      /// Gets or sets the prefab of <see cref="ImagePlane"/>. If null, default will be loaded: `Prefabs/Resources/ArucoCreatorImagePlane`.
      /// </summary>
      public GameObject ImagePlanePrefab { get; set; }

      /// <summary>
      /// Gets the plane that display <see cref="ImageTexture"/>.
      /// </summary>
      public GameObject ImagePlane { get; protected set; }

      /// <summary>
      /// Gets the image of the <see cref="ArucoObject"/> to display.
      /// </summary>
      public Cv.Mat Image { get; protected set; }

      /// <summary>
      /// Gets the texture that contains <see cref="Image"/>.
      /// </summary>
      public Texture2D ImageTexture { get; protected set; }

      // Variables

#if UNITY_EDITOR
      protected ArucoObject lastArucoObjectOnValidate = null;
#endif
      protected Material imagePlaneMaterial;

      // MonoBehaviour methods

      /// <summary>
      /// Calls <see cref="SetArucoObject"/> to display the <see cref="ArucoObject"/> only in play mode.
      /// </summary>
      protected virtual void Start()
      {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
#endif
          if (DisplayInPlayMode && ArucoObject)
          {
            var currentArucoObject = ArucoObject;
            arucoObject = null;
            SetArucoObject(currentArucoObject);
          }
          else
          {
            enabled = false;
          }
#if UNITY_EDITOR
        }
#endif
      }

      /// <summary>
      /// Updates the display in the Unity Editor if arucoObject has been changed.
      /// </summary>
      protected virtual void Update()
      {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
          if (lastArucoObjectOnValidate != ArucoObject)
          {
            if (ArucoObject != null)
            {
              var currentArucoObject = ArucoObject;
              arucoObject = lastArucoObjectOnValidate;
              SetArucoObject(currentArucoObject);
            }
            else
            {
              Reset();
            }
            lastArucoObjectOnValidate = ArucoObject;
          }
        }
#endif
      }

      /// <summary>
      /// Unsubscribes from the <see cref="ArucoObject.PropertyUpdated"/> event.
      /// </summary>
      protected virtual void OnDestroy()
      {
        if (ArucoObject != null)
        {
          ArucoObject.PropertyUpdated -= ArucoObject_PropertyUpdated;
        }
      }

      /// <summary>
      /// Shows <see cref="ImagePlane"/> and calls <see cref="SetArucoObject"/>.
      /// </summary>
      private void OnEnable()
      {
        InitializeImagePlane();
        ImagePlane.SetActive(true);
      }

      /// <summary>
      /// Hides <see cref="ImagePlane"/>.
      /// </summary>
      private void OnDisable()
      {
        ImagePlane.SetActive(false);

#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
          Reset();
        }
#endif
      }

      // Methods

      /// <summary>
      /// Creates <see cref="Image"/> and <see cref="ImageTexture"/> from <see cref="ArucoObject"/>.
      /// </summary>
      public virtual void Create()
      {
        Image = ArucoObject.Draw();

        if (Image != null)
        {
          // Vertical flip to correctly display the image on the texture
          int verticalFlipCode = 0;
          Cv.Mat imageForTexture = Image.Clone();
          Cv.Flip(imageForTexture, imageForTexture, verticalFlipCode);

          // Load the image to the texture
          int markerDataSize = (int)(Image.ElemSize() * Image.Total());
          ImageTexture = new Texture2D(Image.Cols, Image.Rows, TextureFormat.RGB24, false);
          ImageTexture.LoadRawTextureData(imageForTexture.DataIntPtr, markerDataSize);
          ImageTexture.Apply();
        }
        else
        {
          Reset();
        }
      }

      /// <summary>
      /// Updates <see cref="ImagePlane"/> with <see cref="ImageTexture"/>.
      /// </summary>
      public virtual void Display()
      {
        imagePlaneMaterial.mainTexture = ImageTexture;
      }

      /// <summary>
      /// Resets <see cref="Image"/>, <see cref="ImageTexture"/> and <see cref="ImagePlane"/>.
      /// </summary>
      public virtual void Reset()
      {
        Image = null;
        ImageTexture = null;
        imagePlaneMaterial.mainTexture = null;
      }

      /// <summary>
      /// Subscribes to the <see cref="ArucoObject.PropertyUpdated"/> event, and unsubscribes from the previous ArucoObject.
      /// </summary>
      protected virtual void SetArucoObject(ArucoObject arucoObject)
      {
        if (ArucoObject != null)
        {
          ArucoObject.PropertyUpdated -= ArucoObject_PropertyUpdated;
        }

        this.arucoObject = arucoObject;

        if (ArucoObject != null)
        {
          ArucoObject_PropertyUpdated(ArucoObject);
          ArucoObject.PropertyUpdated += ArucoObject_PropertyUpdated;
        }
      }

      /// <summary>
      /// Initializes <see cref="ImagePlane"/>.
      /// </summary>
      protected virtual void InitializeImagePlane()
      {
        if (ImagePlanePrefab == null)
        {
          ImagePlanePrefab = Resources.Load("ArucoCreatorImagePlane") as GameObject;
        }

        if (ImagePlane == null)
        {
          var imagePlaneTransform = transform.Find(ImagePlanePrefab.name);
          if (imagePlaneTransform != null)
          {
            ImagePlane = imagePlaneTransform.gameObject;
          }
          else
          {
            ImagePlane = Instantiate(ImagePlanePrefab, transform);
            ImagePlane.name = ImagePlanePrefab.name;
            ImagePlane.transform.localPosition = Vector3.zero;
            ImagePlane.transform.forward = -transform.up; // Rotated up
            ImagePlane.transform.localScale = Vector3.one;
          }

#if UNITY_EDITOR
          if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
#else
          if (Application.isEditor)
#endif
          {
            var renderer = ImagePlane.GetComponent<Renderer>();
            imagePlaneMaterial = new Material(renderer.sharedMaterial);
            renderer.sharedMaterial = imagePlaneMaterial;
          }
          else
          {
            imagePlaneMaterial = ImagePlane.GetComponent<Renderer>().material;
          }

          ImagePlane.hideFlags = HideFlags.DontSaveInEditor;
        }
      }

      /// <summary>
      /// Calls <see cref="Create"/> and <see cref="Display"/>.
      /// </summary>
      protected virtual void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
      {
        Create();
        Display();
      }
    }
  }

  /// \} aruco_unity_package
}