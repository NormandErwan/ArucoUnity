using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity.Objects
{
  /// <summary>
  /// Describes the shared properties of all the ArUco objects. Trackers, Creators and Calibrators use this interface.
  /// </summary>
  [ExecuteInEditMode]
  public abstract class ArucoObject : MonoBehaviour
  {
    // Constants

    protected const float metersToPixels300ppp = 100f * 300f / 2.54f;

    // Editor fields

    [SerializeField]
    [Tooltip("The dictionary to use.")]
    private Aruco.PredefinedDictionaryName dictionaryName;

    [SerializeField]
    [Tooltip("The side length of each marker. In pixels for Creators. In meters for Trackers and Calibrators.")]
    private float markerSideLength;

    [SerializeField]
    [Tooltip("Number of bits in marker borders (default: 1). Used by Creators.")]
    private uint markerBorderBits;

    private bool displayInEditor = true;

    // Events

    /// <summary>
    /// Executed before a property is going to be updated.
    /// </summary>
    public event Action<ArucoObject> PropertyUpdating = delegate { };

    /// <summary>
    /// Executed after a property has been updated.
    /// </summary>
    public event Action<ArucoObject> PropertyUpdated = delegate { };

    // Properties

    public int ArucoHashCode { get; protected set; }

    /// <summary>
    /// Gets or sets the dictionary to use.
    /// </summary>
    public Aruco.Dictionary Dictionary
    {
      get { return dictionary; }
      set
      {
        OnPropertyUpdating();
        dictionary = value;
        OnPropertyUpdated();
      }
    }

    /// <summary>
    /// Gets or sets the side length of each marker. In pixels for Creators. In meters for Trackers and Calibrators.
    /// </summary>
    public float MarkerSideLength
    {
      get { return markerSideLength; }
      set
      {
        OnPropertyUpdating();
        markerSideLength = value;
        OnPropertyUpdated();
      }
    }

    /// <summary>
    /// Gets or sets the number of bits in marker borders (default: 1). Used by Creators.
    /// </summary>
    public uint MarkerBorderBits
    {
      get { return markerBorderBits; }
      set
      {
        OnPropertyUpdating();
        markerBorderBits = value;
        OnPropertyUpdated();
      }
    }

    public bool DisplayInEditor { get { return displayInEditor; } set { displayInEditor = value; } }

    // Variables

    private Aruco.Dictionary dictionary;

    // MonoBehaviour methods

    /// <summary>
    /// Calls <see cref="UpdateProperties()"/>.
    /// </summary>
    protected virtual void Awake()
    {
      if (Dictionary == null)
      {
        dictionary = Aruco.GetPredefinedDictionary(dictionaryName);
      }
      UpdateProperties();
    }

    /// <summary>
    /// Calls <see cref="OnPropertyUpdated()"/> in editor mode.
    /// </summary>
    protected virtual void OnValidate()
    {
#if UNITY_EDITOR
      if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
      {
        if (Dictionary == null || dictionaryName != Dictionary.Name)
        {
          dictionary = Aruco.GetPredefinedDictionary(dictionaryName);
        }
        OnPropertyUpdated();
      }
#endif
    }

    // Methods

    /// <summary>
    /// Returns the image of the ArUco object. In editor it should returns null and no exception if the object is incorrectly configured.
    /// </summary>
    /// <returns>The image of the ArUco object.</returns>
    public abstract Cv.Mat Draw();

    /// <summary>
    /// Returns a generated name depending on the value of the properties.
    /// </summary>
    public abstract string GenerateName();

    /// <summary>
    /// Gets the scale to <see cref="MarkerSideLength"/> length.
    /// </summary>
    public abstract Vector3 GetGameObjectScale();

    /// <summary>
    /// Updates the ArUco hash code of the object.
    /// </summary>
    protected abstract void UpdateArucoHashCode();

    /// <summary>
    /// Calls the event <see cref="PropertyUpdating"/>.
    /// </summary>
    protected void OnPropertyUpdating()
    {
      PropertyUpdating.Invoke(this);
    }

    /// <summary>
    /// Calls <see cref="UpdateProperties"/> and the <see cref="PropertyUpdated"/> event.
    /// </summary>
    protected void OnPropertyUpdated()
    {
      UpdateProperties();
      PropertyUpdated.Invoke(this);
    }

    /// <summary>
    /// Initializes the properties and calls the <see cref="UpdateArucoHashCode"/> method.
    /// </summary>
    protected virtual void UpdateProperties()
    {
      UpdateArucoHashCode();
    }

    /// <summary>
    /// Returns a value as an int, with a conversion from meters to pixels if the property value is less than 10.
    /// </summary>
    protected int GetInPixels(float propertyValue)
    {
      int propertyValueInt = (int)propertyValue;
      if (propertyValue > 0 && propertyValue < 10)
      {
        propertyValueInt = Mathf.RoundToInt(propertyValue * metersToPixels300ppp);
      }
      return propertyValueInt;
    }
  }
}