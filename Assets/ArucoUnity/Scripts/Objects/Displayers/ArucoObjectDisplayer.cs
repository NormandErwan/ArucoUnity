using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity.Objects.Displayers
{
  [ExecuteInEditMode]
  public class ArucoObjectDisplayer : MonoBehaviour
  {
    // Editor fields

    [SerializeField]
    [Tooltip("The ArUco object to display.")]
    private ArucoObject arucoObject;

    [SerializeField]
    [Tooltip("Display the image in play mode.")]
    private bool displayInPlayMode = false;

    // Properties

    /// <summary>
    /// Get or sets the ArUco object to display.
    /// </summary>
    protected ArucoObject ArucoObject { get { return arucoObject; } set { SetArucoObject(value); } }

    /// <summary>
    /// Gets or sets if <see cref="ImagePlane"/> is displayed in play mode.
    /// </summary>
    protected bool DisplayInPlayMode { get { return displayInPlayMode; } set { displayInPlayMode = value; } }

    /// <summary>
    /// Gets or sets the prefab of <see cref="ImagePlane"/>. If null, default will be loaded:
    /// `Prefabs/Resources/ArucoCreatorImagePlane.prefab`.
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
          UpdateImage();
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
    /// Updates the display in the editor if <see cref="ArucoObject"/> has been changed.
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
            ResetImage();
          }
          lastArucoObjectOnValidate = ArucoObject;
        }

        // The Aruco Object may initialize after the displayer, so we can display the image the frame after
        if (ArucoObject != null && Image == null)
        {
          UpdateImage();
        }

        // Keep the image plane at the same position
        if (ImagePlane != null)
        {
          PlaceImagePlane();
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
    /// Calls <see cref="ResetImage"/> in the editor.
    /// </summary>
    private void OnDisable()
    {
#if UNITY_EDITOR
      if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
      {
        ResetImage();
      }
#endif
    }

    // Methods

    /// <summary>
    /// Creates <see cref="Image"/> and <see cref="ImageTexture"/> from <see cref="ArucoObject"/>.
    /// </summary>
    public virtual void CreateImage()
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
    }

    /// <summary>
    /// Updates <see cref="ImagePlane"/> with <see cref="ImageTexture"/>.
    /// </summary>
    public virtual void DisplayImage()
    {
      InitializeImagePlane();
      PlaceImagePlane();

      imagePlaneMaterial.mainTexture = ImageTexture;
      ImagePlane.SetActive(true);
    }

    /// <summary>
    /// Resets <see cref="Image"/>, <see cref="ImageTexture"/> and <see cref="ImagePlane"/>.
    /// </summary>
    public virtual void ResetImage()
    {
      Image = null;
      ImageTexture = null;
      if (imagePlaneMaterial != null)
      {
        imagePlaneMaterial.mainTexture = null;
      }
      if (ImagePlane != null)
      {
        ImagePlane.SetActive(false);
      }
    }

    /// <summary>
    /// Calls <see cref="CreateImage"/> then <see cref="DisplayImage"/> if <see cref="Image"/> has been created or
    /// <see cref="ResetImage"/>.
    /// </summary>
    protected virtual void UpdateImage()
    {
      CreateImage();
      if (Image != null)
      {
        DisplayImage();
      }
      else
      {
        ResetImage();
      }
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
        ArucoObject.PropertyUpdated += ArucoObject_PropertyUpdated;
      }
    }

    /// <summary>
    /// Initializes <see cref="ImagePlane"/>.
    /// </summary>
    protected virtual void InitializeImagePlane()
    {
      // Loads the prefab
      if (ImagePlanePrefab == null)
      {
        ImagePlanePrefab = Resources.Load("ArucoObjectDisplayerImagePlane") as GameObject;
      }

      // Creates the image plane if null
      if (ImagePlane == null)
      {
        // Finds or creates the image plane gameObject
        var imagePlaneTransform = transform.Find(ImagePlanePrefab.name);
        if (imagePlaneTransform != null)
        {
          ImagePlane = imagePlaneTransform.gameObject;
        }
        else
        {
          ImagePlane = Instantiate(ImagePlanePrefab, transform);
          ImagePlane.name = ImagePlanePrefab.name;
        }

        // Updates the image plane material
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

        // Don't save in the scene : dynamically generated
        ImagePlane.hideFlags = HideFlags.DontSaveInEditor;
      }
    }

    /// <summary>
    /// Places, rotates and scales the image plane.
    /// </summary>
    protected virtual void PlaceImagePlane()
    {
      if (ArucoObject != null)
      {
        var scale = ArucoObject.GetGameObjectScale();

        ImagePlane.transform.SetParent(null);
        ImagePlane.transform.localScale = new Vector3(scale.x, scale.z, scale.y); // Because it's rotated up
        ImagePlane.transform.SetParent(transform);
      }
      ImagePlane.transform.localPosition = Vector3.zero;
      ImagePlane.transform.forward = -transform.up; // Rotated up
    }

    /// <summary>
    /// Calls <see cref="UpdateImage"/>.
    /// </summary>
    protected virtual void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
    {
      UpdateImage();
    }
  }
}