using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity.Cameras.Undistortions
{
  /// <summary>
  /// Manages the undistortion and rectification process for pinhole <see cref="StereoArucoCamera"/>.
  /// </summary>
  public class StereoPinholeCameraUndistortion : PinholeCameraUndistortionGeneric<StereoArucoCamera>
  {
    // Editor fields

    [SerializeField]
    [Tooltip("If true (default), the principal points of the images have the same pixel coordinates in the rectified views. Only applied if" +
      "using a stereo camera.")]
    private bool rectificationZeroDisparity = true;

    // Properties

    /// <summary>
    /// Gets or sets if the principal point of the images have the same pixel coordinates in the rectified views (true by default). Only applied if
    /// using a stereo camera.
    /// </summary>
    public bool RectificationZeroDisparity { get { return rectificationZeroDisparity; } set { rectificationZeroDisparity = value; } }

    // Variables

    StereoArucoCameraParameters stereoCameraParameters;

    // ConfigurableController methods

    protected override void Configuring()
    {
      base.Configuring();

      stereoCameraParameters = CameraParameters.StereoCameraParameters;
      if (stereoCameraParameters == null)
      {
        throw new Exception("The camera parameters must contains a valid StereoCameraParameters to undistort and rectify a StereoArucoCamera.");
      }
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