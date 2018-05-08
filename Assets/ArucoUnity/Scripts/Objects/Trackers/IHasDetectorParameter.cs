using ArucoUnity.Plugin;

namespace ArucoUnity.Objects.Trackers
{
  /// <summary>
  /// Contains a <see cref="Aruco.DetectorParameters"/>.
  /// </summary>
  public interface IHasDetectorParameter
  {
    /// <summary>
    /// Gets or sets the parameters to use for the detection.
    /// </summary>
    Aruco.DetectorParameters DetectorParameters { get; set; }
  }
}