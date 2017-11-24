using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraDisplays;
using ArucoUnity.Plugin;
using System;
using UnityEngine;

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
          for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
          {
            float imageWidth = CameraParameters.ImageWidths[cameraId];
            float imageHeight = CameraParameters.ImageHeights[cameraId];

            float cameraF = ArucoCameraDisplay.Cameras[cameraId].pixelHeight / (2f * Mathf.Tan(0.5f * ArucoCameraDisplay.Cameras[cameraId].fieldOfView * Mathf.Deg2Rad));
            RectifiedCameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] {
              cameraF, 0, CameraParameters.ImageWidths[cameraId] / 2,
              0, cameraF, CameraParameters.ImageHeights[cameraId] / 2,
              0, 0, 1
            }).Clone();
          }
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