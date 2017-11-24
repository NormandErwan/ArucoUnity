using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
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

  /// \} aruco_unity_package
}