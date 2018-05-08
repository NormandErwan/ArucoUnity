using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity.Objects
{
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
    /// Gets or sets the marker id in the used dictionary.
    /// </summary>
    public int MarkerId
    {
      get { return markerId; }
      set
      {
        OnPropertyUpdating();
        markerId = value;
        OnPropertyUpdated();
      }
    }

    // ArucoObject methods

    public override Cv.Mat Draw()
    {
#if UNITY_EDITOR
      if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && (MarkerSideLength <= 0 || MarkerBorderBits == 0 || Dictionary == null))
      {
        return null;
      }
#endif
      Cv.Mat image;
      Dictionary.DrawMarker(MarkerId, GetInPixels(MarkerSideLength), out image, (int)MarkerBorderBits);

      return image;
    }

    public override string GenerateName()
    {
      return "ArUcoUnity_Marker_" + Dictionary.Name + "_Id_" + MarkerId;
    }

    public override Vector3 GetGameObjectScale()
    {
      return MarkerSideLength * Vector3.one;
    }

    protected override void UpdateArucoHashCode()
    {
      ArucoHashCode = GetArucoHashCode(MarkerId);
    }

    // Methods

    /// <summary>
    /// Computes the hash code of a marker based on its id.
    /// </summary>
    /// <param name="markerId">The marker id.</param>
    /// <returns>The calculated ArUco hash code.</returns>
    public static int GetArucoHashCode(int markerId)
    {
      int hashCode = 17;
      hashCode = hashCode * 31 + typeof(ArucoMarker).GetHashCode();
      hashCode = hashCode * 31 + markerId;
      return hashCode;
    }
  }
}