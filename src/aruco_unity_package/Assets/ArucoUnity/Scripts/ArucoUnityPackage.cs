// This file is for documenting the namespaces.

namespace ArucoUnity
{
  /// \defgroup aruco_unity_package ArUco Unity package
  /// \brief Unity 5 package that provide the OpenCV's ArUco Marker Detection extra module features using the ArUco Unity library.
  ///
  /// See the OpenCV documentation for more information about its ArUco Marker Detection extra module: http://docs.opencv.org/3.2.0/d9/d6a/group__aruco.html
  /// \{

  /// <summary>
  /// Manages cameras for calibration and ArUco objects tracking.
  /// </summary>
  namespace Cameras
  {
    /// <summary>
    /// Scripts to manage OpenCV's camera parameters from calibrations.
    /// </summary>
    namespace Parameters
    {
    }
  }

  /// <summary>
  /// Camera calibration, and ArUco objects creation and tracking scripts.
  /// </summary>
  namespace Controllers
  {
    /// <summary>
    /// Manages calibration flags.
    /// </summary>
    namespace CalibrationFlagsControllers
    {
    }

    /// <summary>
    /// Tracking scripts for the ArUco objects.
    /// </summary>
    namespace ObjectTrackers
    {
    }

    /// <summary>
    /// Utility scripts.
    /// </summary>
    namespace Utility
    {
    }
  }

  /// <summary>
  /// Represent the different ArUco objects that can be tracked or used for camera calibration.
  /// </summary>
  namespace Objects
  {
  }

  /// <summary>
  /// Bindings to the ArUco Unity library.
  /// </summary>
  namespace Plugin
  {
  }

  /// \}
}