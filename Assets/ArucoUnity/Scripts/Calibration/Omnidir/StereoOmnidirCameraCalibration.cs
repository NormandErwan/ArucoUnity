using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity.Calibration.Omnidir
{
    public class StereoOmnidirCameraCalibration : ArucoCameraCalibrationGeneric<StereoArucoCamera, OmnidirCameraCalibrationFlags>
    {
        // ArucoCameraCalibration methods

        protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
        {
            int cameraId1 = StereoArucoCamera.CameraId1, cameraId2 = StereoArucoCamera.CameraId2;
            var cameraParameters = CameraParametersController.CameraParameters;

            Cv.Vec3d rvec, tvec;
            Std.VectorVec3d rvecsCamera1, tvecsCamera1;
            var reprojectionError = Cv.Omnidir.StereoCalibrate(
                objectPoints[cameraId1],
                imagePoints[cameraId1],
                imagePoints[cameraId2],
                ArucoCamera.Images[cameraId1].Size,
                ArucoCamera.Images[cameraId2].Size,
                cameraParameters.CameraMatrices[cameraId1],
                cameraParameters.OmnidirXis[cameraId1],
                cameraParameters.DistCoeffs[cameraId1],
                cameraParameters.CameraMatrices[cameraId2],
                cameraParameters.OmnidirXis[cameraId2],
                cameraParameters.DistCoeffs[cameraId2],
                out rvec,
                out tvec,
                out rvecsCamera1,
                out tvecsCamera1,
                calibrationFlags.Flags);

            Rvecs[StereoArucoCamera.CameraId1] = rvecsCamera1;
            Tvecs[StereoArucoCamera.CameraId1] = tvecsCamera1;

            cameraParameters.StereoCameraParameters = new StereoArucoCameraParameters()
            {
                ReprojectionError = reprojectionError,
                RotationVector = rvec,
                TranslationVector = tvec,
                CalibrationFlagsValue = calibrationFlags.Value
            };
        }
    }
}