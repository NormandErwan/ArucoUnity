using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraCalibrations.Flags;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraCalibrations
  {
    /// <summary>
    /// Manages the calibration, undistortion and rectification processes of pinhole cameras.
    /// 
    /// See the OpenCV's calibd module documentation for more information:
    /// http://docs.opencv.org/3.3.0/d9/d0c/group__calib3d.html
    /// </summary>
    public class PinholeArucoCameraCalibration : ArucoCameraCalibration
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The flags for the cameras calibration.")]
      private PinholeCameraCalibrationFlags calibrationFlags;

      [SerializeField]
      [Tooltip("The flags for the stereo calibration of camera pairs.")]
      private PinholeCameraCalibrationFlags stereoCalibrationFlags;

      // Properties

      /// <summary>
      /// Gets or sets the flags for the cameras calibration.
      /// </summary>
      public PinholeCameraCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

      /// <summary>
      /// Gets or sets the flags for the stereo calibration of camera pairs.
      /// </summary>
      public PinholeCameraCalibrationFlags StereoCalibrationFlags { get { return stereoCalibrationFlags; } set { stereoCalibrationFlags = value; } }

      // CalibrationController methods

      protected override void InitializeCameraParameters(CameraCalibrationFlags calibrationFlags)
      {
        base.InitializeCameraParameters(CalibrationFlags);

        if (CalibrationFlags.FixAspectRatio)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            CameraParametersController.CameraParameters.CameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] {
              CalibrationFlags.FixAspectRatioValue, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 });
          }
          CameraParametersController.CameraParameters.FixAspectRatioValue = CalibrationFlags.FixAspectRatioValue;
        }
      }

      protected override void Calibrate(int cameraId, Std.VectorVectorPoint3f objectPoints, Std.VectorVectorPoint2f imagePoints, Cv.Size imageSize,
        out Std.VectorVec3d rvecs, out Std.VectorVec3d tvecs)
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        cameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(objectPoints, imagePoints, imageSize,
          cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs,
          CalibrationFlags.CalibrationFlags);
      }

      protected override void StereoCalibrate(Std.VectorVectorPoint3f[] objectPoints, Std.VectorVectorPoint2f[] imagePoints, Cv.Size[] imageSizes,
        StereoCameraParameters stereoCameraParameters)
      {
        int cameraId1 = StereoArucoCamera.CameraId1;
        int cameraId2 = StereoArucoCamera.CameraId2;
        var cameraParameters = CameraParametersController.CameraParameters;
        var cameraMatrix1 = cameraParameters.CameraMatrices[cameraId1];
        var distCoeffs1 = cameraParameters.DistCoeffs[cameraId1];
        var cameraMatrix2 = cameraParameters.CameraMatrices[cameraId2];
        var distCoeffs2 = cameraParameters.DistCoeffs[cameraId2];
        var imageSize = imageSizes[cameraId1];

        Cv.Vec3d rvec, tvec;
        Cv.Mat essentialMatrix, fundamentalMatrix;
        stereoCameraParameters.ReprojectionError = Cv.StereoCalibrate(objectPoints[cameraId1], imagePoints[cameraId1], imagePoints[cameraId2],
          cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rvec, out tvec, out essentialMatrix, out fundamentalMatrix,
          CalibrationFlags.CalibrationFlags);

        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.CalibrationFlagsValue = CalibrationFlags.CalibrationFlagsValue;
      }
    }
  }

  /// \} aruco_unity_package
}