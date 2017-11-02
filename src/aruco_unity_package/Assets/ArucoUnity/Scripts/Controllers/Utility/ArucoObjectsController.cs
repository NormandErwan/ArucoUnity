using UnityEngine;
using System.Collections.Generic;
using ArucoUnity.Plugin;
using ArucoUnity.Objects;
using System;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.Utility
  {
    /// <summary>
    /// Manages a list of <see cref="ArucoObject"/> to detect for a <see cref="ArucoCamera"/> camera system.
    /// </summary>
    public abstract class ArucoObjectsController : ArucoObjectDetector
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The list of the ArUco objects to detect.")]
      private ArucoObject[] arucoObjects;

      // Events

      /// <summary>
      /// When an ArUco object has been added to the list.
      /// </summary>
      public event Action<ArucoObject> ArucoObjectAdded = delegate { };

      /// <summary>
      /// When an ArUco object has been removed from the list.
      /// </summary>
      public event Action<ArucoObject> ArucoObjectRemoved = delegate { };

      /// <summary>
      /// When a new dictionary among the ArUco objects has been added.
      /// </summary>
      public event Action<Aruco.Dictionary> DictionaryAdded = delegate { };

      /// <summary>
      /// When a new dictionary is no more used by any ArUco objects.
      /// </summary>
      public event Action<Aruco.Dictionary> DictionaryRemoved = delegate { };

      // Properties

      /// <summary>
      /// The list of the ArUco objects to detect.
      /// </summary>
      public Dictionary<Aruco.Dictionary, Dictionary<int, ArucoObject>> ArucoObjects { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();
        ArucoObjects = new Dictionary<Aruco.Dictionary, Dictionary<int, ArucoObject>>();
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
        // Make sure the object is started and initialized
        arucoObject.gameObject.SetActive(true);

        // Try to find a list with the same dictionary than the new ArUco object
        Dictionary<int, ArucoObject> arucoObjectsCollection = null;
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          if (arucoObjectDictionary.Key.Name == arucoObject.Dictionary.Name || arucoObjectDictionary.Key == arucoObject.Dictionary)
          {
            arucoObjectsCollection = arucoObjectDictionary.Value;
            break;
          }
        }

        // If not found, create the new list attached to this dictionary
        if (arucoObjectsCollection == null)
        {
          arucoObjectsCollection = new Dictionary<int, ArucoObject>();
          ArucoObjects.Add(arucoObject.Dictionary, arucoObjectsCollection);
          DictionaryAdded(arucoObject.Dictionary);
        }
        // Return if the ArUco object is already in the list 
        else
        {
          if (arucoObjectsCollection.ContainsKey(arucoObject.ArucoHashCode))
          {
            return;
          }
        }

        // Suscribe to property events on the aruco object
        arucoObject.PropertyUpdating += ArucoObject_PropertyUpdating;
        arucoObject.PropertyUpdated += ArucoObject_PropertyUpdated;

        // Add the ArUco object to the list
        arucoObjectsCollection.Add(arucoObject.ArucoHashCode, arucoObject);
        ArucoObjectAdded(arucoObject);
      }

      /// <summary>
      /// Remove an ArUco object to the <see cref="ArucoObjects"/> list.
      /// </summary>
      /// <param name="arucoObject">The ArUco object to remove.</param>
      public virtual void Remove(ArucoObject arucoObject)
      {
        // Find the list with the same dictionary than the ArUco object to remove
        Dictionary<int, ArucoObject> arucoObjectsCollection = null;
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          if (arucoObjectDictionary.Key.Name == arucoObject.Dictionary.Name || arucoObjectDictionary.Key == arucoObject.Dictionary)
          {
            arucoObjectsCollection = arucoObjectDictionary.Value;
            break;
          }
        }

        if (arucoObjectsCollection == null)
        {
          throw new ArgumentException("Can't remove the ArUco object: not found.", "arucoObject");
        }

        // Remove the ArUco object
        arucoObjectsCollection.Remove(arucoObject.ArucoHashCode);
        ArucoObjectRemoved(arucoObject);

        // Unsuscribe to property events on the aruco object
        arucoObject.PropertyUpdating -= ArucoObject_PropertyUpdating;
        arucoObject.PropertyUpdated -= ArucoObject_PropertyUpdated;

        // If the list is empty, remove it with its dictionary
        if (arucoObjectsCollection.Count == 0)
        {
          ArucoObjects.Remove(arucoObject.Dictionary);
          DictionaryRemoved(arucoObject.Dictionary);
        }
      }

      // TODO: cache the results
      /// <summary>
      /// Return a sublist from <see cref="ArucoObjects"/> of ArUco objects of a precise type <typeparamref name="T"/> in a certain
      /// <paramref name="dictionary"/>.
      /// </summary>
      /// <typeparam name="T">The type of the ArUco objects in the returned sublist.</typeparam>
      /// <param name="dictionary">The <see cref="Aruco.Dictionary" /> to use.</param>
      /// <returns>The sublist.</returns>
      public virtual HashSet<T> GetArucoObjects<T>(Aruco.Dictionary dictionary) where T : ArucoObject
      {
        if (!ArucoObjects.ContainsKey(dictionary))
        {
          throw new ArgumentException("This dictionary is not found.", "dictionary");
        }

        HashSet<T> arucoTObjectsCollection = new HashSet<T>();
        foreach (var arucoObject in ArucoObjects[dictionary])
        {
          T arucoTObject = arucoObject.Value as T;
          if (arucoTObject != null)
          {
            arucoTObjectsCollection.Add(arucoTObject);
          }
        }
        return arucoTObjectsCollection;
      }

      /// <summary>
      /// Before the ArUco object's properties will be updated, remove it from the ArUco objects list.
      /// </summary>
      /// <param name="arucoObject">The updated ArUco object.</param>
      protected virtual void ArucoObject_PropertyUpdating(ArucoObject arucoObject)
      {
        Remove(arucoObject);
      }

      /// <summary>
      /// Add again the updated ArUco object.
      /// </summary>
      /// <param name="arucoObject">The updated ArUco object.</param>
      // TODO: find a more elegant way to adjust the aruco object list from the aruco object's dictionary and hashcode changes.
      protected virtual void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
      {
        Add(arucoObject);
      }
    }
  }

  /// \} aruco_unity_package
}
