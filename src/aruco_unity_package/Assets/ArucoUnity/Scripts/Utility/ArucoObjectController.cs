using UnityEngine;
using System.Collections.Generic;
using ArucoUnity.Plugin;

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
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      private float markerSideLength;

      // Properties

      public Dictionary Dictionary { get; set; } // TODO : can be moved to ArucoObject without impacting to much the performances?

      public float MarkerSideLength
      {
        get { return markerSideLength; }
        set
        {
          // Restore the previous scale
          if (markerSideLength != 0)
          {
            foreach (var arucoObject in ArucoObjects)
            {
              arucoObject.transform.localScale /= markerSideLength;
            }
          }

          // Adjust to the new scale
          markerSideLength = value;
          if (markerSideLength != 0)
          {
            foreach (var arucoObject in ArucoObjects)
            {
              arucoObject.transform.localScale *= markerSideLength;
            }
          }
        }
      }

      public HashSet<ArucoObject> ArucoObjects { get; protected set; }

      // MonoBehaviour methods

      protected virtual void Start()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
        ArucoObjects = new HashSet<ArucoObject>();
      }

      // Methods

      public void Add(ArucoObject arucoObject)
      {
        ArucoObjects.Add(arucoObject);

        if (MarkerSideLength != 0)
        {
          arucoObject.transform.localScale *= MarkerSideLength;
        }
      }

      public void Remove(ArucoObject arucoObject)
      {
        ArucoObjects.Remove(arucoObject);

        if (MarkerSideLength != 0)
        {
          arucoObject.transform.localScale /= MarkerSideLength;
        }
      }
    }
  }

  /// \} aruco_unity_package
}
