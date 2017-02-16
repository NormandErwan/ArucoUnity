using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  /// <summary>
  /// Describes an ChArUco diamond marker.
  /// </summary>
  public class ArucoDiamond : ArucoObject
  {
    // Editor fields

    [SerializeField]
    [Tooltip("Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.")]
    private int squareSideLength;

    [SerializeField]
    [Tooltip("The four ids of the four markers of the diamond.")]
    private int[] ids;

    // Properties

    /// <summary>
    /// Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
    /// </summary>
    public int SquareSideLength
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
        if (ids.Length != 4)
        {
          Debug.LogError("Invalid number of Ids: ArucoDiamond requires 4 ids.");
        }

        return ids;
      }
      set
      {
        OnPropertyUpdating();
        ids = value;
        OnPropertyUpdated();
      }
    }

    public VectorVectorPoint2f DetectedCorners { get; set; }

    public VectorVec4i DetectedIds { get; set; }

    public VectorVec3d Rvecs { get; set; }

    public VectorVec3d Tvecs { get; set; }

    // MonoBehaviour methods

    protected override void Awake()
    {
      base.Awake();

      DetectedCorners = null;
      DetectedIds = null;
    }
  }

  /// \} aruco_unity_package
}