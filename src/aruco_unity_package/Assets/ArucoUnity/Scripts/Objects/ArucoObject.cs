using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
  {
    /// <summary>
    /// Describes the shared properties of all the ArUco objects. Trackers, Creators and Calibrators use this interface.
    /// </summary>
    [ExecuteInEditMode]
    public abstract class ArucoObject : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The dictionary to use.")]
      private Aruco.PredefinedDictionaryName dictionaryName;

      [SerializeField]
      [Tooltip("The side length of each marker. In pixels for Creators. In meters for Trackers and Calibrators.")]
      private float markerSideLength;

      [SerializeField]
      [Tooltip("Number of bits in marker borders (default: 1). Used by Creators.")]
      private int markerBorderBits;

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
      public int MarkerBorderBits
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
      /// Adjusts the scale to <see cref="MarkerSideLength"/> length.
      /// </summary>
      protected abstract void AdjustGameObjectScale();

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
      /// Initializes the properties and calls the <see cref="UpdateArucoHashCode"/> and <see cref="AdjustGameObjectScale"/> methods.
      /// </summary>
      protected virtual void UpdateProperties()
      {
        UpdateArucoHashCode();
        AdjustGameObjectScale();
      }
    }
  }

  /// \} aruco_unity_package
}