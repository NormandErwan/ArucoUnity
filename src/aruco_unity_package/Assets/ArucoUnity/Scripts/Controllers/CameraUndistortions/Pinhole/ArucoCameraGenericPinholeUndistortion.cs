using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraDisplays;
using ArucoUnity.Plugin;
using UnityEngine;

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
    public abstract class ArucoCameraGenericPinholeUndistortion<T, U> : ArucoCameraUndistortion<T, U> 
      where T : ArucoCamera
      where U : ArucoCameraGenericDisplay<T>
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Scaling factor (alpha coefficient) between 0 and 1: 0 to zoom the images so that only valid pixels are visible (no black areas" +
        " after rectification), 1 to shift the images so that no source image pixels are lost. Applied both on mono and stereo cameras.")]
      [Range(0, 1)]
      private float rectificationScalingFactor = 1;

      // Properties

      /// <summary>
      /// Gets or sets the scaling factor (alpha coefficient) between 0 and 1: 0 to zoom the images so that only valid pixels are visible (no black
      /// areas after rectification), 1 to shift the images so that no source image pixels are lost. Applied both on mono and stereo cameras.
      /// </summary>
      public float RectificationScalingFactor { get { return rectificationScalingFactor; } set { rectificationScalingFactor = value; } }

      // ArucoCameraUndistortion methods

      protected override void InitializeUndistortionMaps()
      {
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          Cv.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
            RectificationMatrices[cameraId], RectifiedCameraMatrices[cameraId], ArucoCamera.Images[cameraId].Size, Cv.Type.CV_16SC2,
            out UndistortionRectificationMaps[cameraId][0], out UndistortionRectificationMaps[cameraId][1]);
        }
      }
    }
  }

  /// \} aruco_unity_package
}