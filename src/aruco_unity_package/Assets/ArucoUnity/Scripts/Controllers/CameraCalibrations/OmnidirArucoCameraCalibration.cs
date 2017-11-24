using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraCalibrations.Flags;
using ArucoUnity.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraCalibrations
  {
    /// <summary>
    /// Manages the calibration, undistortion and rectification processes of fisheye and omnidir cameras.
    /// 
    /// See the OpenCV's ccalib module documentation for more information:
    /// http://docs.opencv.org/3.3.0/dd/d12/tutorial_omnidir_calib_main.html
    /// </summary>
    public class OmnidirArucoCameraCalibration : ArucoCameraCalibration
    {
      /// <summary>
      /// The different algorithms to use for the undistortion of the images.
      /// </summary>
      public enum RectificationTypes
      {
        Perspective,
        Cylindrical,
        LongitudeLatitude,
        Stereographic
      }

      // Editor fields

      [SerializeField]
      [Tooltip("The flags for the cameras calibration and the image rectification.")]
      private OmnidirCameraCalibrationFlags calibrationFlags;

      [SerializeField]
      [Tooltip("The flags for the stereo calibration of camera pairs.")]
      private OmnidirCameraCalibrationFlags stereoCalibrationFlags;

      [SerializeField]
      [Tooltip("The algorithm to use for the recitification of the images.")]
      private RectificationTypes rectificationType = RectificationTypes.Perspective;

      // Properties

      /// <summary>
      /// Gets or sets the flags for the cameras calibration and the image rectification.
      /// </summary>
      public OmnidirCameraCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

      /// <summary>
      /// Gets or sets the flags for the stereo calibration of camera pairs.
      /// </summary>
      public OmnidirCameraCalibrationFlags StereoCalibrationFlags { get { return stereoCalibrationFlags; } set { stereoCalibrationFlags = value; } }

      /// <summary>
      /// Gets or sets the algorithm to use for the rectification of the images. See this tutorial for illustrated examples:
      /// https://docs.opencv.org/3.3.0/dd/d12/tutorial_omnidir_calib_main.html
      /// </summary>
      public RectificationTypes RectificationType { get { return rectificationType; } set { rectificationType = value; } }

      // Variables

      protected Dictionary<RectificationTypes, Cv.Omnidir.Rectifify> rectifyFlags = new Dictionary<RectificationTypes, Cv.Omnidir.Rectifify>()
      {
        { RectificationTypes.Perspective,       Cv.Omnidir.Rectifify.Perspective },
        { RectificationTypes.Cylindrical,       Cv.Omnidir.Rectifify.Cylindrical },
        { RectificationTypes.LongitudeLatitude, Cv.Omnidir.Rectifify.Longlati },
        { RectificationTypes.Stereographic,     Cv.Omnidir.Rectifify.Stereographic }
      };

      // CalibrationController methods

      protected override void InitializeCameraParameters(CameraCalibrationFlags calibrationFlags)
      {
        base.InitializeCameraParameters(CalibrationFlags);
      }

      protected override void Calibrate(int cameraId, Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Cv.Size imageSize,
        out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs)
      {
        if (ArucoCamera is StereoArucoCamera)
        {
          rvecs = null;
          tvecs = null;
        }
        else
        {
          var cameraParameters = CameraParametersController.CameraParameters;
          cameraParameters.ReprojectionErrors[cameraId] = Cv.Omnidir.Calibrate(objectPoints, imagePoints, imageSize,
            cameraParameters.CameraMatrices[cameraId], cameraParameters.OmnidirXis[cameraId], cameraParameters.DistCoeffs[cameraId], out rvecs,
            out tvecs, CalibrationFlags.CalibrationFlags);
        }
      }

      protected override void StereoCalibrate(Std.VectorVectorPoint3f[] objectPoints, Std.VectorVectorPoint2f[] imagePoints, Cv.Size[] imageSizes,
        StereoCameraParameters stereoCameraParameters)
      {
        int cameraId1 = StereoArucoCamera.CameraId1;
        int cameraId2 = StereoArucoCamera.CameraId2;
        var cameraParameters = CameraParametersController.CameraParameters;
        var cameraMatrix1 = cameraParameters.CameraMatrices[cameraId1];
        var distCoeffs1 = cameraParameters.DistCoeffs[cameraId1];
        var xi1 = cameraParameters.OmnidirXis[cameraId1];
        var cameraMatrix2 = cameraParameters.CameraMatrices[cameraId2];
        var distCoeffs2 = cameraParameters.DistCoeffs[cameraId2];
        var xi2 = cameraParameters.OmnidirXis[cameraId2];

        Cv.Vec3d rvec, tvec;
        Std.VectorVec3d rvecsCamera1, tvecsCamera1;
        stereoCameraParameters.ReprojectionError = Cv.Omnidir.StereoCalibrate(objectPoints[cameraId1], imagePoints[cameraId1],
          imagePoints[cameraId2], imageSizes[cameraId1], imageSizes[cameraId2], cameraMatrix1, xi1, distCoeffs1,
          cameraMatrix2, xi2, distCoeffs2, out rvec, out tvec, out rvecsCamera1, out tvecsCamera1, CalibrationFlags.CalibrationFlags);

        Rvecs[StereoArucoCamera.CameraId1] = rvecsCamera1;
        Tvecs[StereoArucoCamera.CameraId1] = tvecsCamera1;

        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.CalibrationFlagsValue = CalibrationFlags.CalibrationFlagsValue;
      }
    }
  }

  /// \} aruco_unity_package
}