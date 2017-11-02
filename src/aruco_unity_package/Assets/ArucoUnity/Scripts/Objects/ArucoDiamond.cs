using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Objects
  {
    /// <summary>
    /// Describes an ChArUco diamond marker.
    /// </summary>
    public class ArucoDiamond : ArucoObject
    {
      // Const

      /// <summary>
      /// A ChArUco diamond marker is composed of four markers.
      /// </summary>
      protected const int IdsLength = 4;

      // Editor fields

      [SerializeField]
      [Tooltip("Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.")]
      private float squareSideLength;

      [SerializeField]
      [Tooltip("The four ids of the four markers of the diamond.")]
      private int[] ids;

      // Properties

      /// <summary>
      /// The side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
      /// </summary>
      public float SquareSideLength
      {
        get { return squareSideLength; }
        set
        {
          OnPropertyUpdating();
          squareSideLength = value;
          OnPropertyUpdated();
        }
      }

      /// <summary>
      /// The four ids of the four markers of the diamond.
      /// </summary>
      public int[] Ids
      {
        get
        {
          if (ids.Length != IdsLength)
          {
            Debug.LogError("Invalid number of Ids: ArucoDiamond requires " + IdsLength  + " ids.");
          }

          return ids;
        }
        set
        {
          if (value.Length != IdsLength)
          {
            Debug.LogError("Invalid number of Ids: ArucoDiamond requires " + IdsLength + " ids.");
            return;
          }

          OnPropertyUpdating();
          ids = value;
          OnPropertyUpdated();
        }
      }

      // ArucoObject methods

      protected override void UpdateArucoHashCode()
      {
        ArucoHashCode = GetArucoHashCode(Ids);
      }

      // Methods

      /// <summary>
      /// Computes the hash code of a ChArUco diamond marker.
      /// </summary>
      /// <param name="ids">The list of ids of the four markers.</param>
      /// <returns>The calculated ArUco hash code.</returns>
      public static int GetArucoHashCode(int[] ids)
      {
        int hashCode = 17;
        hashCode = hashCode * 31 + typeof(ArucoDiamond).GetHashCode();
        foreach (var id in ids)
        {
          hashCode = hashCode * 31 + id;
        }
        return hashCode;
      }

      /// <summary>
      /// Keep the ids array to its fixed size in the editor.
      /// </summary>
      protected override void OnValidate()
      {
        base.OnValidate();
        if (ids.Length != IdsLength)
        {
          Array.Resize(ref ids, IdsLength);
        }
      }
    }
  }

  /// \} aruco_unity_package
}