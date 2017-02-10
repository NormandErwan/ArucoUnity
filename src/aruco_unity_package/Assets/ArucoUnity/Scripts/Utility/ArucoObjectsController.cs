using UnityEngine;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Manages a list of <see cref="ArucoObject"/>.
    /// </summary>
    public abstract class ArucoObjectsController : ArucoObjectDetector
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The list of the used ArUco objects.")]
      private ArucoObject[] arucoObjects;

      // Events

      public delegate void ArucoObjectEventHandler(ArucoObject arucoObject);
      public delegate void DictionaryEventHandler(ArucoUnity.Plugin.Dictionary dictionary);

      /// <summary>
      /// When an ArUco object has been added to the list.
      /// </summary>
      public event ArucoObjectEventHandler ArucoObjectAdded = delegate { };

      /// <summary>
      /// When an ArUco object has been removed to the list.
      /// </summary>
      public event ArucoObjectEventHandler ArucoObjectRemoved = delegate { };

      /// <summary>
      /// When a new dictionary among the ArUco objects has been added.
      /// </summary>
      public event DictionaryEventHandler DictionaryAdded = delegate { };

      /// <summary>
      /// When a new dictionary is no more used by any ArUco objects.
      /// </summary>
      public event DictionaryEventHandler DictionaryRemoved = delegate { };

      // Properties

      /// <summary>
      /// The list of the used ArUco objects.
      /// </summary>
      public Dictionary<ArucoUnity.Plugin.Dictionary, HashSet<ArucoObject>> ArucoObjects { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();

        ArucoObjects = new Dictionary<ArucoUnity.Plugin.Dictionary, HashSet<ArucoObject>>();
      }

      /// <summary>
      /// Add to the <see cref="ArucoObjects"/> list the ArUco objects added from the editor array <see cref="arucoObjects"/>.
      /// </summary>
      protected virtual void Start()
      {
        foreach (ArucoObject arucoObject in arucoObjects)
        {
          Add(arucoObject);
        }
      }

      // Methods

      /// <summary>
      /// Add an ArUco object to the <see cref="ArucoObjects"/> list.
      /// </summary>
      /// <param name="arucoObject">The ArUco object to add.</param>
      public virtual void Add(ArucoObject arucoObject)
      {
        // Try to find a list with the same dictionary than the new ArUco object
        HashSet<ArucoObject> arucoObjectsCollection = null;
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          if (arucoObjectDictionary.Key.name == arucoObject.Dictionary.name || arucoObjectDictionary.Key == arucoObject.Dictionary)
          {
            arucoObjectsCollection = arucoObjectDictionary.Value;
          }
        }

        // If not found, create the new list
        if (arucoObjectsCollection == null)
        {
          ArucoObjects.Add(arucoObject.Dictionary, new HashSet<ArucoObject>());
          arucoObjectsCollection = ArucoObjects[arucoObject.Dictionary];
          DictionaryAdded(arucoObject.Dictionary);
        }

        // Add the ArUco object to the list
        arucoObjectsCollection.Add(arucoObject);
        ArucoObjectAdded(arucoObject);
      }

      /// <summary>
      /// Remove an ArUco object to the <see cref="ArucoObjects"/> list.
      /// </summary>
      /// <param name="arucoObject">The ArUco object to remove.</param>
      public virtual void Remove(ArucoObject arucoObject)
      {
        // Find the list with the same dictionary than the ArUco object to remove
        HashSet<ArucoObject> arucoObjectsCollection = null;
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          if (arucoObjectDictionary.Key.name == arucoObject.Dictionary.name || arucoObjectDictionary.Key == arucoObject.Dictionary)
          {
            arucoObjectsCollection = arucoObjectDictionary.Value;
          }
        }

        // If not found, nothing to remove
        if (arucoObjectsCollection == null)
        {
          return;
        }

        // Remove the ArUco object
        arucoObjectsCollection.Remove(arucoObject);
        ArucoObjectRemoved(arucoObject);

        // If the list is empty, remove it with its dictionary
        if (arucoObjectsCollection.Count == 0)
        {
          ArucoObjects.Remove(arucoObject.Dictionary);
          DictionaryRemoved(arucoObject.Dictionary);
        }
      }
    }
  }

  /// \} aruco_unity_package
}
