using ArucoUnity.Cameras;
using ArucoUnity.Plugin;

namespace ArucoUnity.Calibration.Pinhole
{
    public class PinholeCameraCalibration : PinholeCameraCalibrationBase<ArucoCamera>
    {
        // ArucoCameraCalibration methods

        protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
        {
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
        }
    }
}