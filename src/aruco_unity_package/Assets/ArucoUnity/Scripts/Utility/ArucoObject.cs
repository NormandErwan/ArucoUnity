using ArucoUnity.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Describes the shared properties of all the ArUco objects. Trackers, Creators and Calibrators use this interface.
    /// </summary>
    public abstract class ArucoObject : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The dictionary to use.")]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      [Tooltip("The size of each marker. In pixels for Creators. In meters for Trackers and Calibrators.")]
      private float markerSideLength;

      [SerializeField]
      [Tooltip("Number of bits in marker borders (default: 1). Used by Creators.")]
      private int markerBorderBits;

      // Properties

      /// <summary>
      /// The dictionary to use.
      /// </summary>
      public Dictionary Dictionary
      {
        get { return dictionary; }
        set
        {
          PropertyPreUpdate();
          dictionary = value;
          PropertyUpdated();
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
          PropertyPreUpdate();
          markerSideLength = value;
          PropertyUpdated();
        }
      }

      /// <summary>
      /// Number of bits in marker borders (default: 1). Used by Creators.
      /// </summary>
      public int MarkerBorderBits {
        get { return markerBorderBits; }
        set
        {
          PropertyPreUpdate();
          markerBorderBits = value;
          PropertyUpdated();
        }
      }

      // Variables

      private Dictionary dictionary;

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties, adjust the gameObject localScale to the <see cref="markerSideLength"/> and hide the gameObject.
      /// </summary>
      protected virtual void Awake()
      {
        dictionary = Functions.GetPredefinedDictionary(dictionaryName);

        if (markerSideLength != 0)
        {
          gameObject.transform.localScale *= markerSideLength;
        }

        gameObject.SetActive(false);
      }

      // Methods

      /// <summary>
      /// Called before a property is going to be updated.
      /// </summary>
      protected virtual void PropertyPreUpdate()
      {
        // Restore the previous scale
        if (markerSideLength != 0)
        {
          gameObject.transform.localScale /= markerSideLength;
        }
      }

      /// <summary>
      /// Called after a property has been updated.
      /// </summary>
      protected virtual void PropertyUpdated()
      {
        // Adjust to the new scale
        if (markerSideLength != 0)
        {
          gameObject.transform.localScale *= markerSideLength;
        }
      }
    }
  }

  /// \} aruco_unity_package
}