using ArucoUnity.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public abstract class ArucoObject : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      private ArucoObjectController[] arucoObjectControllers;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      public int marginsSize;

      [SerializeField]
      public int markerBorderBits;

      // Properties

      public HashSet<ArucoObjectController> ArucoObjectControllers { get; set; }

      public Dictionary Dictionary { get; set; }

      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }

      public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize properties.
      /// </summary>
      protected void Awake()
      {
        ArucoObjectControllers = new HashSet<ArucoObjectController>(arucoObjectControllers);
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
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