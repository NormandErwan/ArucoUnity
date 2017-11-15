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
      private PinholeCalibrationFlags calibrationFlags;

      [SerializeField]
      [Tooltip("The flags for the stereo calibration of camera pairs.")]
      private PinholeCalibrationFlags stereoCalibrationFlags;

      // Properties

      /// <summary>
      /// Gets or sets the flags for the cameras calibration.
      /// </summary>
      public PinholeCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

      /// <summary>
      /// Gets or sets the flags for the stereo calibration of camera pairs.
      /// </summary>
      public PinholeCalibrationFlags StereoCalibrationFlags { get { return stereoCalibrationFlags; } set { stereoCalibrationFlags = value; } }

      // CalibrationController methods

      protected override void InitializeCameraParameters(CalibrationFlags calibrationFlags)
      {
        base.InitializeCameraParameters(CalibrationFlags);

        if (CalibrationFlags.FixAspectRatio)
        {
          for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
          {
            CameraParametersController.CameraParameters.CameraMatrices[cameraId] = (CameraMatrix)new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] {
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

      protected override void StereoCalibrate(CameraPair cameraPair, Std.VectorVectorPoint3f[] objectPoints, Std.VectorVectorPoint2f[] imagePoints,
        Cv.Size[] imageSizes, StereoCameraParameters stereoCameraParameters)
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        var cameraMatrix1 = cameraParameters.CameraMatrices[cameraPair.CameraId1];
        var distCoeffs1 = cameraParameters.DistCoeffs[cameraPair.CameraId1];
        var cameraMatrix2 = cameraParameters.CameraMatrices[cameraPair.CameraId2];
        var distCoeffs2 = cameraParameters.DistCoeffs[cameraPair.CameraId2];
        var imageSize = imageSizes[cameraPair.CameraId1];

        // Estimates transformation between the two cameras 
        Cv.Vec3d rvec, tvec;
        Cv.Mat essentialMatrix, fundamentalMatrix;
        stereoCameraParameters.ReprojectionError = Cv.StereoCalibrate(objectPoints[cameraPair.CameraId1], imagePoints[cameraPair.CameraId1],
          imagePoints[cameraPair.CameraId2], cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rvec, out tvec,
          out essentialMatrix, out fundamentalMatrix, CalibrationFlags.CalibrationFlags);

        // Computes rectification transforms
        Cv.Mat rotationMatrix1, rotationMatrix2, newCameraMatrix1, newCameraMatrix2, Q;
        Cv.StereoRectifyFlags stereoRectifyFlags = StereoCalibrationFlags.ZeroDisparity ? Cv.StereoRectifyFlags.ZeroDisparity : 0;
        Cv.StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rvec, tvec, out rotationMatrix1,
          out rotationMatrix2, out newCameraMatrix1, out newCameraMatrix2, out Q, stereoRectifyFlags, StereoCalibrationFlags.Skew, imageSize);

        // Save the camera parameters
        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.RotationMatrices = new Cv.Mat[] { rotationMatrix1, rotationMatrix2 };
        stereoCameraParameters.NewCameraMatrices = new CameraMatrix[] { (CameraMatrix)newCameraMatrix1, (CameraMatrix)newCameraMatrix2 };
        stereoCameraParameters.CalibrationFlagsValue = CalibrationFlags.CalibrationFlagsValue;
      }
    }
  }

  /// \} aruco_unity_package
}