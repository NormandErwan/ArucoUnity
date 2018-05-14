using ArucoUnity.Plugin;

namespace ArucoUnity.Cameras.Undistortions
{
  /// <summary>
  /// Manages the undistortion and rectification process for pinhole <see cref="ArucoCamera"/>.
  /// </summary>
  public class PinholeCameraUndistortion : PinholeCameraUndistortionGeneric<ArucoCamera>
  {
    // ArucoCameraUndistortion methods

    protected override void InitializeRectification()
    {
      for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
      {
        RectifiedCameraMatrices[cameraId] = Cv.GetOptimalNewCameraMatrix(CameraParameters.CameraMatrices[cameraId],
          CameraParameters.DistCoeffs[cameraId], ArucoCamera.Images[cameraId].Size, RectificationScalingFactor,
          ArucoCamera.Images[cameraId].Size, noROI, centerPrincipalPoint: true);
        RectificationMatrices[cameraId] = noRectificationMatrix;
      }
    }
  }
}