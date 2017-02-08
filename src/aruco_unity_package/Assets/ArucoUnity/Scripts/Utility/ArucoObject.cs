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
      private ArucoObjectController[] arucoObjectControllers;

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

      public HashSet<ArucoObjectController> ArucoObjectControllers { get; set; }

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
      /// Initialize the properties and adjust the gameOject localScale to the <see cref="markerSideLength"/>.
      /// </summary>
      protected void Awake()
      {
        ArucoObjectControllers = new HashSet<ArucoObjectController>(arucoObjectControllers);
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);

        if (markerSideLength != 0)
        {
          gameObject.transform.localScale *= markerSideLength;
        }
      }

      /// <summary>
      /// Hide at start, until it will be used by a <see cref="ArucoObjectControllers"/>.
      /// </summary>
      protected void Start()
      {
        gameObject.SetActive(false);

        foreach (var arucoObjectController in ArucoObjectControllers)
        {
          arucoObjectController.Add(this);
        }
      }

      // Methods

      public void AddController(ArucoObjectController arucoObjectController)
      {
        if (ArucoObjectControllers.Add(arucoObjectController))
        {
          arucoObjectController.Add(this);
        }
      }

      public void RemoveController(ArucoObjectController arucoObjectController)
      {
        if (ArucoObjectControllers.Remove(arucoObjectController))
        {
          arucoObjectController.Remove(this);
        }
      }
    }
  }

  /// \} aruco_unity_package
}