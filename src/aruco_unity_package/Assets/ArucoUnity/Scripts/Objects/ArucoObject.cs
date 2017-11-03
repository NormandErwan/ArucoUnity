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

      // Variables

      private Aruco.Dictionary dictionary;

      // MonoBehaviour methods

      /// <summary>
      /// Initializes the properties.
      /// </summary>
      protected virtual void Awake()
      {
        if (Dictionary == null)
        {
          dictionary = Aruco.GetPredefinedDictionary(dictionaryName);
        }
        UpdateArucoHashCode();
      }

      /// <summary>
      /// Calls <see cref="OnPropertyUpdated()"/> when an editor field is updated.
      /// </summary>
      protected virtual void OnValidate()
      {
        OnPropertyUpdated();
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
      protected virtual void OnPropertyUpdating()
      {
        PropertyUpdating.Invoke(this);
      }

      /// <summary>
      /// Calls the <see cref="UpdateArucoHashCode"/> method and the <see cref="PropertyUpdated"/> event.
      /// </summary>
      protected virtual void OnPropertyUpdated()
      {
        UpdateArucoHashCode();
        AdjustGameObjectScale();
        PropertyUpdated.Invoke(this);
      }
    }
  }

  /// \} aruco_unity_package
}