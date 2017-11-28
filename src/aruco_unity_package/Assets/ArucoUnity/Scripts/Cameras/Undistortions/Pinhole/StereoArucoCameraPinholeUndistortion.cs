using ArucoUnity.Cameras;
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
    /// Manages the undistortion and rectification process of pinhole <see cref="StereoArucoCamera"/>.
    /// </summary>
    public class StereoArucoCameraPinholeUndistortion : ArucoCameraGenericPinholeUndistortion
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private StereoArucoCamera stereoArucoCamera;

      [SerializeField]
      [Tooltip("If true (default), the principal points of the images have the same pixel coordinates in the rectified views. Only applied if" +
        "using a stereo camera.")]
      private bool rectificationZeroDisparity = true;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return stereoArucoCamera; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public StereoArucoCamera StereoArucoCamera { get { return stereoArucoCamera; } set { stereoArucoCamera = value; } }

      /// <summary>
      /// Gets or sets if the principal point of the images have the same pixel coordinates in the rectified views (true by default). Only applied if
      /// using a stereo camera.
      /// </summary>
      public bool RectificationZeroDisparity { get { return rectificationZeroDisparity; } set { rectificationZeroDisparity = value; } }

      // Variables

      StereoCameraParameters stereoCameraParameters;

      // ArucoCameraController methods

      public override void Configure()
      {
        stereoCameraParameters = CameraParameters.StereoCameraParameters;
        if (stereoCameraParameters == null)
        {
          throw new Exception("The camera parameters must contains a valid StereoCameraParameters to undistort and rectify a StereoArucoCamera.");
        }
        base.Configure();
      }

      // ArucoCameraUndistortion methods

      protected override void InitializeRectification()
      {
        int cameraId1 = StereoArucoCamera.CameraId1;
        int cameraId2 = StereoArucoCamera.CameraId2;

        Cv.Mat rotationMatrix, rectificationMatrix1, rectificationMatrix2, newCameraMatrix1, newCameraMatrix2, disparityMatrix;
        Cv.StereoRectifyFlags stereoRectifyFlags = RectificationZeroDisparity ? Cv.StereoRectifyFlags.ZeroDisparity : 0;

        Cv.Rodrigues(stereoCameraParameters.RotationVector, out rotationMatrix);
        Cv.StereoRectify(CameraParameters.CameraMatrices[cameraId1], CameraParameters.DistCoeffs[cameraId1],
          CameraParameters.CameraMatrices[cameraId2], CameraParameters.DistCoeffs[cameraId2], ArucoCamera.Images[cameraId1].Size, rotationMatrix,
          stereoCameraParameters.TranslationVector, out rectificationMatrix1, out rectificationMatrix2, out newCameraMatrix1, out newCameraMatrix2,
          out disparityMatrix, stereoRectifyFlags, RectificationScalingFactor);

        RectifiedCameraMatrices[cameraId1] = newCameraMatrix1;
        RectifiedCameraMatrices[cameraId2] = newCameraMatrix2;
        RectificationMatrices[cameraId1] = rectificationMatrix1;
        RectificationMatrices[cameraId2] = rectificationMatrix2;
      }
    }
  }

  /// \} aruco_unity_package
}