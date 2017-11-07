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
      // Constants

      protected const float metersToPixels300ppp = 100f * 300f / 2.54f;

      // Editor fields

      [SerializeField]
      [Tooltip("The ArUco object to display.")]
      private ArucoObject arucoObject;

      [SerializeField]
      [Tooltip("Display the image in game.")]
      private bool displayInGame = false;

      // Properties

      /// <summary>
      /// Get or sets the ArUco object to display.
      /// </summary>
      protected ArucoObject ArucoObject { get { return arucoObject; } set { SetArucoObject(value); } }

      protected bool DisplayInGame { get { return displayInGame; } set { displayInGame = value; } }

      /// <summary>
      /// Get or sets the image of the <see cref="ArucoObject"/> to display.
      /// </summary>
      public Cv.Mat Image { get; protected set; }

      /// <summary>
      /// Gets or sets the texture that contains <see cref="Image"/>.
      /// </summary>
      public Texture2D ImageTexture { get; protected set; }

      /// <summary>
      /// Gets or sets the prefab of <see cref="ImagePlane"/>. Default is `ArucoCreatorImagePlane` in 'Prefabs/Resources'.
      /// </summary>
      public GameObject ImagePlanePrefab { get; protected set; }

      /// <summary>
      /// Gets or sets the plane that display <see cref="ImageTexture"/>.
      /// </summary>
      public GameObject ImagePlane { get; protected set; }

      // Variables

#if UNITY_EDITOR
      protected ArucoObject lastArucoObjectOnValidate = null;
#endif
      protected Material imagePlaneMaterial;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes <see cref="ImagePlane"/>.
      /// </summary>
      protected virtual void Awake()
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
            ImagePlane.transform.localRotation = Quaternion.identity;
            ImagePlane.transform.localScale = Vector3.one;
          }

#if UNITY_EDITOR
          if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
          {
            var renderer = ImagePlane.GetComponent<Renderer>();
            imagePlaneMaterial = new Material(renderer.sharedMaterial);
            renderer.sharedMaterial = imagePlaneMaterial;
          }
          else
          {
            imagePlaneMaterial = ImagePlane.GetComponent<Renderer>().material;
          }
#else
          imagePlaneMaterial = imagePlane.GetComponent<Renderer>().material;
#endif

          ImagePlane.hideFlags = HideFlags.DontSaveInEditor;
        }
      }

      /// <summary>
      /// Calls <see cref="SetArucoObject"/> to creates and displays the <see cref="ArucoObject"/>.
      /// </summary>
      protected virtual void Start()
      {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
#endif
          if (DisplayInGame && ArucoObject)
          {
            SetArucoObject(ArucoObject);
          }
          else
          {
            ImagePlane.SetActive(false);
          }
#if UNITY_EDITOR
        }
#endif
      }

      /// <summary>
      /// Updates the display in the Unity Editor.
      /// </summary>
      protected virtual void Update()
      {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
          Awake();

          if (lastArucoObjectOnValidate != ArucoObject)
          {
            var currentArucoObject = ArucoObject;
            arucoObject = lastArucoObjectOnValidate;

            SetArucoObject(currentArucoObject);
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
        if (ArucoObject)
        {
          ArucoObject.PropertyUpdated -= ArucoObject_PropertyUpdated;
        }
      }

      // Methods

      /// <summary>
      /// Creates <see cref="Image"/> and <see cref="ImageTexture"/> from <see cref="ArucoObject"/>.
      /// </summary>
      public virtual void Create()
      {
        Cv.Mat image = null;
        ImageTexture = null;

        // In case of a marker
        ArucoMarker marker = ArucoObject as ArucoMarker;
        if (marker != null && marker.Dictionary != null)
        {
          int markerSideLength = (int)marker.MarkerSideLength; // MarkerSideLength in pixels
          if (marker.MarkerSideLength > 0 && marker.MarkerSideLength < 1) // MarkerSideLength in meters
          {
            markerSideLength = Mathf.RoundToInt(marker.MarkerSideLength * metersToPixels300ppp);
          }
          marker.Dictionary.DrawMarker(marker.MarkerId, markerSideLength, out image, marker.MarkerBorderBits);
        }

        // In case of a grid board
        ArucoGridBoard arucoGridBoard = ArucoObject as ArucoGridBoard;
        if (arucoGridBoard != null)
        {
          Aruco.GridBoard gridBoard = arucoGridBoard.Board as Aruco.GridBoard;
          if (gridBoard != null)
          {
            gridBoard.Draw(arucoGridBoard.ImageSize, out image, arucoGridBoard.MarginsSize, arucoGridBoard.MarkerBorderBits);
          }
        }

        // In case of a charuco board
        ArucoCharucoBoard arucoCharucoBoard = ArucoObject as ArucoCharucoBoard;
        if (arucoCharucoBoard != null)
        {
          Aruco.CharucoBoard charucoBoard = arucoCharucoBoard.Board as Aruco.CharucoBoard;
          if (charucoBoard != null)
          {
            charucoBoard.Draw(arucoCharucoBoard.ImageSize, out image, arucoCharucoBoard.MarginsSize, arucoCharucoBoard.MarkerBorderBits);
          }
        }

        // In case of a diamond
        ArucoDiamond diamond = ArucoObject as ArucoDiamond;
        if (diamond != null && diamond.Ids.Length == 4)
        {
          Cv.Vec4i ids = new Cv.Vec4i();
          for (int i = 0; i < diamond.Ids.Length; ++i)
          {
            ids.Set(i, diamond.Ids[i]);
          }
          Aruco.DrawCharucoDiamond(diamond.Dictionary, ids, (int)diamond.SquareSideLength, (int)diamond.MarkerSideLength, out image);
        }

        // Set the properties
        Image = image;
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
      /// Updates <see cref="ImagePlane"/> with <see cref="ImageTexture"/> if <see cref="DisplayImage"/> is true.
      /// </summary>
      public virtual void Display()
      {
        imagePlaneMaterial.mainTexture = ImageTexture;
        ImagePlane.SetActive(true);
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
          ArucoObject_PropertyUpdated(ArucoObject);
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