using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Undistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of fisheye and omnidir <see cref="Cameras.StereoArucoCamera"/>.
    /// </summary>
    public class StereoArucoCameraOmnidirUndistortion : ArucoCameraGenericOmnidirUndistortion
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private StereoArucoCamera stereoArucoCamera;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return stereoArucoCamera; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public StereoArucoCamera StereoArucoCamera { get { return stereoArucoCamera; } set { stereoArucoCamera = value; } }

      // ArucoCameraController methods

      public override void Configure()
      {
        if (CameraParameters.StereoCameraParameters == null)
        {
          throw new Exception("The camera parameters must contains a valid StereoCameraParameters to undistort and rectify a StereoArucoCamera.");
        }
        base.Configure();
      }

      // ArucoCameraUndistortion methods

      /// <summary>
      /// Initializes <see cref="RectificationMatrices"/> with the stereo camera parameters.
      /// </summary>
      protected override void InitializeRectification()
      {
        base.InitializeRectification();

        Cv.Mat rectificationMatrix1, rectificationMatrix2;
        Cv.Omnidir.StereoRectify(CameraParameters.StereoCameraParameters.RotationVector, CameraParameters.StereoCameraParameters.TranslationVector,
          out rectificationMatrix1, out rectificationMatrix2);

        RectificationMatrices[StereoArucoCamera.CameraId1] = rectificationMatrix1;
        RectificationMatrices[StereoArucoCamera.CameraId2] = rectificationMatrix2;
      }
    }
  }

  /// \} aruco_unity_package
}