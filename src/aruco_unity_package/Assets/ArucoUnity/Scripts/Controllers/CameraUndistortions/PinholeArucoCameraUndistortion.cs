using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of pinhole cameras.
    /// 
    /// See the OpenCV's calibd module documentation for more information:
    /// http://docs.opencv.org/3.3.0/d9/d0c/group__calib3d.html
    /// </summary>
    public class PinholeArucoCameraUndistortion : ArucoCameraUndistortion
    {
      /// <summary>
      /// Configures the <see cref="ArucoCameraUndistortion.UndistortionRectificationMaps"/> and
      /// <see cref="ArucoCameraUndistortion.RectifiedCameraMatrices"/> according to the <see cref="ArucoCameraUndistortion.CameraParameters"/>
      /// </summary>
      protected override void ConfigureUndistortionRectification(int cameraId, Cv.Mat rectificationMatrix, Cv.Mat newCameraMatrix)
      {
        var cameraParameters = CameraParametersController.CameraParameters;

        Cv.InitUndistortRectifyMap(cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId], rectificationMatrix,
            newCameraMatrix, ArucoCamera.Images[cameraId].Size, Cv.Type.CV_16SC2, out UndistortionRectificationMaps[cameraId][0],
            out UndistortionRectificationMaps[cameraId][1]);

        RectifiedCameraMatrices[cameraId] = newCameraMatrix;
        
      }
    }
  }

  /// \} aruco_unity_package
}