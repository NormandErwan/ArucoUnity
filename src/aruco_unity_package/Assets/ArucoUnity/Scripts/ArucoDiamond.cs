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
    private float squareSideLength;

    [SerializeField]
    [Tooltip("The four ids of the four markers of the diamond.")]
    private int[] ids;

    // Properties

    /// <summary>
    /// Side length of each square. In pixels for Creators. In meters for Trackers and Calibrators.
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

    public float AxisLength { get; protected set; }

    public VectorVectorPoint2f DetectedCorners { get; set; }

    public VectorVec4i DetectedIds { get; set; }

    public int DetectedMarkers { get; set; }

    public VectorVec3d Rvecs { get; set; }

    public VectorVec3d Tvecs { get; set; }

    // MonoBehaviour methods

    /// <summary>
    /// Initialize the properties and suscribe to <see cref="ArucoObject.PropertyUpdated"/>.
    protected override void Awake()
    {
      base.Awake();

      DetectedCorners = null;
      DetectedIds = null;

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
    /// Update the properties.
    /// </summary>
    private void ArucoBoard_PropertyUpdated(ArucoObject currentArucoObject)
    {
      AxisLength = SquareSideLength * 0.5f;
    }
  }

  /// \} aruco_unity_package
}