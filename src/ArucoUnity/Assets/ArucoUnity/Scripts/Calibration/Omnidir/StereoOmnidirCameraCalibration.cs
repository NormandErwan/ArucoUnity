using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Calibration.Omnidir
  {
    public class StereoOmnidirCameraCalibration : ArucoCameraCalibrationGeneric<StereoArucoCamera, OmnidirCameraCalibrationFlags>
    {
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
          cameraMatrix2, xi2, distCoeffs2, out rvec, out tvec, out rvecsCamera1, out tvecsCamera1, calibrationFlags.Flags);

        Rvecs[StereoArucoCamera.CameraId1] = rvecsCamera1;
        Tvecs[StereoArucoCamera.CameraId1] = tvecsCamera1;

        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.CalibrationFlagsValue = calibrationFlags.Value;
        cameraParameters.StereoCameraParameters = stereoCameraParameters;
      }
    }
  }

  /// \} aruco_unity_package
}