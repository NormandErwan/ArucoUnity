using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraDisplays;
using ArucoUnity.Plugin;
using System;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of fisheye and omnidir <see cref="StereoArucoCamera"/>.
    /// </summary>
    public class StereoArucoCameraOmnidirUndistortion : ArucoCameraGenericOmnidirUndistortion<StereoArucoCamera, StereoArucoCameraDisplay>
    {
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

      protected override void InitializeRectification()
      {
        var stereoCameraParameters = CameraParameters.StereoCameraParameters;

        // Initializes RectifiedCameraMatrices
        if (RectificationType == RectificationTypes.Perspective)
        {
          // TODO
        }
        else
        {
          base.InitializeRectification(); // Initalizes RectifiedCameraMatrices with default values for other types
        }

        // Initializes RectificationMatrices
        Cv.Mat rectificationMatrix1, rectificationMatrix2;
        Cv.Omnidir.StereoRectify(stereoCameraParameters.RotationVector, stereoCameraParameters.TranslationVector, out rectificationMatrix1,
          out rectificationMatrix2);

        RectificationMatrices[StereoArucoCamera.CameraId1] = rectificationMatrix1;
        RectificationMatrices[StereoArucoCamera.CameraId2] = rectificationMatrix2;
      }
    }
  }

  /// \} aruco_unity_package
}