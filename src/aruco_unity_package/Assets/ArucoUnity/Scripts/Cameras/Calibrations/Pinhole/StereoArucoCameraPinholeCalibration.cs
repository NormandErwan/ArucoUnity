using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Cameras.Calibrations.Flags;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Calibrations.Pinhole
  {
    public class StereoArucoCameraPinholeCalibration : ArucoCameraGenericPinholeCalibration
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private StereoArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The flags for the cameras calibration.")]
      private PinholeCameraCalibrationFlags calibrationFlags;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // ArucoCameraGenericPinholeCalibration properties

      public override PinholeCameraCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public StereoArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

      // ArucoCameraCalibration methods

      protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
      {
        // Calibrate first each camera
        var cameraParameters = CameraParametersController.CameraParameters;
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          Std.VectorVec3d rvecs, tvecs;
          cameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(objectPoints[cameraId], imagePoints[cameraId],
            calibrationImageSizes[cameraId], cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs,
            CalibrationFlags.CalibrationFlags);

          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }

        // Stereo calibration
        int cameraId1 = StereoArucoCamera.CameraId1;
        int cameraId2 = StereoArucoCamera.CameraId2;
        var cameraMatrix1 = cameraParameters.CameraMatrices[cameraId1];
        var distCoeffs1 = cameraParameters.DistCoeffs[cameraId1];
        var cameraMatrix2 = cameraParameters.CameraMatrices[cameraId2];
        var distCoeffs2 = cameraParameters.DistCoeffs[cameraId2];
        var imageSize = calibrationImageSizes[cameraId1];

        var stereoCameraParameters = new StereoCameraParameters();

        Cv.Vec3d rvec, tvec;
        Cv.Mat rotationMatrix, essentialMatrix, fundamentalMatrix;
        stereoCameraParameters.ReprojectionError = Cv.StereoCalibrate(objectPoints[cameraId1], imagePoints[cameraId1], imagePoints[cameraId2],
          cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rotationMatrix, out tvec, out essentialMatrix, out fundamentalMatrix,
          CalibrationFlags.CalibrationFlags);
        Cv.Rodrigues(rotationMatrix, out rvec);

        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.CalibrationFlagsValue = CalibrationFlags.CalibrationFlagsValue;
        cameraParameters.StereoCameraParameters = stereoCameraParameters;
      }
    }
  }

  /// \} aruco_unity_package
}