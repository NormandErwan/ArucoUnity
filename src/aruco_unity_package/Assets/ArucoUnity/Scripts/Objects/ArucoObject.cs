using ArucoUnity.Plugin;
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
      [Tooltip("The size of each marker. In pixels for Creators. In meters for Trackers and Calibrators.")]
      private float markerSideLength;

      [SerializeField]
      [Tooltip("Number of bits in marker borders (default: 1). Used by Creators.")]
      private int markerBorderBits;

      // Events

      public delegate void ArucoObjectEventHandler(ArucoObject arucoObject);

      /// <summary>
      /// Executed before a property is going to be updated.
      /// </summary>
      public event ArucoObjectEventHandler PropertyUpdating = delegate { };

      /// <summary>
      /// Executed after a property has been updated.
      /// </summary>
      public event ArucoObjectEventHandler PropertyUpdated = delegate { };

      // Properties

      public int HashCode { get; protected set; }

      /// <summary>
      /// The dictionary to use.
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
      /// The size of each marker. In pixels for Creators. In meters for Trackers and Calibrators.
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
      /// Number of bits in marker borders (default: 1). Used by Creators.
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
      /// Initialize the properties.
      /// </summary>
      protected virtual void Awake()
      {
        if (Dictionary == null)
        {
          dictionary = Aruco.GetPredefinedDictionary(dictionaryName);
        }
        UpdateHashCode();
      }

      // Methods

      /// <summary>
      /// Update the hash code of the object.
      /// </summary>
      protected abstract void UpdateHashCode();

      /// <summary>
      /// Call the event <see cref="PropertyUpdating"/>.
      /// </summary>
      protected virtual void OnPropertyUpdating()
      {
        PropertyUpdating(this);
      }

      /// <summary>
      /// Call the event <see cref="PropertyUpdated"/>.
      /// </summary>
      protected virtual void OnPropertyUpdated()
      {
        UpdateHashCode();
        PropertyUpdated(this);
      }
    }
  }

  /// \} aruco_unity_package
}