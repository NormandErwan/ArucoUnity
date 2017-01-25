using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public class ArucoObject : MonoBehaviour
    {
      // Editor fields
      [SerializeField]
      [Tooltip("")]
      private ArucoObjectController arucoObjectController;

      // Properties

      public ArucoObjectController ArucoObjectController
      {
        get { return arucoObjectController; }
        set
        {
          ArucoObjectController previousArucoObjectController = arucoObjectController;
          arucoObjectController = value;

          if (previousArucoObjectController != null)
          {
            previousArucoObjectController.Remove(this);
          }
          if (arucoObjectController != null)
          {
            ArucoObjectController.Add(this);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}