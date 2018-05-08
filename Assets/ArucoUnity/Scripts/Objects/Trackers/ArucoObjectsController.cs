using UnityEngine;
using System.Collections.Generic;
using ArucoUnity.Plugin;
using System;

namespace ArucoUnity.Objects.Trackers
{
  /// <summary>
  /// Manages a list of <see cref="ArucoObject"/> to detect for a <see cref="ArucoCamera"/> camera system.
  /// </summary>
  public abstract class ArucoObjectsController : ArucoObjectDetector, IArucoObjectsController
  {
    // Editor fields

    [SerializeField]
    [Tooltip("The list of the ArUco objects to detect.")]
    private ArucoObject[] arucoObjects;

    // IArucoObjectsController events

    public event Action<ArucoObject> ArucoObjectAdded = delegate { };
    public event Action<ArucoObject> ArucoObjectRemoved = delegate { };
    public event Action<Aruco.Dictionary> DictionaryAdded = delegate { };
    public event Action<Aruco.Dictionary> DictionaryRemoved = delegate { };

    // IArucoObjectsController Properties

    public Dictionary<Aruco.Dictionary, Dictionary<int, ArucoObject>> ArucoObjects { get; protected set; }

    // MonoBehaviour methods

    /// <summary>
    /// Initializes the properties.
    /// </summary>
    protected override void Awake()
    {
      base.Awake();
      ArucoObjects = new Dictionary<Aruco.Dictionary, Dictionary<int, ArucoObject>>();
    }

    /// <summary>
    /// Adds to the <see cref="ArucoObjects"/> list the ArUco objects added from the editor field array <see cref="arucoObjects"/>.
    /// </summary>
    protected override void Start()
    {
      foreach (ArucoObject arucoObject in arucoObjects)
      {
        AddArucoObject(arucoObject);
      }
      base.Start();
    }

    // IArucoObjectsController Methods

    public virtual void AddArucoObject(ArucoObject arucoObject)
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

    public virtual void RemoveArucoObject(ArucoObject arucoObject)
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

    public virtual HashSet<U> GetArucoObjects<U>(Aruco.Dictionary dictionary) where U : ArucoObject
    {
      if (!ArucoObjects.ContainsKey(dictionary))
      {
        throw new ArgumentException("This dictionary is not found.", "dictionary");
      }

      HashSet<U> arucoTObjectsCollection = new HashSet<U>();
      foreach (var arucoObject in ArucoObjects[dictionary])
      {
        U arucoTObject = arucoObject.Value as U;
        if (arucoTObject != null)
        {
          arucoTObjectsCollection.Add(arucoTObject);
        }
      }
      return arucoTObjectsCollection;
    }

    // Methods

    /// <summary>
    /// Remove an ArucoObject from the <see cref="ArucoObjects"/> list, before the its properties will be updated.
    /// </summary>
    /// <param name="arucoObject">The updated ArUco object.</param>
    protected virtual void ArucoObject_PropertyUpdating(ArucoObject arucoObject)
    {
      RemoveArucoObject(arucoObject);
    }

    /// <summary>
    /// Re-adds the updated ArUco object the <see cref="ArucoObjects"/> list.
    /// </summary>
    /// <param name="arucoObject">The updated ArUco object.</param>
    protected virtual void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
    {
      AddArucoObject(arucoObject);
    }
  }
}