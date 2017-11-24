using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraDisplays;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of pinhole <see cref="ArucoCamera"/>.
    /// </summary>
    public class ArucoCameraPinholeUndistortion : ArucoCameraGenericPinholeUndistortion<ArucoCamera, ArucoCameraDisplay>
    {
      protected override void InitializeRectification()
      {
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          RectifiedCameraMatrices[cameraId] = Cv.GetOptimalNewCameraMatrix(CameraParameters.CameraMatrices[cameraId],
            CameraParameters.DistCoeffs[cameraId], ArucoCamera.Images[cameraId].Size, RectificationScalingFactor);
          RectificationMatrices[cameraId] = noRectificationMatrix;
        }
      }
    }
  }

  /// \} aruco_unity_package
}