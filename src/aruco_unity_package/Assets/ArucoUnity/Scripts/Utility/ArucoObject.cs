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
      public Dictionary Dictionary { get; set; }

      /// <summary>
      /// The size of each marker. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public float MarkerSideLength
      {
        get { return markerSideLength; }
        set
        {
          // Restore the previous scale
          if (markerSideLength != 0)
          {
            gameObject.transform.localScale /= markerSideLength;
          }

          // Adjust to the new scale
          markerSideLength = value;
          if (markerSideLength != 0)
          {
            gameObject.transform.localScale *= markerSideLength;
          }
        }
      }

      public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties, adjust the gameObject localScale to the <see cref="markerSideLength"/> and hide the gameObject.
      /// </summary>
      protected void Awake()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

        if (markerSideLength != 0)
        {
          gameObject.transform.localScale *= markerSideLength;
        }

        gameObject.SetActive(false);
      }
    }
  }

  /// \} aruco_unity_package
}