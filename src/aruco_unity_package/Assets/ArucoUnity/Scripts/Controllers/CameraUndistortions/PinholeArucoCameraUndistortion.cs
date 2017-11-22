using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
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
    public class PinholeArucoCameraUndistortion : ArucoCameraUndistortion
    {
      // Editor fields

      [SerializeField]
      [Tooltip("Scaling factor (alpha coefficient) between 0 and 1: 0 to zoom the images so that only valid pixels are visible (no black areas" +
        " after rectification), 1 to shift the images so that no source image pixels are lost. Applied both on mono and stereo cameras.")]
      [Range(0, 1)]
      private float rectificationScalingFactor = 1;

      [SerializeField]
      [Tooltip("If true (default), the principal points of the images have the same pixel coordinates in the rectified views. Only applied if" +
        "using a stereo camera.")]
      private bool stereoRectificationDisparity = true;

      // Properties

      /// <summary>
      /// Gets or sets the scaling factor (alpha coefficient) between 0 and 1: 0 to zoom the images so that only valid pixels are visible (no black
      /// areas after rectification), 1 to shift the images so that no source image pixels are lost. Applied both on mono and stereo cameras.
      /// </summary>
      public float RectificationScalingFactor { get { return rectificationScalingFactor; } set { rectificationScalingFactor = value; } }

      /// <summary>
      /// Gets or sets if the principal point of the images have the same pixel coordinates in the rectified views (true by default). Only applied if
      /// using a stereo camera.
      /// </summary>
      public bool StereoRectificationDisparity { get { return stereoRectificationDisparity; } set { stereoRectificationDisparity = value; } }

      // ArucoCameraUndistortion methods

      protected override void InitializeUndistortionRectification()
      {
        base.InitializeUndistortionRectification();

        var cameraParameters = CameraParametersController.CameraParameters;
        var stereoCameraParameters = cameraParameters.StereoCameraParameters;

        // Initializes rectified camera matrices
        if (stereoCameraParameters == null)
        {
          for (int cameraId = 0; cameraId < cameraParameters.CameraNumber; cameraId++)
          {
            RectifiedCameraMatrices[cameraId] = Cv.GetOptimalNewCameraMatrix(cameraParameters.CameraMatrices[cameraId],
              cameraParameters.DistCoeffs[cameraId], ArucoCamera.Images[cameraId].Size, RectificationScalingFactor);
            RectificationMatrices[cameraId] = noRectificationMatrix;
          }
        }
        else
        {
          int cameraId1 = StereoArucoCamera.CameraId1;
          int cameraId2 = StereoArucoCamera.CameraId2;

          Cv.Mat rotationMatrix, rectificationMatrix1, rectificationMatrix2, newCameraMatrix1, newCameraMatrix2, disparityMatrix;
          Cv.StereoRectifyFlags stereoRectifyFlags = StereoRectificationDisparity ? Cv.StereoRectifyFlags.ZeroDisparity : 0;

          Cv.Rodrigues(stereoCameraParameters.RotationVector, out rotationMatrix);
          Cv.StereoRectify(cameraParameters.CameraMatrices[cameraId1], cameraParameters.DistCoeffs[cameraId1],
            cameraParameters.CameraMatrices[cameraId2], cameraParameters.DistCoeffs[cameraId2], ArucoCamera.Images[cameraId1].Size, rotationMatrix,
            stereoCameraParameters.TranslationVector, out rectificationMatrix1, out rectificationMatrix2, out newCameraMatrix1, out newCameraMatrix2,
            out disparityMatrix, stereoRectifyFlags, rectificationScalingFactor);

          RectifiedCameraMatrices[cameraId1] = newCameraMatrix1;
          RectifiedCameraMatrices[cameraId2] = newCameraMatrix2;
          RectificationMatrices[cameraId1] = rectificationMatrix1;
          RectificationMatrices[cameraId2] = rectificationMatrix2;
        }

        // Initializes undistortion maps
        for (int cameraId = 0; cameraId < cameraParameters.CameraNumber; cameraId++)
        {
          Cv.InitUndistortRectifyMap(cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
            RectificationMatrices[cameraId], RectifiedCameraMatrices[cameraId], ArucoCamera.Images[cameraId].Size, Cv.Type.CV_16SC2,
            out UndistortionRectificationMaps[cameraId][0], out UndistortionRectificationMaps[cameraId][1]);
        }
      }
    }
  }

  /// \} aruco_unity_package
}