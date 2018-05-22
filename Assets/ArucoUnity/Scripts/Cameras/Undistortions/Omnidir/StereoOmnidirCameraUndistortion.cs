using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System;

namespace ArucoUnity.Cameras.Undistortions
{
  /// <summary>
  /// Manages the undistortion and rectification process for fisheye and omnidir <see cref="StereoArucoCamera"/>.
  /// </summary>
  public class StereoOmnidirCameraUndistortion : OmnidirCameraUndistortionGeneric<StereoArucoCamera>
  {
    // Variables

    StereoArucoCameraParameters stereoCameraParameters;

    // ConfigurableController methods

    protected override void Configuring()
    {
      base.Configuring();

      stereoCameraParameters = CameraParameters.StereoCameraParameters;
      if (stereoCameraParameters == null)
      {
        throw new Exception("The camera parameters must contains a valid StereoCameraParameters to undistort and" +
          " rectify a StereoArucoCamera.");
      }
    }

    // ArucoCameraUndistortion methods

    /// <summary>
    /// Initializes <see cref="RectificationMatrices"/> with the stereo camera parameters.
    /// </summary>
    protected override void InitializeRectification()
    {
      base.InitializeRectification();

      Cv.Omnidir.StereoRectify(stereoCameraParameters.RotationVector, stereoCameraParameters.TranslationVector,
        out RectificationMatrices[StereoArucoCamera.CameraId1], out RectificationMatrices[StereoArucoCamera.CameraId2]);
    }
  }
}