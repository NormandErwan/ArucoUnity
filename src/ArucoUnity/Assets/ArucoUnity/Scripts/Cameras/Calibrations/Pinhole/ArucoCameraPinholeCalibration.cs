using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Calibrations.Pinhole
  {
    public class ArucoCameraPinholeCalibration : ArucoCameraGenericPinholeCalibration<ArucoCamera>
    {
      // ArucoCameraCalibration methods

      protected override void Calibrate(Std.VectorVectorPoint2f[] imagePoints, Std.VectorVectorPoint3f[] objectPoints)
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        for (int cameraId = 0; cameraId < ArucoCamera.CameraNumber; cameraId++)
        {
          Std.VectorVec3d rvecs, tvecs;
          cameraParameters.ReprojectionErrors[cameraId] = Cv.CalibrateCamera(objectPoints[cameraId], imagePoints[cameraId],
            calibrationImageSizes[cameraId], cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
            out rvecs, out tvecs, calibrationFlags.Flags);

          Rvecs[cameraId] = rvecs;
          Tvecs[cameraId] = tvecs;
        }
      }
    }
  }

  /// \} aruco_unity_package
}