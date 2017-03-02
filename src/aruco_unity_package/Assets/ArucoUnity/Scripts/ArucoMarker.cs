using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  /// <summary>
  /// Describes an ArUco marker.
  /// </summary>
  public class ArucoMarker : ArucoObject
  {
    // Editor fields

    [SerializeField]
    [Tooltip("The marker id in the used dictionary.")]
    private int markerId;

    // Properties

    /// <summary>
    /// <see cref="ArucoObject.HashCode"/>.
    /// </summary>
    public override int HashCode { get { return hashCode; } }

    /// <summary>
    /// The marker id in the used dictionary.
    /// </summary>
    public int MarkerId
    {
      get { return markerId; }
      set
      {
        OnPropertyUpdating();
        markerId = value;
        UpdateHashCode();
        OnPropertyUpdated();
      }
    }

    // Variables

    protected int hashCode;

    // MonoBehaviour methods

    protected override void Awake()
    {
      base.Awake();
      UpdateHashCode();
    }

    // Methods

    protected virtual void UpdateHashCode()
    {
      hashCode = 17;
      hashCode = hashCode * 31 + typeof(ArucoMarker).GetHashCode();
      hashCode = hashCode * 31 + MarkerId;
    }
  }

  /// \} aruco_unity_package
}