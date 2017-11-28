using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraCalibrations.Flags;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraCalibrations.Omnidir
  {
    public class StereoArucoCameraOmnidirCalibration : ArucoCameraGenericOmnidirCalibration
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The camera system to use.")]
      private ArucoCamera arucoCamera;

      [SerializeField]
      [Tooltip("The flags for the camera calibration.")]
      private OmnidirCameraCalibrationFlags calibrationFlags;

      // ArucoCameraController properties

      public override IArucoCamera ArucoCamera { get { return arucoCamera; } }

      // ArucoCameraGenericOmnidirCalibration properties

      public override OmnidirCameraCalibrationFlags CalibrationFlags { get { return calibrationFlags; } set { calibrationFlags = value; } }

      // Properties

      /// <summary>
      /// Gets or sets the camera system to use.
      /// </summary>
      public ArucoCamera ConcreteArucoCamera { get { return arucoCamera; } set { arucoCamera = value; } }

      // ArucoCameraCalibration methods

      protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
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

        var stereoCameraParameters = new StereoCameraParameters();

        Cv.Vec3d rvec, tvec;
        Std.VectorVec3d rvecsCamera1, tvecsCamera1;
        stereoCameraParameters.ReprojectionError = Cv.Omnidir.StereoCalibrate(objectPoints[cameraId1], imagePoints[cameraId1],
          imagePoints[cameraId2], calibrationImageSizes[cameraId1], calibrationImageSizes[cameraId2], cameraMatrix1, xi1, distCoeffs1,
          cameraMatrix2, xi2, distCoeffs2, out rvec, out tvec, out rvecsCamera1, out tvecsCamera1, CalibrationFlags.CalibrationFlags);

        Rvecs[StereoArucoCamera.CameraId1] = rvecsCamera1;
        Tvecs[StereoArucoCamera.CameraId1] = tvecsCamera1;

        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.CalibrationFlagsValue = CalibrationFlags.CalibrationFlagsValue;
        cameraParameters.StereoCameraParameters = stereoCameraParameters;
      }
    }
  }

  /// \} aruco_unity_package
}