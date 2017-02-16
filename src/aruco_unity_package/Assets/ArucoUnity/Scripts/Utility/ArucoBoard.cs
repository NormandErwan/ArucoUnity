using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Describes the shared properties of the ArUco boards.
    /// </summary>
    public abstract class ArucoBoard<T> : ArucoObject where T : Board
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The size of the margins in pixels, used by Creators (default: 0).")]
      private int marginsSize;

      // Properties

      /// <summary>
      /// The size of the margins in pixels (default: 0). Used by the Creators.
      /// </summary>
      public int MarginsSize {
        get { return marginsSize; }
        set
        {
          OnPropertyUpdating();
          marginsSize = value;
          OnPropertyUpdated();
        }
      }

      /// <summary>
      /// The image size for drawing the board.
      /// </summary>
      public Size ImageSize { get; protected set; }

      /// <summary>
      /// The associated board from the ArucoUnity plugin library.
      /// </summary>
      public T Board { get; protected set; }

      // MonoBehaviour methods

      /// <summary>
      /// Initialize the properties and suscribe to <see cref="ArucoObject.PropertyUpdated"/>.
      /// </summary>
      protected override void Awake()
      {
        base.Awake();

        ImageSize = new Size();
        UpdateBoard();

        base.PropertyUpdated += ArucoBoard_PropertyUpdated;
      }

      /// <summary>
      /// Unsuscribe from events.
      /// </summary>
      protected void OnDestroy()
      {
        base.PropertyUpdated -= ArucoBoard_PropertyUpdated;
      }

      // Methods

      /// <summary>
      /// Update the board property.
      /// </summary>
      protected abstract void UpdateBoard();

      private void ArucoBoard_PropertyUpdated(ArucoObject currentArucoObject)
      {
        UpdateBoard();
      }
    }
  }

  /// \} aruco_unity_package
}