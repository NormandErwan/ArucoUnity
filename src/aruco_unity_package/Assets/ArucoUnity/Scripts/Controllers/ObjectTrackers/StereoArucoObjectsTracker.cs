using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CameraDisplays;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.ObjectTrackers
  {
    public class StereoArucoObjectsTracker : ArucoObjectsGenericTracker<StereoArucoCamera>
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera display associated with the ArucoCamera.")]
      private StereoArucoCameraDisplay stereoArucoCameraDisplay;

      // Properties

      public override ArucoCameraGenericDisplay<StereoArucoCamera> ArucoCameraDisplay { get { return stereoArucoCameraDisplay; } }

      /// <summary>
      /// Gets or sets the camera display associated with the ArucoCamera.
      /// </summary>
      public StereoArucoCameraDisplay StereoArucoCameraDisplay { get { return stereoArucoCameraDisplay; } set { stereoArucoCameraDisplay = value; } }
    }
  }

  /// \} aruco_unity_package
}
