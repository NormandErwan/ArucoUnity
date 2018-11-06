using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity.Calibration.Pinhole
{
    public class StereoPinholeCameraCalibration : PinholeCameraCalibrationBase<StereoArucoCamera>
    {
        // ArucoCameraCalibration methods

        protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
        {
            // Calibrate first each camera
            var cameraParameters = CameraParametersController.CameraParameters;
            for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
            {
                Std.VectorVec3d rvecs, tvecs;
                cameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(objectPoints[cameraId], imagePoints[cameraId],
                    ArucoCamera.Images[cameraId].Size, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
                    out rvecs, out tvecs, calibrationFlags.Flags);

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
            var imageSize = ArucoCamera.Images[cameraId1].Size;

            var stereoCameraParameters = new StereoArucoCameraParameters();

            Cv.Vec3d rvec, tvec;
            Cv.Mat rotationMatrix, essentialMatrix, fundamentalMatrix;
            stereoCameraParameters.ReprojectionError = Cv.StereoCalibrate(objectPoints[cameraId1], imagePoints[cameraId1],
                imagePoints[cameraId2], cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rotationMatrix,
                out tvec, out essentialMatrix, out fundamentalMatrix, calibrationFlags.Flags);
            Cv.Rodrigues(rotationMatrix, out rvec);

            stereoCameraParameters.RotationVector = rvec;
            stereoCameraParameters.TranslationVector = tvec;
            stereoCameraParameters.CalibrationFlagsValue = calibrationFlags.Value;
            cameraParameters.StereoCameraParameters = stereoCameraParameters;
        }
    }
}