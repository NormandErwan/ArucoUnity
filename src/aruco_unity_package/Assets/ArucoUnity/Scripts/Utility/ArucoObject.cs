using ArucoUnity.Plugin;
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
      private ArucoObjectController arucoObjectController;

      [SerializeField]
      private PREDEFINED_DICTIONARY_NAME dictionaryName;

      [SerializeField]
      public int marginsSize;

      [SerializeField]
      public int markerBorderBits;

      // Properties

      public ArucoObjectController ArucoObjectController // TODO: multiple controllers?
      {
        get { return arucoObjectController; }
        set
        {
          // Remove the previous controller
          if (arucoObjectController != null)
          {
            arucoObjectController.Remove(this);
          }

          // Add the new controller
          arucoObjectController = value;
          if (arucoObjectController != null)
          {
            arucoObjectController.Add(this);
          }
        }
      }

      public Dictionary Dictionary { get; set; }

      public int MarginsSize { get { return marginsSize; } set { marginsSize = value; } }

      public int MarkerBorderBits { get { return markerBorderBits; } set { markerBorderBits = value; } }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize properties.
      /// </summary>
      protected void Awake()
      {
        Dictionary = Functions.GetPredefinedDictionary(dictionaryName);
      }

      /// <summary>
      /// Hide at start, until it will be used by a <see cref="ArucoObjectController"/>.
      /// </summary>
      protected void Start()
      {
        gameObject.SetActive(false);

        if (ArucoObjectController != null)
        {
          ArucoObjectController.Add(this);
        }
      }
    }
  }

  /// \} aruco_unity_package
}