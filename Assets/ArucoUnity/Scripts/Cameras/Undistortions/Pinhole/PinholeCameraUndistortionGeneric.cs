using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity.Cameras.Undistortions
{
  /// <summary>
  /// Manages the undistortion and rectification process for pinhole cameras.
  /// 
  /// See the OpenCV's calibd module documentation for more information:
  /// http://docs.opencv.org/3.4/d9/d0c/group__calib3d.html
  /// </summary>
  public abstract class PinholeCameraUndistortionGeneric<T> : ArucoCameraUndistortionGeneric<T>
    where T : ArucoCamera
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