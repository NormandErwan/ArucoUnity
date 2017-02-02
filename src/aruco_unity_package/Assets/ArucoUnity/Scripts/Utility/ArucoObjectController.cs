using UnityEngine;
using System.Collections.Generic;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public abstract class ArucoObjectController : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      private float markerSideLength;

      // Events

      public delegate void ArucoObjectEventHandler(ArucoObject arucoObject);

      public event ArucoObjectEventHandler ArucoObjectAdded = delegate { };
      public event ArucoObjectEventHandler ArucoObjectRemoved = delegate { };

      public delegate void DictionaryEventHandler(ArucoUnity.Plugin.Dictionary dictionary);

      public event DictionaryEventHandler DictionaryAdded = delegate { };
      public event DictionaryEventHandler DictionaryRemoved = delegate { };

      // Properties

      public float MarkerSideLength
      {
        get { return markerSideLength; }
        set
        {
          // Restore the previous scale
          if (markerSideLength != 0)
          {
            foreach (var arucoObjectDictionary in ArucoObjects)
            {
              foreach (var arucoObject in arucoObjectDictionary.Value)
              {
                arucoObject.transform.localScale /= markerSideLength;
              }
            }
          }

          // Adjust to the new scale
          markerSideLength = value;
          if (markerSideLength != 0)
          {
            foreach (var arucoObjectDictionary in ArucoObjects)
            {
              foreach (var arucoObject in arucoObjectDictionary.Value)
              {
                arucoObject.transform.localScale *= markerSideLength;
              }
            }
          }
        }
      }

      public Dictionary<ArucoUnity.Plugin.Dictionary, HashSet<ArucoObject>> ArucoObjects { get; protected set; }

      // MonoBehaviour methods

      protected virtual void Awake()
      {
        ArucoObjects = new Dictionary<ArucoUnity.Plugin.Dictionary, HashSet<ArucoObject>>();
      }

      // Methods

      public virtual void Add(ArucoObject arucoObject)
      {
        HashSet<ArucoObject> arucoObjectsCollection = null;
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          if (arucoObjectDictionary.Key.name == arucoObject.Dictionary.name || arucoObjectDictionary.Key == arucoObject.Dictionary)
          {
            arucoObjectsCollection = arucoObjectDictionary.Value;
          }
        }

        if (arucoObjectsCollection == null)
        {
          ArucoObjects.Add(arucoObject.Dictionary, new HashSet<ArucoObject>());
          arucoObjectsCollection = ArucoObjects[arucoObject.Dictionary];
          DictionaryAdded(arucoObject.Dictionary);
        }

        arucoObjectsCollection.Add(arucoObject);
        ArucoObjectAdded(arucoObject);

        if (MarkerSideLength != 0)
        {
          arucoObject.transform.localScale *= MarkerSideLength;
        }
      }

      public virtual void Remove(ArucoObject arucoObject)
      {
        HashSet<ArucoObject> arucoObjectsCollection = null;
        foreach (var arucoObjectDictionary in ArucoObjects)
        {
          if (arucoObjectDictionary.Key.name == arucoObject.Dictionary.name || arucoObjectDictionary.Key == arucoObject.Dictionary)
          {
            arucoObjectsCollection = arucoObjectDictionary.Value;
          }
        }

        arucoObjectsCollection.Remove(arucoObject);
        ArucoObjectRemoved(arucoObject);

        if (arucoObjectsCollection.Count == 0)
        {
          ArucoObjects.Remove(arucoObject.Dictionary);
          DictionaryRemoved(arucoObject.Dictionary);
        }

        if (MarkerSideLength != 0)
        {
          arucoObject.transform.localScale /= MarkerSideLength;
        }
      }
    }
  }

  /// \} aruco_unity_package
}
